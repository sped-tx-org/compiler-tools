using System.CodeDom;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IGreenRewriterCodeGenerator
    {
        CodeCompileUnit GenerateRewriter();
    }
}





