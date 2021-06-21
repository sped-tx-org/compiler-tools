// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;

using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Green
{
    public class GreenNodeCodeGenerator : AbstractCodeGenerator, IGreenNodeCodeGenerator
    {
        public GreenNodeCodeGenerator(
            CodeGeneratorDependencies dependencies,
            GreenNodeCodeGeneratorDependencies generatorDependencies) : base(dependencies)
        {
            GeneratorDependencies = generatorDependencies;
        }

        public GreenNodeCodeGeneratorDependencies GeneratorDependencies { get; }

        public CodeCompileUnit GenerateGreenNodes()
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            CodeNamespace ns = GenerateGreenNodeNamespace();
            unit.Namespaces.Add(ns);
            ns.Imports.AddRange(GetUsings());
            return unit;
        }

        private CodeNamespace GenerateGreenNodeNamespace()
        {
            CodeNamespace ns = new CodeNamespace(Tree.InternalNamespace);
            foreach (AbstractNode node in Tree.Types.OfType<AbstractNode>())
            {
                CodeTypeDeclaration codeType = GenerateAbstractNode(node);
                ns.Types.Add(codeType);
            }
            foreach (Node node in Tree.Types.OfType<Node>())
            {
                CodeTypeDeclaration codeType = GenerateNode(node);
                ns.Types.Add(codeType);
            }
            return ns;
        }

        private CodeTypeDeclaration GenerateNode(Node node)
        {
            CodeTypeDeclaration c = new CodeTypeDeclaration(node.Name);
            //if (node.TypeComment != null)
            //{
            //    c.Comments.Add(GenerateComment(node.TypeComment.ToString()));
            //}
            c.IsClass = true;
            c.IsPartial = true;
            c.TypeAttributes = TypeAttributes.NotPublic | TypeAttributes.Sealed;
            c.BaseTypes.Add(new CodeTypeReference(node.Base));
            c.Members.AddRange(GeneratorDependencies.FieldGenerator.GenerateNodeFields(node));
            c.Members.AddRange(GeneratorDependencies.ConstructorGenerator.GenerateNodeConstructors(node));
            c.Members.AddRange(GeneratorDependencies.PropertyGenerator.GenerateNodeProperties(node));
            c.Members.AddRange(GeneratorDependencies.MethodGenerator.GenerateNodeMethods(node));
            return c;
        }

        private CodeTypeDeclaration GenerateAbstractNode(AbstractNode node)
        {
            CodeCommentStatement comment = 
                new CodeCommentStatement($"Provides the base class from which the classes that represent {StripPost(node.Name, "Syntax")} syntax nodes are derived. This is an abstract class.", true);
            CodeTypeDeclaration c = new CodeTypeDeclaration(node.Name);
            //if (node.TypeComment != null)
            //{
            //    c.Comments.Add(GenerateComment(node.TypeComment.ToString()));
            //}
            c.Comments.Add(comment);
            c.IsClass = true;
            c.IsPartial = true;
            c.TypeAttributes = TypeAttributes.NotPublic | TypeAttributes.Abstract;
            c.BaseTypes.Add(new CodeTypeReference(node.Base));

            c.Members.AddRange(GeneratorDependencies.ConstructorGenerator.GenerateAbstractNodeConstructors(node));

            c.Members.AddRange(GeneratorDependencies.PropertyGenerator.GenerateAbstractNodeProperties(node));

            return c;
        }
    }
}


