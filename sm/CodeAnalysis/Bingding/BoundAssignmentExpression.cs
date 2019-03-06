using System;

namespace mc.CodeAlalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(VariableSymbol variable, BoundExpression boundExpression)
        {
            Variable = variable;
            BoundExpression = boundExpression;
        }

        public string Name => Variable.Name;
        public VariableSymbol Variable { get; }
        public BoundExpression BoundExpression { get; }

        public override Type Type => Variable.Type;

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
    }
}