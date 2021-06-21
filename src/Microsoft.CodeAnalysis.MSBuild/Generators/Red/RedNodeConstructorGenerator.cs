// -----------------------------------------------------------------------
// <copyright file="RedNodeConstructorGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;

using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    public class RedNodeConstructorGenerator : AbstractCodeGenerator, IRedNodeConstructorGenerator
    {
        public RedNodeConstructorGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeTypeMemberCollection GenerateAbstractNodeConstructors(AbstractNode node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            collection.Add(GenerateRedNodeConstructor(node));
            return collection;
        }

        public CodeTypeMemberCollection GenerateNodeConstructors(Node node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            collection.Add(GenerateRedNodeConstructor(node));
            return collection;
        }

        private CodeConstructor GenerateRedNodeConstructor(TreeType nd)
        {
            CodeConstructor ctor = new CodeConstructor
            {
                Name = nd.Name,
                Attributes = MemberAttributes.FamilyAndAssembly
            };
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


