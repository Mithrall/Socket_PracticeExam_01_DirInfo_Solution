using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace SocketConsoleServer {
    class Program {
        private static bool _exitSystem;


        static void Main(string[] args) {
            Program run = new Program();
            run.Run();
        }

        private void Run() {
            Console.WriteLine("Online");

            TcpListener server = new TcpListener(IPAddress.Any, 12345);
            server.Start();

            while(!_exitSystem) {
                var client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ClientConnection, client);
            }
        }

        private void ClientConnection(object obj) {
            var client = (TcpClient)obj;

            ClientHandler handler = new ClientHandler(client);

            Console.WriteLine("New Connection: " + handler.Ip());
            handler.Handle();
        }
    }
}
