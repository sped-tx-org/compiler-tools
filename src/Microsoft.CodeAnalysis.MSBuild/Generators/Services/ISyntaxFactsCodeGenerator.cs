using System.CodeDom;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface ISyntaxFactsCodeGenerator
    {
        CodeCompileUnit GenerateSyntaxFacts();
    }
}





