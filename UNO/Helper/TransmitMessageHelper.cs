using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Helper
{
    public static class TransmitMessageHelper
    {
        public static void SendMessage(Socket socket, string message)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                socket.Send(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static string GetMessage(Socket socket)
        {
            try
            {
                var buffer = new byte[64];
                int size = 0;
                var builder = new StringBuilder();
                do
                {
                    size = socket.Receive(buffer);
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (socket.Available > 0);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        public static void SendMessageUTF8(NetworkStream stream, string message)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                stream.Write(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static string GetMessageUTF8(NetworkStream stream)
        {
            try
            {
                var buffer = new byte[64];
                int size = 0;
                var builder = new StringBuilder();
                do
                {
                    size = stream.Read(buffer, 0, buffer.Length);
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (stream.DataAvailable);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        public static void SendMessageUnicode(NetworkStream stream, string message)
        {
            try
            {
                var bytes = Encoding.Unicode.GetBytes(message);
                stream.Write(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static string GetMessageUnicode(NetworkStream stream)
        {
            try
            {
                var buffer = new byte[64];
                int size = 0;
                var builder = new StringBuilder();
                do
                {
                    size = stream.Read(buffer, 0, buffer.Length);
                    builder.Append(Encoding.Unicode.GetString(buffer, 0, size));
                }
                while (stream.DataAvailable);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        public static async Task<string> GetMessageUnicodeAsync(NetworkStream stream)
        {
            return await Task.Run(() => GetMessageUnicode(stream));
        }
    }
}
