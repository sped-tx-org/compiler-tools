// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    public class Comment
    {
        [XmlAnyElement]
        public XmlElement[] Body { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (XmlElement element in Body)
            {
                string[] lines = element.OuterXml.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
                {
                    sb.Append(line.TrimStart());
                }
            }

            return sb.ToString();
        }
    }
}


