using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // 创建socket
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 43991);
            // 绑定ip和端口
            serverSocket.Bind(ipEndPoint);
            // 开始监听端口号 0表示不限制连接数
            serverSocket.Listen(0);
            Console.WriteLine("服务端已启动成功...");

            // 等待客户端连接
            // Socket clientSocket = serverSocket.Accept();
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

            Console.ReadKey();
        }

        static byte[] dataBuffer = new byte[1024];
        static Message msg = new Message();
        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);

            // 向客户端发送消息
            string msgStr = "hello client！你好。";
            byte[] data = Encoding.UTF8.GetBytes(msgStr);
            clientSocket.Send(data);

            // 接受客户端消息
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            // 继续处理下个客户端连接
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }
        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                // 更新存储了多少数据长度
                msg.AddIndex(count);
                // 装换byte到string
                //string clientMsgReceive = Encoding.UTF8.GetString(dataBuffer, 0, count);
                //Console.WriteLine("从客户端接受到消息：" + clientMsgReceive);
                msg.ReadMessage();
                // 循环监听
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch (Exception e)
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
                Console.WriteLine("客户端未正常关闭：" + e);
            }
        }
    }
}
