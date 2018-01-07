using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows;

namespace SocketConsoleClient {
    class Program {
        TcpClient client;
        NetworkStream stream;
        StreamReader sr;
        StreamWriter sw;

        static void Main(string[] args) {
            Program run = new Program();
            run.Run();
        }

        public void Run() {
            var connected = Connect("127.0.0.1");
            while(connected) {
                var message = Console.ReadLine();
                sw.WriteLine(message);

                var answer = sr.ReadLine();
                try {
                    answer = answer.Replace("NEWLINE", Environment.NewLine);
                } catch(Exception) {
                }
                if(answer == "terminate") {
                    connected = false;
                }
                Console.WriteLine(answer);
            }
            Console.WriteLine("You connection has been terminated");
            Console.WriteLine("Press any key to leave");
            Console.ReadKey();
        }

        public bool Connect(string ip) {
            try {
                client = new TcpClient(ip, 12345);
                stream = client.GetStream();
                sr = new StreamReader(stream);
                sw = new StreamWriter(stream);
                sw.AutoFlush = true;
                return true;
            } catch(Exception e) {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Restart to program and try again");
                return false;
            }
        }

    }
}
