namespace mc.CodeAlalysis.Syntax
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
        IdentifierToken,
        

        // Keywords
        TrueKeyword,
        FalseKeyword,
        
        // Expressions
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression
    }
}
