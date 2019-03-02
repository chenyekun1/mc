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
        BangToken,
        PipePipeToken,
        AmpersandAmpersandToken,
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        IdentifierToken,
        AssignEqualsToken,
        

        // Keywords
        TrueKeyword,
        FalseKeyword,
        
        // Expressions
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,
        EqualsToken,
        BangEqualsToken,
        NameExpression,
        AssignmentExpression
    }
}
