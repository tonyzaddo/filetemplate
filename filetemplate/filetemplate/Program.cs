using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fclp;
using System.IO;

namespace filetemplate
{
    class Program
    {
        protected static log4net.ILog log;

        static void Main(string[] args)
        {

            List<string> words = new List<string>();

            try
            {
                log4net.Config.XmlConfigurator.Configure();
                log = LogHelper.GetLogger();

                log.Debug("Begin Main");

                try
                {
                    // create a builder for the ApplicationArguments type
                    var b = new FluentCommandLineParser<ApplicationArguments>();

                    b.Setup(arg => arg.CopyFrom)
                     .As('f', "copyfrom");

                    b.Setup(arg => arg.CopyTo)
                     .As('t', "copyto");

                    var result = b.Parse(args);

                    if (result.HasErrors == false)
                    {
                        int counter = 0;
                        bool getwords = true;

                        while (getwords)
                        {
                            Console.WriteLine($"Enter word {counter}. (Blank if no more words)");
                            String word = Console.ReadLine();
                            if (string.IsNullOrEmpty(word)) getwords = false;
                            else
                            {
                                words.Add(word);
                                counter++;
                            }
                        }

                        if (words.Count == 0)
                        {
                            Console.WriteLine("No words entered.");
                        }
                        
                    }
                    else log.Error(String.Format("Startup Parameters invalid: {0}" + result.Errors.ToString()));
                }
                catch (Exception ex)
                {
                    log.Error(String.Format("Error: {0}", ex.Message));
                    Console.WriteLine(ex.Message);
                }

                log.Debug("End Main");

            }
            catch { }

        }

        public static void CreateFile(ApplicationArguments args, List<string> words, string seperator)
        {

            string value = string.Empty;
            List<string> outputLines = new List<string>();
            List<string> upperWords = new List<string>();


            foreach (var word in words)
            {
                upperWords.Add(word.ToUpper());
            }

            var inputLines = File.ReadAllLines(args.CopyFrom);

            foreach (var line in inputLines)
            {

                // Normal Case
                var inLine = Encrypt(line, "@");
                var outLine = Unencrypt(string.Format(inLine, words.ToArray()));

                // Upper Case
                inLine = Encrypt(outLine, "^");
                outLine = Unencrypt(string.Format(inLine, upperWords.ToArray()));

                outputLines.Add(outLine);
            }

            File.WriteAllLines(args.CopyTo, outputLines);
        }

        private static string Encrypt(string line, string key)
        {
            string result = line.Replace("{", "^^").Replace("}", "^#");
            for (int i = 0; i < 30; i++)
            {
                string str1 = key + $"{i}";
                string str2 = "{" + $"{i}" + "}";
                result = result.Replace(str1, str2);
            }
            return result;
        }

        private static string Unencrypt(string line)
        {
            string result = line.Replace("^^", "{").Replace("^#", "}");
            return result;
        }

    }

 
}
