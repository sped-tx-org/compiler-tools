using System.CodeDom;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Factories;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Syntax
{
    internal class SyntaxFactsCodeGenerator : AbstractCodeGenerator, ISyntaxFactsCodeGenerator
    {
        public SyntaxFactsCodeGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }
        public CodeCompileUnit GenerateSyntaxFacts()
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(GenerateMainNamespace());
            return unit;
        }

        private CodeNamespace GenerateMainNamespace()
        {
            CodeNamespace ns = new CodeNamespace(Tree.MainNamespace);

            ns.Types.Add(GenerateSyntaxFactsClass());

            return ns;
        }

        private CodeTypeDeclaration GenerateSyntaxFactsClass()
        {
            CodeTypeDeclaration ct = new CodeTypeDeclaration("SyntaxFacts")
            {
                IsPartial = true,
                IsClass = true,
                Attributes = MemberAttributes.Static,
                TypeAttributes = TypeAttributes.NotPublic
            };

            ct.Members.Add(GenerateIsAutomaticTokenMethod());
            ct.Members.Add(GenerateIsAnyTokenMethod());
            ct.Members.Add(GenerateGetTextMethod());
            ct.Members.Add(GenerateGetKindMethod());

            return ct;
        }

        private CodeMemberMethod GenerateGetKindMethod()
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = "GetKind",
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                ReturnType = CreateType("SyntaxKind")
            };
            method.Parameters.Add(GenerateParameter("string", "text"));

            method.Statements.Add(GenerateGetKindSwitchStatement());

            return method;
        }

        private CodeMemberMethod GenerateIsAutomaticTokenMethod()
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = "IsAutomaticToken",
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                ReturnType = CreateType("bool")
            };
            method.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
            method.Statements.Add(ReturnStatement(
                new CodeBinaryOperatorExpression(
                    new CodeMethodInvokeExpression(VariableReference("SyntaxFacts"), "GetText", VariableReference("kind")),
                    CodeBinaryOperatorType.IdentityInequality,
                    new CodePrimitiveExpression(string.Empty))));
            return method;
        }

        private CodeMemberMethod GenerateGetTextMethod()
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = "GetText",
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                ReturnType = CreateType("string")
            };
            method.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));

            method.Statements.Add(GenerateGetTextSwitchStatement());

            return method;
        }

        private CodeStatement GenerateGetKindSwitchStatement()
        {
            CodeSwitchStatement statement = new CodeSwitchStatement
            {
                CheckExpression = new CodeVariableReferenceExpression("text")
            };
            List<SyntaxKind> kinds = Tree.SyntaxKinds.Where(k => k.Category == Category.Token && !string.IsNullOrEmpty(k.Value)).ToList();
            for (int index = 0; index < kinds.Count; index++)
            {
                SyntaxKind kind = kinds[index];
                CodeReturnValueSwitchSectionStatement section =
                    new CodeReturnValueSwitchSectionStatement
                    {
                        SingleLine = true,
                        Label = new CodeSwitchSectionLabelExpression(new CodePrimitiveExpression(kind.Value)),
                        ReturnStatement = ReturnStatement(new CodeVariableReferenceExpression($"SyntaxKind.{kind.Name}"))
                    };
                statement.Sections.Add(section);
            }
            statement.Sections.Add(new CodeDefaultReturnSwitchSectionStatement
            {
                ReturnStatement = ReturnStatement(VariableReference("SyntaxKind.None"))
            });
            return statement;
        }

        private CodeStatement GenerateGetTextSwitchStatement()
        {
            CodeSwitchStatement statement = new CodeSwitchStatement
            {
                CheckExpression = new CodeVariableReferenceExpression("kind")
            };
            List<SyntaxKind> kinds = Tree.SyntaxKinds.Where(k => k.Category == Category.Token && !string.IsNullOrEmpty(k.Value)).ToList();
            for (int index = 0; index < kinds.Count; index++)
            {
                SyntaxKind kind = kinds[index];
                CodeReturnValueSwitchSectionStatement section =
                    new CodeReturnValueSwitchSectionStatement
                    {
                        SingleLine = true,
                        Label = new CodeSwitchSectionLabelExpression(new CodeVariableReferenceExpression($"SyntaxKind.{kind.Name}")),
                        ReturnStatement = ReturnStatement(new CodePrimitiveExpression(kind.Value))
                    };
                statement.Sections.Add(section);
            }
            statement.Sections.Add(new CodeDefaultReturnSwitchSectionStatement
            {
                ReturnStatement = ReturnStatement(new CodePrimitiveExpression(string.Empty))
            });

            return statement;
        }

        private CodeMemberMethod GenerateIsAnyTokenMethod()
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = "IsAnyToken",
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                ReturnType = CreateType("bool")
            };
            method.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));

            method.Statements.Add(GenerateIsAnyTokenSwitchKindStatement());



            return method;
        }

        private CodeStatement GenerateIsAnyTokenSwitchKindStatement()
        {
            CodeSwitchStatement statement = new CodeSwitchStatement();

            List<SyntaxKind> kinds = Tree.SyntaxKinds.Where(k => k.Category == Category.Token).ToList();

            statement.CheckExpression = new CodeVariableReferenceExpression("kind");
            for (int index = 0; index < kinds.Count; index++)
            {
                SyntaxKind kind = kinds[index];
                if (index != kinds.Count - 1)
                {
                    CodeFallThroughSwitchSectionStatement section =
                        new CodeFallThroughSwitchSectionStatement
                        {
                            Label = new CodeSwitchSectionLabelExpression(new CodeVariableReferenceExpression($"SyntaxKind.{kind.Name}")),
                        };
                    statement.Sections.Add(section);
                }
            }

            SyntaxKind lastkind = kinds[kinds.Count - 1];
            CodeReturnValueSwitchSectionStatement lastSection =
                new CodeReturnValueSwitchSectionStatement
                {
                    Label = new CodeSwitchSectionLabelExpression(new CodeVariableReferenceExpression($"SyntaxKind.{lastkind.Name}")),
                    ReturnStatement = ReturnTrue()
                };

            statement.Sections.Add(lastSection);

            statement.Sections.Add(
                new CodeDefaultReturnSwitchSectionStatement
                {
                    ReturnStatement = ReturnFalse()
                });

            return statement;
        }
    }
}





