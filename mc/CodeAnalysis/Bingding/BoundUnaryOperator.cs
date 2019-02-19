using System;
using mc.CodeAlalysis.Syntax;

namespace mc.CodeAlalysis.Binding
{
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