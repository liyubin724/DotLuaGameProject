namespace DotTools.CodeGen
{
    public enum TokenType
    {
        Code = 0,
        Eval,
        Text,
    }

    public class Chunk
    {
        public TokenType Token { get; private set; }
        public string Text { get; private set; }

        public Chunk(TokenType token, string text)
        {
            Token = token;
            Text = text;
        }
    }
}
