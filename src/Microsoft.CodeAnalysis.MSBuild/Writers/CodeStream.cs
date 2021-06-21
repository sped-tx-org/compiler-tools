using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft.CodeAnalysis.MSBuild.Writers
{
    [DebuggerDisplay("=> '{ToString()}'")]
    public partial class CodeStream : Stream
    {
        private readonly List<char> _buffer;

        private const int InitialLength = 2048;

        private readonly int _origin;
        private readonly Mode _mode;
        private int _position;
        private int _length;
        private readonly int _declaredCapacity;

        public CodeStream()
            : this(InitialLength)
        {

        }

        public CodeStream(int capacity)
        {
            _buffer = new List<char>(capacity);
            _length = _declaredCapacity = _buffer.Count;
            _mode = Mode.Read | Mode.Write;
            _origin = 0;
        }

        public CodeStream(string text)
        {
            _buffer = CreateList(text);
            _length = _declaredCapacity = _buffer.Count;
            _mode = Mode.Read | Mode.Write;
            _origin = 0;
        }

        [Flags]
        private enum Mode : byte
        {
            None = 0,
            Read = 1,
            Write = 2
        }

        public virtual int Capacity => _declaredCapacity;
        public override bool CanRead => (_mode & Mode.Read) != 0;
        public override bool CanSeek => CanRead;
        public override bool CanWrite => (_mode & Mode.Write) != 0;
        public override long Length => _length;
        public override long Position
        {
            get
            {
                return _position - _origin;
            }
            set
            {
                _position = Convert.ToInt32(value);
            }
        }

        public override bool CanTimeout
        {
            get
            {
                return false;
            }
        }

        public override int ReadTimeout
        {
            get
            {
                return 0;
            }

            set
            {

            }
        }

        public override int WriteTimeout
        {
            get
            {
                return 0;
            }

            set
            {

            }
        }

        public override void Flush()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public char ReadChar()
        {
            return Convert.ToChar(ReadByte());
        }

        public override int ReadByte()
        {
            if (_position >= _length) return -1;

            int v = _buffer[_position];

            Advance();

            return v;
        }

        //public override int Read(byte[] buffer, int offset, int count)
        //{
        //    int n = _length - _position;
        //    if (n > count) n = count;
        //    if (n <= 0)
        //        return 0;

        //    if (n <= 8)
        //    {
        //        int byteCount = n;
        //        while (--byteCount >= 0)
        //            buffer[offset + byteCount] = _buffer[_position + byteCount];
        //    }
        //    else
        //        InternalBlockCopy(_buffer.ToArray(), _position, buffer, offset, n);
        //    _position += n;

        //    return n;
        //}

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _position = 0;
        }

        public override void SetLength(long value)
        {
            _length = Convert.ToInt32(value);
        }

        public void Write(char value)
        {
            _buffer.Add(value);
        }

        private void Advance()
        {
            _position++;
        }

        private static byte[] ConvertToByteArray(char[] array)
        {
            byte[] byteArray = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                char c = array[i];
                byteArray[i] = Convert.ToByte(c);
            }
            return byteArray;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            List<char> list = CreateList(_buffer);
            foreach (char c in list)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        private static List<char> CreateList(IEnumerable<char> chars)
        {
            return new List<char>(chars);
        }

        private static List<char> CreateList(string text)
        {
            return new List<char>(text.ToCharArray());
        }

        private static char[] ConvertToCharArray(byte[] array)
        {
            char[] charArray = new char[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                char c = Convert.ToChar(b);
                charArray[i] = c;
            }
            return charArray;
        }

        private static char ConvertToChar(int b)
        {
            return Convert.ToChar(b);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void InternalBlockCopy(Array src, int srcOffsetBytes, Array dst, int dstOffsetBytes, int byteCount);

        public override void Close()
        {
            base.Close();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}


