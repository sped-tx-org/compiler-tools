using System.CodeDom;

using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Green
{
    public class GreenNodePropertyGenerator : AbstractCodeGenerator, IGreenNodePropertyGenerator
    {
        public GreenNodePropertyGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeTypeMemberCollection GenerateAbstractNodeProperties(AbstractNode node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();

            var valueFields = GetValueFields(node);
            var nodeFields = GetNodeFields(node);
            for (int i = 0, n = nodeFields.Count; i < n; i++)
            {
                Field field = nodeFields[i];
                if (IsNodeOrNodeList(field.Type))
                {
                    if (IsSeparatedNodeList(field.Type)
                        || IsNodeList(field.Type))
                    {
                        CodeMemberProperty property = new CodeMemberProperty
                        {
                            Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                            Name = field.Name,
                            Type = new CodeTypeReference($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.{field.Type}"),
                            HasGet = true,
                            HasSet = false
                        };
                        collection.Add(property);
                    }
                    else
                    {
                        CodeMemberProperty property = new CodeMemberProperty
                        {
                            Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                            Name = field.Name,
                            Type = new CodeTypeReference(field.Type),
                            HasGet = true,
                            HasSet = false
                        };
                        collection.Add(property);
                    }
                }
            }
            for (int i = 0, n = valueFields.Count; i < n; i++)
            {
                Field field = valueFields[i];
                CodeMemberProperty property = new CodeMemberProperty
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                    Name = field.Name,
                    Type = new CodeTypeReference(field.Type),
                    HasGet = true,
                    HasSet = false
                };
                collection.Add(property);
            }

            return collection;
        }

        public CodeTypeMemberCollection GenerateNodeProperties(Node node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            var valueFields = GetValueFields(node);
            var nodeFields = GetNodeFields(node);
            for (int i = 0, n = nodeFields.Count; i < n; i++)
            {
                var field = nodeFields[i];
                CodeMemberProperty property = new CodeMemberProperty();
                property.Name = field.Name;
                if (IsOverride(field))
                {
                    property.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                }
                else
                {
                    property.Attributes = MemberAttributes.Public;
                }

                if (IsNodeList(field.Type))
                {
                    property.Type = CreateType($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.{field.Type}");
                    property.GetStatements.Add(
                        new CodeMethodReturnStatement(
                            new CodeObjectCreateExpression
                            {
                                CreateType = CreateType($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.{field.Type}"),
                                Parameters =
                                {
                                    VariableReference(FieldName(field))
                                }
                            }));

                    collection.Add(property);
                }
                else if (IsSeparatedNodeList(field.Type))
                {
                    property.Type = CreateType($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.{field.Type}");
                    property.GetStatements.Add(
                        new CodeMethodReturnStatement(
                            new CodeObjectCreateExpression
                            {
                                CreateType = CreateType($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.{field.Type}"),
                                Parameters =
                                {
                                    new CodeObjectCreateExpression
                                    {
                                        CreateType = CreateType($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.SyntaxList<{Tree.LanguageName}SyntaxNode>"),
                                        Parameters =
                                        {
                                            VariableReference(FieldName(field))
                                        }
                                    },
                                }
                            }));

                    collection.Add(property);
                }
                else if (IsSyntaxNodeOrTokenList(field))
                {
                    property.Type = CreateType($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.SyntaxList<{Tree.LanguageName}SyntaxNode>");
                    property.GetStatements.Add(
                        new CodeMethodReturnStatement(
                            new CodeObjectCreateExpression
                            {
                                CreateType = CreateType($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.{field.Type}"),
                                Parameters =
                                {
                                    new CodeObjectCreateExpression
                                    {
                                        CreateType = CreateType($"Microsoft.CodeAnalysis.Syntax.InternalSyntax.SyntaxList<{Tree.LanguageName}SyntaxNode>"),
                                        Parameters =
                                        {
                                            VariableReference(FieldName(field))
                                        }
                                    },
                                }
                            }));
                    collection.Add(property);
                }
                else
                {
                    property.Type = CreateType(field.Type);
                    property.GetStatements.Add(
                        new CodeMethodReturnStatement(
                            VariableReference(FieldName(field))));
                    collection.Add(property);
                }
            }
            for (int i = 0, n = valueFields.Count; i < n; i++)
            {
                var field = valueFields[i];

                CodeMemberProperty property = new CodeMemberProperty();
                property.Name = field.Name;
                if (IsOverride(field))
                {
                    property.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                }
                else
                {
                    property.Attributes = MemberAttributes.Public;
                }

                property.Type = CreateType(field.Type);
                property.GetStatements.Add(
                    new CodeMethodReturnStatement(
                        VariableReference(FieldName(field))));
                collection.Add(property);
            }
            return collection;
        }
    }
}


