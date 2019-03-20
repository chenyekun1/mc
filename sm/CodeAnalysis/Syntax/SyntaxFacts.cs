using System;
using System.Collections.Generic;

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
                case SyntaxKind.PipeToken:
                case SyntaxKind.AmpersandToken:
                case SyntaxKind.LeftShiftToken:
                case SyntaxKind.RightShiftToken:
                    return 5;

                case SyntaxKind.BangEqualsToken:
                case SyntaxKind.EqualsToken:
                case SyntaxKind.BiggerCompareToken:
                case SyntaxKind.LesserCompareToken:
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

        public static IEnumerable<SyntaxKind> GetBinaryOperatorKinds()
        {
            var kinds = (SyntaxKind[]) Enum.GetValues(typeof(SyntaxKind));
            foreach (var kind in kinds)
            {
                if (GetBinaryOperatorPrecedence(kind) > 0   &&
                    kind != SyntaxKind.OpenParenthesisToken &&
                    kind != SyntaxKind.CloseParenthesisToken)
                    yield return kind;
            }
        }

        public static IEnumerable<SyntaxKind> GetUnaryOperatorKinds()
        {
            var kinds = (SyntaxKind[]) Enum.GetValues(typeof(SyntaxKind));
            foreach (var kind in kinds)
            {
                if (GetUnaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }

        public static string GetText(SyntaxKind kind)
        {
            switch (kind)
            {
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
                case SyntaxKind.PipeToken:
                    return "|";
                case SyntaxKind.AmpersandToken:
                    return "&";
                case SyntaxKind.LeftShiftToken:
                    return "<<";
                case SyntaxKind.RightShiftToken:
                    return ">>";
                case SyntaxKind.BiggerCompareToken:
                    return ">";
                case SyntaxKind.LesserCompareToken:
                    return "<";
                
                default:
                    return null;
            }
        }
    }
}
