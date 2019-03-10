using System;
using System.Collections.Generic;
using System.Linq;
using mc.CodeAlalysis.Syntax;
using Xunit;

namespace Mc.Tests.CodeAnalysis.Syntax
{
    internal sealed class AssertingEnumerator : IDisposable
    {
        private readonly IEnumerator<SyntaxNode> _enumerator;
        private bool _hasErrors;

        public AssertingEnumerator(SyntaxNode node)
        {
            _enumerator = Flatten(node).GetEnumerator();
        }

        private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
        {
            var stack = new Stack<SyntaxNode>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;

                foreach (var child in n.GetChildren().Reverse())
                    stack.Push(child);
            }
        }
    
        private bool MarkFailed()
        {
            _hasErrors = true;
            return false;
        }

        public void AssertToken(SyntaxKind kind, string text)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(kind, _enumerator.Current.Kind);
                var token = Assert.IsType<SyntaxToken>(_enumerator.Current);
                Assert.Equal(kind, token.Kind);
                Assert.Equal(text, token.Text);
            }
            catch when (MarkFailed())
            {
                _hasErrors = true;
                throw;
            }

        }

        public void AssertNode(SyntaxKind kind)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(kind, _enumerator.Current.Kind);
                Assert.IsNotType<SyntaxToken>(_enumerator.Current);
            }
            catch when (MarkFailed())
            {
                _hasErrors = true;
                throw;
            }

        }

        public void Dispose()
        {
            if (_hasErrors)
                Assert.False(_enumerator.MoveNext());
            _enumerator.Dispose();
        }
    }

    public class ParserTests
    {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void Parser_BinaryExpression_HonorsPrecedences(SyntaxKind op1, SyntaxKind op2)
        {
            var op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
            var op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
            var op1Text       = SyntaxFacts.GetTest(op1);
            var op2Text       = SyntaxFacts.GetTest(op2);
            var text          = $"a {op1Text} b {op2Text} c";
            var expression    = SyntaxTree.Parse(text).Root;

            if (op1Precedence >= op2Precedence)
            {
                using (var e = new AssertingEnumerator(expression))
                {
                    e.AssertNode(SyntaxKind.BinaryExpression);
                        e.AssertNode(SyntaxKind.BinaryExpression);
                            e.AssertNode(SyntaxKind.NameExpression);
                                e.AssertToken(SyntaxKind.IdentifierToken, "a");
                                e.AssertToken(op1, op1Text);
                            e.AssertNode(SyntaxKind.NameExpression);
                                e.AssertToken(SyntaxKind.IdentifierToken, "b");
                                e.AssertToken(op2, op2Text);
                        e.AssertNode(SyntaxKind.NameExpression);
                            e.AssertToken(SyntaxKind.IdentifierToken, "c");
                }
            }
            else
            {
                using (var e = new AssertingEnumerator(expression))
                {
                    e.AssertNode(SyntaxKind.BinaryExpression);
                        e.AssertNode(SyntaxKind.NameExpression);
                            e.AssertToken(SyntaxKind.IdentifierToken, "a");
                            e.AssertToken(op1, op1Text);
                        e.AssertNode(SyntaxKind.BinaryExpression);
                            e.AssertNode(SyntaxKind.NameExpression);
                                e.AssertToken(SyntaxKind.IdentifierToken, "b");
                                e.AssertToken(op2, op2Text);
                            e.AssertNode(SyntaxKind.NameExpression);
                                e.AssertToken(SyntaxKind.IdentifierToken, "c");
                }
            }
        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach (var op1 in SyntaxFacts.GetBinaryOperatorKinds())
            {
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorKinds())
                    yield return new object[] { op1, op2 };
            }
        }
    }
}
