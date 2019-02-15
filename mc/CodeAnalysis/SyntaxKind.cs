namespace mc.CodeAlalysis
{
    public enum SyntaxKind
    {
        // Tokens
        NumberToken,
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
        ParenthesizedExpression
    }
}
