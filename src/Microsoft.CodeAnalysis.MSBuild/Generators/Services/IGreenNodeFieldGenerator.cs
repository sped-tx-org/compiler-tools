using System.CodeDom;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IGreenNodeFieldGenerator
    {
        CodeTypeMemberCollection GenerateNodeFields(Node node);
    }
}





