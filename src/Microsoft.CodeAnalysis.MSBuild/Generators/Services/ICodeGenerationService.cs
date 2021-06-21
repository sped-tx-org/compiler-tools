// -----------------------------------------------------------------------
// <copyright file="ICodeGenerationService.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;

using System.IO;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface ICodeGenerationService
    {
        void GenerateGreenFactory(string targetDirectory, string fileName);
        void GenerateGreenRewriter(string targetDirectory, string fileName);
        void GenerateGreenVisitors(string targetDirectory, string fileName);
        void GenerateGreenNodes(string targetDirectory, string fileName);
        void GenerateRedNodes(string targetDirectory, string fileName);
        void GenerateRedVisitors(string targetDirectory, string fileName);
        void GenerateRedRewriter(string targetDirectory, string fileName);
        void GenerateRedFactory(string targetDirectory, string fileName);
        void GenerateSyntaxKind(string targetDirectory, string fileName);
        void GenerateSyntaxFacts(string targetDirectory, string fileName);
    }
}





