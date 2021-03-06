using System.Collections.Generic;
using mc.CodeAlalysis.Text;

namespace mc.CodeAlalysis.Syntax
{
    public class Lexer
    {
        private readonly SourceText _text;
        private int _position;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public DiagnosticBag Diagnostics => _diagnostics;

        private int _start;
        private object _value;
        private SyntaxKind _kind;

        public Lexer(SourceText text)
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
            _start = _position;
            _kind  = SyntaxKind.BadToken;
            _value = null;

            if (char.IsDigit(Current))
            {
                ReadNumberToken();
            }
            else if (char.IsWhiteSpace(Current))
            {
                ReadWhiteSpaceToken();
            }
            else if (char.IsLetter(Current))
            {
                ReadIdentifierOrKeywordToken();
            }
            else
            {
                switch (Current)
                {
                    case '\0':
                        _kind = SyntaxKind.EndOfFileToken;
                        break;
                    case '+':
                        _kind = SyntaxKind.PlusToken;
                        _position++;
                        break;
                    case '-':
                        _kind = SyntaxKind.MinusToken;
                        _position++;
                        break;
                    case '*':
                        _kind = SyntaxKind.StarToken;
                        _position++;
                        break;
                    case '/':
                        _kind = SyntaxKind.SlashToken;
                        _position++;
                        break;
                    case '(':
                        _kind = SyntaxKind.OpenParenthesisToken;
                        _position++;
                        break;
                    case ')':
                        _kind = SyntaxKind.CloseParenthesisToken;
                        _position++;
                        break;
                    case '&':
                        if (Lookahead == '&')
                        {
                            _position+=2;
                            _kind = SyntaxKind.AmpersandAmpersandToken;
                        }
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.AmpersandToken;
                        }
                        break;
                    case '|':
                        if (Lookahead == '|')
                        {
                            _position+=2;
                            _kind = SyntaxKind.PipePipeToken;
                        }
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.PipeToken;
                        }
                        break;
                    case '=':
                        if (Lookahead == '=')
                        {
                            _position+=2;
                            _kind = SyntaxKind.EqualsToken;
                        }
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.AssignEqualsToken;
                        }
                        break;
                    case '!':
                        if (Lookahead == '=')
                        {
                            _position+=2;
                            _kind = SyntaxKind.BangEqualsToken;
                        }
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.BangToken;
                        }
                        break;
                    case '>':
                        if (Lookahead == '>')
                        {
                            _position+=2;
                            _kind = SyntaxKind.RightShiftToken;
                        }
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.BiggerCompareToken;
                        }
                        break;
                    case '<':
                        if (Lookahead == '<')
                        {
                            _position+=2;
                            _kind = SyntaxKind.LeftShiftToken;
                        }
                        else
                        {
                            _position++;
                            _kind = SyntaxKind.LesserCompareToken;
                        }
                        break;
                        
                    default:
                        _diagnostics.ReportNadCharacter(_position, Current);
                        _position++;
                        break;
                }
            }
            
            var length = _position - _start;
            var text   = SyntaxFacts.GetText(_kind);
            if (text == null) text = _text.Substring(_start, length);

            return new SyntaxToken(_kind, _start, text, _value);
        }

        private void ReadIdentifierOrKeywordToken()
        {
            while (char.IsLetter(Current))
                Next();

            var length = _position - _start;
            var text = _text.Substring(_start, length);
            _kind = SyntaxFacts.GetKeywordKind(text);
        }

        private void ReadWhiteSpaceToken()
        {
            while (char.IsWhiteSpace(Current))
                Next();
            _kind = SyntaxKind.WhitespaceToken;
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
                Next();

            var length = _position - _start;
            var text = _text.Substring(_start, length);
            if (!int.TryParse(text, out var value))
                _diagnostics.ReportInvalidNumber(new TextSpan(_start, length), text, typeof(int));

            _value = value;
            _kind  = SyntaxKind.LiteralToken;
        }
    }
}
