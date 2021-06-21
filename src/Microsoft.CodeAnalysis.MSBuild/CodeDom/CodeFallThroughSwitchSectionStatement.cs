namespace System.CodeDom
{
    internal class CodeFallThroughSwitchSectionStatement : AbstractSwitchSectionStatement
    {
        public CodeFallThroughSwitchSectionStatement()
        {
            Label = new CodeSwitchSectionLabelExpression();
        }

        public CodeFallThroughSwitchSectionStatement(CodeSwitchSectionLabelExpression label)
        {
            Label = label;
        }

        public override CodeSwitchSectionLabelExpression Label { get; set; }
    }
}

