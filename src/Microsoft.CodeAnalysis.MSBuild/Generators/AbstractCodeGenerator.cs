using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.MSBuild.Factories;
using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators
{
    public abstract class AbstractCodeGenerator
    {
        protected AbstractCodeGenerator(CodeGeneratorDependencies dependencies)
        {
            Dependencies = dependencies;
            Initialize(Dependencies.Tree);
        }

        public CodeGeneratorDependencies Dependencies { get; }
        protected IDictionary<string, string> ParentMap { get; set; }
        protected ILookup<string, string> ChildMap { get; set; }

        protected IDictionary<string, Node> NodeMap { get; set; }
        protected ISyntaxTreeModel Tree { get; set; }

        protected void Initialize(ISyntaxTreeModel tree)
        {
            Tree = tree;
            NodeMap = tree.Types.OfType<Node>().ToDictionary(n => n.Name);
            ParentMap = tree.Types.ToDictionary(n => n.Name, n => n.Base);
            ParentMap.Add(tree.Root, null);
            ChildMap = tree.Types.ToLookup(n => n.Base, n => n.Name);
        }

        protected string GetStringConverterMethod(Field field)
        {
            if (IsIdentifierToken(field))
            {
                return "SyntaxFactory.Identifier";
            }
            else if (IsIdentifierNameSyntax(field))
            {
                return "SyntaxFactory.IdentifierName";
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected bool CanAutoConvertFromString(Field field)
        {
            return IsIdentifierToken(field) || IsIdentifierNameSyntax(field);
        }

        protected bool IsIdentifierToken(Field field)
        {
            return field.Type == "SyntaxToken" && field.Kinds != null && field.Kinds.Count == 1 && field.Kinds[0].Name == "IdentifierToken";
        }

        protected bool IsIdentifierNameSyntax(Field field)
        {
            return field.Type == "IdentifierNameSyntax";
        }

        protected string GetDefaultValue(Node nd, Field field)
        {
            System.Diagnostics.Debug.Assert(!IsRequiredFactoryField(nd, field));

            if (IsOptional(field) || IsAnyList(field.Type))
            {
                if (IsNodeList(field.Type))
                {
                    string elementType = GetElementType(field.Type);
                    return $"SyntaxFactory.List<{elementType}>()";
                }

                if (IsSeparatedNodeList(field.Type))
                {
                    string elementType = GetElementType(field.Type);
                    return $"SyntaxFactory.SeparatedList<{elementType}>()";
                }

                if (field.Type == "SyntaxList<SyntaxToken>")
                {
                    return "SyntaxFactory.TokenList()";
                }

                return string.Format("default({0})", GetRedPropertyType(field));
            }
            else if (field.Type == "SyntaxToken")
            {
                // auto construct token?
                if (field.Kinds.Count == 1)
                {
                }
                //else
                //{
                //    return string.Format("SyntaxFactory.Token(Get{0}{1}Kind(kind))", StripPost(nd.Name, "Syntax"), StripPost(field.Name, "Opt"));
                //}

                return string.Format("SyntaxFactory.Token(SyntaxKind.{0})", field.Kinds[0].Name);
            }
            else
            {
                var referencedNode = GetNode(field.Type);
                return string.Format("SyntaxFactory.{0}()", StripPost(referencedNode.Name, "Syntax"));
            }
        }

        protected CodeMethodReturnStatement ReturnCastExpression(string targetType, CodeExpression expression)
        {
            return CodeDomFactory.ReturnStatement(
                CodeDomFactory.CastExpression(targetType, expression));
        }

        protected void GenerateParameterValidation(CodeMemberMethod method, Field field, bool green)
        {
            string parameterName = ParameterName(field);
            if (!IsAnyList(field.Type) && !IsOptional(field))
            {
                CodeConditionStatement ifParameterEqualsNull =
                    new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            VariableReference(parameterName),
                            CodeBinaryOperatorType.IdentityEquality,
                            VariableReference("null")),
                        new CodeThrowExceptionStatement(
                            new CodeObjectCreateExpression("ArgumentNullException", VariableReference($"nameof({parameterName})"))));
                method.Statements.Add(ifParameterEqualsNull);
            }
            if (field.Type == "SyntaxToken" && field.Kinds != null && field.Kinds.Count > 0)
            {
                if (green)
                {
                    method.Statements.Add(GenerateGreenSyntaxTokenKindsSwitchStatement(field));
                }
                else
                {
                    method.Statements.Add(GenerateRedSyntaxTokenKindsSwitchStatement(field));
                }
            }
        }

        protected CodeStatement GenerateRedSyntaxTokenKindsSwitchStatement(Field field)
        {
            IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ") { Indent = 3 };
            if (IsOptional(field))
            {
                writer.WriteLine();
                writer.WriteLine($"if ({ParameterName(field)} != null)");
                writer.WriteLine("{");
            }
            writer.Indent++;
            writer.WriteLine($"switch({ParameterName(field)}.Kind())");
            writer.WriteLine("{");
            writer.Indent++;
            foreach (var k in field.Kinds)
            {
                writer.WriteLine($"case SyntaxKind.{k.Name}:");
            }
            if (IsOptional(field))
            {
                writer.WriteLine("case SyntaxKind.None:");
            }
            writer.Indent++;
            writer.WriteLine("break;");
            writer.Indent--;
            writer.WriteLine("default:");
            writer.Indent++;
            writer.WriteLine("throw new ArgumentException(\"kind\");");
            writer.Indent--;
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            if (IsOptional(field))
            {
                writer.WriteLine("}");
            }
            writer.Indent = 3;
            writer.Write("");
            return new CodeSnippetStatement(writer.InnerWriter.ToString());
        }

        protected CodeStatement GenerateGreenSyntaxTokenKindsSwitchStatement(Field field)
        {
            IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(), "    ") { Indent = 3 };
            if (IsOptional(field))
            {
                writer.WriteLine();
                writer.WriteLine($"if ({ParameterName(field)} != null)");
                writer.WriteLine("{");
            }
            writer.Indent++;
            writer.WriteLine($"switch({ParameterName(field)}.Kind)");
            writer.WriteLine("{");
            writer.Indent++;
            foreach (var k in field.Kinds)
            {
                writer.WriteLine($"case SyntaxKind.{k.Name}:");
            }
            if (IsOptional(field))
            {
                writer.WriteLine("case SyntaxKind.None:");
            }
            writer.Indent++;
            writer.WriteLine("break;");
            writer.Indent--;
            writer.WriteLine("default:");
            writer.Indent++;
            writer.WriteLine("throw new ArgumentException(\"kind\");");
            writer.Indent--;
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            if (IsOptional(field))
            {
                writer.WriteLine("}");
            }
            writer.Indent = 3;
            writer.Write("");
            return new CodeSnippetStatement(writer.InnerWriter.ToString());
        }

        protected CodeMethodReturnStatement ReturnVariableReference(string variableName)
        {
            return ReturnStatement(VariableReference(variableName));
        }

        protected CodeMethodReturnStatement ReturnArrayCreateExpression(string createType, IEnumerable<CodeExpression> initializers)
        {
            return ReturnStatement(
                new CodeArrayCreateExpression(createType, initializers.ToArray()));
        }

        protected CodeMethodReturnStatement ReturnArrayCreateExpression(Type createType, params CodeExpression[] initializers)
        {
            return ReturnStatement(
                new CodeArrayCreateExpression(createType, initializers));
        }

        protected CodeMethodReturnStatement ReturnArrayCreateExpression(CodeTypeReference createType, params CodeExpression[] initializers)
        {
            return ReturnStatement(
                new CodeArrayCreateExpression(createType, initializers));
        }

        protected CodeMethodReturnStatement ReturnArrayCreateExpression(string createType, params CodeExpression[] initializers)
        {
            return ReturnStatement(
                new CodeArrayCreateExpression(createType, initializers));
        }

        protected CodeMethodReturnStatement ReturnObjectCreateExpresion(Type createType, IEnumerable<CodeExpression> parameters)
        {
            return ReturnStatement(
                new CodeObjectCreateExpression(createType, parameters.ToArray()));
        }

        protected CodeMethodReturnStatement ReturnObjectCreateExpresion(Type createType, params CodeExpression[] parameters)
        {
            return ReturnStatement(
                new CodeObjectCreateExpression(createType, parameters));
        }

        protected CodeMethodReturnStatement ReturnObjectCreateExpresion(string createType, params CodeExpression[] parameters)
        {
            return ReturnStatement(
                new CodeObjectCreateExpression(createType, parameters));
        }

        protected CodeMethodReturnStatement ReturnObjectCreateExpresion(CodeTypeReference createType, params CodeExpression[] parameters)
        {
            return ReturnStatement(
                new CodeObjectCreateExpression(createType, parameters));
        }

        protected CodeMethodReturnStatement ReturnInvokeMethodExpression(
            CodeExpression targetObject,
            string methodName,
            params CodeExpression[] parameters)
        {
            return ReturnStatement(new CodeMethodInvokeExpression(targetObject, methodName, parameters));
        }

        protected CodeMethodReturnStatement ReturnStatement(CodeExpression expression)
        {
            return new CodeMethodReturnStatement(expression);
        }

        protected CodeMethodReturnStatement ReturnTrue()
        {
            return ReturnStatement(new CodePrimitiveExpression(true));
        }

        protected CodeMethodReturnStatement ReturnFalse()
        {
            return ReturnStatement(new CodePrimitiveExpression(false));
        }

        protected string InternalSyntaxNodeName => $"{Tree.InternalNamespace}.{Tree.LanguageName}SyntaxNode";

        protected string FieldName(Field field) => $"_{CodeIdentifier.MakeCamel(field.Name)}";

        protected string ParameterName(Field field) => $"{CodeIdentifier.MakeCamel(field.Name)}";

        protected string StripSyntax(string input) => input.Replace("Syntax", string.Empty);

        protected string InternalFactoryMethodFullName(Node nd)
        {
            return $"{Tree.InternalNamespace}.SyntaxFactory.{StripSyntax(nd.Name)}";
        }

        protected IEnumerable<Field> DetermineRedFactoryWithNoAutoCreatableTokenFields(Node nd)
        {
            return nd.Fields.Where(f => !IsAutoCreatableToken(nd, f));
        }

        protected IEnumerable<Field> DetermineMinimalFactoryFields(Node nd)
        {
            // special case to allow a single optional argument if there would have been no arguments
            // and we can determine a best single argument.
            Field allowOptionalField = null;

            var optionalCount = OptionalFactoryArgumentCount(nd);
            if (optionalCount == 0)
            {
                return null; // no fields...
            }

            var requiredCount = RequiredFactoryArgumentCount(nd, includeKind: false);
            if (requiredCount == 0 && optionalCount > 1)
            {
                allowOptionalField = DetermineMinimalOptionalField(nd);
            }

            return nd.Fields.Where(f => IsRequiredFactoryField(nd, f) || allowOptionalField == f);
        }

        protected int OptionalFactoryArgumentCount(Node nd)
        {
            int count = 0;
            for (int i = 0, n = nd.Fields.Count; i < n; i++)
            {
                var field = nd.Fields[i];
                if (IsOptional(field) || CanBeAutoCreated(nd, field) || IsAnyList(field.Type))
                {
                    count++;
                }
            }

            return count;
        }

        protected Field DetermineMinimalOptionalField(Node nd)
        {
            // first if there is a single list, then choose the list because it would not have been optional
            int listCount = nd.Fields.Count(f => IsAnyNodeList(f.Type));
            if (listCount == 1)
            {
                return nd.Fields.First(f => IsAnyNodeList(f.Type));
            }
            else
            {
                // otherwise, if there is a single optional node, use that..
                int nodeCount = nd.Fields.Count(f => IsNode(f.Type) && f.Type != "SyntaxToken");
                if (nodeCount == 1)
                {
                    return nd.Fields.First(f => IsNode(f.Type) && f.Type != "SyntaxToken");
                }
                else
                {
                    return null;
                }
            }
        }

        protected string GetRedPropertyType(Field field)
        {
            if (field.Type == "SyntaxList<SyntaxToken>")
                return "SyntaxTokenList";
            return field.Type;
        }

        protected static string StripPost(string name, string post)
        {
            return name.EndsWith(post, StringComparison.Ordinal)
                ? name.Substring(0, name.Length - post.Length)
                : name;
        }

        protected CodeNamespaceImport[] GetUsings()
        {
            List<CodeNamespaceImport> list = new List<CodeNamespaceImport>
            {
                new CodeNamespaceImport("System"),
                new CodeNamespaceImport("System.Collections"),
                new CodeNamespaceImport("System.Collections.Generic"),
                new CodeNamespaceImport("System.Linq"),
                new CodeNamespaceImport("System.Threading"),
                new CodeNamespaceImport("Roslyn.Utilities"),
                new CodeNamespaceImport("Microsoft.CodeAnalysis.Syntax.InternalSyntax")
            };
            return list.ToArray();
        }

        protected CodeParameterDeclarationExpression GenerateParameter(string type, string name)
        {
            CodeParameterDeclarationExpression exp = new CodeParameterDeclarationExpression();
            exp.Name = name;
            exp.Type = new CodeTypeReference(type);
            return exp;
        }

        protected CodeCommentStatement GenerateComment(string summary)
        {
            CodeCommentStatement stmt = new CodeCommentStatement(summary, true);
            return stmt;
        }

        protected List<Field> GetNodeFields(RealNode nd)
        {
            return nd.Fields.Where(n => IsNodeOrNodeList(n.Type)).ToList();
        }

        protected List<Field> GetValueFields(RealNode nd)
        {
            return nd.Fields.Where(n => !IsNodeOrNodeList(n.Type)).ToList();
        }

        protected CodeAssignStatement GenerateAssignment(Field field)
        {
            CodeAssignStatement assign = new CodeAssignStatement();
            var left = new CodeVariableReferenceExpression(FieldName(field));
            var right = new CodeVariableReferenceExpression(ParameterName(field));
            return new CodeAssignStatement(left, right);
        }

        protected static string OverrideOrNewModifier(Field field)
        {
            return IsOverride(field) ? "override " : IsNew(field) ? "new " : "";
        }

        protected static bool CanBeField(Field field)
        {
            return field.Type != "SyntaxToken" && !IsAnyList(field.Type) && !IsOverride(field) && !IsNew(field);
        }

        protected static string GetFieldType(Field field, bool green)
        {
            if (IsAnyList(field.Type))
            {
                return green
                    ? "GreenNode"
                    : "SyntaxNode";
            }

            return field.Type;
        }

        protected CodeVariableDeclarationStatement VariableDeclaration(string type, string name)
        {
            return new CodeVariableDeclarationStatement(type, name);
        }

        protected bool IsDerivedOrListOfDerived(string baseType, string derivedType)
        {
            return IsDerivedType(baseType, derivedType)
                || ((IsNodeList(derivedType) || IsSeparatedNodeList(derivedType))
                    && IsDerivedType(baseType, GetElementType(derivedType)));
        }

        protected bool IsDerivedType(string typeName, string derivedTypeName)
        {
            if (typeName == derivedTypeName)
                return true;
            if (derivedTypeName != null && ParentMap.TryGetValue(derivedTypeName, out var baseType))
            {
                return IsDerivedType(typeName, baseType);
            }
            return false;
        }

        protected static bool IsSeparatedNodeList(string typeName)
        {
            return typeName.StartsWith("SeparatedSyntaxList<", StringComparison.Ordinal);
        }

        protected static bool IsNodeList(string typeName)
        {
            bool result = typeName.StartsWith("SyntaxList<", StringComparison.Ordinal);
            if (result)
            {
                string elementType = GetElementType(typeName);
                return elementType != "SyntaxToken";
            }
            return false;
        }

        protected bool HasField(Node node, Predicate<Field> predicate)
        {
            foreach (Field field in node.Fields)
            {
                if (predicate(field))
                {
                    return true;
                }
            }
            return false;
        }

        protected static bool IsAnyNodeList(string typeName)
        {
            return IsNodeList(typeName) || IsSeparatedNodeList(typeName);
        }

        protected bool IsNodeOrNodeList(string typeName)
        {
            return IsNode(typeName) || IsNodeList(typeName) || IsSeparatedNodeList(typeName) || typeName == "SyntaxNodeOrTokenList";
        }

        protected bool IsTokenOrTokenList(Field f)
        {
            return f.Type.Contains("Token");
        }

        protected bool IsSyntaxNodeOrTokenList(Field f)
        {
            return f.Type == "SyntaxNodeOrTokenList";
        }

        protected static string GetElementType(string typeName)
        {
            if (!typeName.Contains("<"))
                return string.Empty;
            int iStart = typeName.IndexOf('<');
            int iEnd = typeName.IndexOf('>', iStart + 1);
            if (iEnd < iStart)
                return string.Empty;
            var sub = typeName.Substring(iStart + 1, iEnd - iStart - 1);
            return sub;
        }

        protected static bool IsAnyList(string typeName)
        {
            return IsNodeList(typeName) || IsSeparatedNodeList(typeName) || typeName == "SyntaxNodeOrTokenList";
        }

        protected bool IsSyntaxList(Field f)
        {
            return !IsTokenList(f) && f.Type.StartsWith("SyntaxList<", StringComparison.OrdinalIgnoreCase);
        }

        protected bool IsSeparatedSyntaxList(Field f)
        {
            return f.Type.StartsWith("SeparatedSyntaxList<", StringComparison.OrdinalIgnoreCase);
        }

        protected bool IsTokenList(Field f)
        {
            return f.Type == "SyntaxTokenList" || f.Type == "SyntaxList<SyntaxToken>";
        }

        protected bool IsToken(Field f)
        {
            return f.Type.StartsWith("SyntaxToken", StringComparison.OrdinalIgnoreCase);
        }

        protected bool IsNode(string typeName)
        {
            return ParentMap.ContainsKey(typeName);
        }

        protected Node GetNode(string typeName)
        {
            return NodeMap.TryGetValue(typeName, out var node) ? node : null;
        }

        protected static bool IsOptional(Field f)
        {
            return f.Optional != null && String.Compare(f.Optional, "true", StringComparison.OrdinalIgnoreCase) == 0;
        }

        protected static bool IsOverride(Field f)
        {
            return f.Override != null && String.Compare(f.Override, "true", StringComparison.OrdinalIgnoreCase) == 0;
        }

        protected static bool IsNew(Field f)
        {
            return f.New != null && String.Compare(f.New, "true", StringComparison.OrdinalIgnoreCase) == 0;
        }

        protected bool IsValueField(Field field)
        {
            return !IsNodeOrNodeList(field.Type);
        }

        protected string GetGreenNodeName(Node node)
        {
            return $"{Tree.InternalNamespace}.{node.Name}";
        }

        protected CodeTypeReference CreateType(string name) => new CodeTypeReference(name);

        protected CodeExpression GetChildPosition(int position)
        {
            if (position == 0)
            {
                return VariableReference("Position");
            }

            CodeMethodInvokeExpression expression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    MethodName = "GetChildPosition"
                }
            };
            expression.Parameters.Add(new CodePrimitiveExpression(position));
            return expression;
        }

        protected CodeExpression GetChildIndex(int position)
        {
            if (position == 0)
            {
                return new CodePrimitiveExpression(0);
            }
            CodeMethodInvokeExpression expression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    MethodName = "GetChildIndex"
                }
            };
            expression.Parameters.Add(new CodePrimitiveExpression(position));
            return expression;
        }

        protected CodeVariableReferenceExpression VariableReference(string reference)
        {
            return new CodeVariableReferenceExpression(reference);
        }

        protected CodeStatementCollection GenerateConstructorStatements(List<Field> fields)
        {
            CodeStatementCollection col = new CodeStatementCollection();

            foreach (Field field in fields)
            {
                col.Add(GenerateAssignment(field));
            }

            return col;
        }

        protected void ProcessFields(CodeTypeDeclaration c, RealNode nd, Func<Field, int, CodeMemberField> fieldGenerator)
        {
            List<Field> nodeFields = GetNodeFields(nd);
            List<Field> valueFields = GetValueFields(nd);

            for (int i = 0; i < nodeFields.Count; i++)
            {
                Field f = nodeFields[i];
                CodeMemberField field = fieldGenerator(f, i);
                c.Members.Add(field);
            }
        }

        protected void ProcessProperties(
            CodeTypeDeclaration c,
            RealNode nd,
            Func<Field, int, CodeMemberProperty> nodePropertyGenerator,
            Func<Field, int, CodeMemberProperty> valuePropertyGenerator)
        {
            List<Field> valueFields = GetValueFields(nd);
            List<Field> nodeFields = GetNodeFields(nd);

            for (int i = 0; i < nodeFields.Count; i++)
            {
                Field field = nd.Fields[i];
                CodeMemberProperty property = nodePropertyGenerator(field, i);
                c.Members.Add(property);
            }
            for (int i = 0; i < valueFields.Count; i++)
            {
                Field field = valueFields[i];
                CodeMemberProperty property = valuePropertyGenerator(field, i);
                c.Members.Add(property);
            }
        }

        protected bool IsAutoCreatableToken(Node node, Field field)
        {
            return field.Type == "SyntaxToken"
                   && field.Kinds != null
                   && ((field.Kinds.Count == 1 && field.Kinds[0].Name != "IdentifierToken" && !field.Kinds[0].Name.EndsWith("LiteralToken", StringComparison.Ordinal)) || (field.Kinds.Count > 1 && field.Kinds.Count == node.Kinds.Count));
        }

        protected bool IsAutoCreatableNode(Field field)
        {
            var referencedNode = GetNode(field.Type);
            return (referencedNode != null && RequiredFactoryArgumentCount(referencedNode) == 0);
        }

        protected bool CanBeAutoCreated(Node node, Field field)
        {
            return IsAutoCreatableToken(node, field) || IsAutoCreatableNode(field);
        }

        protected bool IsRequiredFactoryField(Node node, Field field)
        {
            return (!IsOptional(field) && !IsAnyList(field.Type) && !CanBeAutoCreated(node, field)) || IsValueField(field);
        }

        protected int RequiredFactoryArgumentCount(Node nd, bool includeKind = true)
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

        protected IEnumerable<Node> GetNodes()
        {
            foreach (TreeType treeType in Tree.Types)
            {
                if (treeType is Node node)
                {
                    yield return node;
                }
            }
        }
    }
}


