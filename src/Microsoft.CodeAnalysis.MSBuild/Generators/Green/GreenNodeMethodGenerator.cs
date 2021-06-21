using System.CodeDom;
using System.CodeDom.Compiler;

using System.IO;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Green
{
    public class GreenNodeMethodGenerator : AbstractCodeGenerator, IGreenNodeMethodGenerator
    {
        public GreenNodeMethodGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeTypeMemberCollection GenerateAbstractNodeMethods(AbstractNode node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();

            return collection;
        }

        public CodeTypeMemberCollection GenerateNodeMethods(Node node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();

            collection.Add(GenerateGetSlotMethod(node));
            collection.Add(GenerateCreateRedMethod(node));
            collection.Add(GenerateGreenAcceptMethod(node, false));
            collection.Add(GenerateGreenAcceptMethod(node, true));
            collection.Add(GenerateGreenUpdateMethod(node));
            collection.Add(GenerateSetMethod(node, "Annotation"));
            collection.Add(GenerateSetMethod(node, "Diagnostic"));
            return collection;
        }

        private CodeMemberMethod GenerateGreenUpdateMethod(Node nd)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public;
            method.Name = "Update";
            method.ReturnType = CreateType(nd.Name);

            var nodeFields = GetNodeFields(nd);
            var valueFields = GetValueFields(nd);

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
                    var type =
                        field.Type == "SyntaxNodeOrTokenList" ? $"Microsoft.CodeAnalysis.Syntax.InternalSyntax.SyntaxList<{Tree.LanguageName}SyntaxNode>" :
                            field.Type == "SyntaxTokenList" ? "Microsoft.CodeAnalysis.Syntax.InternalSyntax.SyntaxList<SyntaxToken>" :
                                IsNodeList(field.Type) ? "Microsoft.CodeAnalysis.Syntax.InternalSyntax." + field.Type :
                                    IsSeparatedNodeList(field.Type) ? "Microsoft.CodeAnalysis.Syntax.InternalSyntax." + field.Type :
                                        field.Type;

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
                        writer.Write("Kind, ");
                    }
                    for (int f = 0; f < nd.Fields.Count; f++)
                    {
                        var field = nd.Fields[f];
                        if (f > 0)
                            writer.Write(", ");
                        writer.Write(ParameterName(field));
                    }
                    writer.WriteLine(");");
                    writer.WriteLine("var diagnostics = GetDiagnostics();");
                    writer.WriteLine("if (diagnostics != null && diagnostics.Length > 0)");
                    writer.Indent++;
                    writer.WriteLine("newNode = newNode.WithDiagnosticsGreen(diagnostics);");
                    writer.Indent--;
                    writer.WriteLine("var annotations = GetAnnotations();");
                    writer.WriteLine("if (annotations != null && annotations.Length > 0)");
                    writer.Indent++;
                    writer.WriteLine("newNode = newNode.WithAnnotationsGreen(annotations);");
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

        private CodeMemberMethod GenerateSetMethod(RealNode nd, string type)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.FamilyAndAssembly | MemberAttributes.Override;
            method.ReturnType = CreateType("GreenNode");
            method.Name = $"Set{type}s";

            string adjType = type == "Annotation" ? "SyntaxAnnotation" : "DiagnosticInfo";

            method.Parameters.Add(GenerateParameter(adjType + "[]", type.ToLower() + "s"));
            CodeObjectCreateExpression objCreate = new CodeObjectCreateExpression(nd.Name,
                new CodeVariableReferenceExpression("Kind"));
            for (int f = 0; f < nd.Fields.Count; f++)
            {
                var field = nd.Fields[f];
                objCreate.Parameters.Add(VariableReference(FieldName(field)));
            }
            if (type == "Annotation")
            {
                objCreate.Parameters.Add(VariableReference($"GetDiagnostics()"));
                objCreate.Parameters.Add(VariableReference("annotations"));
            }
            else
            {
                objCreate.Parameters.Add(VariableReference($"diagnostics"));
                objCreate.Parameters.Add(VariableReference("GetAnnotations()"));
            }

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(objCreate);
            method.Statements.Add(returnStatement);

            return method;
        }

        private CodeMemberMethod GenerateGreenAcceptMethod(RealNode nd, bool generic)
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
                    $"Visit{StripPost(nd.Name, "Syntax")}"
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
                        $"visitor.Visit{StripPost(nd.Name, "Syntax")}(this)")
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
                            $"visitor.Visit{StripPost(nd.Name, "Syntax")}(this)")));
            }

            return method;
        }

        private CodeMemberMethod GenerateGetSlotMethod(Node node)
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = "GetSlot",
                ReturnType = CreateType("GreenNode"),
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
            for (int i = 0; i < node.Fields.Count; i++)
            {
                Field f = node.Fields[i];
                if (IsNodeOrNodeList(f.Type))
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

        private CodeMemberMethod GenerateCreateRedMethod(Node node)
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = "CreateRed",
                ReturnType = CreateType($"SyntaxNode"),
                Attributes = MemberAttributes.Assembly | MemberAttributes.Override,
                Parameters =
                {
                    GenerateParameter("SyntaxNode", "parent"),
                    GenerateParameter("int", "position")
                },
                Statements =
                {
                    new CodeMethodReturnStatement
                    {
                        Expression = new CodeObjectCreateExpression
                        {
                            CreateType = CreateType($"{Tree.SyntaxNamespace}.{node.Name}"),
                            Parameters =
                            {
                                new CodeThisReferenceExpression(),
                                VariableReference("parent"),
                                VariableReference("position")
                            }
                        }
                    }
                }
            };

            return method;
        }
    }
}


