using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    public class RedNodeCodeGeneratorDependencies
    {
        public RedNodeCodeGeneratorDependencies(
            IRedNodePropertyGenerator propertyGenerator,
            IRedNodeFieldGenerator fieldGenerator,
            IRedNodeMethodGenerator methodGenerator)
        {
            PropertyGenerator = propertyGenerator;
            FieldGenerator = fieldGenerator;
            MethodGenerator = methodGenerator;
        }

        public IRedNodePropertyGenerator PropertyGenerator { get; }
        public IRedNodeFieldGenerator FieldGenerator { get; }
        public IRedNodeMethodGenerator MethodGenerator { get; set; }
    }
}
