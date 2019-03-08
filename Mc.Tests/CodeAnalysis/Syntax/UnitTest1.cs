using System;
using System.Collections.Generic;
using System.Linq;
using mc.CodeAlalysis.Syntax;
using Xunit;

namespace Mc.Tests.CodeAnalysis.Syntax
{
    public class LexerTest
    {
        [Theory]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_Lexes_Token(SyntaxKind kind, string text)
        {
            var tokens = SyntaxTree.ParseToken(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsData))]
        public void Lexer_Lexes_TokenPairs(SyntaxKind t1Kind, string t1Text,
                                           SyntaxKind t2Kind, string t2Text)
        {
            var text = t1Text + t2Text;
            var tokens = SyntaxTree.ParseToken(text).ToArray();
            
            Assert.Equal(2, tokens.Length);

            Assert.Equal(tokens[0].Kind, t1Kind);
            Assert.Equal(tokens[0].Text, t1Text);
            Assert.Equal(tokens[1].Kind, t2Kind);
            Assert.Equal(tokens[1].Text, t2Text);
        }

        public static IEnumerable<object[]> GetTokensData()
        {
            foreach (var t in GetTokens().Concat(GetSeperators()))
                yield return new object[] {t.kind, t.text};
        }

        public static IEnumerable<object[]> GetTokenPairsData()
        {
            foreach (var t in GetTokensPair())
                yield return new object[] {t.kind1, t.text1, t.kind2, t.text2};
        }

        private static bool RequiredSeperated(SyntaxKind t1Kind, SyntaxKind t2Kind)
        {
            var t1IsKeyword = t1Kind.ToString().EndsWith("Keyword");
            var t2IsKeyword = t2Kind.ToString().EndsWith("Keyword");

            if (t1Kind == SyntaxKind.IdentifierToken && t2Kind == SyntaxKind.IdentifierToken)
                return true;
            if (t1IsKeyword && t2IsKeyword)
                return true;
            if (t1IsKeyword && t2Kind == SyntaxKind.IdentifierToken)
                return true;
            if (t1Kind == SyntaxKind.LiteralToken && t2Kind == SyntaxKind.LiteralToken)
                return true;
            if (t1Kind == SyntaxKind.IdentifierToken && t2IsKeyword)
                return true;
            if (t1Kind == SyntaxKind.EqualsToken && t2Kind == SyntaxKind.EqualsToken)
                return true;
            if (t1Kind == SyntaxKind.AssignEqualsToken && t2Kind == SyntaxKind.AssignEqualsToken)
                return true;
            if (t1Kind == SyntaxKind.AssignEqualsToken && t2Kind == SyntaxKind.EqualsToken)
                return true;
            if (t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EqualsToken)
                return true;
            if (t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.AssignEqualsToken)
                return true;
            return false;
        }

        private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
        {
            return new[]
            {
                (SyntaxKind.IdentifierToken, "vc"),
                (SyntaxKind.IdentifierToken, "vg"),
                (SyntaxKind.LiteralToken, "1"),
                (SyntaxKind.LiteralToken, "123"),
                (SyntaxKind.PlusToken, "+"),
                (SyntaxKind.MinusToken, "-"),
                (SyntaxKind.StarToken, "*"),
                (SyntaxKind.SlashToken, "/"),
                (SyntaxKind.OpenParenthesisToken, "("),
                (SyntaxKind.CloseParenthesisToken, ")"),
                (SyntaxKind.BangToken, "!"),
                (SyntaxKind.PipePipeToken, "||"),
                (SyntaxKind.AmpersandAmpersandToken, "&&"),
                (SyntaxKind.BadToken, "$"),
                (SyntaxKind.TrueKeyword, "true"),
                (SyntaxKind.FalseKeyword, "false"),

                (SyntaxKind.BangEqualsToken, "!="),
                (SyntaxKind.EqualsToken, "=="),
                (SyntaxKind.AssignEqualsToken, "=")
            };
        }

        private static IEnumerable<(SyntaxKind kind, string text)> GetSeperators()
        {
            return new[]
            {
                (SyntaxKind.WhitespaceToken, " "),
                (SyntaxKind.WhitespaceToken, "  "),
                (SyntaxKind.WhitespaceToken, "\r"),
                (SyntaxKind.WhitespaceToken, "\n"),
                (SyntaxKind.WhitespaceToken, "\r\n")
            };
        }

        private static IEnumerable<(SyntaxKind kind1, string text1,SyntaxKind kind2, string text2)> GetTokensPair()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                    if (!RequiredSeperated(t1.kind, t2.kind))
                        yield return (t1.kind, t1.text, t2.kind, t2.text);
            }
        }
    }
}
