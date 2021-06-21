// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Microsoft.CodeAnalysis.MSBuild.Model
{
    public class RealNode : TreeType
    {
        [XmlElement(ElementName = "Field", Type = typeof(Field))]
        public List<Field> Fields { get; set; }

        public int FieldCount
        {
            get
            {
                if (Fields != null)
                {
                    return Fields.Count;
                }

                return 0;
            }
        }

        public bool HasRequiredField
        {
            get
            {
                bool retVal = false;
                if (Fields != null)
                {
                    foreach (Field field in Fields)
                    {
                        if (!string.IsNullOrEmpty(field.Optional))
                        {
                            if (bool.TryParse(field.Optional, out bool result) && result)
                            {
                                retVal = true;
                            }
                        }
                    }
                }
                return retVal;
            }
        }

        public bool HasAutoCreatableField
        {
            get
            {
                return Fields.Count(f => IsAutoCreatableToken(this, f)) > 0;
            }
        }

        private bool IsAutoCreatableToken(RealNode node, Field field)
        {
            Node rnode = node as Node;

            return field.Type == "SyntaxToken"
                && field.Kinds != null
                && ((field.Kinds.Count == 1 && field.Kinds[0].Name != "IdentifierToken" && !field.Kinds[0].Name.EndsWith("LiteralToken", StringComparison.Ordinal)) || (field.Kinds.Count > 1 && field.Kinds.Count == rnode?.Kinds.Count));
        }

        private bool IsAutoCreatableNode(Node node, Field field)
        {
            return (node != null && RequiredFactoryArgumentCount(node) == 0);
        }

        private int RequiredFactoryArgumentCount(Node nd, bool includeKind = true)
        {
            int count = 0;

            // kind must be specified in factory
            if (nd.Kinds.Count > 1 && includeKind)
            {
                count++;
            }

            for (int i = 0, n = nd.Fields.Count; i < n; i++)
            {
                var field = nd.Fields[i];
                if (IsRequiredFactoryField(nd, field))
                {
                    count++;
                }
            }

            return count;
        }

        private bool IsRequiredFactoryField(Node node, Field field)
        {
            return (!IsOptional(field) && !IsAnyList(field.Type) && !CanBeAutoCreated(node, field)) || IsValueField(field);
        }

        protected static bool IsOptional(Field f)
        {
            return f.Optional != null && string.Compare(f.Optional, "true", true) == 0;
        }

        protected static bool IsOverride(Field f)
        {
            return f.Override != null && string.Compare(f.Override, "true", true) == 0;
        }

        protected static bool IsNew(Field f)
        {
            return f.New != null && string.Compare(f.New, "true", true) == 0;
        }

        protected static bool IsAnyList(string typeName)
        {
            return IsNodeList(typeName) || IsSeparatedNodeList(typeName) || typeName == "SyntaxNodeOrTokenList";
        }

        protected static bool IsAnyNodeList(string typeName)
        {
            return IsNodeList(typeName) || IsSeparatedNodeList(typeName);
        }

        protected bool IsNodeOrNodeList(string typeName)
        {
            return IsNodeList(typeName) || IsSeparatedNodeList(typeName) || typeName == "SyntaxNodeOrTokenList";
        }

        protected static bool IsSeparatedNodeList(string typeName)
        {
            return typeName.StartsWith("SeparatedSyntaxList<", StringComparison.Ordinal);
        }

        protected static bool IsNodeList(string typeName)
        {
            return typeName.StartsWith("SyntaxList<", StringComparison.Ordinal);
        }

        protected bool CanBeAutoCreated(Node node, Field field)
        {
            return IsAutoCreatableToken(node, field) || IsAutoCreatableNode(node, field);
        }

        private bool IsValueField(Field field)
        {
            return !IsNodeOrNodeList(field.Type);
        }

        public List<Field> GetAutomaticallyCreatableFields()
        {
            List<Field> list = new List<Field>();
            if (HasRequiredField || Fields == null)
            {
                return list;
            }

            foreach (Field field in Fields)
            {
                if (field.Kinds != null && field.Kinds.Count >= 1)
                {
                    if (field.Type == "SyntaxToken")
                    {
                        if (field.Kinds[0].Name != "IdentifierToken" && !field.Kinds[0].Name.EndsWith("LiteralToken"))
                        {
                            list.Add(field);
                        }
                    }
                }
            }
            return list;
        }

        public List<Field> GetMandatoryFields()
        {
            List<Field> list = new List<Field>();
            if (HasRequiredField || Fields == null)
            {
                return list;
            }

            foreach (Field field in Fields)
            {
                if (field.Kinds != null && field.Kinds.Count >= 1)
                {
                    if (field.Type == "SyntaxToken")
                    {
                        if (field.Kinds[0].Name != "IdentifierToken" && !field.Kinds[0].Name.EndsWith("LiteralToken"))
                        {
                            list.Add(field);
                        }
                    }
                }
            }

            var temp = new List<Field>(Fields);

            temp.RemoveAll(f => GetAutomaticallyCreatableFields().Contains(f));

            return new List<Field>(temp);
        }
    }
}


