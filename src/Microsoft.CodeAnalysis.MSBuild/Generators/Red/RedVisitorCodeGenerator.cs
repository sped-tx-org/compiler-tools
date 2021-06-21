// -----------------------------------------------------------------------
// <copyright file="RedVisitorCodeGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    public class RedVisitorCodeGenerator : AbstractCodeGenerator, IRedVisitorCodeGenerator
    {
        public RedVisitorCodeGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeTypeDeclaration GenerateVisitor(bool withArgument, bool withResult)
        {
            if (!withArgument && !withResult)
            {
                return GenerateVisitorNoArgumentNoResult();
            }

            if (!withArgument && withResult)
            {
                return GenerateVisitorNoArgumentWithResult();
            }

            return new CodeTypeDeclaration();
        }

        private CodeTypeDeclaration GenerateVisitorNoArgumentWithResult()
        {
            List<TreeType> nodes = GetNodes();
            CodeTypeDeclaration visitor = new CodeTypeDeclaration($"{Tree.LanguageName}SyntaxVisitor<TResult>");
            visitor.IsPartial = true;
            visitor.IsClass = true;
            visitor.TypeAttributes = TypeAttributes.Public;
            for (int i = 0, n = nodes.Count; i < n; i++)
            {
                if (nodes[i] is Node node)
                {
                    CodeMemberMethod method = new CodeMemberMethod();
                    method.Name = $"Visit{StripPost(node.Name, "Syntax")}";
                    method.ReturnType = CreateType("TResult");
                    method.Parameters.Add(GenerateParameter($"{node.Name}", "node"));
                    method.Attributes = MemberAttributes.Public;
                    method.UserData["IsVirtual"] = true;
                    method.Statements.Add(new CodeMethodReturnStatement(
                        new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression(), "DefaultVisit", VariableReference("node"))));
                    visitor.Members.Add(method);
                }
            }

            return visitor;
        }

        private CodeTypeDeclaration GenerateVisitorNoArgumentNoResult()
        {
            List<TreeType> nodes = GetNodes();
            CodeTypeDeclaration visitor = new CodeTypeDeclaration($"{Tree.LanguageName}SyntaxVisitor");
            visitor.IsPartial = true;
            visitor.IsClass = true;
            visitor.TypeAttributes = TypeAttributes.Public;
            for (int i = 0, n = nodes.Count; i < n; i++)
            {
                if (nodes[i] is Node node)
                {
                    CodeMemberMethod method = new CodeMemberMethod();
                    method.Name = $"Visit{StripPost(node.Name, "Syntax")}";
                    method.ReturnType = CreateType("void");
                    method.Parameters.Add(GenerateParameter(node.Name, "node"));
                    method.Attributes = MemberAttributes.Public;
                    method.UserData["IsVirtual"] = true;
                    method.Statements.Add(new CodeExpressionStatement(
                        new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression(), "DefaultVisit", VariableReference("node"))));
                    visitor.Members.Add(method);
                }
            }

            return visitor;
        }

        private List<TreeType> GetNodes()
        {
            return Tree.Types.Where(n => !(n is PredefinedNode)).ToList();
        }
    }
}


