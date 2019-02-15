using System.Collections.Generic;

namespace mc.CodeAlalysis
{
    public class Parser
    {
        private SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public Parser(string text)
        {
            var lexer = new Lexer(text);
            var tokens = new List<SyntaxToken>();
            SyntaxToken token;

            do
            {
                token = lexer.NextToken();

                if (token.Kind != SyntaxKind.BadToken &&
                    token.Kind != SyntaxKind.WhitespaceToken)
                {
                    tokens.Add(token);
                }
                
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _diagnostics.AddRange(lexer.Diagnostics);
            _tokens = tokens.ToArray();
        }

        private SyntaxToken Peek(int offset)
        {
            int index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];
            
            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var token = Current;
            _position++;
            return token;
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseTerm();
        }
        private SyntaxToken Match(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();
            
            _diagnostics.Add($"Error: Unexpected token <{Current.Kind}>. Expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            ExpressionSyntax expression = ParseTerm();
            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Kind == SyntaxKind.PlusToken ||
                   Current.Kind == SyntaxKind.MinusToken)
            {
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParseFactor()
        {
            var left = ParsePrimaryExpression();
            while (Current.Kind == SyntaxKind.StarToken ||
                   Current.Kind == SyntaxKind.SlashToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = NextToken();

                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            var numberToken = Match(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}
