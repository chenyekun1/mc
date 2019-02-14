using System.Collections.Generic;

namespace mc.CodeAlalysis
{
    public sealed class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax(SyntaxToken token)
        {
            Token = token;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;

        public SyntaxToken Token { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}
