using System.CodeDom;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IRedVisitorCodeGenerator
    {
        CodeTypeDeclaration GenerateVisitor(bool withArgument, bool withResult);
    }
}





