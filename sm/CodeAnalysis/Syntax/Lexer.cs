using System.Collections.Generic;

namespace mc.CodeAlalysis.Syntax
{
    public class Lexer
    {
        private readonly string _text;
        private int _position;
        private DiagnosticBag _diagnostics = new DiagnosticBag();

        public DiagnosticBag Diagnostics => _diagnostics;

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

        private char Lookahead => Peek(1);

        private void Next()
        {
            _position++;
        }

        private char Peek(int offset)
        {
            var index = _position + offset;

            if (index >= _text.Length)
                return '\0';
            
            return _text[index];
        }

        public SyntaxToken NextToken()
        {
            if (_position >= _text.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

            var strt = _position;

            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current))
                    Next();
                
                var length = _position - strt;
                var text = _text.Substring(strt, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.ReportInvalidNumber(new TextSpan(strt, length), _text, typeof(int));

                return new SyntaxToken(SyntaxKind.LiteralToken, strt, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    Next();
                
                var length = _position - strt;
                var text = _text.Substring(strt, length);

                return new SyntaxToken(SyntaxKind.WhitespaceToken, strt, text, null);
            }

            if (char.IsLetter(Current))
            {
                while (char.IsLetter(Current))
                    Next();
                
                var length = _position - strt;
                var text = _text.Substring(strt, length);
                var kind = SyntaxFacts.GetKeywordKind(text);
                return new SyntaxToken(kind, strt, text, null);
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
                case '&':
                    if (Lookahead == '&')
                    {
                        _position+=2;
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, strt, "&&", null);
                    }
                    break;
                case '|':
                    if (Lookahead == '|')
                    {
                        _position+=2;
                        return new SyntaxToken(SyntaxKind.PipePipeToken, strt, "||", null);
                    }
                    break;
                case '=':
                    if (Lookahead == '=')
                    {
                        _position+=2;
                        return new SyntaxToken(SyntaxKind.EqualsToken, strt, "==", null);
                    }
                    else
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.AssignEqualsToken, strt, "=", null);
                    }
                case '!':
                    if (Lookahead == '=')
                    {
                        _position+=2;
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, strt, "!=", null);
                    }
                    else
                    {
                        _position++;
                        return new SyntaxToken(SyntaxKind.BangToken, strt, "!", null);
                    }
            }

            _diagnostics.ReportNadCharacter(_position, Current);
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}
