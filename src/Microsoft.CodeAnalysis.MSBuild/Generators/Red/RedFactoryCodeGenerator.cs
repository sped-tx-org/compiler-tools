// -----------------------------------------------------------------------
// <copyright file="RedFactoryCodeGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Factories;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    internal class RedFactoryCodeGenerator : AbstractCodeGenerator, IRedFactoryCodeGenerator
    {
        public RedFactoryCodeGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeCompileUnit GenerateFactory()
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            CodeNamespace ns = GenerateRedNodeNamespace();
            unit.Namespaces.Add(ns);
            ns.Imports.Add(new CodeNamespaceImport(Tree.SyntaxNamespace));
            ns.Imports.AddRange(GetUsings());
            return unit;
        }

        private CodeNamespace GenerateRedNodeNamespace()
        {
            CodeNamespace ns = new CodeNamespace(Tree.MainNamespace);
            ns.Types.Add(GenerateRedFactory());
            return ns;
        }

        private CodeTypeDeclaration GenerateRedFactory()
        {
            CodeTypeDeclaration codeType = new CodeTypeDeclaration("SyntaxFactory")
            {
                IsPartial = true,
                Attributes = MemberAttributes.Static,
                TypeAttributes = TypeAttributes.Public,
                IsClass = true
            };

            foreach (Node node in Tree.Types.OfType<Node>())
            {
                var nodeFields = GetNodeFields(node);
                var valueFields = GetValueFields(node);

                codeType.Members.Add(GenerateRedFactoryMethod(node));

                if (node.Fields.Count > 0)
                {
                    var nonAutomaticFields = node.GetMandatoryFields();
                    if (nonAutomaticFields.Count > 0)
                    {
                    }

                    var nAutoCreatableTokens = node.Fields.Count(f => IsAutoCreatableToken(node, f));
                    if (nAutoCreatableTokens == 0)
                    {
                        var factoryWithNoAutoCreatableTokenFields = new HashSet<Field>(DetermineRedFactoryWithNoAutoCreatableTokenFields(node));
                        var minimalFactoryFields = DetermineMinimalFactoryFields(node);

                        if (!(minimalFactoryFields != null && factoryWithNoAutoCreatableTokenFields.SetEquals(minimalFactoryFields)))
                        {
                            if (HasField(node, IsSyntaxList) || HasField(node, IsTokenList) || HasField(node, IsSeparatedSyntaxList) || HasField(node, IsIdentifierNameSyntax))
                            {
                                if (node.Name != "ProductionSyntax")
                                    codeType.Members.Add(GenerateRedFactoryMethodWithNoAutoCreatableTokens(node));
                            }
                        }
                    }
                    else
                    {
                        if (node.Name != "LiteralExpressionSyntax")
                            codeType.Members.Add(GenerateRedMinimalFactoryMethod(node, true));
                    }
                }

                GenerateKindConverters(codeType, node);
            }

            codeType.Members.Add(GenerateRedTypeList());

            var list = Tree.SyntaxKinds.Where(k => k.Name.EndsWith("Token")).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ");

                string name = list[i].Name;

                if (name.EndsWith("LiteralToken")
                    || name == "IdentifierToken"
                    || name == "SpecialSequenceToken"
                    || name == "NumericLiteralToken"
                    || name == "CharacterLiteralToken"
                    || name == "StringLiteralToken"
                    || name == "BadToken")
                {
                    continue;
                }

                string shortName = name.Replace("Token", "");

                if (i == 0)
                {
                    writer.WriteLine($"public static Microsoft.CodeAnalysis.SyntaxToken {shortName} {{ get; }} = new Microsoft.CodeAnalysis.SyntaxToken(Microsoft.CodeAnalysis.Ebnf.Syntax.InternalSyntax.SyntaxFactory.{shortName});");
                }
                else
                {
                    writer.WriteLine($"        public static Microsoft.CodeAnalysis.SyntaxToken {shortName} {{ get; }} = new Microsoft.CodeAnalysis.SyntaxToken(Microsoft.CodeAnalysis.Ebnf.Syntax.InternalSyntax.SyntaxFactory.{shortName});");
                }
                writer.Write("");
                codeType.Members.Add(new CodeSnippetTypeMember(writer.InnerWriter.ToString()));
            }

            return codeType;
        }

        private CodeMemberMethod GenerateRedFactoryMethod(Node node)
        {
            var nodeFields = GetNodeFields(node);
            var valueFields = GetValueFields(node);

            CodeCommentStatement comment = new CodeCommentStatement(
                $"Creates a new <see cref=\"{node.Name}\"/> node.", true);

            CodeMemberMethod method = new CodeMemberMethod();
            method.Comments.Add(comment);
            method.Name = StripSyntax(node.Name);
            method.ReturnType = CreateType(node.Name);
            method.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            if (node.Kinds.Count > 1)
            {
                method.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
            }
            foreach (Field field in node.Fields)
            {
                method.Parameters.Add(GenerateParameter(GetRedPropertyType(field), ParameterName(field)));
            }
            GenerateNodeFieldValidation(method, nodeFields);
            List<CodeExpression> arguments = new List<CodeExpression>();
            if (node.Kinds.Count > 1)
            {
                arguments.Add(VariableReference("kind"));
            }
            foreach (Field field in nodeFields)
            {
                string expression = null;
                if (field.Type == "SyntaxToken")
                {
                    expression = $"({Tree.InternalNamespace}.SyntaxToken){ParameterName(field)}.Node";
                }
                else if (field.Type == "SyntaxList<SyntaxToken>")
                {
                    expression = $"{ParameterName(field)}.Node.ToGreenList<{Tree.InternalNamespace}.SyntaxToken>()";
                }
                else if (IsNodeList(field.Type))
                {
                    expression = $"{ParameterName(field)}.Node.ToGreenList<{Tree.InternalNamespace}.{GetElementType(field.Type)}>()";
                }
                else if (IsSeparatedNodeList(field.Type))
                {
                    expression = $"{ParameterName(field)}.Node.ToGreenSeparatedList<{Tree.InternalNamespace}.{GetElementType(field.Type)}>()";
                }
                else if (field.Type == "SyntaxNodeOrTokenList")
                {
                    expression = $"{ParameterName(field)}.Node.ToGreenList<{Tree.InternalNamespace}.{Tree.LanguageName}SyntaxNode>()";
                }
                else
                {
                    expression =
                            $"({Tree.InternalNamespace}.{field.Type}){ParameterName(field)}.Green";
                }

                arguments.Add(VariableReference(expression));
            }

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement();
            CodeCastExpression castExpression = new CodeCastExpression
            {
                TargetType = CreateType(node.Name),
                Expression =
                    new CodeMethodInvokeExpression(

                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            VariableReference($"{Tree.InternalNamespace}.SyntaxFactory"), StripSyntax(node.Name)),
                            arguments.ToArray()
                            ), "CreateRed")
            };
            returnStatement.Expression = castExpression;

            method.Statements.Add(returnStatement);

            return method;
        }

        private void GenerateNodeFieldValidation(CodeMemberMethod method, IEnumerable<Field> nodeFields)
        {
            foreach (Field field in nodeFields)
            {
                GenerateParameterValidation(method, field, false);
            }
        }

        private CodeMemberMethod GenerateRedFactoryMethodWithNoAutoCreatableTokens(Node node)
        {
            CodeCommentStatement comment = new CodeCommentStatement(
                $"Creates a new <see cref=\"{node.Name}\"/> node.", true);

            var nodeFields = GetNodeFields(node);
            var valueFields = GetValueFields(node);
            var factoryWithNoAutoCreatableTokenFields = new HashSet<Field>(DetermineRedFactoryWithNoAutoCreatableTokenFields(node));
            var minimalFactoryFields = DetermineMinimalFactoryFields(node);

            CodeMemberMethod method = new CodeMemberMethod();
            method.Comments.Add(comment);
            method.Name = StripSyntax(node.Name);
            method.ReturnType = CreateType(node.Name);
            method.Attributes = MemberAttributes.Public | MemberAttributes.Static;

            if (node.Name == "LiteralExpressionSyntax")
            {
            }

            if (node.Kinds.Count > 1)
            {
                method.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
            }
            foreach (Field field in node.Fields)
            {
                if (factoryWithNoAutoCreatableTokenFields.Contains(field))
                {
                    if (IsNodeList(field.Type) || IsSeparatedSyntaxList(field))
                    {
                        method.Parameters.Add(new CodeParameterDeclarationExpression($"params {GetElementType(field.Type)}[]", ParameterName(field)));
                    }
                    else if (IsTokenList(field))
                    {
                        method.Parameters.Add(new CodeParameterDeclarationExpression($"params SyntaxToken[]", ParameterName(field)));
                    }
                    else
                    {
                        method.Parameters.Add(GenerateParameter(GetRedPropertyType(field), ParameterName(field)));
                    }
                }
            }
            List<CodeExpression> arguments = new List<CodeExpression>();
            if (node.Kinds.Count > 1)
            {
                arguments.Add(VariableReference("kind"));
            }

            foreach (Field field in node.Fields)
            {
                if (factoryWithNoAutoCreatableTokenFields.Contains(field))
                {
                    if (IsTokenList(field))
                    {
                        arguments.Add(CodeDomFactory.ArgumentReferenceExpression(
                            $"SyntaxFactory.TokenList({ParameterName(field)})"));
                    }
                    else if (IsSeparatedSyntaxList(field))
                    {
                        arguments.Add(CodeDomFactory.ArgumentReferenceExpression(
                            $"SyntaxFactory.SeparatedList({ParameterName(field)})"));
                    }
                    else if (IsSyntaxList(field))
                    {
                        arguments.Add(CodeDomFactory.ArgumentReferenceExpression(
                            $"SyntaxFactory.List({ParameterName(field)})"));
                    }
                    else
                    {
                        arguments.Add(VariableReference(ParameterName(field)));
                    }
                }
                else
                {
                    arguments.Add(VariableReference(GetDefaultValue(node, field)));
                }
            }
            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement
            {
                Expression = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        VariableReference($"SyntaxFactory"),
                        StripSyntax(node.Name)),
                    arguments.ToArray()
                )
            };

            method.Statements.Add(returnStatement);
            return method;
        }

        private CodeMemberMethod GenerateRedMinimalFactoryMethod(Node node, bool stringNames = false)
        {
            var nodeFields = GetNodeFields(node);
            var valueFields = GetValueFields(node);

            var minimalFactoryfields = new HashSet<Field>(DetermineMinimalFactoryFields(node));

            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = StripSyntax(node.Name);
            method.ReturnType = CreateType(node.Name);
            method.Attributes = MemberAttributes.Public | MemberAttributes.Static;

            if (node.Kinds.Count > 1)
            {
                method.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
            }

            foreach (Field field in node.Fields)
            {
                if (minimalFactoryfields.Contains(field))
                {
                    var type = GetRedPropertyType(field);

                    if (IsRequiredFactoryField(node, field))
                    {
                        if (stringNames && CanAutoConvertFromString(field))
                        {
                            type = "string";
                        }

                        method.Parameters.Add(GenerateParameter(type, ParameterName(field)));
                    }
                    else
                    {
                        method.Parameters.Add(GenerateParameter(type, $"{ParameterName(field)} = default({field.Type})"));
                    }
                }
                else
                {
                    if (node.Name == "IdentifierNameSyntax")
                    {
                        method.Parameters.Add(GenerateParameter("string", ParameterName(field)));
                    }
                }
            }
            List<CodeExpression> arguments = new List<CodeExpression>();
            if (node.Kinds.Count > 1)
            {
                arguments.Add(VariableReference("kind"));
            }

            foreach (Field field in node.Fields)
            {
                if (minimalFactoryfields.Contains(field))
                {
                    if (IsRequiredFactoryField(node, field))
                    {
                        if (stringNames && CanAutoConvertFromString(field))
                        {
                            string expr = GetStringConverterMethod(field);
                            arguments.Add(VariableReference($"{expr}({ParameterName(field)})"));
                        }
                        else
                        {
                            arguments.Add(VariableReference(ParameterName(field)));
                        }
                    }
                    else
                    {
                        if (IsOptional(field) || IsAnyList(field.Type))
                        {
                            arguments.Add(VariableReference(ParameterName(field)));
                        }
                        else
                        {
                            arguments.Add(VariableReference($"{ParameterName(field)} ?? {GetDefaultValue(node, field)}"));
                        }
                    }
                }
                else
                {
                    if (node.Name == "IdentifierNameSyntax")
                    {
                        string expr = GetStringConverterMethod(field);
                        arguments.Add(VariableReference($"{expr}({ParameterName(field)})"));
                    }
                    else
                    {
                        arguments.Add(VariableReference(GetDefaultValue(node, field)));
                    }
                }
            }

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement
            {
                Expression = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        VariableReference($"SyntaxFactory"),
                        StripSyntax(node.Name)),
                    arguments.ToArray()
                )
            };

            method.Statements.Add(returnStatement);
            return method;
        }

        private void GenerateKindConverters(CodeTypeDeclaration codeType, Node nd)
        {
            foreach (Field field in nd.Fields)
            {
                if (field.Type == "SyntaxToken" && CanBeAutoCreated(nd, field) && field.Kinds.Count > 1)
                {
                    CodeMemberMethod method = new CodeMemberMethod();
                    method.Name = $"Get{StripSyntax(nd.Name)}{field.Name}Kind";
                    method.Attributes = MemberAttributes.Private | MemberAttributes.Static;
                    method.ReturnType = CreateType("SyntaxKind");
                    method.Statements.Add(GenerateConverterKindsSwitchStatement(nd, field));
                    method.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
                    codeType.Members.Add(method);
                }
            }
        }

        private CodeStatement GenerateConverterKindsSwitchStatement(Node nd, Field field)
        {
            IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ");
            writer.WriteLine("switch(kind)");
            writer.Indent++;
            writer.Indent++;
            writer.Indent++;
            writer.Indent++;
            writer.WriteLine("{");

            for (int k = 0; k < field.Kinds.Count; k++)
            {
                var nKind = nd.Kinds[k];
                var pKind = field.Kinds[k];
                writer.WriteLine($"case SyntaxKind.{nKind.Name}: return SyntaxKind.{pKind.Name};");
            }
            writer.WriteLine("default: throw new ArgumentException(\"kind\");");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.Write("");
            return new CodeSnippetStatement(writer.InnerWriter.ToString());
        }

        private CodeMemberMethod GenerateRedTypeList()
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = "GetNodeTypes",
                Attributes = MemberAttributes.FamilyAndAssembly | MemberAttributes.Static,
                ReturnType = CreateType("Type[]")
            };
            List<CodeExpression> list = new List<CodeExpression>();
            foreach (Node node in Tree.Types.OfType<Node>())
            {
                list.Add(new CodeTypeOfExpression(CreateType(node.Name)));
            }

            CodeMethodReturnStatement statement = ReturnArrayCreateExpression("Type", list);
            method.Statements.Add(statement);
            return method;
        }
    }
}


