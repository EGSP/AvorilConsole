using System;

namespace AvorilConsole
{

    public interface IPrinter
    {
        void Print(object info);
    }

    public static class Log
    {
        private static IPrinter printer;

        public static void SetDefaultPrinter(IPrinter _printer)
        {
            if(printer == null)
                throw new SystemException("Null printer in Log.cs - setdefaultprinter method");

            printer = _printer;
        }

        public static void print(object info)
        {
            if (printer == null)
                throw new SystemException("Null printer in Log.cs - print method");

            printer.Print(info);
        }

        public static void print(object info, IPrinter anotherPrinter)
        {
            if (anotherPrinter == null)
                throw new SystemException("Null anotherPrinter in Log.cs - print method");

            anotherPrinter.Print(info);
        }
    }
    

    public class DesktopConsolePrinter : IPrinter
    {
        public void Print(object info)
        {
            Console.WriteLine(info);
        }
    }
}
