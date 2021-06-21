// -----------------------------------------------------------------------
// <copyright file="RedNodeFieldGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    public class RedNodeFieldGenerator : AbstractCodeGenerator, IRedNodeFieldGenerator
    {
        public RedNodeFieldGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeTypeMemberCollection GenerateAbstractNodeFields(AbstractNode node)
        {
            return new CodeTypeMemberCollection();
        }

        public CodeTypeMemberCollection GenerateNodeFields(Node node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();

            List<Field> nodeFields = GetNodeFields(node);

            for (int i = 0, n = nodeFields.Count; i < n; i++)
            {
                var field = nodeFields[i];
                if (field.Type != "SyntaxToken" && field.Type != "SyntaxList<SyntaxToken>")
                {
                    if (IsSeparatedNodeList(field.Type) || field.Type == "SyntaxNodeOrTokenList")
                    {
                        CodeMemberField f = new CodeMemberField();
                        f.Attributes = MemberAttributes.Private;
                        f.Type = CreateType("SyntaxNode");
                        f.Name = FieldName(field);
                        collection.Add(f);
                    }
                    else
                    {
                        var type = GetFieldType(field, green: false);
                        CodeMemberField f = new CodeMemberField();
                        f.Attributes = MemberAttributes.Private;
                        f.Type = CreateType(type);
                        f.Name = FieldName(field);
                        collection.Add(f);
                    }
                }
            }

            return collection;
        }
    }
}


