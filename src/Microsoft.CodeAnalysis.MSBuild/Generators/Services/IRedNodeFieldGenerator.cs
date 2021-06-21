using System.CodeDom;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IRedNodeFieldGenerator
    {
        CodeTypeMemberCollection GenerateAbstractNodeFields(AbstractNode node);

        CodeTypeMemberCollection GenerateNodeFields(Node node);
    }
}





