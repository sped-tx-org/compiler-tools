using System.CodeDom;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IGreenNodeMethodGenerator
    {
        CodeTypeMemberCollection GenerateAbstractNodeMethods(AbstractNode node);
        CodeTypeMemberCollection GenerateNodeMethods(Node node);
    }
}





