using System.IO;

namespace OoxmlToHtml
{
    public class Source
    {
        public static char EOL = '\n';
        public static char EOF = '\0';
        private StringReader reader;
        private string line;
        private int lineNumber;
        private int currentPos;

        public Source(StringReader reader)
        {
            lineNumber = 0;
            currentPos = -2;
            this.reader = reader;
        }

        public char currentChar()
        {
            if (currentPos == -2)
            {
                readLine();
                return nextChar();
            }
            else if (line == null)
            {
                return EOF;
            }
            else if (currentPos == -1 || currentPos == line.Length)
            {
                return EOL;
            }

            return line[currentPos];
        }
    }
}