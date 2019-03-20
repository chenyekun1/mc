using System;
using mc.CodeAlalysis.Syntax;

namespace mc.CodeAlalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        private BoundBinaryOperator(
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

        private BoundBinaryOperator(
        SyntaxKind syntaxKind,
        BoundBinaryOperatorKind kind,
        Type operandType,
        Type resultType) : this(syntaxKind, kind, operandType, operandType, resultType)
        {
        }

        private BoundBinaryOperator(
            SyntaxKind syntaxKind,
            BoundBinaryOperatorKind kind,
            Type type
        ) : this(syntaxKind, kind, type, type, type)
        {
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
                typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.MinusToken,
                BoundBinaryOperatorKind.Subtraction,
                typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.StarToken,
                BoundBinaryOperatorKind.Multiplication,
                typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.SlashToken,
                BoundBinaryOperatorKind.Division,
                typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.PipePipeToken,
                BoundBinaryOperatorKind.LogicalOr,
                typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.AmpersandAmpersandToken,
                BoundBinaryOperatorKind.LogicalAnd,
                typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.EqualsToken,
                BoundBinaryOperatorKind.Equal,
                typeof(int), typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.BangEqualsToken,
                BoundBinaryOperatorKind.BangEquals,
                typeof(int), typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.BangEqualsToken,
                BoundBinaryOperatorKind.BangEquals,
                typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.EqualsToken,
                BoundBinaryOperatorKind.Equal,
                typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.AmpersandToken,
                BoundBinaryOperatorKind.MathmaticalAnd,
                typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.PipeToken,
                BoundBinaryOperatorKind.MathmaticalOr,
                typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.LesserCompareToken,
                BoundBinaryOperatorKind.LessCompare,
                typeof(int), typeof(int),
                typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.BiggerCompareToken,
                BoundBinaryOperatorKind.BiggerCompare,
                typeof(int), typeof(int),
                typeof(bool)),
            new BoundBinaryOperator(
                SyntaxKind.LeftShiftToken,
                BoundBinaryOperatorKind.LeftShift,
                typeof(int)),
            new BoundBinaryOperator(
                SyntaxKind.RightShiftToken,
                BoundBinaryOperatorKind.RIghtShift,
                typeof(int))
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
}