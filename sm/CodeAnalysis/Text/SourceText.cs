using System;
using System.Collections.Immutable;

namespace mc.CodeAlalysis.Text
{
    public sealed class SourceText
    {
        private readonly string _text;

        private SourceText(string text)
        {
            Lines = ParseLines(this, text);
            _text = text;
        }

        public ImmutableArray<TextLine> Lines { get; private set; }
        public int Length => _text.Length;

        public char this[int index] => _text[index];

        public int GetLineIndex(int position)
        {
            var lower = 0;
            var upper = _text.Length - 1;
            
            while (lower <= upper)
            {
                var index = lower + (upper - lower) / 2;
                var start = Lines[index].Start;

                if (start == index) return index;

                if (start > position)
                {
                    upper = index - 1;
                }
                else
                {
                    lower = index + 1;
                }
            }

            return lower - 1;
        }

        private ImmutableArray<TextLine> ParseLines(SourceText sourceCode, string text)
        {
            var result = ImmutableArray.CreateBuilder<TextLine>();
            
            var position  = 0;
            var lineStart = 0;
            while (position < text.Length)
            {
                var lineBreakWith = GetLineBreakWith(text, position);

                if (lineBreakWith == 0)
                {
                    position++;
                }
                else
                {
                    AddLine(result, sourceCode, position, lineStart, lineBreakWith);

                    position += lineBreakWith;
                    lineStart = position;
                }
            }

            if (position > text.Length)
                AddLine(result, sourceCode, position, lineStart, 0);
            return result.ToImmutableArray();
        }

        private static void AddLine(
        ImmutableArray<TextLine>.Builder result,
        SourceText sourceCode,
        int position,
        int lineStart,
        int lineBreakWith)
        {
            var lineLength                = position - lineStart;
            var lineLengthIncludingBreak  = lineLength + lineBreakWith;
            var line                      = new TextLine(sourceCode,
                                                         lineStart,
                                                         lineLength,
                                                         lineLengthIncludingBreak);
            result.Add(line);
        }

        private static int GetLineBreakWith(string text, int i)
        {
            var c = text[i];
            var l = i + 1 >= text.Length ? '\0' : text[i+1];

            if (c == '\r' && l == '\n')
                return 2;
            if (c == '\r' || c == '\n')
                return 1;
            
            return 0;
        }

        public static SourceText From(string text)
        {
            return new SourceText(text);
        }

        public override string ToString() => _text;

        public string ToString(int start, int length) => _text.Substring(start, length);

        public string ToString(TextSpan span) => _text.Substring(span.Strt, span.Len);

        public string Substring(int start, int length) => _text.Substring(start, length);
    }
}
