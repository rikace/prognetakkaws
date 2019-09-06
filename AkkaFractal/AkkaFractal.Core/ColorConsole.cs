namespace AkkaFractal.Core
{
    using System;

    public static class ColorConsole
    {        
        public static void WriteLineGreen(string message, params object[] args)
        {
            Write(ConsoleColor.Green, string.Format(message, args));            
        }

        public static void WriteLineYellow(string message, params object[] args)
        {
            Write(ConsoleColor.Yellow, string.Format(message, args));
        }

        public static void WriteLineRed(string message, params object[] args)
        {
            Write(ConsoleColor.Red, string.Format(message, args));
        }

        public static void WriteLineCyan(string message, params object[] args)
        {
            Write(ConsoleColor.Cyan, string.Format(message, args));
        }

        public static void WriteLineGray(string message, params object[] args)
        {
            Write(ConsoleColor.Gray, string.Format(message, args));
        }

        public static void WriteMagenta(string message, params object[] args)
        {
            Write(ConsoleColor.Magenta, string.Format(message, args));
        }

        public static void WriteWhite(string message, params object[] args)
        {
            Write(ConsoleColor.White, string.Format(message, args));
        }


        static object locker = new object();
        private static void Write(ConsoleColor color, string message)
        {
            lock (locker)
            {
                var bakColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = bakColor;
            }
        }
    }
}
