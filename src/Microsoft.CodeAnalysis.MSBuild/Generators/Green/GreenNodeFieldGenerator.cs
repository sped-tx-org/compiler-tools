using System.CodeDom;

using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Green
{
    public class GreenNodeFieldGenerator : AbstractCodeGenerator, IGreenNodeFieldGenerator
    {
        public GreenNodeFieldGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeTypeMemberCollection GenerateNodeFields(Node node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            var valueFields = GetValueFields(node);
            var nodeFields = GetNodeFields(node);

            for (int i = 0, n = nodeFields.Count; i < n; i++)
            {
                var field = nodeFields[i];
                var type = GetFieldType(field, green: true);
                CodeMemberField f = new CodeMemberField();
                f.Attributes = MemberAttributes.Assembly;
                f.UserData["ReadOnly"] = true;
                f.Name = FieldName(field);
                f.Type = CreateType(type);
                collection.Add(f);
            }

            for (int i = 0, n = valueFields.Count; i < n; i++)
            {
                var field = valueFields[i];
                CodeMemberField f = new CodeMemberField();
                f.Attributes = MemberAttributes.Assembly;
                f.UserData["ReadOnly"] = true;
                f.Name = FieldName(field);
                f.Type = CreateType(field.Type);
                collection.Add(f);
            }

            return collection;
        }
    }
}


