using System.CodeDom;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IRedNodeConstructorGenerator
    {
        CodeTypeMemberCollection GenerateAbstractNodeConstructors(AbstractNode node);

        CodeTypeMemberCollection GenerateNodeConstructors(Node node);
    }
}





