// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    [XmlRoot]
    public class Tree : ISyntaxTreeModel
    {
        public Tree()
        {
            Types = new List<TreeType>();
            SyntaxKinds = new List<SyntaxKind>();
        }

        [XmlAttribute]
        public string Root { get; set; }

        [XmlArrayItem(typeof(SyntaxKind))]
        public List<SyntaxKind> SyntaxKinds { get; set; }

        [XmlElement(ElementName = "Node", Type = typeof(Node))]
        [XmlElement(ElementName = "AbstractNode", Type = typeof(AbstractNode))]
        [XmlElement(ElementName = "PredefinedNode", Type = typeof(PredefinedNode))]
        public List<TreeType> Types { get; set; }

        [XmlAttribute]
        public string LanguageName { get; set; }

        [XmlAttribute]
        public string InternalNamespace { get; set; }

        [XmlAttribute]
        public string MainNamespace { get; set; }

        [XmlAttribute]
        public string SyntaxNamespace { get; set; }

        [XmlIgnore]
        public IDictionary<string, string> ParentMap =>
            Types.ToDictionary(n => n.Name, n => n.Base);

        [XmlIgnore]
        public ILookup<string, string> ChildMap =>
            Types.ToLookup(n => n.Base, n => n.Name);

        [XmlIgnore]
        public IDictionary<string, Node> NodeMap =>
            Types.OfType<Node>().ToDictionary(n => n.Name);
    }
}


