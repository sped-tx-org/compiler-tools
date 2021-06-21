// -----------------------------------------------------------------------
// <copyright file="IGreenNodeCodeGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;


namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IGreenNodeCodeGenerator
    {
        CodeCompileUnit GenerateGreenNodes();
    }
}





