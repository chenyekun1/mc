using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using mc.CodeAlalysis;
using mc.CodeAlalysis.Text;

namespace mc.CodeAlalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(
        SourceText text,
        ImmutableArray<Diagnostic> diagnostics,
        ExpressionSyntax root,
        SyntaxToken endOfFileToken)
        {
            Text = text;
            Diagnostics = diagnostics;
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public SourceText Text { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
    
        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }

        public static IEnumerable<SyntaxToken> ParseToken(SourceText text)
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

        public static IEnumerable<SyntaxToken> ParseToken(string text)
        {
            var sourceText = SourceText.From(text);
            return ParseToken(sourceText);
        }
    }
}
