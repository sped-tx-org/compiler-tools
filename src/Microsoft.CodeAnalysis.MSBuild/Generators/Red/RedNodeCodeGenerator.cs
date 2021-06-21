using System;
using System.CodeDom;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    public class RedNodeCodeGenerator : AbstractCodeGenerator, IRedNodeCodeGenerator
    {
        public RedNodeCodeGenerator(
            CodeGeneratorDependencies dependencies,
            RedNodeCodeGeneratorDependencies generatorDependencies) : base(dependencies)
        {
            GeneratorDependencies = generatorDependencies;
        }

        public RedNodeCodeGeneratorDependencies GeneratorDependencies { get; }

        public CodeCompileUnit GenerateRedNodes()
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            CodeNamespace ns = GenerateRedNodeNamespace();
            unit.Namespaces.Add(ns);
            ns.Imports.AddRange(GetUsings());
            return unit;
        }

        private CodeNamespace GenerateRedNodeNamespace()
        {
            CodeNamespace ns = new CodeNamespace(Tree.SyntaxNamespace);
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

        private CodeTypeDeclaration GenerateAbstractNode(AbstractNode nd)
        {
            CodeTypeDeclaration c = new CodeTypeDeclaration(nd.Name);
            if (nd.TypeComment != null)
            {
                c.Comments.Add(GenerateComment(nd.TypeComment.ToString()));
            }
            c.IsClass = true;
            c.IsPartial = true;
            c.TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract;
            c.BaseTypes.Add(new CodeTypeReference(nd.Base));
            CodeConstructor ctor = GenerateRedNodeConstructor(nd);
            c.Members.Add(ctor);
            c.Members.AddRange(GeneratorDependencies.PropertyGenerator.GenerateAbstractNodeProperties(nd));
            return c;
        }

        private CodeTypeDeclaration GenerateNode(Node nd)
        {
            CodeTypeDeclaration c = new CodeTypeDeclaration(nd.Name);
            if (nd.TypeComment != null)
            {
                c.Comments.Add(GenerateComment(nd.TypeComment.ToString()));
            }
            c.IsClass = true;
            c.IsPartial = true;
            c.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            c.BaseTypes.Add(new CodeTypeReference(nd.Base));

            c.Members.AddRange(GeneratorDependencies.FieldGenerator.GenerateNodeFields(nd));

            CodeConstructor ctor = GenerateRedNodeConstructor(nd);
            c.Members.Add(ctor);
            c.Members.AddRange(GeneratorDependencies.PropertyGenerator.GenerateNodeProperties(nd));
            c.Members.AddRange(GeneratorDependencies.MethodGenerator.GenerateNodeMethods(nd));
            return c;
        }

        private CodeConstructor GenerateRedNodeConstructor(TreeType nd)
        {
            CodeConstructor ctor = new CodeConstructor();
            ctor.Name = nd.Name;
            ctor.Attributes = MemberAttributes.FamilyAndAssembly;
            ctor.Parameters.Add(GenerateParameter(InternalSyntaxNodeName, "green"));
            ctor.Parameters.Add(GenerateParameter("SyntaxNode", "parent"));
            ctor.Parameters.Add(GenerateParameter("int", "position"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("green"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("parent"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("position"));
            return ctor;
        }
    }
}


