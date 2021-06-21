using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Green
{
    internal class GreenFactoryCodeGenerator : AbstractCodeGenerator, IGreenFactoryCodeGenerator
    {
        public GreenFactoryCodeGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeCompileUnit GenerateFactory()
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            CodeNamespace ns = GenerateGreenNodeNamespace();
            unit.Namespaces.Add(ns);
            ns.Imports.AddRange(GetUsings());
            return unit;
        }

        private CodeNamespace GenerateGreenNodeNamespace()
        {
            CodeNamespace ns = new CodeNamespace(Tree.InternalNamespace);
            ns.Types.Add(GenerateGreenFactory());
            return ns;
        }

        private CodeTypeDeclaration GenerateGreenFactory()
        {
            CodeTypeDeclaration codeType = new CodeTypeDeclaration("SyntaxFactory");
            codeType.IsPartial = true;
            codeType.Attributes = MemberAttributes.Static;
            codeType.TypeAttributes = TypeAttributes.NotPublic;
            codeType.IsClass = true;

            foreach (Node node in Tree.Types.OfType<Node>())
            {
                var nodeFields = GetNodeFields(node);
                var valueFields = GetValueFields(node);

                codeType.Members.Add(GenerateGreenFactoryMethod(node, nodeFields, valueFields));
            }

            codeType.Members.Add(GenerateGreenTypeList());

            var list = Tree.SyntaxKinds.Where(k => k.Name.EndsWith("Token")).ToList();
            for (int i = 0; i < list.Count; i++)
            {
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


                IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ");

                string shortName = name.Replace("Token", "");

                if (i == 0)
                {
                    writer.WriteLine($"internal static SyntaxToken {shortName} {{ get; }} = Token(SyntaxKind.{name});");
                }
                else
                {
                    writer.WriteLine($"        internal static SyntaxToken {shortName} {{ get; }} = Token(SyntaxKind.{name});");
                }

                writer.Write("");

                codeType.Members.Add(new CodeSnippetTypeMember(writer.InnerWriter.ToString()));
            }

            return codeType;
        }

        private CodeMemberMethod GenerateGreenFactoryMethod(Node nd, List<Field> nodeFields, List<Field> valueFields)
        {
            CodeCommentStatement comment = new CodeCommentStatement(
                $"Creates a new <see cref=\"{nd.Name}\"/> node.", true);
            CodeMemberMethod method = new CodeMemberMethod();
            method.Comments.Add(comment);
            method.Name = StripSyntax(nd.Name);
            method.ReturnType = CreateType(nd.Name);
            method.Attributes = MemberAttributes.Public | MemberAttributes.Static;

            if (nd.Kinds.Count > 1)
            {
                method.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
                method.Statements.Add(GenerateKindsSwitchStatement(nd));
            }
            foreach (Field field in nd.Fields)
            {
                method.Parameters.Add(GenerateGreenParameter(field));
            }

            foreach (Field field in nodeFields)
            {
                GenerateParameterValidation(method, field, true);
            }

            method.Statements.Add(
                ReturnObjectCreateExpresion(nd.Name, GenerateCtorArgList(nd, valueFields, nodeFields)));

            return method;
        }

        private CodeStatement GenerateKindsSwitchStatement(Node nd)
        {
            IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ") { Indent = 3 };
            writer.WriteLine("switch(kind)");
            writer.WriteLine("{");
            writer.Indent = 4;
            foreach (var k in nd.Kinds)
            {
                writer.WriteLine($"case SyntaxKind.{k.Name}:");
            }
            writer.Indent = 5;
            writer.WriteLine("break;");
            writer.Indent = 4;
            writer.WriteLine("default:");
            writer.Indent = 5;
            writer.WriteLine("throw new ArgumentException(\"kind\");");
            writer.Indent = 3;
            writer.WriteLine("}");
            writer.Indent = 3;
            writer.Write("");
            return new CodeSnippetStatement(writer.InnerWriter.ToString());
        }

        private CodeExpression[] GenerateCtorArgList(Node nd, List<Field> valueFields, List<Field> nodeFields)
        {
            List<CodeExpression> list = new List<CodeExpression>();
            if (nd.Kinds.Count == 1)
            {
                list.Add(VariableReference($"SyntaxKind.{nd.Kinds[0].Name}"));
            }
            else
            {
                list.Add(VariableReference("kind"));
            }
            foreach (Field field in nodeFields)
            {
                if (field.Type == "SyntaxList<SyntaxToken>" || IsAnyList(field.Type))
                {
                    list.Add(VariableReference($"{ParameterName(field)}.Node"));
                }
                else
                {
                    list.Add(VariableReference(ParameterName(field)));
                }
            }
            foreach (Field field in valueFields)
            {
                list.Add(VariableReference(ParameterName(field)));
            }

            return list.ToArray();
        }

        private CodeParameterDeclarationExpression GenerateGreenParameter(Field field)
        {
            string type = field.Type;
            if (type == "SyntaxNodeOrTokenList")
            {
                type = $"Microsoft.CodeAnalysis.Syntax.InternalSyntax.SyntaxList<{Tree.LanguageName}SyntaxNode>";
            }
            else if (IsSeparatedNodeList(field.Type)
                     || IsNodeList(field.Type))
            {
                type = "Microsoft.CodeAnalysis.Syntax.InternalSyntax." + type;
            }
            return GenerateParameter(type, ParameterName(field));
        }

        private CodeMemberMethod GenerateGreenTypeList()
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


