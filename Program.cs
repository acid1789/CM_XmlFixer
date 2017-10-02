using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XmlFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: XmlFixer.exe <properties_file> <xml_file>");
                return;
            }

            Dictionary<string, string> properties = new Dictionary<string, string>();
            string[] propLines = File.ReadAllLines(args[0]);
            foreach (string propLine in propLines)
            {
                string[] kvp = propLine.Split('=');
                if (kvp.Length == 2)
                {
                    properties[kvp[0]] = kvp[1];
                }
            }

            List<string> outLines = new List<string>();
            string[] xmlLines = File.ReadAllLines(args[1]);
            foreach (string xmlLine in xmlLines)
            {
                string writeLine = xmlLine;
                int index = xmlLine.IndexOf('@');
                if (index >= 0)
                {
                    index++;
                    int secondAt = xmlLine.IndexOf('@', index);
                    if (secondAt > index)
                    {
                        string key = xmlLine.Substring(index, secondAt - index);
                        if (properties.ContainsKey(key))
                        {
                            string left = xmlLine.Substring(0, index - 1);
                            string right = xmlLine.Substring(secondAt + 1);
                            writeLine = left + properties[key] + right;
                        }
                    }
                }
                outLines.Add(writeLine);
            }

            File.WriteAllLines(args[1], outLines.ToArray());
        }
    }
}
