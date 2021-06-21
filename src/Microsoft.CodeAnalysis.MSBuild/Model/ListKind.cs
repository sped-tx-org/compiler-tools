// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    [XmlType(TypeName = "ListKind")]
    public enum ListKind
    {
        [XmlEnum]
        None,

        [XmlEnum]
        Syntax,

        [XmlEnum]
        Separated,

        [XmlEnum]
        SyntaxToken
    }
}


