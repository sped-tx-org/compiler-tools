﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface ISyntaxKindCodeGenerator
    {
        CodeCompileUnit GenerateSyntaxKind();
    }
}





