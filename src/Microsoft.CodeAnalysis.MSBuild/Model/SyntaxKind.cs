using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    public class SyntaxKind : IEquatable<SyntaxKind>
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        [XmlAttribute]
        public Category Category { get; set; }

#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            return Equals(obj as SyntaxKind);
        }

        public bool Equals(SyntaxKind other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public static bool operator ==(SyntaxKind kind1, SyntaxKind kind2)
        {
            return EqualityComparer<SyntaxKind>.Default.Equals(kind1, kind2);
        }

        public static bool operator !=(SyntaxKind kind1, SyntaxKind kind2)
        {
            return !(kind1 == kind2);
        }
    }
}


