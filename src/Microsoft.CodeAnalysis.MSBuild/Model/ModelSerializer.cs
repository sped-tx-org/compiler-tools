using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    public static class ModelSerializer
    {
        public static Tree DeserializeFile(string filePath)
        {
            var reader = XmlReader.Create(filePath, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });
            var serializer = new XmlSerializer(typeof(Tree));
            return (Tree)serializer.Deserialize(reader);
        }
    }
}


