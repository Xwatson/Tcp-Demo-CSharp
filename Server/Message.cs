using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Message
    {
        private byte[] data = new byte[1024]; // 数据缓存
        private int startIndex = 0; // 标记已经存取了多少数据长度

        public byte[] Data
        {
            get { return data; }
        }
        public int StartIndex
        {
            get { return startIndex; }
        }
        // 获取剩余空间
        public int RemainSize
        {
            get
            {
                // 返回总长度减去已经存储的长度
                return data.Length - startIndex;
            }
        }
        /// <summary>
        /// 更新存储了多少数据
        /// </summary>
        /// <param name="count"></param>
        public void AddIndex(int count)
        {
            startIndex += count;
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <returns></returns>
        public void ReadMessage()
        {
            while (true)
            {
                // 数据长度还不完整
                if (startIndex <= 4) { return; }
                // 开始解析数据, 解析出标识的数据长度
                int count = BitConverter.ToInt32(data, 0);
                // startIndex - 4得出除去标识数据长度，剩余的真实数据长度
                // 如果真实数据长度大于标识的长度，说明数据完整
                if (startIndex - 4 >= count)
                {
                    // 开始读取数据，从4开始代表0到3为标识数据长度所占用
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出一条数据：" + s);
                    // 移动数据 count+4表示上面已经读取了， startIndex-4-count表示剩余长度
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= count + 4; // 读取后，更新存储数据长度
                }
                else
                {
                    break;
                }
            }
        }
    }
}
