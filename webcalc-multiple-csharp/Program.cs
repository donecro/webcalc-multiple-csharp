using System;
using System.Threading;

namespace webcalc_multiple_csharp
{
    internal static class Program
    {
        private const int Port = 8003;
        
        public static void Main(string[] args)
        {
            HttpServer httpServer =
                args.GetLength(0) > 0 ?
                    new MyHttpServer(Convert.ToInt16(args[0])) :
                    new MyHttpServer(Port);
            new Thread(httpServer.Listen).Start();

            Console.WriteLine("Multiple: Starting server, listen at: http://127.0.0.1:" + Port);

        }
    }
}