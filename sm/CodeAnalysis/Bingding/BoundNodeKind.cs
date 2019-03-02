namespace mc.CodeAlalysis.Binding
{
    internal enum BoundNodeKind
    {
        UnaryExpression,
        LiteralExpression,
        BinaryExpression,
        ParenthesisExpression,
        VariableExpression,
        AssignmentExpression
    }
}