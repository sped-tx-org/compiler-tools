// -----------------------------------------------------------------------
// <copyright file="AbstractCodeWriter.cs" company="Ollon, LLC">
//     Copyright © 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.CodeAnalysis.MSBuild.Writers
{
    public class CodeWriter : TextWriter
    {
        #region Indent Support

        public int Indent = 0;

        public virtual string IndentString { get; }
        public virtual void PushIndent() => Indent++;
        public virtual void PopIndent() => Indent--;
        public virtual void ClearIndent() => Indent = 0;

        private bool _canWriteIndent = false;
        public void WriteIndent()
        {
            if (_canWriteIndent)
            {
                for (int i = 0; i < Indent; i++)
                {
                    _stream.Write((string) IndentString);
                }

                _canWriteIndent = false;
            }
        }

        #endregion

        public void OpenBrace(bool newLine = true)
        {
            if (newLine)
            {
                WriteLine('{');
                PushIndent();
            }
            else
            {
                Write('}');
            }
        }

        public void CloseBrace(bool newLine = true)
        {
            if (newLine)
            {
                PopIndent();
                WriteLine('}');
            }
            else
            {
                Write('}');
            }
        }

        public void OpenBracket(bool newLine = true)
        {
            if (newLine)
            {
                WriteLine('[');
                PushIndent();
            }
            else
            {
                Write('[');
            }
        }

        public void CloseBracket(bool newLine = true)
        {
            if (newLine)
            {
                PopIndent();
                WriteLine(']');
            }
            else
            {
                Write(']');
            }
        }

        public void OpenParen(bool newLine = true)
        {
            if (newLine)
            {
                WriteLine('(');
                PushIndent();
            }
            else
            {
                Write('(');
            }
        }

        public void CloseParen(bool newLine = true)
        {
            if (newLine)
            {
                PopIndent();
                WriteLine(')');
            }
            else
            {
                Write(')');
            }
        }

        public override void Write(char value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(char[] buffer)
        {
            WriteIndent();
            _stream.Write(buffer);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            WriteIndent();
            _stream.Write(buffer, index, count);
        }

        public override void Write(bool value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(int value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(uint value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(long value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(ulong value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(float value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(double value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(decimal value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(string value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(object value)
        {
            WriteIndent();
            _stream.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            WriteIndent();
            _stream.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            WriteIndent();
            _stream.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            WriteIndent();
            _stream.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            WriteIndent();
            _stream.Write(format, arg);
        }

        public override void WriteLine()
        {
            WriteIndent();
            _stream.WriteLine();
            _canWriteIndent = true;
        }

        public override void WriteLine(char value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(char[] buffer)
        {
            WriteIndent();
            _stream.WriteLine(buffer);
            _canWriteIndent = true;
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            WriteIndent();
            _stream.WriteLine(buffer, index, count);
            _canWriteIndent = true;
        }

        public override void WriteLine(bool value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(int value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(uint value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(long value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(ulong value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(float value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(double value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(decimal value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(string value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(object value)
        {
            WriteIndent();
            _stream.WriteLine(value);
            _canWriteIndent = true;
        }

        public override void WriteLine(string format, object arg0)
        {
            WriteIndent();
            _stream.WriteLine(format, arg0);
            _canWriteIndent = true;
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            WriteIndent();
            _stream.WriteLine(format, arg0, arg1);
            _canWriteIndent = true;
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            WriteIndent();
            _stream.WriteLine(format, arg0, arg1, arg2);
            _canWriteIndent = true;
        }

        public override void WriteLine(string format, params object[] arg)
        {
            WriteIndent();
            _stream.WriteLine(format, arg);
            _canWriteIndent = true;
        }

        private readonly CodeStream _stream;
        public CodeWriter(string indentString = "    ")
        {
            NewLine = Environment.NewLine;
            _stream = new CodeStream();
            IndentString = indentString;
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        public void WriteUsing(string name)
        {
            WriteLine($"using {name};");
        }

        #region Dispose/Close/Flush
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stream.Close();
            }
        }

        public override void Close()
        {
            _stream.Close();
        }

        public override void Flush()
        {
            _stream.Flush();
        }
        #endregion

        private static byte[] CreateByteArray(char[] chars)
        {
            List<byte> list = new List<byte>();
            foreach (var c in chars)
            {
                list.Add(Convert.ToByte(c));
            }
            return list.ToArray();
        }

        public override string ToString()
        {
            return _stream.ToString();
        }

        public CodeStream GetStream() => _stream;
    }
}


