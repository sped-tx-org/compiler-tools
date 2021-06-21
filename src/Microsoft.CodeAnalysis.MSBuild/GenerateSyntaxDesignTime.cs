using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;

namespace Microsoft.CodeAnalysis.MSBuild
{
    public class GenerateSyntaxDesignTime : Task
    {
        /// <summary>
        /// Gets or sets the SyntaxModelFile
        /// </summary>
        [Required]
        public string SyntaxModelFile { get; set; }

        /// <summary>
        /// Gets or sets the TargetDirectory
        /// </summary>
        public string TargetDirectory { get; set; }

        /// <summary>
        /// Gets or sets the LanguageName
        /// </summary>
        public string LanguageName { get; set; }

        /// <summary>
        /// Gets or sets the MainNamespace
        /// </summary>
        public string MainNamespace { get; set; }

        /// <summary>
        /// Gets or sets the SyntaxNamespace
        /// </summary>
        public string SyntaxNamespace { get; set; }

        /// <summary>
        /// Gets or sets the InternalNamespace
        /// </summary>
        public string InternalNamespace { get; set; }

        /// <summary>
        /// Gets or sets the OutputFile
        /// </summary>
        [Output]
        public ITaskItem[] OutputFile { get; set; }

        private void LogMessage(string message, params object[] args)
        {
            Log.LogMessage(MessageImportance.High, message, args);
        }

        public override bool Execute()
        {
            var targetDirectory = new DirectoryInfo(TargetDirectory);

            CheckTargetDirectory(targetDirectory);

            IServiceProvider provider = CodeGenerationServices.CreateServiceProvider(this);
            ICodeGenerationService service = provider.GetService<ICodeGenerationService>();

            ProcessCodeGenerationService(service);


            OutputFile = CreateTaskItemList();

            return true;
        }

        private void ProcessCodeGenerationService(ICodeGenerationService service)
        {
            Log.LogMessage(MessageImportance.High, "Writing file Red.Syntax.g.cs...", TargetDirectory);
            service.GenerateRedNodes(TargetDirectory, "Red.Syntax.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file Green.Syntax.g.cs...", TargetDirectory);
            service.GenerateGreenNodes(TargetDirectory, "Green.Syntax.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file Red.Visitors.g.cs...", TargetDirectory);
            service.GenerateRedVisitors(TargetDirectory, "Red.Visitors.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file Green.Visitors.g.cs...", TargetDirectory);
            service.GenerateGreenVisitors(TargetDirectory, "Green.Visitors.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file Red.Rewriter.g.cs...", TargetDirectory);
            service.GenerateRedRewriter(TargetDirectory, "Red.Rewriter.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file Green.Rewriter.g.cs...", TargetDirectory);
            service.GenerateGreenRewriter(TargetDirectory, "Green.Rewriter.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file Green.Factory.g.cs...", TargetDirectory);
            service.GenerateGreenFactory(TargetDirectory, "Green.Factory.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file Red.Factory.g.cs...", TargetDirectory);
            service.GenerateRedFactory(TargetDirectory, "Red.Factory.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file SyntaxKind.g.cs...", TargetDirectory);
            service.GenerateSyntaxKind(TargetDirectory, "SyntaxKind.g.cs");
            Log.LogMessage(MessageImportance.High, "Writing file SyntaxFacts.g.cs...", TargetDirectory);
            service.GenerateSyntaxFacts(TargetDirectory, "SyntaxFacts.g.cs");
        }

        private void CheckTargetDirectory(DirectoryInfo targetDirectory)
        {
            LogMessage("Checking if directory '{0}' exists...", targetDirectory.Name);
            if (!Directory.Exists(TargetDirectory))
            {
                LogMessage("Created target directory '{0}' because it did not exist", targetDirectory.Name);
                Directory.CreateDirectory(TargetDirectory);
            }
            else
            {
                LogMessage("Deleting target directory '{0}' because it exists.", targetDirectory.Name);
                Directory.Delete(TargetDirectory, true);

                LogMessage("Creating target directory '{0}'.", targetDirectory.Name);
                Directory.CreateDirectory(TargetDirectory);
            }
        }

        private ITaskItem[] CreateTaskItemList()
        {
            List<ITaskItem> list = new List<ITaskItem>();
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "Red.Syntax.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "Red.Visitors.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "Red.Rewriter.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "Red.Factory.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "Green.Syntax.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "Green.Visitors.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "Green.Rewriter.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "Green.Factory.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "SyntaxKind.g.cs")));
            list.Add(new TaskItem(Path.Combine(TargetDirectory, "SyntaxFacts.g.cs")));
            return list.ToArray();
        }
    }
}
