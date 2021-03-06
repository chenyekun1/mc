using System;

namespace mc.CodeAlalysis.Binding
{
    internal sealed class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }

        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;

        public string Name => Variable.Name;
        public override Type Type => Variable.Type;
        public VariableSymbol Variable { get; }
    }
}