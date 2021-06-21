using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CodeAnalysis.MSBuild.Writers
{
    public partial class CodeStream
    {
        public void Write(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                foreach (char c in value)
                {
                    _buffer.Add(c);
                }
            }
        }

        public void Write(char[] buffer)
        {
            if (buffer?.Length > 0)
            {
                foreach (char c in buffer)
                {
                    _buffer.Add(c);
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte value = buffer[offset];
            for (int i = 0; i < count; i++)
            {
                WriteByte(value);
            }
        }

        public void WriteChar(char c)
        {
            WriteByte(Convert.ToByte(c));
        }

        public override void WriteByte(byte value)
        {
            _buffer.Add(Convert.ToChar(value));
            Advance();
        }

        public void Write(bool value)
        {
            if (value)
            {
                Write("true");
            }
            else
            {
                Write("false");
            }
        }

        public void Write(char[] buffer, int index, int count)
        {
            for (int i = index; i < count; i++)
            {
                char c = buffer[i];
                Write(c);
            }
        }

        public void Write(decimal value)
        {
            if (value != null)
            {
                Write(value.ToString());
            }
        }

        public void Write(double value)
        {
            if (value != null)
            {
                Write(value.ToString());
            }
        }

        public void Write(float value)
        {
            if (value != null)
            {
                Write(value.ToString());
            }
        }

        public void Write(int value)
        {
            if (value != null)
            {
                Write(value.ToString());
            }
        }

        public void Write(long value)
        {
            if (value != null)
            {
                Write(value.ToString());
            }
        }

        public void Write(object value)
        {
            Write(value.ToString());
        }

        public void Write(string format, object arg0)
        {
            Write(string.Format(format, arg0));
        }

        public void Write(string format, object arg0, object arg1)
        {
            Write(string.Format(format, arg0, arg1));
        }

        public void Write(string format, object arg0, object arg1, object arg2)
        {
            Write(string.Format(format, arg0, arg1, arg2));
        }

        public void Write(string format, params object[] arg)
        {
            Write(string.Format(format, arg));
        }

        public void Write(uint value)
        {
            Write(value.ToString());
        }

        public void Write(ulong value)
        {
            Write(value.ToString());
        }

        public void WriteLine()
        {
            Write(Environment.NewLine);
        }

        public void WriteLine(bool value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(char value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(char[] buffer)
        {
            Write(buffer);
            WriteLine();
        }

        public void WriteLine(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
            WriteLine();
        }

        public void WriteLine(decimal value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(double value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(float value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(int value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(long value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(object value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(string format, object arg0)
        {
            Write(format, arg0);
            WriteLine();
        }

        public void WriteLine(string format, object arg0, object arg1)
        {
            Write(format, arg0, arg1);
            WriteLine();
        }

        public void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Write(format, arg0, arg1, arg2);
            WriteLine();
        }

        public void WriteLine(string format, params object[] arg)
        {
            Write(format, arg);
            WriteLine();
        }

        public void WriteLine(uint value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(ulong value)
        {
            Write(value);
            WriteLine();
        }
    }
}


