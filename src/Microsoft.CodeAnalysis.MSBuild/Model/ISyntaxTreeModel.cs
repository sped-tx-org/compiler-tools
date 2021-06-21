// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    public interface ISyntaxTreeModel
    {
        string InternalNamespace { get; set; }
        List<SyntaxKind> SyntaxKinds { get; set; }
        string LanguageName { get; set; }
        string MainNamespace { get; set; }
        string Root { get; set; }
        string SyntaxNamespace { get; set; }
        List<TreeType> Types { get; set; }
    }
}