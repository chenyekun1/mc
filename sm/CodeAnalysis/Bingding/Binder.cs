using System;
using System.Collections.Generic;
using mc.CodeAlalysis.Syntax;

namespace mc.CodeAlalysis.Binding
{
    /*
    * TODO
    * Add parenthesis support Binder
     */
    internal sealed class Binder
    {
        private readonly List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.NumberExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.ParenthesizedExpression:
                    return BindParenthesisExpression((ParenthesizedExpressionSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax : {syntax.Kind}");
            }
        }

        private BoundExpression BindParenthesisExpression(ParenthesizedExpressionSyntax syntax)
        {
            var boundExpression = BindExpression(syntax.Expression);
            return new BoundParenthesisExpression(syntax.OpenParenthesisToken, boundExpression, syntax.CloseParenthesisToken);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var val = syntax.Value ?? 0;
            return new BoundLiteralExpression(val);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator == null)
            {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Kind}' is not defined for type '{boundOperand.Type}'");
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var left = BindExpression(syntax.Left);
            var right = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, left.Type, right.Type);

            if (boundOperator == null)
            {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Kind}' is not defined for type '{left.Type}' and '{right.Type}'");
                return left;
            }

            return new BoundBinaryExpression(left, boundOperator, right);
        }
    }
}