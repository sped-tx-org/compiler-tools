// -----------------------------------------------------------------------
// <copyright file="RedNodeMethodGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using System.IO;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    public class RedNodeMethodGenerator : AbstractCodeGenerator, IRedNodeMethodGenerator
    {
        public RedNodeMethodGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeTypeMemberCollection GenerateAbstractNodeMethods(AbstractNode node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();

            return collection;
        }

        public CodeTypeMemberCollection GenerateNodeMethods(Node node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection
            {
                GenerateGetNodeSlotMethod(node),
                GenerateGetCachedSlotMethod(node),
                GenerateRedAcceptMethod(node, true),
                GenerateRedAcceptMethod(node, false),
                GenerateRedUpdateMethod(node)
            };

            GenerateRedListHelpers(collection, node);

            for (int f = 0; f < node.Fields.Count; f++)
            {
                var field = node.Fields[f];
                var type = GetRedPropertyType(field);

                CodeMemberMethod method = new CodeMemberMethod();
                method.Attributes = MemberAttributes.Public;
                method.Name = $"With{field.Name}";
                method.ReturnType = CreateType(node.Name);
                method.Parameters.Add(GenerateParameter(type, ParameterName(field)));

                CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement();

                CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(), "Update");

                for (int f2 = 0; f2 < node.Fields.Count; f2++)
                {
                    var field2 = node.Fields[f2];
                    if (field2 == field)
                    {
                        invoke.Parameters.Add(
                            new CodeVariableReferenceExpression(ParameterName(field2)));
                    }
                    else
                    {
                        invoke.Parameters.Add(
                            new CodeVariableReferenceExpression(field2.Name));
                    }
                }

                returnStatement.Expression = invoke;

                method.Statements.Add(returnStatement);

                collection.Add(method);
            }
            return collection;
        }

        private CodeMemberMethod GenerateGetCachedSlotMethod(Node nd)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = "GetCachedSlot";
            method.ReturnType = CreateType("SyntaxNode");
            method.Attributes = MemberAttributes.Assembly | MemberAttributes.Override;

            method.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = "index",
                Type = CreateType("int")
            });

            IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ") { Indent = 3 };
            writer.WriteLine("switch(index)");
            writer.WriteLine("{");
            writer.Indent = 4;
            for (int i = 0; i < nd.Fields.Count; i++)
            {
                Field f = nd.Fields[i];
                if (!IsTokenOrTokenList(f))
                {
                    string fieldName = FieldName(f);

                    writer.WriteLine($"case {i}: return {fieldName};");
                }
            }
            writer.WriteLine("default: return null;");
            writer.Indent = 3;
            writer.WriteLine("}");

            method.Statements.Add(new CodeSnippetStatement(writer.InnerWriter.ToString()));

            return method;
        }

        private CodeMemberMethod GenerateGetNodeSlotMethod(Node nd)
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = "GetNodeSlot",
                ReturnType = CreateType("SyntaxNode"),
                Attributes = MemberAttributes.Assembly | MemberAttributes.Override
            };

            method.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = "index",
                Type = CreateType("int")
            });

            IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ") { Indent = 3 };
            writer.WriteLine("switch(index)");
            writer.WriteLine("{");
            writer.Indent = 4;

            var nodeFields = GetNodeFields(nd);

            for (int i = 0; i < nodeFields.Count; i++)
            {
                var field = nodeFields[i];
                if (field.Type != "SyntaxToken" && field.Type != "SyntaxList<SyntaxToken>")
                {
                    string fieldName = FieldName(field);

                    if (i == 0)
                    {
                        writer.WriteLine($"case {i}: return GetRedAtZero(ref {fieldName});");
                    }
                    else
                    {
                        writer.WriteLine($"case {i}: return GetRed(ref {fieldName}, {i});");
                    }
                }
            }
            writer.WriteLine("default: return null;");
            writer.Indent = 3;
            writer.WriteLine("}");

            method.Statements.Add(new CodeSnippetStatement(writer.InnerWriter.ToString()));

            return method;
        }

        private void GenerateRedListHelpers(CodeTypeMemberCollection collection, Node node)
        {
            for (int f = 0; f < node.Fields.Count; f++)
            {
                var field = node.Fields[f];

                if (IsAnyList(field.Type))
                {
                    // write list helper methods for list properties
                    GenerateRedListHelperMethods(collection, node, field);
                }
                else
                {
                    Node referencedNode = GetNode(field.Type);
                    if (referencedNode != null && (!IsOptional(field) || RequiredFactoryArgumentCount(referencedNode) == 0))
                    {
                        // look for list members...
                        for (int rf = 0; rf < referencedNode.Fields.Count; rf++)
                        {
                            var referencedNodeField = referencedNode.Fields[rf];
                            if (IsAnyList(referencedNodeField.Type))
                            {
                                GenerateRedNestedListHelperMethods(collection, node, field, referencedNode, referencedNodeField);
                            }
                        }
                    }
                }
            }
        }

        private void GenerateRedNestedListHelperMethods(CodeTypeMemberCollection collection, Node node, Field field, Node referencedNode, Field referencedNodeField)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public;
            var argType = GetElementType(referencedNodeField.Type);
            method.ReturnType = CreateType(node.Name);
            string newFieldName = $"Add{StripPost(referencedNode.Name, "Syntax")}{field.Name}";
            method.Name = newFieldName;
            CodeParameterDeclarationExpression parameter = GenerateParameter(argType, "items");
            parameter.UserData["IsParams"] = true;
            method.Parameters.Add(parameter);
            if (field.Type == "SkippedTokensTriviaSyntax")
            {
                method.Statements.Add(new CodeMethodReturnStatement(
                        new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), $"With{field.Name}"),
                            new CodeVariableReferenceExpression($"{referencedNodeField.Name}.Add{field.Name}(items)")
                        )
                    )
                );
            }
            else if (IsOptional(field))
            {
                var factoryName = StripPost(referencedNode.Name, "Syntax");
                var varName = FieldName(field);

                method.Statements.Add(new CodeVariableDeclarationStatement("var", varName,
                    new CodeVariableReferenceExpression($"{field.Name} ?? SyntaxFactory.{factoryName}()")));

                method.Statements.Add(new CodeMethodReturnStatement(
                        new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), $"With{field.Name}"),
                            new CodeVariableReferenceExpression($"{varName}.With{referencedNodeField.Name}({varName}.AddRange(items))")
                        )
                    )
                );
            }
            else
            {
                method.Statements.Add(new CodeMethodReturnStatement(
                        new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), $"With{field.Name}"),
                            new CodeVariableReferenceExpression($"{field.Name}.Add{StripSyntax(argType)}s(items)")
                        )
                    )
                );
            }
            collection.Add(method);
        }

        private void GenerateRedListHelperMethods(CodeTypeMemberCollection collection, Node node, Field field)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public;
            var argType = GetElementType(field.Type);
            method.ReturnType = CreateType(node.Name);
            method.Name = $"Add{field.Name}";
            CodeParameterDeclarationExpression parameter = GenerateParameter(argType, "items");
            parameter.UserData["IsParams"] = true;
            method.Parameters.Add(parameter);
            method.Statements.Add(new CodeMethodReturnStatement(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), $"With{field.Name}"),
                    new CodeVariableReferenceExpression($"{field.Name}.AddRange(items)")
                )
            ));
            collection.Add(method);
        }

        private CodeMemberMethod GenerateRedAcceptMethod(RealNode nd, bool generic)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            if (generic)
            {
                method.Name = "Accept";
                method.ReturnType = CreateType("TResult");
                method.TypeParameters.Add(
                    new CodeTypeParameter("TResult"));
                method.Parameters.Add(
                    new CodeParameterDeclarationExpression(
                        CreateType($"{Tree.LanguageName}SyntaxVisitor<TResult>"),
                        "visitor"));
                CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(
                    new CodeVariableReferenceExpression("visitor"),
                    $"Visit{nd.Name.Replace("Syntax", string.Empty)}"
                );
                CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression
                {
                    Method = codeMethodReferenceExpression
                };
                codeMethodInvokeExpression.Parameters.Add(
                    new CodeThisReferenceExpression());
                CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement
                {
                    Expression = codeMethodInvokeExpression
                };

                method.Statements.Add(new CodeMethodReturnStatement
                {
                    Expression = new CodeVariableReferenceExpression(
                        $"visitor.Visit{nd.Name.Replace("Syntax", string.Empty)}(this)")
                });
            }
            else
            {
                method.Name = "Accept";
                method.ReturnType = CreateType("void");
                method.Parameters.Add(
                    new CodeParameterDeclarationExpression(
                        CreateType($"{Tree.LanguageName}SyntaxVisitor"),
                        "visitor"));
                method.Statements.Add(
                    new CodeExpressionStatement(
                        new CodeVariableReferenceExpression(
                            $"visitor.Visit{nd.Name.Replace("Syntax", string.Empty)}(this)")));
            }

            return method;
        }

        private CodeMemberMethod GenerateRedUpdateMethod(Node nd)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public;
            method.Name = "Update";
            method.ReturnType = CreateType(nd.Name);

            var nodeFields = GetNodeFields(nd);
            var valueFields = GetValueFields(nd);

            if (nd.Name == "SingleDefinitionSyntax")
            {
            }

            if (nd.Fields.Count < 1)
            {
                method.Statements.Add(new CodeMethodReturnStatement
                {
                    Expression = new CodeThisReferenceExpression()
                });
            }
            else
            {
                foreach (Field field in nd.Fields)
                {
                    string type = GetRedPropertyType(field);
                    method.Parameters.Add(GenerateParameter(type, ParameterName(field)));
                }

                CodeSnippetStatement statement = new CodeSnippetStatement();
                IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ")
                {
                    Indent = 3
                };

                writer.Write("if (");
                int nCompared = 0;
                foreach (Field field in nd.Fields)
                {
                    if (nCompared > 0)
                        writer.Write(" || ");
                    writer.Write($"{ParameterName(field)} != {field.Name}");
                    nCompared++;
                }
                writer.Write(")");
                if (nCompared > 0)
                {
                    writer.WriteLine();
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.Write("var newNode = SyntaxFactory.{0}(", StripSyntax(nd.Name));
                    if (nd.Kinds.Count > 1)
                    {
                        writer.Write("this.Kind(), ");
                    }
                    for (int f = 0; f < nd.Fields.Count; f++)
                    {
                        var field = nd.Fields[f];
                        if (f > 0)
                            writer.Write(", ");
                        writer.Write(ParameterName(field));
                    }
                    writer.WriteLine(");");
                    writer.WriteLine("var annotations = GetAnnotations();");
                    writer.WriteLine("if (annotations != null && annotations.Length > 0)");
                    writer.Indent++;
                    writer.WriteLine("newNode = newNode.WithAnnotations(annotations);");
                    writer.Indent--;
                    writer.WriteLine("return newNode;");

                    writer.Indent--;
                    writer.WriteLine("}");
                }
                writer.WriteLine();
                writer.WriteLine("return this;");

                method.Statements.Add(new CodeSnippetStatement(writer.InnerWriter.ToString()));
            }

            return method;
        }
    }
}


