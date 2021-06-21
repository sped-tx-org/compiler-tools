// -----------------------------------------------------------------------
// <copyright file="IGreenNodeConstructorGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Services
{
    public interface IGreenNodeConstructorGenerator
    {
        CodeTypeMemberCollection GenerateAbstractNodeConstructors(AbstractNode node);
        CodeTypeMemberCollection GenerateNodeConstructors(Node node);
    }
}





