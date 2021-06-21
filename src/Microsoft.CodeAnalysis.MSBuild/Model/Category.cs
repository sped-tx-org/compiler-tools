using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    public enum Category
    {
        [XmlEnum("None")]
        None = 0,

        [XmlEnum("Token")]
        Token = 1,

        [XmlEnum("Trivia")]
        Trivia = 2,

        [XmlEnum("Syntax")]
        Syntax = 3
    }
}


