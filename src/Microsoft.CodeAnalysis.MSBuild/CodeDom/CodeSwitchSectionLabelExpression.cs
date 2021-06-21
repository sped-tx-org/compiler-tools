namespace System.CodeDom
{
    internal class CodeSwitchSectionLabelExpression : CodeExpression
    {
        public CodeSwitchSectionLabelExpression()
        {
            Expression = new CodeExpression();
        }

        public CodeSwitchSectionLabelExpression(CodeExpression expression)
        {
            Expression = expression;
        }

        public CodeExpression Expression { get; set; }
    }
}

