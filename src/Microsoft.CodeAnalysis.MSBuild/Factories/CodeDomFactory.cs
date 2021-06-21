// -----------------------------------------------------------------------
// <copyright file="CodeDomFactory.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CodeAnalysis.MSBuild.Factories
{
    /// <summary>
    /// Represents a factory for creating CodeDom code objects.
    /// </summary>
    /// <see cref="CodeObject"/>
    public static class CodeDomFactory
    {
        /// <summary>
        /// Creates a new <see cref="CodeArgumentReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeArgumentReferenceExpression"/></returns>
        public static CodeArgumentReferenceExpression ArgumentReferenceExpression()
        {
            return new CodeArgumentReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeArgumentReferenceExpression"/> code object.
        /// </summary>
        /// <param name="parameterName"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeArgumentReferenceExpression"/></returns>
        public static CodeArgumentReferenceExpression ArgumentReferenceExpression(string parameterName)
        {
            return new CodeArgumentReferenceExpression(parameterName);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="Type"/></param>
        /// <param name="size"> the <see cref="Int32"/></param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(Type createType, Int32 size)
        {
            return new CodeArrayCreateExpression(createType, size);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression()
        {
            return new CodeArrayCreateExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="initializers"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(CodeTypeReference createType, params CodeExpression[] initializers)
        {
            return new CodeArrayCreateExpression(createType, initializers);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="initializers"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(CodeTypeReference createType, IEnumerable<CodeExpression> initializers)
        {
            return new CodeArrayCreateExpression(createType, initializers.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="string"/></param>
        /// <param name="initializers"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(string createType, params CodeExpression[] initializers)
        {
            return new CodeArrayCreateExpression(createType, initializers);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="string"/></param>
        /// <param name="initializers"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(string createType, IEnumerable<CodeExpression> initializers)
        {
            return new CodeArrayCreateExpression(createType, initializers.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="Type"/></param>
        /// <param name="initializers"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(Type createType, params CodeExpression[] initializers)
        {
            return new CodeArrayCreateExpression(createType, initializers);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="Type"/></param>
        /// <param name="initializers"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(Type createType, IEnumerable<CodeExpression> initializers)
        {
            return new CodeArrayCreateExpression(createType, initializers.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="size"> the <see cref="Int32"/></param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(CodeTypeReference createType, Int32 size)
        {
            return new CodeArrayCreateExpression(createType, size);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="string"/></param>
        /// <param name="size"> the <see cref="Int32"/></param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(string createType, Int32 size)
        {
            return new CodeArrayCreateExpression(createType, size);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="size"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(CodeTypeReference createType, CodeExpression size)
        {
            return new CodeArrayCreateExpression(createType, size);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="string"/></param>
        /// <param name="size"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(string createType, CodeExpression size)
        {
            return new CodeArrayCreateExpression(createType, size);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="Type"/></param>
        /// <param name="size"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeArrayCreateExpression"/></returns>
        public static CodeArrayCreateExpression ArrayCreateExpression(Type createType, CodeExpression size)
        {
            return new CodeArrayCreateExpression(createType, size);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayIndexerExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeArrayIndexerExpression"/></returns>
        public static CodeArrayIndexerExpression ArrayIndexerExpression()
        {
            return new CodeArrayIndexerExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayIndexerExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="indices"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeArrayIndexerExpression"/></returns>
        public static CodeArrayIndexerExpression ArrayIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
        {
            return new CodeArrayIndexerExpression(targetObject, indices);
        }

        /// <summary>
        /// Creates a new <see cref="CodeArrayIndexerExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="indices"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeArrayIndexerExpression"/></returns>
        public static CodeArrayIndexerExpression ArrayIndexerExpression(CodeExpression targetObject, IEnumerable<CodeExpression> indices)
        {
            return new CodeArrayIndexerExpression(targetObject, indices.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeAssignStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeAssignStatement"/></returns>
        public static CodeAssignStatement AssignStatement()
        {
            return new CodeAssignStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeAssignStatement"/> code object.
        /// </summary>
        /// <param name="left"> the <see cref="CodeExpression"/></param>
        /// <param name="right"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeAssignStatement"/></returns>
        public static CodeAssignStatement AssignStatement(CodeExpression left, CodeExpression right)
        {
            return new CodeAssignStatement(left, right);
        }

        /// <summary>
        /// Creates a new <see cref="CodeAttachEventStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeAttachEventStatement"/></returns>
        public static CodeAttachEventStatement AttachEventStatement()
        {
            return new CodeAttachEventStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeAttachEventStatement"/> code object.
        /// </summary>
        /// <param name="eventRef"> the <see cref="CodeEventReferenceExpression"/></param>
        /// <param name="listener"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeAttachEventStatement"/></returns>
        public static CodeAttachEventStatement AttachEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener)
        {
            return new CodeAttachEventStatement(eventRef, listener);
        }

        /// <summary>
        /// Creates a new <see cref="CodeAttachEventStatement"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="eventName"> the <see cref="string"/></param>
        /// <param name="listener"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeAttachEventStatement"/></returns>
        public static CodeAttachEventStatement AttachEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener)
        {
            return new CodeAttachEventStatement(targetObject, eventName, listener);
        }

        /// <summary>
        /// Creates a new <see cref="CodeBaseReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeBaseReferenceExpression"/></returns>
        public static CodeBaseReferenceExpression BaseReferenceExpression()
        {
            return new CodeBaseReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeBinaryOperatorExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeBinaryOperatorExpression"/></returns>
        public static CodeBinaryOperatorExpression BinaryOperatorExpression()
        {
            return new CodeBinaryOperatorExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeBinaryOperatorExpression"/> code object.
        /// </summary>
        /// <param name="left"> the <see cref="CodeExpression"/></param>
        /// <param name="op"> the <see cref="CodeBinaryOperatorType"/></param>
        /// <param name="right"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeBinaryOperatorExpression"/></returns>
        public static CodeBinaryOperatorExpression BinaryOperatorExpression(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, op, right);
        }

        /// <summary>
        /// Creates a new <see cref="CodeCastExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeCastExpression"/></returns>
        public static CodeCastExpression CastExpression()
        {
            return new CodeCastExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeCastExpression"/> code object.
        /// </summary>
        /// <param name="targetType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="expression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeCastExpression"/></returns>
        public static CodeCastExpression CastExpression(CodeTypeReference targetType, CodeExpression expression)
        {
            return new CodeCastExpression(targetType, expression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeCastExpression"/> code object.
        /// </summary>
        /// <param name="targetType"> the <see cref="string"/></param>
        /// <param name="expression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeCastExpression"/></returns>
        public static CodeCastExpression CastExpression(string targetType, CodeExpression expression)
        {
            return new CodeCastExpression(targetType, expression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeCastExpression"/> code object.
        /// </summary>
        /// <param name="targetType"> the <see cref="Type"/></param>
        /// <param name="expression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeCastExpression"/></returns>
        public static CodeCastExpression CastExpression(Type targetType, CodeExpression expression)
        {
            return new CodeCastExpression(targetType, expression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeChecksumPragma"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeChecksumPragma"/></returns>
        public static CodeChecksumPragma ChecksumPragma()
        {
            return new CodeChecksumPragma();
        }

        /// <summary>
        /// Creates a new <see cref="CodeChecksumPragma"/> code object.
        /// </summary>
        /// <param name="fileName"> the <see cref="string"/></param>
        /// <param name="checksumAlgorithmId"> the <see cref="Guid"/></param>
        /// <param name="checksumData"> a <see cref="Byte"/> array.</param>
        /// <returns>a <see cref="CodeChecksumPragma"/></returns>
        public static CodeChecksumPragma ChecksumPragma(string fileName, Guid checksumAlgorithmId, params Byte[] checksumData)
        {
            return new CodeChecksumPragma(fileName, checksumAlgorithmId, checksumData);
        }

        /// <summary>
        /// Creates a new <see cref="CodeChecksumPragma"/> code object.
        /// </summary>
        /// <param name="fileName"> the <see cref="string"/></param>
        /// <param name="checksumAlgorithmId"> the <see cref="Guid"/></param>
        /// <param name="checksumData"> a <see cref="Byte"/> array.</param>
        /// <returns>a <see cref="CodeChecksumPragma"/></returns>
        public static CodeChecksumPragma ChecksumPragma(string fileName, Guid checksumAlgorithmId, IEnumerable<Byte> checksumData)
        {
            return new CodeChecksumPragma(fileName, checksumAlgorithmId, checksumData.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeComment"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeComment"/></returns>
        public static CodeComment Comment()
        {
            return new CodeComment();
        }

        /// <summary>
        /// Creates a new <see cref="CodeComment"/> code object.
        /// </summary>
        /// <param name="text"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeComment"/></returns>
        public static CodeComment Comment(string text)
        {
            return new CodeComment(text);
        }

        /// <summary>
        /// Creates a new <see cref="CodeComment"/> code object.
        /// </summary>
        /// <param name="text"> the <see cref="string"/></param>
        /// <param name="docComment"> the <see cref="bool"/></param>
        /// <returns>a <see cref="CodeComment"/></returns>
        public static CodeComment Comment(string text, bool docComment)
        {
            return new CodeComment(text, docComment);
        }

        /// <summary>
        /// Creates a new <see cref="CodeCommentStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeCommentStatement"/></returns>
        public static CodeCommentStatement CommentStatement()
        {
            return new CodeCommentStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeCommentStatement"/> code object.
        /// </summary>
        /// <param name="comment"> the <see cref="CodeComment"/></param>
        /// <returns>a <see cref="CodeCommentStatement"/></returns>
        public static CodeCommentStatement CommentStatement(CodeComment comment)
        {
            return new CodeCommentStatement(comment);
        }

        /// <summary>
        /// Creates a new <see cref="CodeCommentStatement"/> code object.
        /// </summary>
        /// <param name="text"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeCommentStatement"/></returns>
        public static CodeCommentStatement CommentStatement(string text)
        {
            return new CodeCommentStatement(text);
        }

        /// <summary>
        /// Creates a new <see cref="CodeCommentStatement"/> code object.
        /// </summary>
        /// <param name="text"> the <see cref="string"/></param>
        /// <param name="docComment"> the <see cref="bool"/></param>
        /// <returns>a <see cref="CodeCommentStatement"/></returns>
        public static CodeCommentStatement CommentStatement(string text, bool docComment)
        {
            return new CodeCommentStatement(text, docComment);
        }

        /// <summary>
        /// Creates a new <see cref="CodeCompileUnit"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeCompileUnit"/></returns>
        public static CodeCompileUnit CompileUnit()
        {
            return new CodeCompileUnit();
        }

        /// <summary>
        /// Creates a new <see cref="CodeConditionStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeConditionStatement"/></returns>
        public static CodeConditionStatement ConditionStatement()
        {
            return new CodeConditionStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeConditionStatement"/> code object.
        /// </summary>
        /// <param name="condition"> the <see cref="CodeExpression"/></param>
        /// <param name="trueStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <returns>a <see cref="CodeConditionStatement"/></returns>
        public static CodeConditionStatement ConditionStatement(CodeExpression condition, params CodeStatement[] trueStatements)
        {
            return new CodeConditionStatement(condition, trueStatements);
        }

        /// <summary>
        /// Creates a new <see cref="CodeConditionStatement"/> code object.
        /// </summary>
        /// <param name="condition"> the <see cref="CodeExpression"/></param>
        /// <param name="trueStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <returns>a <see cref="CodeConditionStatement"/></returns>
        public static CodeConditionStatement ConditionStatement(CodeExpression condition, IEnumerable<CodeStatement> trueStatements)
        {
            return new CodeConditionStatement(condition, trueStatements.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeConditionStatement"/> code object.
        /// </summary>
        /// <param name="condition"> the <see cref="CodeExpression"/></param>
        /// <param name="trueStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <param name="falseStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <returns>a <see cref="CodeConditionStatement"/></returns>
        public static CodeConditionStatement ConditionStatement(CodeExpression condition, CodeStatement[] trueStatements, params CodeStatement[] falseStatements)
        {
            return new CodeConditionStatement(condition, trueStatements, falseStatements);
        }

        /// <summary>
        /// Creates a new <see cref="CodeConditionStatement"/> code object.
        /// </summary>
        /// <param name="condition"> the <see cref="CodeExpression"/></param>
        /// <param name="trueStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <param name="falseStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <returns>a <see cref="CodeConditionStatement"/></returns>
        public static CodeConditionStatement ConditionStatement(CodeExpression condition, IEnumerable<CodeStatement> trueStatements, IEnumerable<CodeStatement> falseStatements)
        {
            return new CodeConditionStatement(condition, trueStatements.ToArray(), falseStatements.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeConstructor"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeConstructor"/></returns>
        public static CodeConstructor Constructor()
        {
            return new CodeConstructor();
        }

        /// <summary>
        /// Creates a new <see cref="CodeDefaultValueExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeDefaultValueExpression"/></returns>
        public static CodeDefaultValueExpression DefaultValueExpression()
        {
            return new CodeDefaultValueExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeDefaultValueExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="CodeTypeReference"/></param>
        /// <returns>a <see cref="CodeDefaultValueExpression"/></returns>
        public static CodeDefaultValueExpression DefaultValueExpression(CodeTypeReference type)
        {
            return new CodeDefaultValueExpression(type);
        }

        /// <summary>
        /// Creates a new <see cref="CodeDelegateCreateExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeDelegateCreateExpression"/></returns>
        public static CodeDelegateCreateExpression DelegateCreateExpression()
        {
            return new CodeDelegateCreateExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeDelegateCreateExpression"/> code object.
        /// </summary>
        /// <param name="delegateType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="methodName"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeDelegateCreateExpression"/></returns>
        public static CodeDelegateCreateExpression DelegateCreateExpression(CodeTypeReference delegateType, CodeExpression targetObject, string methodName)
        {
            return new CodeDelegateCreateExpression(delegateType, targetObject, methodName);
        }

        /// <summary>
        /// Creates a new <see cref="CodeDelegateInvokeExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeDelegateInvokeExpression"/></returns>
        public static CodeDelegateInvokeExpression DelegateInvokeExpression()
        {
            return new CodeDelegateInvokeExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeDelegateInvokeExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeDelegateInvokeExpression"/></returns>
        public static CodeDelegateInvokeExpression DelegateInvokeExpression(CodeExpression targetObject)
        {
            return new CodeDelegateInvokeExpression(targetObject);
        }

        /// <summary>
        /// Creates a new <see cref="CodeDelegateInvokeExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeDelegateInvokeExpression"/></returns>
        public static CodeDelegateInvokeExpression DelegateInvokeExpression(CodeExpression targetObject, params CodeExpression[] parameters)
        {
            return new CodeDelegateInvokeExpression(targetObject, parameters);
        }

        /// <summary>
        /// Creates a new <see cref="CodeDelegateInvokeExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeDelegateInvokeExpression"/></returns>
        public static CodeDelegateInvokeExpression DelegateInvokeExpression(CodeExpression targetObject, IEnumerable<CodeExpression> parameters)
        {
            return new CodeDelegateInvokeExpression(targetObject, parameters.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeDirectionExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeDirectionExpression"/></returns>
        public static CodeDirectionExpression DirectionExpression()
        {
            return new CodeDirectionExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeDirectionExpression"/> code object.
        /// </summary>
        /// <param name="direction"> the <see cref="FieldDirection"/></param>
        /// <param name="expression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeDirectionExpression"/></returns>
        public static CodeDirectionExpression DirectionExpression(FieldDirection direction, CodeExpression expression)
        {
            return new CodeDirectionExpression(direction, expression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeDirective"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeDirective"/></returns>
        public static CodeDirective Directive()
        {
            return new CodeDirective();
        }

        /// <summary>
        /// Creates a new <see cref="CodeEntryPointMethod"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeEntryPointMethod"/></returns>
        public static CodeEntryPointMethod EntryPointMethod()
        {
            return new CodeEntryPointMethod();
        }

        /// <summary>
        /// Creates a new <see cref="CodeEventReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeEventReferenceExpression"/></returns>
        public static CodeEventReferenceExpression EventReferenceExpression()
        {
            return new CodeEventReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeEventReferenceExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="eventName"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeEventReferenceExpression"/></returns>
        public static CodeEventReferenceExpression EventReferenceExpression(CodeExpression targetObject, string eventName)
        {
            return new CodeEventReferenceExpression(targetObject, eventName);
        }

        /// <summary>
        /// Creates a new <see cref="CodeExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeExpression"/></returns>
        public static CodeExpression Expression()
        {
            return new CodeExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeExpressionStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeExpressionStatement"/></returns>
        public static CodeExpressionStatement ExpressionStatement()
        {
            return new CodeExpressionStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeExpressionStatement"/> code object.
        /// </summary>
        /// <param name="expression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeExpressionStatement"/></returns>
        public static CodeExpressionStatement ExpressionStatement(CodeExpression expression)
        {
            return new CodeExpressionStatement(expression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeFieldReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeFieldReferenceExpression"/></returns>
        public static CodeFieldReferenceExpression FieldReferenceExpression()
        {
            return new CodeFieldReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeFieldReferenceExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="fieldName"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeFieldReferenceExpression"/></returns>
        public static CodeFieldReferenceExpression FieldReferenceExpression(CodeExpression targetObject, string fieldName)
        {
            return new CodeFieldReferenceExpression(targetObject, fieldName);
        }

        /// <summary>
        /// Creates a new <see cref="CodeGotoStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeGotoStatement"/></returns>
        public static CodeGotoStatement GotoStatement()
        {
            return new CodeGotoStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeGotoStatement"/> code object.
        /// </summary>
        /// <param name="label"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeGotoStatement"/></returns>
        public static CodeGotoStatement GotoStatement(string label)
        {
            return new CodeGotoStatement(label);
        }

        /// <summary>
        /// Creates a new <see cref="CodeIndexerExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeIndexerExpression"/></returns>
        public static CodeIndexerExpression IndexerExpression()
        {
            return new CodeIndexerExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeIndexerExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="indices"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeIndexerExpression"/></returns>
        public static CodeIndexerExpression IndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
        {
            return new CodeIndexerExpression(targetObject, indices);
        }

        /// <summary>
        /// Creates a new <see cref="CodeIndexerExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="indices"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeIndexerExpression"/></returns>
        public static CodeIndexerExpression IndexerExpression(CodeExpression targetObject, IEnumerable<CodeExpression> indices)
        {
            return new CodeIndexerExpression(targetObject, indices.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeIterationStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeIterationStatement"/></returns>
        public static CodeIterationStatement IterationStatement()
        {
            return new CodeIterationStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeIterationStatement"/> code object.
        /// </summary>
        /// <param name="initStatement"> the <see cref="CodeStatement"/></param>
        /// <param name="testExpression"> the <see cref="CodeExpression"/></param>
        /// <param name="incrementStatement"> the <see cref="CodeStatement"/></param>
        /// <param name="statements"> a <see cref="CodeStatement"/> array.</param>
        /// <returns>a <see cref="CodeIterationStatement"/></returns>
        public static CodeIterationStatement IterationStatement(CodeStatement initStatement, CodeExpression testExpression, CodeStatement incrementStatement, params CodeStatement[] statements)
        {
            return new CodeIterationStatement(initStatement, testExpression, incrementStatement, statements);
        }

        /// <summary>
        /// Creates a new <see cref="CodeIterationStatement"/> code object.
        /// </summary>
        /// <param name="initStatement"> the <see cref="CodeStatement"/></param>
        /// <param name="testExpression"> the <see cref="CodeExpression"/></param>
        /// <param name="incrementStatement"> the <see cref="CodeStatement"/></param>
        /// <param name="statements"> a <see cref="CodeStatement"/> array.</param>
        /// <returns>a <see cref="CodeIterationStatement"/></returns>
        public static CodeIterationStatement IterationStatement(CodeStatement initStatement, CodeExpression testExpression, CodeStatement incrementStatement, IEnumerable<CodeStatement> statements)
        {
            return new CodeIterationStatement(initStatement, testExpression, incrementStatement, statements.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeLabeledStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeLabeledStatement"/></returns>
        public static CodeLabeledStatement LabeledStatement()
        {
            return new CodeLabeledStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeLabeledStatement"/> code object.
        /// </summary>
        /// <param name="label"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeLabeledStatement"/></returns>
        public static CodeLabeledStatement LabeledStatement(string label)
        {
            return new CodeLabeledStatement(label);
        }

        /// <summary>
        /// Creates a new <see cref="CodeLabeledStatement"/> code object.
        /// </summary>
        /// <param name="label"> the <see cref="string"/></param>
        /// <param name="statement"> the <see cref="CodeStatement"/></param>
        /// <returns>a <see cref="CodeLabeledStatement"/></returns>
        public static CodeLabeledStatement LabeledStatement(string label, CodeStatement statement)
        {
            return new CodeLabeledStatement(label, statement);
        }

        /// <summary>
        /// Creates a new <see cref="CodeMemberEvent"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeMemberEvent"/></returns>
        public static CodeMemberEvent MemberEvent()
        {
            return new CodeMemberEvent();
        }

        /// <summary>
        /// Creates a new <see cref="CodeMemberField"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeMemberField"/></returns>
        public static CodeMemberField Field()
        {
            return new CodeMemberField();
        }

        /// <summary>
        /// Creates a new <see cref="CodeMemberField"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="CodeTypeReference"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeMemberField"/></returns>
        public static CodeMemberField Field(CodeTypeReference type, string name)
        {
            return new CodeMemberField(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeMemberField"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="string"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeMemberField"/></returns>
        public static CodeMemberField Field(string type, string name)
        {
            return new CodeMemberField(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeMemberField"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="Type"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeMemberField"/></returns>
        public static CodeMemberField Field(Type type, string name)
        {
            return new CodeMemberField(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeMemberMethod"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeMemberMethod"/></returns>
        public static CodeMemberMethod MemberMethod()
        {
            return new CodeMemberMethod();
        }

        /// <summary>
        /// Creates a new <see cref="CodeMemberProperty"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeMemberProperty"/></returns>
        public static CodeMemberProperty MemberProperty()
        {
            return new CodeMemberProperty();
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodInvokeExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeMethodInvokeExpression"/></returns>
        public static CodeMethodInvokeExpression InvokeExpression()
        {
            return new CodeMethodInvokeExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodInvokeExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="methodName"> the <see cref="string"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeMethodInvokeExpression"/></returns>
        public static CodeMethodInvokeExpression InvokeExpression(CodeExpression targetObject, string methodName, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(targetObject, methodName, parameters);
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodInvokeExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="methodName"> the <see cref="string"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeMethodInvokeExpression"/></returns>
        public static CodeMethodInvokeExpression InvokeExpression(CodeExpression targetObject, string methodName, IEnumerable<CodeExpression> parameters)
        {
            return new CodeMethodInvokeExpression(targetObject, methodName, parameters.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodInvokeExpression"/> code object.
        /// </summary>
        /// <param name="method"> the <see cref="CodeMethodReferenceExpression"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeMethodInvokeExpression"/></returns>
        public static CodeMethodInvokeExpression InvokeExpression(CodeMethodReferenceExpression method, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(method, parameters);
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodInvokeExpression"/> code object.
        /// </summary>
        /// <param name="method"> the <see cref="CodeMethodReferenceExpression"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeMethodInvokeExpression"/></returns>
        public static CodeMethodInvokeExpression InvokeExpression(CodeMethodReferenceExpression method, IEnumerable<CodeExpression> parameters)
        {
            return new CodeMethodInvokeExpression(method, parameters.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeMethodReferenceExpression"/></returns>
        public static CodeMethodReferenceExpression MethodReferenceExpression()
        {
            return new CodeMethodReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodReferenceExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="methodName"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeMethodReferenceExpression"/></returns>
        public static CodeMethodReferenceExpression MethodReferenceExpression(CodeExpression targetObject, string methodName)
        {
            return new CodeMethodReferenceExpression(targetObject, methodName);
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodReferenceExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="methodName"> the <see cref="string"/></param>
        /// <param name="typeParameters"> a <see cref="CodeTypeReference"/> array.</param>
        /// <returns>a <see cref="CodeMethodReferenceExpression"/></returns>
        public static CodeMethodReferenceExpression MethodReferenceExpression(CodeExpression targetObject, string methodName, params CodeTypeReference[] typeParameters)
        {
            return new CodeMethodReferenceExpression(targetObject, methodName, typeParameters);
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodReferenceExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="methodName"> the <see cref="string"/></param>
        /// <param name="typeParameters"> a <see cref="CodeTypeReference"/> array.</param>
        /// <returns>a <see cref="CodeMethodReferenceExpression"/></returns>
        public static CodeMethodReferenceExpression MethodReferenceExpression(CodeExpression targetObject, string methodName, IEnumerable<CodeTypeReference> typeParameters)
        {
            return new CodeMethodReferenceExpression(targetObject, methodName, typeParameters.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodReturnStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeMethodReturnStatement"/></returns>
        public static CodeMethodReturnStatement ReturnStatement()
        {
            return new CodeMethodReturnStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeMethodReturnStatement"/> code object.
        /// </summary>
        /// <param name="expression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeMethodReturnStatement"/></returns>
        public static CodeMethodReturnStatement ReturnStatement(CodeExpression expression)
        {
            return new CodeMethodReturnStatement(expression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeNamespace"/> code object.
        /// </summary>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeNamespace"/></returns>
        public static CodeNamespace Namespace(string name)
        {
            return new CodeNamespace(name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeNamespace"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeNamespace"/></returns>
        public static CodeNamespace Namespace()
        {
            return new CodeNamespace();
        }

        /// <summary>
        /// Creates a new <see cref="CodeNamespaceImport"/> code object.
        /// </summary>
        /// <param name="nameSpace"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeNamespaceImport"/></returns>
        public static CodeNamespaceImport NamespaceImport(string nameSpace)
        {
            return new CodeNamespaceImport(nameSpace);
        }

        /// <summary>
        /// Creates a new <see cref="CodeNamespaceImport"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeNamespaceImport"/></returns>
        public static CodeNamespaceImport NamespaceImport()
        {
            return new CodeNamespaceImport();
        }

        /// <summary>
        /// Creates a new <see cref="CodeObject"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeObject"/></returns>
        public static CodeObject Object()
        {
            return new CodeObject();
        }

        /// <summary>
        /// Creates a new <see cref="CodeObjectCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="string"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeObjectCreateExpression"/></returns>
        public static CodeObjectCreateExpression ObjectCreateExpression(string createType, params CodeExpression[] parameters)
        {
            return new CodeObjectCreateExpression(createType, parameters);
        }

        /// <summary>
        /// Creates a new <see cref="CodeObjectCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="string"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeObjectCreateExpression"/></returns>
        public static CodeObjectCreateExpression ObjectCreateExpression(string createType, IEnumerable<CodeExpression> parameters)
        {
            return new CodeObjectCreateExpression(createType, parameters.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeObjectCreateExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeObjectCreateExpression"/></returns>
        public static CodeObjectCreateExpression ObjectCreateExpression()
        {
            return new CodeObjectCreateExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeObjectCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeObjectCreateExpression"/></returns>
        public static CodeObjectCreateExpression ObjectCreateExpression(CodeTypeReference createType, params CodeExpression[] parameters)
        {
            return new CodeObjectCreateExpression(createType, parameters);
        }

        /// <summary>
        /// Creates a new <see cref="CodeObjectCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeObjectCreateExpression"/></returns>
        public static CodeObjectCreateExpression ObjectCreateExpression(CodeTypeReference createType, IEnumerable<CodeExpression> parameters)
        {
            return new CodeObjectCreateExpression(createType, parameters.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeObjectCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="Type"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeObjectCreateExpression"/></returns>
        public static CodeObjectCreateExpression ObjectCreateExpression(Type createType, params CodeExpression[] parameters)
        {
            return new CodeObjectCreateExpression(createType, parameters);
        }

        /// <summary>
        /// Creates a new <see cref="CodeObjectCreateExpression"/> code object.
        /// </summary>
        /// <param name="createType"> the <see cref="Type"/></param>
        /// <param name="parameters"> a <see cref="CodeExpression"/> array.</param>
        /// <returns>a <see cref="CodeObjectCreateExpression"/></returns>
        public static CodeObjectCreateExpression ObjectCreateExpression(Type createType, IEnumerable<CodeExpression> parameters)
        {
            return new CodeObjectCreateExpression(createType, parameters.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeParameterDeclarationExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeParameterDeclarationExpression"/></returns>
        public static CodeParameterDeclarationExpression ParameterDeclarationExpression()
        {
            return new CodeParameterDeclarationExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeParameterDeclarationExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="CodeTypeReference"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeParameterDeclarationExpression"/></returns>
        public static CodeParameterDeclarationExpression ParameterDeclarationExpression(CodeTypeReference type, string name)
        {
            return new CodeParameterDeclarationExpression(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeParameterDeclarationExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="string"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeParameterDeclarationExpression"/></returns>
        public static CodeParameterDeclarationExpression ParameterDeclarationExpression(string type, string name)
        {
            return new CodeParameterDeclarationExpression(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeParameterDeclarationExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="Type"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeParameterDeclarationExpression"/></returns>
        public static CodeParameterDeclarationExpression ParameterDeclarationExpression(Type type, string name)
        {
            return new CodeParameterDeclarationExpression(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodePrimitiveExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodePrimitiveExpression"/></returns>
        public static CodePrimitiveExpression PrimitiveExpression()
        {
            return new CodePrimitiveExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodePrimitiveExpression"/> code object.
        /// </summary>
        /// <param name="value"> the <see cref="Object"/></param>
        /// <returns>a <see cref="CodePrimitiveExpression"/></returns>
        public static CodePrimitiveExpression PrimitiveExpression(Object value)
        {
            return new CodePrimitiveExpression(value);
        }

        /// <summary>
        /// Creates a new <see cref="CodePropertyReferenceExpression"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="propertyName"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodePropertyReferenceExpression"/></returns>
        public static CodePropertyReferenceExpression PropertyReferenceExpression(CodeExpression targetObject, string propertyName)
        {
            return new CodePropertyReferenceExpression(targetObject, propertyName);
        }

        /// <summary>
        /// Creates a new <see cref="CodePropertyReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodePropertyReferenceExpression"/></returns>
        public static CodePropertyReferenceExpression PropertyReferenceExpression()
        {
            return new CodePropertyReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodePropertySetValueReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodePropertySetValueReferenceExpression"/></returns>
        public static CodePropertySetValueReferenceExpression PropertySetValueReferenceExpression()
        {
            return new CodePropertySetValueReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeRegionDirective"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeRegionDirective"/></returns>
        public static CodeRegionDirective RegionDirective()
        {
            return new CodeRegionDirective();
        }

        /// <summary>
        /// Creates a new <see cref="CodeRegionDirective"/> code object.
        /// </summary>
        /// <param name="regionMode"> the <see cref="CodeRegionMode"/></param>
        /// <param name="regionText"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeRegionDirective"/></returns>
        public static CodeRegionDirective RegionDirective(CodeRegionMode regionMode, string regionText)
        {
            return new CodeRegionDirective(regionMode, regionText);
        }

        /// <summary>
        /// Creates a new <see cref="CodeRemoveEventStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeRemoveEventStatement"/></returns>
        public static CodeRemoveEventStatement RemoveEventStatement()
        {
            return new CodeRemoveEventStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeRemoveEventStatement"/> code object.
        /// </summary>
        /// <param name="eventRef"> the <see cref="CodeEventReferenceExpression"/></param>
        /// <param name="listener"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeRemoveEventStatement"/></returns>
        public static CodeRemoveEventStatement RemoveEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener)
        {
            return new CodeRemoveEventStatement(eventRef, listener);
        }

        /// <summary>
        /// Creates a new <see cref="CodeRemoveEventStatement"/> code object.
        /// </summary>
        /// <param name="targetObject"> the <see cref="CodeExpression"/></param>
        /// <param name="eventName"> the <see cref="string"/></param>
        /// <param name="listener"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeRemoveEventStatement"/></returns>
        public static CodeRemoveEventStatement RemoveEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener)
        {
            return new CodeRemoveEventStatement(targetObject, eventName, listener);
        }

        /// <summary>
        /// Creates a new <see cref="CodeSnippetCompileUnit"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeSnippetCompileUnit"/></returns>
        public static CodeSnippetCompileUnit SnippetCompileUnit()
        {
            return new CodeSnippetCompileUnit();
        }

        /// <summary>
        /// Creates a new <see cref="CodeSnippetCompileUnit"/> code object.
        /// </summary>
        /// <param name="value"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeSnippetCompileUnit"/></returns>
        public static CodeSnippetCompileUnit SnippetCompileUnit(string value)
        {
            return new CodeSnippetCompileUnit(value);
        }

        /// <summary>
        /// Creates a new <see cref="CodeSnippetExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeSnippetExpression"/></returns>
        public static CodeSnippetExpression SnippetExpression()
        {
            return new CodeSnippetExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeSnippetExpression"/> code object.
        /// </summary>
        /// <param name="value"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeSnippetExpression"/></returns>
        public static CodeSnippetExpression SnippetExpression(string value)
        {
            return new CodeSnippetExpression(value);
        }

        /// <summary>
        /// Creates a new <see cref="CodeSnippetStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeSnippetStatement"/></returns>
        public static CodeSnippetStatement SnippetStatement()
        {
            return new CodeSnippetStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeSnippetStatement"/> code object.
        /// </summary>
        /// <param name="value"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeSnippetStatement"/></returns>
        public static CodeSnippetStatement SnippetStatement(string value)
        {
            return new CodeSnippetStatement(value);
        }

        /// <summary>
        /// Creates a new <see cref="CodeSnippetTypeMember"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeSnippetTypeMember"/></returns>
        public static CodeSnippetTypeMember SnippetTypeMember()
        {
            return new CodeSnippetTypeMember();
        }

        /// <summary>
        /// Creates a new <see cref="CodeSnippetTypeMember"/> code object.
        /// </summary>
        /// <param name="text"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeSnippetTypeMember"/></returns>
        public static CodeSnippetTypeMember SnippetTypeMember(string text)
        {
            return new CodeSnippetTypeMember(text);
        }

        /// <summary>
        /// Creates a new <see cref="CodeStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeStatement"/></returns>
        public static CodeStatement Statement()
        {
            return new CodeStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeThisReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeThisReferenceExpression"/></returns>
        public static CodeThisReferenceExpression ThisReferenceExpression()
        {
            return new CodeThisReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeThrowExceptionStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeThrowExceptionStatement"/></returns>
        public static CodeThrowExceptionStatement ThrowExceptionStatement()
        {
            return new CodeThrowExceptionStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeThrowExceptionStatement"/> code object.
        /// </summary>
        /// <param name="toThrow"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeThrowExceptionStatement"/></returns>
        public static CodeThrowExceptionStatement ThrowExceptionStatement(CodeExpression toThrow)
        {
            return new CodeThrowExceptionStatement(toThrow);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTryCatchFinallyStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTryCatchFinallyStatement"/></returns>
        public static CodeTryCatchFinallyStatement TryCatchFinallyStatement()
        {
            return new CodeTryCatchFinallyStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTryCatchFinallyStatement"/> code object.
        /// </summary>
        /// <param name="tryStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <param name="catchClauses"> a <see cref="CodeCatchClause"/> array.</param>
        /// <returns>a <see cref="CodeTryCatchFinallyStatement"/></returns>
        public static CodeTryCatchFinallyStatement TryCatchFinallyStatement(CodeStatement[] tryStatements, params CodeCatchClause[] catchClauses)
        {
            return new CodeTryCatchFinallyStatement(tryStatements, catchClauses);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTryCatchFinallyStatement"/> code object.
        /// </summary>
        /// <param name="tryStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <param name="catchClauses"> a <see cref="CodeCatchClause"/> array.</param>
        /// <returns>a <see cref="CodeTryCatchFinallyStatement"/></returns>
        public static CodeTryCatchFinallyStatement TryCatchFinallyStatement(IEnumerable<CodeStatement> tryStatements, IEnumerable<CodeCatchClause> catchClauses)
        {
            return new CodeTryCatchFinallyStatement(tryStatements.ToArray(), catchClauses.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeTryCatchFinallyStatement"/> code object.
        /// </summary>
        /// <param name="tryStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <param name="catchClauses"> a <see cref="CodeCatchClause"/> array.</param>
        /// <param name="finallyStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <returns>a <see cref="CodeTryCatchFinallyStatement"/></returns>
        public static CodeTryCatchFinallyStatement TryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses, params CodeStatement[] finallyStatements)
        {
            return new CodeTryCatchFinallyStatement(tryStatements, catchClauses, finallyStatements);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTryCatchFinallyStatement"/> code object.
        /// </summary>
        /// <param name="tryStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <param name="catchClauses"> a <see cref="CodeCatchClause"/> array.</param>
        /// <param name="finallyStatements"> a <see cref="CodeStatement"/> array.</param>
        /// <returns>a <see cref="CodeTryCatchFinallyStatement"/></returns>
        public static CodeTryCatchFinallyStatement TryCatchFinallyStatement(IEnumerable<CodeStatement> tryStatements, IEnumerable<CodeCatchClause> catchClauses, IEnumerable<CodeStatement> finallyStatements)
        {
            return new CodeTryCatchFinallyStatement(tryStatements.ToArray(), catchClauses.ToArray(), finallyStatements.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeConstructor"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTypeConstructor"/></returns>
        public static CodeTypeConstructor TypeConstructor()
        {
            return new CodeTypeConstructor();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeDeclaration"/> code object.
        /// </summary>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeTypeDeclaration"/></returns>
        public static CodeTypeDeclaration TypeDeclaration(string name)
        {
            return new CodeTypeDeclaration(name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeDeclaration"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTypeDeclaration"/></returns>
        public static CodeTypeDeclaration TypeDeclaration()
        {
            return new CodeTypeDeclaration();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeDelegate"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTypeDelegate"/></returns>
        public static CodeTypeDelegate TypeDelegate()
        {
            return new CodeTypeDelegate();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeDelegate"/> code object.
        /// </summary>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeTypeDelegate"/></returns>
        public static CodeTypeDelegate TypeDelegate(string name)
        {
            return new CodeTypeDelegate(name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeMember"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTypeMember"/></returns>
        public static CodeTypeMember TypeMember()
        {
            return new CodeTypeMember();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeOfExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTypeOfExpression"/></returns>
        public static CodeTypeOfExpression TypeOfExpression()
        {
            return new CodeTypeOfExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeOfExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="CodeTypeReference"/></param>
        /// <returns>a <see cref="CodeTypeOfExpression"/></returns>
        public static CodeTypeOfExpression TypeOfExpression(CodeTypeReference type)
        {
            return new CodeTypeOfExpression(type);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeOfExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeTypeOfExpression"/></returns>
        public static CodeTypeOfExpression TypeOfExpression(string type)
        {
            return new CodeTypeOfExpression(type);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeOfExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="Type"/></param>
        /// <returns>a <see cref="CodeTypeOfExpression"/></returns>
        public static CodeTypeOfExpression TypeOfExpression(Type type)
        {
            return new CodeTypeOfExpression(type);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeParameter"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTypeParameter"/></returns>
        public static CodeTypeParameter TypeParameter()
        {
            return new CodeTypeParameter();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeParameter"/> code object.
        /// </summary>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeTypeParameter"/></returns>
        public static CodeTypeParameter TypeParameter(string name)
        {
            return new CodeTypeParameter(name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="Type"/></param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(Type type)
        {
            return new CodeTypeReference(type);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference()
        {
            return new CodeTypeReference();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="Type"/></param>
        /// <param name="codeTypeReferenceOption"> the <see cref="CodeTypeReferenceOptions"/></param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(Type type, CodeTypeReferenceOptions codeTypeReferenceOption)
        {
            return new CodeTypeReference(type, codeTypeReferenceOption);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="typeName"> the <see cref="string"/></param>
        /// <param name="codeTypeReferenceOption"> the <see cref="CodeTypeReferenceOptions"/></param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(string typeName, CodeTypeReferenceOptions codeTypeReferenceOption)
        {
            return new CodeTypeReference(typeName, codeTypeReferenceOption);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="typeName"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(string typeName)
        {
            return new CodeTypeReference(typeName);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="typeName"> the <see cref="string"/></param>
        /// <param name="typeArguments"> a <see cref="CodeTypeReference"/> array.</param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(string typeName, params CodeTypeReference[] typeArguments)
        {
            return new CodeTypeReference(typeName, typeArguments);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="typeName"> the <see cref="string"/></param>
        /// <param name="typeArguments"> a <see cref="CodeTypeReference"/> array.</param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(string typeName, IEnumerable<CodeTypeReference> typeArguments)
        {
            return new CodeTypeReference(typeName, typeArguments.ToArray());
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="typeParameter"> the <see cref="CodeTypeParameter"/></param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(CodeTypeParameter typeParameter)
        {
            return new CodeTypeReference(typeParameter);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="baseType"> the <see cref="string"/></param>
        /// <param name="rank"> the <see cref="Int32"/></param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(string baseType, Int32 rank)
        {
            return new CodeTypeReference(baseType, rank);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReference"/> code object.
        /// </summary>
        /// <param name="arrayType"> the <see cref="CodeTypeReference"/></param>
        /// <param name="rank"> the <see cref="Int32"/></param>
        /// <returns>a <see cref="CodeTypeReference"/></returns>
        public static CodeTypeReference TypeReference(CodeTypeReference arrayType, Int32 rank)
        {
            return new CodeTypeReference(arrayType, rank);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeTypeReferenceExpression"/></returns>
        public static CodeTypeReferenceExpression TypeReferenceExpression()
        {
            return new CodeTypeReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReferenceExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="CodeTypeReference"/></param>
        /// <returns>a <see cref="CodeTypeReferenceExpression"/></returns>
        public static CodeTypeReferenceExpression TypeReferenceExpression(CodeTypeReference type)
        {
            return new CodeTypeReferenceExpression(type);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReferenceExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeTypeReferenceExpression"/></returns>
        public static CodeTypeReferenceExpression TypeReferenceExpression(string type)
        {
            return new CodeTypeReferenceExpression(type);
        }

        /// <summary>
        /// Creates a new <see cref="CodeTypeReferenceExpression"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="Type"/></param>
        /// <returns>a <see cref="CodeTypeReferenceExpression"/></returns>
        public static CodeTypeReferenceExpression TypeReferenceExpression(Type type)
        {
            return new CodeTypeReferenceExpression(type);
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableDeclarationStatement"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeVariableDeclarationStatement"/></returns>
        public static CodeVariableDeclarationStatement VariableDeclarationStatement()
        {
            return new CodeVariableDeclarationStatement();
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableDeclarationStatement"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="CodeTypeReference"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeVariableDeclarationStatement"/></returns>
        public static CodeVariableDeclarationStatement VariableDeclarationStatement(CodeTypeReference type, string name)
        {
            return new CodeVariableDeclarationStatement(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableDeclarationStatement"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="string"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeVariableDeclarationStatement"/></returns>
        public static CodeVariableDeclarationStatement VariableDeclarationStatement(string type, string name)
        {
            return new CodeVariableDeclarationStatement(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableDeclarationStatement"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="Type"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeVariableDeclarationStatement"/></returns>
        public static CodeVariableDeclarationStatement VariableDeclarationStatement(Type type, string name)
        {
            return new CodeVariableDeclarationStatement(type, name);
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableDeclarationStatement"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="CodeTypeReference"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <param name="initExpression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeVariableDeclarationStatement"/></returns>
        public static CodeVariableDeclarationStatement VariableDeclarationStatement(CodeTypeReference type, string name, CodeExpression initExpression)
        {
            return new CodeVariableDeclarationStatement(type, name, initExpression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableDeclarationStatement"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="string"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <param name="initExpression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeVariableDeclarationStatement"/></returns>
        public static CodeVariableDeclarationStatement VariableDeclarationStatement(string type, string name, CodeExpression initExpression)
        {
            return new CodeVariableDeclarationStatement(type, name, initExpression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableDeclarationStatement"/> code object.
        /// </summary>
        /// <param name="type"> the <see cref="Type"/></param>
        /// <param name="name"> the <see cref="string"/></param>
        /// <param name="initExpression"> the <see cref="CodeExpression"/></param>
        /// <returns>a <see cref="CodeVariableDeclarationStatement"/></returns>
        public static CodeVariableDeclarationStatement VariableDeclarationStatement(Type type, string name, CodeExpression initExpression)
        {
            return new CodeVariableDeclarationStatement(type, name, initExpression);
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableReferenceExpression"/> code object.
        /// </summary>
        /// <returns>a <see cref="CodeVariableReferenceExpression"/></returns>
        public static CodeVariableReferenceExpression VariableReferenceExpression()
        {
            return new CodeVariableReferenceExpression();
        }

        /// <summary>
        /// Creates a new <see cref="CodeVariableReferenceExpression"/> code object.
        /// </summary>
        /// <param name="variableName"> the <see cref="string"/></param>
        /// <returns>a <see cref="CodeVariableReferenceExpression"/></returns>
        public static CodeVariableReferenceExpression VariableReferenceExpression(string variableName)
        {
            return new CodeVariableReferenceExpression(variableName);
        }
    }
}


