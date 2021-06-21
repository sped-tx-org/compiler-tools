// -----------------------------------------------------------------------
// <copyright file="RedRewriterCodeGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    public class RedRewriterCodeGenerator : AbstractCodeGenerator, IRedRewriterCodeGenerator
    {
        public RedRewriterCodeGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeCompileUnit GenerateRewriter()
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            CodeNamespace ns = GenerateRedNodeNamespace();
            unit.Namespaces.Add(ns);
            ns.Imports.AddRange(GetUsings());
            return unit;
        }

        private CodeNamespace GenerateRedNodeNamespace()
        {
            CodeNamespace ns = new CodeNamespace(Tree.MainNamespace);
            ns.Types.Add(GenerateRedRewriter());
            ns.Imports.Add(new CodeNamespaceImport(Tree.SyntaxNamespace));
            return ns;
        }

        private CodeTypeDeclaration GenerateRedRewriter()
        {
            CodeTypeDeclaration rewriter = new CodeTypeDeclaration($"{Tree.LanguageName}SyntaxRewriter")
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Public
            };
            rewriter.BaseTypes.Add(new CodeTypeReference($"{Tree.LanguageName}SyntaxVisitor<SyntaxNode>"));
            foreach (Node node in Tree.Types.OfType<Node>())
            {
                List<Field> nodeFields = node.Fields.Where(nd => IsNodeOrNodeList(nd.Type)).ToList();

                CodeCommentStatement comment = new CodeCommentStatement($"Called when the visitor visits a <see cref=\"{node.Name}\"/> syntax node.", true);
                CodeMemberMethod method = new CodeMemberMethod();
                method.Comments.Add(comment);
                method.Name = $"Visit{StripPost(node.Name, "Syntax")}";
                method.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                method.ReturnType = CreateType("SyntaxNode");
                method.Parameters.Add(GenerateParameter(node.Name, "node"));
                foreach (var field in nodeFields)
                {
                    var fieldName = CodeIdentifier.MakeValid(field.Name);

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
                            new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                "VisitToken",
                                VariableReference($"node.{field.Name}"))));
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



