// -----------------------------------------------------------------------
// <copyright file="RedNodePropertyGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom;

using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Red
{
    public class RedNodePropertyGenerator : AbstractCodeGenerator, IRedNodePropertyGenerator
    {
        public RedNodePropertyGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        /// <summary>
        ///     The GenerateAbstractNodeProperties
        /// </summary>
        /// <param name="node">The <see cref="AbstractNode" /></param>
        /// <returns>The <see cref="CodeTypeMemberCollection" /></returns>
        public CodeTypeMemberCollection GenerateAbstractNodeProperties(AbstractNode node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            var valueFields = GetValueFields(node);
            var nodeFields = GetNodeFields(node);
            for (int i = 0, n = nodeFields.Count; i < n; i++)
            {
                Field field = nodeFields[i];
                if (IsNodeOrNodeList(field.Type))
                {
                    var fieldType = GetRedPropertyType(field);
                    CodeMemberProperty property = new CodeMemberProperty
                    {
                        Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                        Name = field.Name,
                        Type = new CodeTypeReference(fieldType),
                        HasGet = true,
                        HasSet = false
                    };
                    collection.Add(property);
                }
            }
            for (int i = 0, n = valueFields.Count; i < n; i++)
            {
                Field field = valueFields[i];
                CodeMemberProperty property = new CodeMemberProperty
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                    Name = field.Name,
                    Type = new CodeTypeReference(field.Type),
                    HasGet = true,
                    HasSet = false
                };
                collection.Add(property);
            }
            return collection;
        }

        /// <summary>
        ///     The GenerateDefaultReturnStatement
        /// </summary>
        /// <param name="type">The <see cref="string" /></param>
        /// <returns>The <see cref="CodeMethodReturnStatement" /></returns>
        public CodeMethodReturnStatement GenerateDefaultReturnStatement(string type)
        {
            return new CodeMethodReturnStatement(new CodeDefaultValueExpression(CreateType(type)));
        }

        /// <summary>
        ///     The GenerateIfNotNullCondition
        /// </summary>
        /// <param name="variableName">The <see cref="string" /></param>
        /// <param name="createExpression">The <see cref="CodeObjectCreateExpression" /></param>
        /// <returns>The <see cref="CodeConditionStatement" /></returns>
        public CodeConditionStatement GenerateIfNotNullCondition(string variableName, CodeObjectCreateExpression createExpression)
        {
            CodeConditionStatement condition = new CodeConditionStatement();
            condition.Condition = new CodeBinaryOperatorExpression
            {
                Left = VariableReference(variableName),
                Operator = CodeBinaryOperatorType.IdentityInequality,
                Right = VariableReference("null")
            };
            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(createExpression);
            condition.TrueStatements.Add(returnStatement);
            return condition;
        }

        /// <summary>
        ///     The GenerateIfNotNullCondition
        /// </summary>
        /// <param name="variableName">The <see cref="string" /></param>
        /// <param name="createType">The <see cref="string" /></param>
        /// <param name="num">The <see cref="int" /></param>
        /// <returns>The <see cref="CodeConditionStatement" /></returns>
        public CodeConditionStatement GenerateIfNotNullCondition(string variableName, string createType, int num)
        {
            CodeConditionStatement condition = new CodeConditionStatement();
            condition.Condition = new CodeBinaryOperatorExpression
            {
                Left = VariableReference(variableName),
                Operator = CodeBinaryOperatorType.IdentityInequality,
                Right = VariableReference("null")
            };
            CodeObjectCreateExpression create = GeneratePropertyObjectCreate(num, CreateType(createType), VariableReference(variableName));
            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(create);
            condition.TrueStatements.Add(returnStatement);
            return condition;
        }

        /// <summary>
        ///     The GenerateInitializedVariable
        /// </summary>
        /// <param name="variableName">The <see cref="string" /></param>
        /// <param name="initExpression">The <see cref="CodeExpression" /></param>
        /// <returns>The <see cref="CodeVariableDeclarationStatement" /></returns>
        public CodeVariableDeclarationStatement GenerateInitializedVariable(string variableName, CodeExpression initExpression)
        {
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement();
            statement.Type = new CodeTypeReference("var");
            statement.Name = variableName;
            statement.InitExpression = initExpression;
            return statement;
        }

        /// <summary>
        ///     The GenerateMethodInvoke
        /// </summary>
        /// <param name="methodName">The <see cref="string" /></param>
        /// <param name="i">The <see cref="int" /></param>
        /// <param name="field">The <see cref="Field" /></param>
        /// <returns>The <see cref="CodeMethodInvokeExpression" /></returns>
        public CodeMethodInvokeExpression GenerateMethodInvoke(string methodName, int i, Field field)
        {
            if (i == 0)
            {
                methodName = methodName += "AtZero";
            }
            CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression();
            invoke.Method = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), methodName);
            invoke.Parameters.Add(VariableReference($"ref {FieldName(field)}"));
            if (i != 0)
                invoke.Parameters.Add(new CodePrimitiveExpression(i));
            return invoke;
        }

        /// <summary>
        ///     The GenerateNodeProperties
        /// </summary>
        /// <param name="node">The <see cref="Node" /></param>
        /// <returns>The <see cref="CodeTypeMemberCollection" /></returns>
        public CodeTypeMemberCollection GenerateNodeProperties(Node node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            var valueFields = GetValueFields(node);
            var nodeFields = GetNodeFields(node);
            var greenNodeName = GetGreenNodeName(node);
            for (int i = 0, n = nodeFields.Count; i < n; i++)
            {
                CodeMemberProperty property = new CodeMemberProperty();
                Field field = nodeFields[i];
                if (IsOverride(field))
                {
                    property.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                }
                else
                {
                    property.Attributes = MemberAttributes.Public;
                }
                property.Name = field.Name;
                property.Type = CreateType(GetRedPropertyType(field));

                if (field.Type == "SyntaxToken")
                {
                    if (IsOptional(field))
                    {
                        property.GetStatements.Add(GenerateInitializedVariable("slot", GeneratePropertyReference(node, field)));
                        property.GetStatements.Add(GenerateIfNotNullCondition("slot", "SyntaxToken", i));
                        property.GetStatements.Add(GenerateDefaultReturnStatement("SyntaxToken"));
                        collection.Add(property);
                    }
                    else
                    {
                        CodePropertyReferenceExpression propertyReference = GeneratePropertyReference(node, field);
                        CodeObjectCreateExpression create = GeneratePropertyObjectCreate(i, CreateType("SyntaxToken"), propertyReference);
                        CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(create);
                        property.GetStatements.Add(returnStatement);
                        collection.Add(property);
                    }
                }
                else if (field.Type == "SyntaxList<SyntaxToken>")
                {
                    property.GetStatements.Add(GenerateInitializedVariable("slot", VariableReference($"this.Green.GetSlot({i})")));
                    property.GetStatements.Add(GenerateIfNotNullCondition("slot",
                        GenerateObjectCreate(
                            CreateType("SyntaxTokenList"),
                            new CodeThisReferenceExpression(),
                            VariableReference("slot"),
                            GetChildPosition(i),
                            GetChildIndex(i))));
                    property.GetStatements.Add(GenerateDefaultReturnStatement("SyntaxTokenList"));
                    collection.Add(property);
                }
                else
                {
                    if (IsNodeList(field.Type))
                    {
                        CodeMethodReturnStatement statement = new CodeMethodReturnStatement();
                        CodeObjectCreateExpression expression = new CodeObjectCreateExpression();
                        expression.CreateType = CreateType(field.Type);
                        CodeMethodInvokeExpression invoke = GenerateMethodInvoke("GetRed", i, field);
                        expression.Parameters.Add(invoke);
                        statement.Expression = expression;
                        property.GetStatements.Add(statement);
                        collection.Add(property);
                    }
                    else if (IsSeparatedNodeList(field.Type))
                    {
                        property.GetStatements.Add(GenerateInitializedVariable("red", GenerateMethodInvoke("GetRed", i, field)));
                        property.GetStatements.Add(GenerateIfNotNullCondition("red",
                            GenerateObjectCreate(CreateType(field.Type), VariableReference("red"), GetChildIndex(i))));
                        property.GetStatements.Add(GenerateDefaultReturnStatement(field.Type));
                        collection.Add(property);
                    }
                    else if (field.Type == "SyntaxNodeOrTokenList")
                    {
                        throw new InvalidOperationException("field cannot be a random SyntaxNodeOrTokenList");
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CodeMethodReturnStatement statement = new CodeMethodReturnStatement();
                            CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                "GetRedAtZero",
                                VariableReference($"ref {FieldName(field)}"));
                            statement.Expression = invoke;
                            property.GetStatements.Add(statement);
                            collection.Add(property);
                        }
                        else
                        {
                            CodeMethodReturnStatement statement = new CodeMethodReturnStatement();
                            CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                "GetRed",
                                VariableReference($"ref {FieldName(field)}"),
                                new CodePrimitiveExpression(i));
                            statement.Expression = invoke;
                            property.GetStatements.Add(statement);
                            collection.Add(property);
                        }
                    }
                }
            }
            for (int i = 0, n = valueFields.Count; i < n; i++)
            {
                CodeMemberProperty property = new CodeMemberProperty();
                Field field = valueFields[i];
                if (IsOverride(field))
                {
                    property.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                }
                else
                {
                    property.Attributes = MemberAttributes.Public;
                }
                property.Name = field.Name;
                property.Type = CreateType(GetRedPropertyType(field));
                property.HasGet = true;
                property.HasSet = false;
                collection.Add(property);
            }
            return collection;
        }

        /// <summary>
        ///     The GenerateObjectCreate
        /// </summary>
        /// <param name="createType">The <see cref="CodeTypeReference" /></param>
        /// <param name="parameters">The <see cref="CodeExpression[]" /></param>
        /// <returns>The <see cref="CodeObjectCreateExpression" /></returns>
        public CodeObjectCreateExpression GenerateObjectCreate(CodeTypeReference createType, params CodeExpression[] parameters)
        {
            return new CodeObjectCreateExpression(
                createType,
                parameters
            );
        }

        /// <summary>
        ///     The GeneratePropertyObjectCreate
        /// </summary>
        /// <param name="i">The <see cref="int" /></param>
        /// <param name="createType">The <see cref="CodeTypeReference" /></param>
        /// <param name="secondParameter">The <see cref="CodeExpression" /></param>
        /// <returns>The <see cref="CodeObjectCreateExpression" /></returns>
        public CodeObjectCreateExpression GeneratePropertyObjectCreate(int i, CodeTypeReference createType, CodeExpression secondParameter)
        {
            return new CodeObjectCreateExpression(
                createType,
                new CodeThisReferenceExpression(),
                secondParameter,
                GetChildPosition(i),
                GetChildIndex(i)
            );
        }

        /// <summary>
        ///     The GeneratePropertyReference
        /// </summary>
        /// <param name="node">The <see cref="Node" /></param>
        /// <param name="field">The <see cref="Field" /></param>
        /// <returns>The <see cref="CodePropertyReferenceExpression" /></returns>
        public CodePropertyReferenceExpression GeneratePropertyReference(Node node, Field field)
        {
            CodeCastExpression cast = new CodeCastExpression
            {
                Expression = new CodeVariableReferenceExpression("this.Green"),
                TargetType = new CodeTypeReference(GetGreenNodeName(node))
            };
            CodePropertyReferenceExpression propertyReference = new CodePropertyReferenceExpression
            {
                PropertyName = field.Name,
                TargetObject = cast
            };
            return propertyReference;
        }

        /// <summary>
        ///     The GeneratePropertyReference
        /// </summary>
        /// <param name="nodeName">The <see cref="string" /></param>
        /// <param name="fieldName">The <see cref="string" /></param>
        /// <returns>The <see cref="CodePropertyReferenceExpression" /></returns>
        public CodePropertyReferenceExpression GeneratePropertyReference(string nodeName, string fieldName)
        {
            CodeCastExpression cast = new CodeCastExpression
            {
                Expression = new CodeVariableReferenceExpression("this.Green"),
                TargetType = new CodeTypeReference(nodeName)
            };
            CodePropertyReferenceExpression propertyReference = new CodePropertyReferenceExpression
            {
                PropertyName = fieldName,
                TargetObject = cast
            };
            return propertyReference;
        }
    }
}


