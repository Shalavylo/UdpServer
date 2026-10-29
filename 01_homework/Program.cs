using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    static void Main(string[] args)
    {
        const int port = 8080;
        using UdpClient server = new UdpClient(port);
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine($"UDP-сервер запущено на порту {port}.");
        Console.WriteLine("Очікування повідомлень від клієнта...");

        while (true)
        {
            IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);

            byte[] receivedBytes = server.Receive(ref client);
            string message = Encoding.UTF8.GetString(receivedBytes);
            Console.WriteLine($"[{client}] Клієнт: {message}");

            string response = GenerateResponse(message);

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            server.Send(responseBytes, responseBytes.Length, client);
            Console.WriteLine($"Сервер: {response}");
        }
    }

    static string GenerateResponse(string message)
    {
        string text = message.Trim().ToLowerInvariant();

        if(string.IsNullOrWhiteSpace(text))
        {
            return "Напишіть щось!";
        }

        if (text.Contains("привіт") || text.Contains("hello")
            || text.Contains("хай"))
            return "Привіт! Як справи?";

        if (text.Contains("як справи"))
            return "Усе добре! А в тебе?";

        if (text.Contains("як тебе звати") || text.Contains("хто ти"))
            return "Я Бот, приємно познайомитися!";

        if (text.Contains("погода"))
            return "На жаль, я поки не вмію прогнозувати погоду.";

        if (text.Contains("бувай") || text.Contains("до побачення"))
            return "Бувай! Гарного дня!";

        if (text.Contains("час"))
            return $"Зараз {DateTime.Now:HH:mm}.";

        return "Цікаво! Розкажи більше.";
    }
}