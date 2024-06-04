using System;
using System.Text.Json;

namespace Network
{
    public class Message
    {
        public string Text { get; set; }
        public string NicknameFrom { get; set; }
        public string NicknameTo { get; set; }
        public DateTime DateTime { get; set; }

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
            Console.WriteLine($"[{DateTime}] {NicknameFrom} -> {NicknameTo}: {Text}");
        }
    }
}
