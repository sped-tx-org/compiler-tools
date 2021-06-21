namespace System.CodeDom
{
    internal abstract class AbstractSwitchSectionStatement : CodeStatement
    {
        public abstract CodeSwitchSectionLabelExpression Label { get; set; }
    }
}

