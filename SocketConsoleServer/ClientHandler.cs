using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SocketConsoleServer {
    public class ClientHandler {
        TcpClient client;
        StreamReader sr;
        StreamWriter sw;
        IPEndPoint IPEP;

        public ClientHandler(TcpClient client) {
            this.client = client;
            sr = new StreamReader(client.GetStream());
            sw = new StreamWriter(client.GetStream()) { AutoFlush = true };
            IPEP = (IPEndPoint)client.Client.RemoteEndPoint;
        }

        internal IPEndPoint Ip() {
            return IPEP;
        }
        internal StreamWriter Writer() {
            return sw;
        }

        internal void Handle() {

            while(true) {
                try {
                    string message = sr.ReadLine().ToLower();

                    //DISCONNECT CLIENT
                    if(!client.Connected || message == "EXIT") {
                        Console.WriteLine(IPEP + " - Disconnected");
                        break;
                    }

                    //READ MESSEAGE FROM CLIENT
                    string[] messages = message.Split(' ');
                    switch(messages[0]) {
                        case "dir":
                            DirectoryInfo dir = new DirectoryInfo(messages[1]); // objekt med information for det angive katalognavn
                            if(dir.Exists) {
                                sw.WriteLine("Exists! Created: " + dir.CreationTime.ToString());
                            } else {
                                sw.WriteLine("Doesn't exist!");
                            }
                            break;

                        case "subdir":
                            dir = new DirectoryInfo(messages[1]);
                            var subDirs = dir.GetDirectories();
                            var answer = "";
                            foreach(DirectoryInfo dirInfo in subDirs) {
                                answer += "Name:<" + dirInfo.Name + "> Extension:<" + dirInfo.Extension + ">" + "NEWLINE";
                            }
                            sw.WriteLine(answer);
                            break;
                        case "exit":
                            sw.WriteLine("terminate");
                            client.Close();
                            Console.WriteLine(IPEP + " - Terminated");
                            break;

                    }

                } catch(Exception e) {
                    if(e.GetType() == typeof(IOException)) {
                        client.Close();
                        Console.WriteLine(IPEP + " - Terminated");
                        break;
                    }
                }
            }
        }
    }
}
