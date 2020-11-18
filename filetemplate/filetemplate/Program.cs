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
                        int counter = 1;
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

                        if (words.Count > 0)
                        {
                            if (words.Count > 1)
                            Console.WriteLine($"Enter seperator.");
                            String seperator = Console.ReadLine();

                            CreateFile((ApplicationArguments)(b.Object), words, seperator);
                        }
                        else
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

            bool first = true;
            string value = string.Empty;
            List<string> outputLines = new List<string>();

            foreach (var word in words)
            {
                if (first)
                {
                    value = word;
                    first = false;
                }
                else value += seperator + word;
            }

            var inputLines = File.ReadAllLines(args.CopyFrom);

            foreach (var line in inputLines)
            {
                var inLine = Encrypt(line);
                var outLine = Unencrypt(string.Format(inLine, value, value.ToUpper()));
                outputLines.Add(outLine);
            }

            File.WriteAllLines(args.CopyTo, outputLines);
        }

        private static string Encrypt(string line)
        {
            string result = line.Replace("{", "^^").Replace("}", "^#").Replace("@0", "{0}").Replace("@1", "{1}");
            return result;
        }

        private static string Unencrypt(string line)
        {
            string result = line.Replace("^^", "{").Replace("^#", "}");
            return result;
        }

    }

 
}
