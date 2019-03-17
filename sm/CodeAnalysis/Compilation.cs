using System;
using mc.CodeAlalysis.Syntax;
using mc.CodeAlalysis.Binding;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace mc.CodeAlalysis
{
    public sealed class Compilation
    {
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol,object> variables)
        {
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(Syntax.Root);
            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();

            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);
            
            var evalutor = new Evaluator(boundExpression, variables);
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, evalutor.Evaluate());
        }

        public SyntaxTree Syntax { get; }
    }
}
