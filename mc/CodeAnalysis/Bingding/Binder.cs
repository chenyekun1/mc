using System;
using System.Collections.Generic;
using mc.CodeAlalysis.Syntax;

namespace mc.CodeAlalysis.Binding
{
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
                default:
                    throw new Exception($"Unexpected syntax : {syntax.Kind}");
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var val = syntax.Value ?? 0;
            return new BoundLiteralExpression(val);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Kind}' is not defined for type '{boundOperand.Type}'");
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperatorKind.Value, boundOperand);
        }

        private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type operandType)
        {
            if (operandType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.PlusToken:
                        return BoundUnaryOperatorKind.Identity;
                    case SyntaxKind.MinusToken:
                        return BoundUnaryOperatorKind.Negation;
                    default:
                        throw new Exception($"Unexpected bindUnaryToken: {kind}");
                }
            }

            if (operandType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.BangToken:
                        return BoundUnaryOperatorKind.LogicalNegation;
                    
                    default:
                        throw new Exception($"Unexpected bindUnaryToken: {kind}");
                }
            }

            return null;
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var left = BindExpression(syntax.Left);
            var right = BindExpression(syntax.Right);
            var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind, left.Type, right.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Kind}' is not defined for type '{left.Type}' and '{right.Type}'");
                return left;
            }

            return new BoundBinaryExpression(left, boundOperatorKind.Value, right);
        }

        private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
        {
            if (leftType == typeof(int) && rightType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.PlusToken:
                        return BoundBinaryOperatorKind.Addition;
                    case SyntaxKind.MinusToken:
                        return BoundBinaryOperatorKind.Subtraction;
                    case SyntaxKind.StarToken:
                        return BoundBinaryOperatorKind.Multiplication;
                    case SyntaxKind.SlashToken:
                        return BoundBinaryOperatorKind.Division;
                    
                    default:
                        throw new Exception($"Unexpected bindBinaryToken: {kind}");
                }
            }

            if (leftType == typeof(bool) && rightType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.PipePipeToken:
                        return BoundBinaryOperatorKind.LogicalOr;
                    case SyntaxKind.AmpersandAmpersandToken:
                        return BoundBinaryOperatorKind.LogicalAnd;
                    
                    default:
                        throw new Exception($"Unexpected bindBinaryToken: {kind}");
                }
            }

            return null;
        }
    }
}