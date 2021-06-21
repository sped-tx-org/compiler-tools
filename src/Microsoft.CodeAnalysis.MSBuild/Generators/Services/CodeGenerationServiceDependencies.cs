using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public class CodeGenerationServiceDependencies
    {

        public CodeGenerationServiceDependencies(
            IGreenFactoryCodeGenerator greenFactoryGenerator,
            IGreenRewriterCodeGenerator greenRewriterGenerator,
            IGreenVisitorCodeGenerator greenVisitorGenerator,
            IGreenNodeCodeGenerator greenNodeGenerator,
            IRedFactoryCodeGenerator redFactoryGenerator,
            IRedRewriterCodeGenerator redRewriterGenerator,
            IRedVisitorCodeGenerator redVisitorGenerator,
            IRedNodeCodeGenerator redNodeGenerator,
            ISyntaxKindCodeGenerator syntaxKindGenerator,
            ISyntaxFactsCodeGenerator syntaxFactsCodeGenerator)
        {
            GreenFactoryGenerator = greenFactoryGenerator;
            GreenRewriterGenerator = greenRewriterGenerator;
            GreenVisitorGenerator = greenVisitorGenerator;
            GreenNodeGenerator = greenNodeGenerator;
            RedFactoryGenerator = redFactoryGenerator;
            RedRewriterGenerator = redRewriterGenerator;
            RedVisitorGenerator = redVisitorGenerator;
            RedNodeGenerator = redNodeGenerator;
            SyntaxKindGenerator = syntaxKindGenerator;
            SyntaxFactsCodeGenerator = syntaxFactsCodeGenerator;
        }

        public IGreenFactoryCodeGenerator GreenFactoryGenerator { get; }
        public IGreenRewriterCodeGenerator GreenRewriterGenerator { get; }
        public IGreenVisitorCodeGenerator GreenVisitorGenerator { get; }
        public IGreenNodeCodeGenerator GreenNodeGenerator { get; }
        public IRedFactoryCodeGenerator RedFactoryGenerator { get; }
        public IRedRewriterCodeGenerator RedRewriterGenerator { get; }
        public IRedVisitorCodeGenerator RedVisitorGenerator { get; }
        public IRedNodeCodeGenerator RedNodeGenerator { get; }
        public ISyntaxKindCodeGenerator SyntaxKindGenerator { get; }
        public ISyntaxFactsCodeGenerator SyntaxFactsCodeGenerator { get; }
    }
}
