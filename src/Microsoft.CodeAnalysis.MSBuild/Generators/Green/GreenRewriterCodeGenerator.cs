// -----------------------------------------------------------------------
// <copyright file="GreenRewriterCodeGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Green
{
    internal class GreenRewriterCodeGenerator : AbstractCodeGenerator, IGreenRewriterCodeGenerator
    {
        public GreenRewriterCodeGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeCompileUnit GenerateRewriter()
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
            ns.Types.Add(GenerateGreenRewriter());
            return ns;
        }

        private CodeTypeDeclaration GenerateGreenRewriter()
        {
            CodeTypeDeclaration rewriter = new CodeTypeDeclaration($"{Tree.LanguageName}SyntaxRewriter")
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.NotPublic
            };
            rewriter.BaseTypes.Add(new CodeTypeReference($"{Tree.LanguageName}SyntaxVisitor<EbnfSyntaxNode>"));
            foreach (Node node in Tree.Types.OfType<Node>())
            {
                List<Field> nodeFields = node.Fields.Where(nd => IsNodeOrNodeList(nd.Type)).ToList();
                CodeCommentStatement comment = new CodeCommentStatement($"Called when the visitor visits a <see cref=\"{node.Name}\"/> syntax node.", true);
                CodeMemberMethod method = new CodeMemberMethod
                {
                    Name = $"Visit{StripPost(node.Name, "Syntax")}",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = CreateType("EbnfSyntaxNode")
                };
                method.Parameters.Add(GenerateParameter(node.Name, "node"));
                foreach (var field in nodeFields)
                {
                    if (IsAnyList(field.Type))
                    {
                        method.Statements.Add(new CodeVariableDeclarationStatement("var",
                            ParameterName(field),
                            new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                "VisitList",
                                VariableReference($"node.{field.Name}"))));
                    }
                    else if (field.Type == "SyntaxToken")
                    {
                        method.Statements.Add(new CodeVariableDeclarationStatement("var",
                            ParameterName(field),
                            new CodeCastExpression(
                                "SyntaxToken",
                            new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                "VisitToken",
                                VariableReference($"node.{field.Name}")))));
                    }
                    else
                    {
                        method.Statements.Add(
                            new CodeVariableDeclarationStatement(
                                "var",
                                ParameterName(field),
                                new CodeCastExpression(field.Type,
                                    new CodeMethodInvokeExpression(
                                        new CodeThisReferenceExpression(),
                                        "Visit",
                                        VariableReference($"node.{field.Name}")))
                            ));
                    }
                }
                if (nodeFields.Count > 0)
                {
                    CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement();
                    CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(VariableReference("node"), "Update");
                    returnStatement.Expression = invoke;
                    foreach (var field in node.Fields)
                    {
                        invoke.Parameters.Add(IsNodeOrNodeList(field.Type) ?
                            VariableReference(ParameterName(field)) :
                            VariableReference($"node.{field.Name}"));
                    }
                    method.Statements.Add(returnStatement);
                }
                else
                {
                    method.Statements.Add(new CodeMethodReturnStatement(VariableReference("node")));
                }

                rewriter.Members.Add(method);
            }

            return rewriter;
        }
    }
}


