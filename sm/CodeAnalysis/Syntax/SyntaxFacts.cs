using System;

namespace mc.CodeAlalysis.Syntax
{
    public static class SyntaxFacts
    {
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;

                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;

                case SyntaxKind.BangEqualsToken:
                case SyntaxKind.EqualsToken:
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePipeToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;

                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }

        public static string GetTest(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.IdentifierToken:
                    return "vc";
                case SyntaxKind.LiteralToken:
                    return "1";
                case SyntaxKind.PlusToken:
                    return "+";
                case SyntaxKind.MinusToken:
                    return "-";
                case SyntaxKind.StarToken:
                    return "*";
                case SyntaxKind.SlashToken:
                    return "/";
                case SyntaxKind.OpenParenthesisToken:
                    return "(";
                case SyntaxKind.CloseParenthesisToken:
                    return ")";
                case SyntaxKind.BangToken:
                    return "!";
                case SyntaxKind.PipePipeToken:
                    return "||";
                case SyntaxKind.AmpersandAmpersandToken:
                    return "&&";
                case SyntaxKind.BadToken:
                    return "$";
                case SyntaxKind.TrueKeyword:
                    return "true";
                case SyntaxKind.FalseKeyword:
                    return "false";
                case SyntaxKind.BangEqualsToken:
                    return "!=";
                case SyntaxKind.EqualsToken:
                    return "==";
                case SyntaxKind.AssignEqualsToken:
                    return "=";
                default:
                    return null;
            }
        }
    }
}
