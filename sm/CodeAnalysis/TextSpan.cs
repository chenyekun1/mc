namespace mc.CodeAlalysis
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
    }
}
