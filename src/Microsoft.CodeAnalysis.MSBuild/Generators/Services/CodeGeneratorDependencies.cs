using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public class CodeGeneratorDependencies
    {
        public CodeGeneratorDependencies(ISyntaxTreeModel tree)
        {
            Tree = tree;
        }

        public ISyntaxTreeModel Tree { get; }
    }
}
