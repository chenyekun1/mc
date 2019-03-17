using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using mc.CodeAlalysis;

namespace mc.CodeAlalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(ImmutableArray<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics;
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
    
        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }

        public static IEnumerable<SyntaxToken> ParseToken(string text)
        {
            var lexer = new Lexer(text);

            while (true)
            {
                var token = lexer.NextToken();
                if (token.Kind == SyntaxKind.EndOfFileToken)
                    break;
                
                yield return token;
            }
        }
    }
}
