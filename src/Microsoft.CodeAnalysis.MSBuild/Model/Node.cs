// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    [DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    public class Node : RealNode
    {
        [XmlElement(ElementName = "Kind", Type = typeof(Kind))]
        public List<Kind> Kinds { get; set; }

        private string GetDebuggerDisplay()
        {
            return $"Name: {Name}  Fields: {FieldCount}";
        }
    }
}


