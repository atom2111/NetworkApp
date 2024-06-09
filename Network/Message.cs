using System;
using System.Text.Json;

namespace Network
{
    public enum MessageType
    {
        Regular,
        List
    }

    public class Message
    {
        public string Text { get; set; }
        public string NicknameFrom { get; set; }
        public string NicknameTo { get; set; }
        public DateTime DateTime { get; set; }
        public MessageType Type { get; set; }

        public string SerializeMessageToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Message DeserializeFromJson(string json)
        {
            return JsonSerializer.Deserialize<Message>(json);
        }

        public void Print()
        {
            Console.WriteLine($"[{DateTime}] {NicknameFrom} to {NicknameTo}: {Text}");
        }
    }
}
