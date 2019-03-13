using System;
using System.Collections.Generic;
using mc.CodeAlalysis.Syntax;
using Xunit;
using mc.CodeAlalysis;

namespace Mc.Tests.CodeAnalysis
{
    public class EvaluatorTest
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("+1", 1)]
        [InlineData("-1", -1)]
        [InlineData("1 + 1", 2)]
        [InlineData("1 - 1", 0)]
        [InlineData("2 * 1", 2)]
        [InlineData("4 / 2", 2)]
        [InlineData("4 == 2", false)]
        [InlineData("4 != 2", true)]
        [InlineData("4 == 4", true)]
        [InlineData("true == true", true)]
        [InlineData("false == true", false)]
        [InlineData("false != true", true)]
        [InlineData("(2)", 2)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("!true", false)]
        [InlineData("!false", true)]
        [InlineData("false && true", false)]
        [InlineData("true && true", true)]
        [InlineData("(a = 10) * 2", 20)]
        public void SyntaxFact_GetText_RoundTrips(string text, object expectedResult)
        {
            var expression   = SyntaxTree.Parse(text);
            var complilation = new Compilation(expression);
            var variables    = new Dictionary<VariableSymbol, object>();
            var actualResult = complilation.Evaluate(variables);

            Assert.Empty(actualResult.Diagnostics);
            Assert.Equal(actualResult.Value, expectedResult);
        }
    }
}
