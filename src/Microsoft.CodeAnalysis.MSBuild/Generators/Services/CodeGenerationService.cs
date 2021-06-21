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
    internal class CodeGenerationService : AbstractCodeGenerator, ICodeGenerationService
    {
        public CodeGenerationService(
            CodeGeneratorDependencies dependencies,
            CodeGenerationServiceDependencies serviceDependencies) : base(dependencies)
        {
            ServiceDependencies = serviceDependencies;
        }

        public CodeGenerationServiceDependencies ServiceDependencies { get; }

        private FileStream OpenFile(string path)
        {
            return new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
        }

        public void GenerateGreenFactory(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = ServiceDependencies.GreenFactoryGenerator.GenerateFactory();
                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateGreenRewriter(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = ServiceDependencies.GreenRewriterGenerator.GenerateRewriter();
                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateGreenVisitors(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = new CodeCompileUnit();
                CodeNamespace ns = new CodeNamespace();
                ns.Name = Tree.InternalNamespace;

                CodeTypeDeclaration visitor1 = ServiceDependencies.GreenVisitorGenerator.GenerateVisitor(false, false);
                CodeTypeDeclaration visitor2 = ServiceDependencies.GreenVisitorGenerator.GenerateVisitor(false, true);
                //CodeTypeDeclaration visitor3 = ServiceDependencies.GreenVisitorGenerator.GenerateVisitor(true, true);

                ns.Types.Add(visitor1);
                ns.Types.Add(visitor2);

                unit.Namespaces.Add(ns);

                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateGreenNodes(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = ServiceDependencies.GreenNodeGenerator.GenerateGreenNodes();
                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateRedFactory(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = ServiceDependencies.RedFactoryGenerator.GenerateFactory();
                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateRedRewriter(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = ServiceDependencies.RedRewriterGenerator.GenerateRewriter();
                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateRedVisitors(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = new CodeCompileUnit();
                CodeNamespace ns = new CodeNamespace();
                ns.Imports.Add(new CodeNamespaceImport(Tree.SyntaxNamespace));
                ns.Name = Tree.MainNamespace;

                CodeTypeDeclaration visitor1 = ServiceDependencies.RedVisitorGenerator.GenerateVisitor(false, false);
                CodeTypeDeclaration visitor2 = ServiceDependencies.RedVisitorGenerator.GenerateVisitor(false, true);
                //CodeTypeDeclaration visitor3 = ServiceDependencies.RedVisitorGenerator.GenerateVisitor(true, true);

                ns.Types.Add(visitor1);
                ns.Types.Add(visitor2);

                unit.Namespaces.Add(ns);

                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateRedNodes(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = ServiceDependencies.RedNodeGenerator.GenerateRedNodes();
                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateSyntaxKind(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = ServiceDependencies.SyntaxKindGenerator.GenerateSyntaxKind();
                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }

        public void GenerateSyntaxFacts(string targetDirectory, string fileName)
        {
            using (FileStream fs = OpenFile(Path.Combine(targetDirectory, fileName)))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                CodeCompileUnit unit = ServiceDependencies.SyntaxFactsCodeGenerator.GenerateSyntaxFacts();
                writer.Write(CodeGenerator.GenerateCodeCompileUnit(unit));
            }
        }
    }
}





