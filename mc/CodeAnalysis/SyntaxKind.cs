namespace mc.CodeAlalysis
{
    public enum SyntaxKind
    {
        // Tokens
        LiteralToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        
        // Expressions
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression
    }
}
