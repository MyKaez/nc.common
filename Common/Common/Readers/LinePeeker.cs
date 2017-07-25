using System.IO;

namespace Ns.Common.Readers
{
    internal class LinePeeker : TextReader
    {
        private readonly TextReader _reader;

        private string _line;

        public LinePeeker(TextReader reader)
        {
            _reader = reader;
        }

        public override string ReadLine()
        {
            var line = _line ?? _reader.ReadLine();

            _line = null;

            return line;
        }

        public string PeekLine()
        {
            _line = _line ?? _reader.ReadLine();

            return _line;
        }

        public override int Peek()
        {
            return _reader.Peek();
        }
        
        public override void Close()
        {
            _reader.Close();
        }
    }
}
