// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    public class TreeType
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public string NameNoSyntax
        {
            get
            {
                return Name.Replace("Syntax", "");
            }
        }

        [XmlAttribute]
        public string Base { get; set; }

        [XmlElement]
        public Comment TypeComment { get; set; }

        [XmlElement]
        public Comment FactoryComment { get; set; }
    }
}


