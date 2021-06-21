// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    [DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    public class Field
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public string Optional { get; set; }

        [XmlAttribute]
        public string Override { get; set; }

        [XmlAttribute]
        public ListKind ListKind { get; set; }

        [XmlAttribute]
        public string New { get; set; }

        [XmlElement(ElementName = "Kind", Type = typeof(Kind))]
        public List<Kind> Kinds { get; set; }

        [XmlElement]
        public Comment PropertyComment { get; set; }

        private string GetDebuggerDisplay()
        {
            return $"{Name}: {Type}";
        }
    }
}


