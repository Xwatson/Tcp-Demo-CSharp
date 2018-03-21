using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // 客户端
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 43991));
            Console.WriteLine("客户端已启动...");

            //接受服务端数据
            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);
            Console.WriteLine(Encoding.UTF8.GetString(data, 0, count));
            //// 普通消息发送
            //while (true)
            //{
            //    // 发送消息到服务的
            //    string msgStr = Console.ReadLine();
            //    clientSocket.Send(Encoding.UTF8.GetBytes(msgStr));
            //}

            // 模拟粘包发送
            for (int i = 0; i < 100; i++)
            {
                clientSocket.Send(Message.getBytes(i.ToString()));
            }
            Console.ReadKey();
            clientSocket.Close();
        }
    }
}
