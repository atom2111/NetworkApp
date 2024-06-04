using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task serverTask = Task.Run(() => Server(cts.Token));

            Console.WriteLine("Нажмите любую клавишу для завершения работы сервера...");
            Console.ReadKey();
            cts.Cancel();

            try
            {
                serverTask.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    if (ex is TaskCanceledException)
                    {
                        Console.WriteLine("Сервер был остановлен.");
                    }
                    else
                    {
                        Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    }
                }
            }
        }

        public static void Server(CancellationToken cancellationToken)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Сервер ждёт сообщение от клиента");

            try
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Отмена операции...");
                        break;
                    }

                    // Проверяем, есть ли данные в буфере и выполняем Receive только если они есть
                    if (udpClient.Available > 0)
                    {
                        byte[] buffer = udpClient.Receive(ref iPEndPoint);
                        var messageText = Encoding.UTF8.GetString(buffer);

                        Message message = Message.DeserializeFromJson(messageText);
                        message.Print();

                        // Отправка подтверждения клиенту
                        byte[] confirmationBytes = Encoding.UTF8.GetBytes("ACK");
                        udpClient.Send(confirmationBytes, confirmationBytes.Length, iPEndPoint);
                    }
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine($"Произошла ошибка сокета: {se.Message}");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Сервер завершил работу.");
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}
