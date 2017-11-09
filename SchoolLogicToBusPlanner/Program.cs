using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicToBusPlanner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                string fileName = string.Empty;

                foreach (string argument in args)
                {
                    if (argument.ToLower().StartsWith("/filename:"))
                    {
                        fileName = argument.Substring(10, argument.Length - 10);
                    }
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    SendSyntax();
                }
                else
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    StudentListReport generator = new StudentListReport();
                    WriteFile(generator.Generate(), fileName); 
                }
            }
            else
            {
                // If any argument is missing, send the syntax
                SendSyntax();
            }

        }

        static void SendSyntax()
        {
            Console.WriteLine(@"SYNTAX: /filename:name.tsv");
        }

        public static void WriteFile(MemoryStream fileContent, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            if (fileName.Length <= 1) return;

            using (FileStream fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
            {
                fileContent.WriteTo(fileStream);
                fileStream.Flush();
            }
        }
    }
}
