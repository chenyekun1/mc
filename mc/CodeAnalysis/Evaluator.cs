using System;
using mc.CodeAlalysis.Syntax;
using mc.CodeAlalysis.Binding;

namespace mc.CodeAlalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(BoundExpression root)
        {
            if (root is BoundLiteralExpression n)
                return (int)n.Value;

            if (root is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Negation:
                        return -operand;
                    case BoundUnaryOperatorKind.Identity:
                        return operand;
                    default:
                        throw new Exception($"Error: Unexpect Unary Operator <{u.OperatorKind}>");
                }
            }

            if (root is BoundBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.OperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return left + right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return left - right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return left * right;
                    case BoundBinaryOperatorKind.Division:
                        return left / right;
                    default:
                        throw new Exception($"Error: Unexpected Binary Operator <{b.OperatorKind}>");
                }
            }

            throw new Exception($"Error: Unexpected Node Kind '{root.Kind}'");
        }
    }
}
