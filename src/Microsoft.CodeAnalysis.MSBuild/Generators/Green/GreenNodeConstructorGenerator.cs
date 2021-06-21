// -----------------------------------------------------------------------
// <copyright file="GreenNodeConstructorGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.MSBuild.Generators.Services;
using Microsoft.CodeAnalysis.MSBuild.Model;

namespace Microsoft.CodeAnalysis.MSBuild.Generators.Green
{
    public class GreenNodeConstructorGenerator : AbstractCodeGenerator, IGreenNodeConstructorGenerator
    {
        public GreenNodeConstructorGenerator(CodeGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        public CodeTypeMemberCollection GenerateAbstractNodeConstructors(AbstractNode node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            collection.Add(GenerateConstructorWithOutDiagnosticsAnnotations(node));
            collection.Add(GenerateConstructorWithDiagnosticsAnnotations(node));
            collection.Add(GenerateObjectReaderConstructor(node));
            return collection;
        }

        public CodeTypeMemberCollection GenerateNodeConstructors(Node node)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            collection.Add(GenerateConstructorWithOutDiagnosticsAnnotations(node));
            collection.Add(GenerateConstructorWithDiagnosticsAnnotations(node));
            collection.Add(GenerateObjectReaderConstructor(node));
            return collection;
        }

        private CodeConstructor GenerateObjectReaderConstructor(AbstractNode node)
        {
            CodeConstructor ctor = new CodeConstructor
            {
                Name = node.Name,
                Attributes = MemberAttributes.FamilyAndAssembly
            };
            ctor.Parameters.Add(GenerateParameter("ObjectReader", "reader"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("reader"));
            return ctor;
        }

        private CodeConstructor GenerateObjectReaderConstructor(Node node)
        {
            CodeConstructor ctor = new CodeConstructor
            {
                Name = node.Name,
                Attributes = MemberAttributes.FamilyAndAssembly
            };
            ctor.Parameters.Add(GenerateParameter("ObjectReader", "reader"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("reader"));
            return ctor;
        }

        private CodeConstructor GenerateConstructorWithDiagnosticsAnnotations(AbstractNode node)
        {
            CodeConstructor ctor = new CodeConstructor
            {
                Name = node.Name,
                Attributes = MemberAttributes.Assembly
            };
            ctor.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
            ctor.Parameters.Add(GenerateParameter("DiagnosticInfo[]", "diagnostics"));
            ctor.Parameters.Add(GenerateParameter("SyntaxAnnotation[]", "annotations"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("kind"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("diagnostics"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("annotations"));
            return ctor;
        }

        private CodeConstructor GenerateConstructorWithDiagnosticsAnnotations(Node node)
        {
            CodeConstructor ctor = new CodeConstructor
            {
                Name = node.Name,
                Attributes = MemberAttributes.Assembly
            };
            ctor.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));

            var valueFields = GetValueFields(node);
            var nodeFields = GetNodeFields(node);

            GenerateCtorParameters(ctor, nodeFields, valueFields);

            ctor.Parameters.Add(GenerateParameter("DiagnosticInfo[]", "diagnostics"));
            ctor.Parameters.Add(GenerateParameter("SyntaxAnnotation[]", "annotations"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("kind"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("diagnostics"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("annotations"));

            GenerateCtorBody(ctor, nodeFields, valueFields);
            return ctor;
        }

        private CodeConstructor GenerateConstructorWithOutDiagnosticsAnnotations(AbstractNode node)
        {
            CodeConstructor ctor = new CodeConstructor
            {
                Name = node.Name,
                Attributes = MemberAttributes.Assembly
            };
            ctor.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("kind"));
            return ctor;
        }

        private CodeConstructor GenerateConstructorWithOutDiagnosticsAnnotations(Node node)
        {
            CodeConstructor ctor = new CodeConstructor
            {
                Name = node.Name,
                Attributes = MemberAttributes.Assembly
            };
            ctor.Parameters.Add(GenerateParameter("SyntaxKind", "kind"));
            var valueFields = GetValueFields(node);
            var nodeFields = GetNodeFields(node);

            GenerateCtorParameters(ctor,  nodeFields, valueFields);

            ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("kind"));

            GenerateCtorBody(ctor, nodeFields, valueFields);

            return ctor;
        }

        private void GenerateCtorParameters(CodeConstructor ctor,  List<Field> nodeFields, List<Field> valueFields)
        {
            for (int i = 0, n = nodeFields.Count; i < n; i++)
            {
                var field = nodeFields[i];
                string type = GetFieldType(field, green: true);
                ctor.Parameters.Add(GenerateParameter(type, ParameterName(field)));
            }

            for (int i = 0, n = valueFields.Count; i < n; i++)
            {
                var field = valueFields[i];
                ctor.Parameters.Add(GenerateParameter(field.Type, ParameterName(field)));
            }
        }

        private void GenerateCtorBody(CodeConstructor ctor, List<Field> nodeFields, List<Field> valueFields)
        {
            ctor.Statements.Add(new CodeAssignStatement
            {
                Left = VariableReference("SlotCount"),
                Right = new CodePrimitiveExpression(nodeFields.Count)
            });

            for (int i = 0, n = nodeFields.Count; i < n; i++)
            {
                var field = nodeFields[i];
                if (IsAnyList(field.Type) || IsOptional(field))
                {
                    CodeConditionStatement condition = new CodeConditionStatement
                    {
                        Condition = new CodeBinaryOperatorExpression(
                            VariableReference(ParameterName(field)),
                            CodeBinaryOperatorType.IdentityInequality,
                            VariableReference("null"))
                    };
                    condition.TrueStatements.Add(GenerateAdjustFlagsAndWidthExpression(field));
                    condition.TrueStatements.Add(GenerateAssignment(field));
                    ctor.Statements.Add(condition);
                }
                else
                {
                    CodeConditionStatement condition = new CodeConditionStatement
                    {
                        Condition = new CodeBinaryOperatorExpression(
                        VariableReference(ParameterName(field)),
                        CodeBinaryOperatorType.IdentityInequality,
                        VariableReference("null"))
                    };
                    condition.TrueStatements.Add(GenerateAdjustFlagsAndWidthExpression(field));
                    condition.TrueStatements.Add(GenerateAssignment(field));
                    ctor.Statements.Add(condition);
                }
            }
            for (int i = 0, n = valueFields.Count; i < n; i++)
            {
                var field = valueFields[i];
                CodeConditionStatement condition = new CodeConditionStatement
                {
                    Condition = new CodeBinaryOperatorExpression(
                        VariableReference(ParameterName(field)),
                        CodeBinaryOperatorType.IdentityInequality,
                        VariableReference("null"))
                };
                condition.TrueStatements.Add(GenerateAdjustFlagsAndWidthExpression(field));
                condition.TrueStatements.Add(GenerateAssignment(field));
                ctor.Statements.Add(condition);
            }
        }

        private CodeMethodInvokeExpression GenerateAdjustFlagsAndWidthExpression(Field field)
        {
            return new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression(
                                            new CodeThisReferenceExpression(),
                                            "AdjustFlagsAndWidth"),
                Parameters =
                            {
                                VariableReference(ParameterName(field))
                            }
            };
        }
    }
}


