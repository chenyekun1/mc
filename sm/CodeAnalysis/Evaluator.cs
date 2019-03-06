using System;
using mc.CodeAlalysis.Binding;
using System.Collections;
using System.Collections.Generic;

namespace mc.CodeAlalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables)
        {
            _root = root;
            _variables = variables;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression root)
        {
            if (root is BoundLiteralExpression n)
                return n.Value;

            if (root is BoundVariableExpression v)
                return _variables[v.Variable];

            if (root is BoundAssignmentExpression a)
            {
                var value = EvaluateExpression(a.BoundExpression);
                _variables[a.Variable] = value;
                return value;
            }

            if (root is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.Operator.Kind)
                {
                    case BoundUnaryOperatorKind.Negation:
                        return -(int) operand;
                    case BoundUnaryOperatorKind.Identity:
                        return (int) operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool) operand;

                    default:
                        throw new Exception($"Error: Unexpect Unary Operator <{u.Operator}>");
                }
            }

            if (root is BoundBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.Operator.Kind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int) left + (int) right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int) left - (int) right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int) left * (int) right;
                    case BoundBinaryOperatorKind.Division:
                        return (int) left / (int) right;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool) left && (bool) right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool) left || (bool) right;
                    case BoundBinaryOperatorKind.Equal:
                        return Equals(left, right);
                    case BoundBinaryOperatorKind.BangEquals:
                        return !Equals(left, right);

                    default:
                        throw new Exception($"Error: Unexpected Binary Operator <{b.Operator}>");
                }
            }

            if (root is BoundParenthesisExpression p)
            {
                var value = EvaluateExpression(p.Expression);
                return value;
            }

            throw new Exception($"Error: Unexpected Node Kind '{root.Kind}'");
        }
    }
}
