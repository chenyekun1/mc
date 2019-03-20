using System.Collections.Generic;
using System.Linq;

namespace mc.CodeAlalysis.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public virtual TextSpan Span
        {
            get
            {
                var first = GetChildren().First().Span;
                var last = GetChildren().Last().Span;
                return TextSpan.FromBounds(first.Strt, last.End);
            }
        }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}
