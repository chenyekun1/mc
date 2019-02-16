using System.Collections.Generic;

namespace mc.CodeAlalysis.Syntax
{
    public class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public Lexer(string text)
        {
            _text = text;
        }

        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';
                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken NextToken()
        {
            if (_position >= _text.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

            if (char.IsDigit(Current))
            {
                var strt = _position;

                while (char.IsDigit(Current))
                    Next();
                
                var length = _position - strt;
                var text = _text.Substring(strt, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.Add($"Error: {text} is not a legal int32");

                return new SyntaxToken(SyntaxKind.LiteralToken, strt, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                var strt = _position;

                while (char.IsWhiteSpace(Current))
                    Next();
                
                var length = _position - strt;
                var text = _text.Substring(strt, length);

                return new SyntaxToken(SyntaxKind.WhitespaceToken, strt, text, null);
            }

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
            }

            _diagnostics.Add($"Error: Bad Characters input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}
