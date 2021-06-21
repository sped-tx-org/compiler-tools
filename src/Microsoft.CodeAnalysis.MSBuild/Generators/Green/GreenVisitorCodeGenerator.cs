// -----------------------------------------------------------------------
// <copyright file="GreenVisitorCodeGenerator.cs" company="Ollon, LLC">
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
    public class GreenVisitorCodeGenerator : AbstractCodeGenerator, IGreenVisitorCodeGenerator
    {
        public GreenVisitorCodeGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
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
            visitor.TypeAttributes = TypeAttributes.NotPublic;
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
            visitor.TypeAttributes = TypeAttributes.NotPublic;
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

#if false
        private void WriteGreenVisitor(bool withArgument, bool withResult)
        {
            var nodes = Tree.Types.Where(n => !(n is PredefinedNode)).ToList();

            WriteLine();
            WriteLine("  internal partial class CSharpSyntaxVisitor" + (withResult ? "<" + (withArgument ? "TArgument, " : "") + "TResult>" : ""));
            WriteLine("  {");
            int nWritten = 0;
            for (int i = 0, n = nodes.Count; i < n; i++)
            {
                if (nodes[i] is Node node)
                {
                    if (nWritten > 0)
                        WriteLine();
                    nWritten++;
                    WriteLine("    public virtual " + (withResult ? "TResult" : "void") + " Visit{0}({1} node{2})", StripPost(node.Name, "Syntax"), node.Name, withArgument ? ", TArgument argument" : "");
                    WriteLine("    {");
                    WriteLine("      " + (withResult ? "return " : "") + "this.DefaultVisit(node{0});", withArgument ? ", argument" : "");
                    WriteLine("    }");
                }
            }
            WriteLine("  }");
        }
#endif
    }
}


