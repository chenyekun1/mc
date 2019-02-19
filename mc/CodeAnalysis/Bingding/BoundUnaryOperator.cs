using System;
using mc.CodeAlalysis.Syntax;

namespace mc.CodeAlalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        public BoundBinaryOperator(
        SyntaxKind syntaxKind,
        BoundBinaryOperatorKind kind,
        Type leftType,
        Type rightType,
        Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }

        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(
                SyntaxKind.PlusToken,
                BoundBinaryOperatorKind.Addition,
                typeof(int), typeof(int), typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.MinusToken,
                BoundBinaryOperatorKind.Subtraction,
                typeof(int), typeof(int), typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.StarToken,
                BoundBinaryOperatorKind.Multiplication,
                typeof(int), typeof(int), typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.SlashToken,
                BoundBinaryOperatorKind.Division,
                typeof(int), typeof(int), typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.PipePipeToken,
                BoundBinaryOperatorKind.LogicalOr,
                typeof(bool), typeof(bool), typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.AmpersandAmpersandToken,
                BoundBinaryOperatorKind.LogicalAnd,
                typeof(bool), typeof(bool), typeof(bool))
        };

        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
        {
            foreach (var op in _operators)
            {
                if (op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType)
                    return op;
            }

            return null;
        }
    }

    internal sealed class BoundUnaryOperator
    {
        private BoundUnaryOperator(
        SyntaxKind syntaxKind,
        BoundUnaryOperatorKind kind,
        Type operandType) : this(syntaxKind, kind, operandType, operandType)
        {
        }

        private BoundUnaryOperator(
        SyntaxKind syntaxKind,
        BoundUnaryOperatorKind kind,
        Type operandType,
        Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int))
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operandType)
        {
            foreach (var op in _operators)
            {
                if (op.SyntaxKind == syntaxKind && op.ResultType == operandType)
                    return op;
            }

            return null;
        }
    }
}