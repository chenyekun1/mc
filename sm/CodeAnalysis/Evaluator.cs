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
            switch (root)
            {
                case BoundLiteralExpression n:
                    return EvaluateLiteralExpression(n);
                case BoundVariableExpression v:
                    return EvaluateVariableExpression(v);
                case BoundAssignmentExpression a:
                    return EvaluateAssignmentExpression(a);
                case BoundUnaryExpression u:
                    return EvaluateUnaryExpression(u);
                case BoundBinaryExpression b:
                    return EvaluateBinaryExpression(b);
                case BoundParenthesisExpression p:
                    return EvaluateParenthesisExpression(p);
                default:
                    throw new Exception($"Error: Unexpected Node Kind '{root.Kind}'");
            }
        }

        private object EvaluateParenthesisExpression(BoundParenthesisExpression p)
        {
            return EvaluateExpression(p.Expression);
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            switch (b.Operator.Kind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return (int)left + (int)right;
                case BoundBinaryOperatorKind.Subtraction:
                    return (int)left - (int)right;
                case BoundBinaryOperatorKind.Multiplication:
                    return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division:
                    return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd:
                    return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr:
                    return (bool)left || (bool)right;
                case BoundBinaryOperatorKind.Equal:
                    return Equals(left, right);
                case BoundBinaryOperatorKind.BangEquals:
                    return !Equals(left, right);
                case BoundBinaryOperatorKind.MathmaticalAnd:
                    return (int)left & (int)right;
                case BoundBinaryOperatorKind.MathmaticalOr:
                    return (int)left | (int)right;

                default:
                    throw new Exception($"Error: Unexpected Binary Operator <{b.Operator}>");
            }
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

            switch (u.Operator.Kind)
            {
                case BoundUnaryOperatorKind.Negation:
                    return -(int)operand;
                case BoundUnaryOperatorKind.Identity:
                    return (int)operand;
                case BoundUnaryOperatorKind.LogicalNegation:
                    return !(bool)operand;

                default:
                    throw new Exception($"Error: Unexpect Unary Operator <{u.Operator}>");
            }
        }

        private object EvaluateVariableExpression(BoundVariableExpression v)
        {
            return _variables[v.Variable];
        }

        private object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            return n.Value;
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression a)
        {
            var value = EvaluateExpression(a.BoundExpression);
            _variables[a.Variable] = value;
            return value;
        }
    }
}
