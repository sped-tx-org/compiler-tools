using System.CodeDom;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IRedNodeCodeGenerator
    {
        CodeCompileUnit GenerateRedNodes();
    }
}





