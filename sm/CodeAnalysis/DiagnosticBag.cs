using System;
using System.Collections;
using System.Collections.Generic;
using mc.CodeAlalysis.Syntax;

namespace mc.CodeAlalysis
{
    public sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        private void Report(TextSpan span, string message)
        {
            _diagnostics.Add(new Diagnostic(span, message));
        }

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        public void ReportInvalidNumber(TextSpan textSpan, string text, Type type)
        {
            var message = $"The number {text} isn't valid {type}.";
            Report(textSpan, message);
        }

        public void ReportNadCharacter(int position, char character)
        {
            var message = $"Bad Characters input: '{character}'.";
            var span = new TextSpan(position, 1);
            Report(span, message);
        }

        internal void ReportUnexpectedToken(TextSpan span, SyntaxKind unexpectedKind, SyntaxKind expectedKind)
        {
            var message = $"Unexpected token <{unexpectedKind}>, Expected <{expectedKind}>.";
            Report(span, message);
        }

        internal void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type type)
        {
            var message = $"Unary operator '{operatorText}' is not defined for type '{type}'.";
            Report(span, message);
        }

        internal void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, Type leftType, Type rightType)
        {
            var message = $"Binary operator '{operatorText}' is not defined for type '{leftType}' and '{rightType}'.";
            Report(span, message);
        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
            var message = $"Variable '{name}' is not exist.";
            Report(span, message);
        }
    }
}
