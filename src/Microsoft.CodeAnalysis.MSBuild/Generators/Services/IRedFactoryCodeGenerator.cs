using System.CodeDom;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IRedFactoryCodeGenerator
    {
        CodeCompileUnit GenerateFactory();
    }
}





