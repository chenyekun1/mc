using System.Collections.Generic;
using System.Linq;

namespace mc.CodeAlalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<string> diagnostics, object value)
        {
            Diagnostics = diagnostics.ToList();
            Value = value;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public object Value { get; }
    }
}
