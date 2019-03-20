using System;

namespace mc.CodeAlalysis.Text
{
    public struct TextSpan
    {
        public TextSpan(int strt, int len)
        {
            Strt = strt;
            Len = len;
        }

        public int Strt { get; }
        public int Len { get; }
        public int End => Strt + Len;

        internal static TextSpan FromBounds(int strt, int end)
        {
            var length = end - strt;
            return new TextSpan(strt, length);
        }
    }
}
