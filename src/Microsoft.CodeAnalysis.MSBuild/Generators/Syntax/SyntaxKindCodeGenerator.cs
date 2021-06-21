using System.CodeDom;
using System.Collections.Generic;

using System.Linq;
using Microsoft.CodeAnalysis.MSBuild.Factories;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Syntax
{  
    internal class SyntaxKindCodeGenerator : AbstractCodeGenerator, ISyntaxKindCodeGenerator
    {       
        public SyntaxKindCodeGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeCompileUnit GenerateSyntaxKind()
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(GenerateMainNamespace());
            return unit;
        }

        private CodeNamespace GenerateMainNamespace()
        {
            CodeNamespace ns = new CodeNamespace(Tree.MainNamespace);

            ns.Types.Add(GenerateSyntaxKindDeclaration());

            return ns;
        }

        private CodeTypeDeclaration GenerateSyntaxKindDeclaration()
        {
            CodeCommentStatementCollection comment = new CodeCommentStatementCollection
            {
                new CodeCommentStatement($"Represents the various kinds of {Tree.LanguageName} syntax nodes.", true),
            };
            CodeTypeDeclaration codeType = new CodeTypeDeclaration();
            codeType.Comments.AddRange(comment);
            codeType.Name = "SyntaxKind";
            codeType.IsEnum = true;

            HashSet<SyntaxKind> hashSet = new HashSet<SyntaxKind>();
            foreach (SyntaxKind kind in Tree.SyntaxKinds)
            {
                hashSet.Add(kind);
            }

            foreach (Node node in Tree.Types.OfType<Node>())
            {
                if (node.Kinds.Count > 1)
                {
                    foreach (Kind kind in node.Kinds)
                    {
                        SyntaxKind newKind = new SyntaxKind
                        {
                            Category = Category.Syntax,
                            Name = kind.Name
                        };
                        hashSet.Add(newKind);
                    }
                }
                else
                {
                    SyntaxKind newKind = new SyntaxKind
                    {
                        Category = Category.Syntax,
                        Name = node.NameNoSyntax
                    };
                    hashSet.Add(newKind);
                }

                foreach (Field field in node.Fields)
                {
                    if (field.Type == "SyntaxToken")
                    {
                        foreach (Kind kind in field.Kinds)
                        {
                            SyntaxKind newKind = new SyntaxKind
                            {
                                Category = Category.Token,
                                Name = kind.Name
                            };
                            hashSet.Add(newKind);
                        }
                    }
                }
            }

            int i = 0;
            var ordered = hashSet.ToLookup(k => k.Category).ToList();

            foreach (IGrouping<Category, SyntaxKind> grouping in ordered)
            {
                foreach (SyntaxKind kind in grouping)
                {
                    codeType.Members.Add(GenerateEnumMember(kind.Name, i));
                    i++;
                }
            }

            return codeType;
        }

        private CodeMemberField GenerateEnumMember(string name, int number)
        {
            CodeMemberField field = CodeDomFactory.Field(typeof(int), name);
            field.InitExpression = new CodePrimitiveExpression(number);
            return field;
        }
    }
}










