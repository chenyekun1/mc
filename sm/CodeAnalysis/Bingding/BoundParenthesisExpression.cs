using System;
using mc.CodeAlalysis.Syntax;

namespace mc.CodeAlalysis.Binding
{
    internal sealed class BoundParenthesisExpression : BoundExpression
    {
        public BoundParenthesisExpression(
        SyntaxToken openParenthesisToken,
        BoundExpression expression,
        SyntaxToken closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override Type Type => Expression.Type;

        public override BoundNodeKind Kind => BoundNodeKind.ParenthesisExpression;

        public SyntaxToken OpenParenthesisToken { get; }
        public BoundExpression Expression { get; }
        public SyntaxToken CloseParenthesisToken { get; }
    }
}