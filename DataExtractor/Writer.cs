using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataExtractor
{
    public class Writer
    {
        public string Path { get; set; }

        public Writer(string path)
        {
            Path = path;
        }

        public void Write(string text)
        {
            using (StreamWriter sw = File.AppendText(Path))
            {
                sw.WriteLine(text);
            }
        }
    }
}
