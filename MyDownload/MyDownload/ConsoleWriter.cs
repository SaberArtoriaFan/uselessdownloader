using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDownload
{
    public class ConsoleWriter:TextWriter
    {

            public delegate void WriteFunc(string value);
            WriteFunc write;
            WriteFunc writeLine;

            public ConsoleWriter(WriteFunc writeFunc,WriteFunc writeLine)
            {
                this.write = writeFunc;
                this.writeLine = writeLine;
            }

            /// <summary>
            /// 编码转换-UTF8
            /// </summary>
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
                //get { return Encoding.Unicode; }
            }
        public override void WriteLine(string? value)
        {
            writeLine(value);
        }
        public override void Write(string? value)
        {
            write(value);
        }


    }
 }
