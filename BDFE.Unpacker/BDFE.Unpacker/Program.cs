using System;
using System.IO;

namespace BDFE.Unpacker
{
    class Program
    {
        private static String m_Title = "Bravely Default: Fairy's Effect Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("(c) 2023 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    BDFE.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    BDFE.Unpacker E:\\Games\\BDFE\\assets\\DF27884D864A736188D522D350CFCC1C D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_ArcFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_ArcFile))
            {
                Utils.iSetError("[ERROR]: Input archive file -> " + m_ArcFile + " <- does not exist");
                return;
            }

            ArcUnpack.iDoIt(m_ArcFile, m_Output);
        }
    }
}
