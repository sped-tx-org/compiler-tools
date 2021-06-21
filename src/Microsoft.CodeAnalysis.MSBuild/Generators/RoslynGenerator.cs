using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Model;
using Microsoft.CodeAnalysis.MSBuild.Writers;

namespace Microsoft.CodeAnalysis.MSBuild.Generators
{
    internal static class RoslynGenerator
    {
        private static string GetAssemblyDirectory() =>
            Path.GetDirectoryName(
            Assembly.GetAssembly(typeof(RoslynGenerator)).CodeBase);

        
        public static Tree SyntaxTree = ModelSerializer.DeserializeFile(Path.Combine(GetAssemblyDirectory(), "Syntax.xml"));
    }
}



