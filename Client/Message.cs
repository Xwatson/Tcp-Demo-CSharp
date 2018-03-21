using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Message
    {
        /// <summary>
        /// 传入字符串返回带数据长度的新数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] getBytes(string data)
        {
            // 转换原数据
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataLength = dataBytes.Length; // 数据长度
            // 将数据长度存入byte类型数据中
            byte[] lengthBytes = BitConverter.GetBytes(dataLength);

            return lengthBytes.Concat(dataBytes).ToArray();
        }
    }
}
