using System.CodeDom;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IRedNodePropertyGenerator
    {
        CodeTypeMemberCollection GenerateAbstractNodeProperties(AbstractNode node);

        CodeTypeMemberCollection GenerateNodeProperties(Node node);
    }
}





