using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Green
{
    public class GreenNodeCodeGeneratorDependencies
    {
        public GreenNodeCodeGeneratorDependencies(
            IGreenNodePropertyGenerator propertyGenerator,
            IGreenNodeFieldGenerator fieldGenerator,
            IGreenNodeMethodGenerator methodGenerator,
            IGreenNodeConstructorGenerator constructorGenerator)
        {
            PropertyGenerator = propertyGenerator;
            FieldGenerator = fieldGenerator;
            MethodGenerator = methodGenerator;
            ConstructorGenerator = constructorGenerator;
        }

        public IGreenNodePropertyGenerator PropertyGenerator { get; }
        public IGreenNodeFieldGenerator FieldGenerator { get; }
        public IGreenNodeMethodGenerator MethodGenerator { get; }
        public IGreenNodeConstructorGenerator ConstructorGenerator { get; }
    }
}
