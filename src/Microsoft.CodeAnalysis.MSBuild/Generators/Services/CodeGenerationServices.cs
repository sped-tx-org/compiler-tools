using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Generators.Green;
using Microsoft.CodeAnalysis.MSBuild.Generators.Red;
using Microsoft.CodeAnalysis.MSBuild.Generators.Syntax;
using Microsoft.CodeAnalysis.MSBuild.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public static class CodeGenerationServices
    {
        public static IServiceProvider CreateServiceProvider(GenerateSyntaxDesignTime task)
        {
            var collection = new ServiceCollection();

            var tree = ModelSerializer.DeserializeFile(task.SyntaxModelFile);
            if (!string.IsNullOrEmpty(task.LanguageName))
            {
                tree.LanguageName = task.LanguageName;
            }

            if (!string.IsNullOrEmpty(task.MainNamespace))
            {
                tree.MainNamespace = task.MainNamespace;
            }

            if (!string.IsNullOrEmpty(task.SyntaxNamespace))
            {
                tree.SyntaxNamespace = task.SyntaxNamespace;
            }

            if (!string.IsNullOrEmpty(task.InternalNamespace))
            {
                tree.InternalNamespace = task.InternalNamespace;
            }

            collection.AddSingleton<CodeGeneratorDependencies>(_ => new CodeGeneratorDependencies(tree));

            
            collection.AddSingleton<IGreenFactoryCodeGenerator, GreenFactoryCodeGenerator>();
            
            collection.AddSingleton<IGreenNodeConstructorGenerator, GreenNodeConstructorGenerator>();
            collection.AddSingleton<IGreenNodeFieldGenerator, GreenNodeFieldGenerator>();
            collection.AddSingleton<IGreenNodeMethodGenerator, GreenNodeMethodGenerator>();
            collection.AddSingleton<IGreenNodePropertyGenerator, GreenNodePropertyGenerator>();
            collection.AddSingleton<IGreenRewriterCodeGenerator, GreenRewriterCodeGenerator>();
            collection.AddSingleton<IGreenVisitorCodeGenerator, GreenVisitorCodeGenerator>();

            collection.AddSingleton<GreenNodeCodeGeneratorDependencies>();
            collection.AddSingleton<IGreenNodeCodeGenerator, GreenNodeCodeGenerator>();

            collection.AddSingleton<IRedFactoryCodeGenerator, RedFactoryCodeGenerator>();
            collection.AddSingleton<IRedNodeConstructorGenerator, RedNodeConstructorGenerator>();
            collection.AddSingleton<IRedNodeFieldGenerator, RedNodeFieldGenerator>();
            collection.AddSingleton<IRedNodeMethodGenerator, RedNodeMethodGenerator>();
            collection.AddSingleton<IRedNodePropertyGenerator, RedNodePropertyGenerator>();
            collection.AddSingleton<IRedRewriterCodeGenerator, RedRewriterCodeGenerator>();
            collection.AddSingleton<IRedVisitorCodeGenerator, RedVisitorCodeGenerator>();

            collection.AddSingleton<RedNodeCodeGeneratorDependencies>();
            collection.AddSingleton<IRedNodeCodeGenerator, RedNodeCodeGenerator>();


            collection.AddSingleton<ISyntaxFactsCodeGenerator, SyntaxFactsCodeGenerator>();
            collection.AddSingleton<ISyntaxKindCodeGenerator, SyntaxKindCodeGenerator>();

            collection.AddSingleton<CodeGenerationServiceDependencies>();

            collection.AddSingleton<ICodeGenerationService, CodeGenerationService>();

            return collection.BuildServiceProvider();
        }
    }
}





