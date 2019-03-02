using System;

namespace mc.CodeAlalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(string name, BoundExpression boundExpression)
        {
            Name = name;
            BoundExpression = boundExpression;
        }

        public string Name { get; }
        public BoundExpression BoundExpression { get; }

        public override Type Type => BoundExpression.Type;

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
    }
}