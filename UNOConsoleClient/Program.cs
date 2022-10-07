using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using UNO.Helper;
using UNO.Models;


//ConsoleClient
//Card MakeAction
//Controllers and methods in models

class Program
{
    //Удаляется карта из колоды даже если не подходит + вывод колоды сделать в нужном месте
    private static object localizer = new object();
    static void Main()
    {

        bool isYourMove = false;

        var cards = new List<Card>();
        var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(endPoint);

        var netStream = new NetworkStream(socket);

        Console.Write("Enter you nickname: ");
        string nickname = Console.ReadLine();
        TransmitMessageHelper.SendMessageUnicode(netStream, nickname);
        string cardsString = TransmitMessageHelper.GetMessageUTF8(netStream);

        TransmitMessageHelper.SendMessageUnicode(netStream, "Received deck.");

        var serializer = new DataContractJsonSerializer(typeof(List<Card>));
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(cardsString)))
        {
            cards = serializer.ReadObject(stream) as List<Card>;
        }
        foreach (var item in cards)
        {
            Console.WriteLine($"{item.Value}: {item.Colors.ElementAt(0)}\n");
        }
        var player = new Player(nickname, cards);

        var mooveSerializer = new DataContractJsonSerializer(typeof(TransmitMessage));
        var moveMessage = new TransmitMessage();

        while (true)
        {
            string message = TransmitMessageHelper.GetMessageUTF8(netStream);

            switch (message)
            {
                case "Your move...":
                    while (true)
                    {
                        Console.Write("Your moove: Enter index of a card.");
                        string index = Console.ReadLine();
                        TransmitMessageHelper.SendMessageUnicode(netStream, index);
                        //TODO: ответ с сервера что карта положилась
                        //IsCardDroppedAsync(netStream, cards, int.Parse(index));
                        if (index == "-1")
                        {
                            break;
                        }
                        else
                        {
                            string mes = TransmitMessageHelper.GetMessageUTF8(netStream);
                            if (mes.Contains("Success"))
                            {
                                var card = player.Cards.ElementAt(int.Parse(index));
                                cards.Remove(card);

                                if (card.Value == UNO.Enum.Value.ChooseColor || card.Value == UNO.Enum.Value.ChooseColorPlus4)
                                {
                                    Console.WriteLine("Enter index of a color: ");
                                    index = Console.ReadLine();
                                    TransmitMessageHelper.SendMessageUnicode(netStream, index);
                                }
                                //isYourMove = false;
                                foreach (var item in cards)
                                {
                                    Console.WriteLine($"{item.Value}: {item.Colors.ElementAt(0)}\n");
                                }
                                Console.WriteLine("--------------------------------------");
                                break;
                            }
                        }
                    }
                    break;
                case "AddCards":
                    TransmitMessageHelper.SendMessageUTF8(netStream, "Synchronisation");
                    string newCardsString = TransmitMessageHelper.GetMessageUTF8(netStream);
                    TransmitMessageHelper.SendMessageUTF8(netStream, "Received");
                    #region VeryShiiiitCodeee
                    if (newCardsString.Contains("Your move..."))
                    {
                        isYourMove = true;
                        newCardsString = newCardsString.Replace("Your move...", "");
                        newCardsString = newCardsString.Remove(newCardsString.Length - 2, 2);
                    }
                    #endregion
                    var newCards = new List<Card>();
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(newCardsString)))
                    {
                        newCards = serializer.ReadObject(stream) as List<Card>;
                    }
                    foreach (var card in newCards)
                        player.AddCard(card);
                    foreach (var item in cards)
                    {
                        Console.WriteLine($"{item.Value}: {item.Colors.ElementAt(0)}\n");
                    }
                    break;
                case "PreviousPlayerMove":
                    try
                    {
                        message = TransmitMessageHelper.GetMessageUTF8(netStream);
                        message = message.Remove(message.Length - 2, 2);
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
                        {
                            moveMessage = mooveSerializer.ReadObject(stream) as TransmitMessage;
                            Console.WriteLine($"{moveMessage.SenderNickname}: dropped {moveMessage.TopCard.ToString()}");
                        }
                        TransmitMessageHelper.SendMessageUTF8(netStream, "Received");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }
        }

        //foreach (var card in cards)
        //{
        //    Console.WriteLine($"{card.Value}: {card.Colors.ElementAt(0)}\n");
        //}
        //while (true)
        //{

        //    string data = TransmitMessageHelper.GetMessageUTF8(netStream);
        //    #region ShitCode
        //    if (data.Contains("AddCards"))
        //    {
        //        Console.WriteLine(data);
        //        data = data.Replace("AddCards", "");
        //        string newCardsString = TransmitMessageHelper.GetMessageUTF8(netStream);
        //        TransmitMessageHelper.SendMessageUTF8(netStream, "Received");
        //        #region VeryShiiiitCodeee
        //        if (newCardsString.Contains("Your move..."))
        //        {
        //            isYourMove = true;
        //            newCardsString = newCardsString.Replace("Your move...", "");
        //            newCardsString = newCardsString.Remove(newCardsString.Length - 2, 2);
        //        }
        //        #endregion
        //        var newCards = new List<Card>();
        //        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(newCardsString)))
        //        {
        //            newCards = serializer.ReadObject(stream) as List<Card>;
        //        }
        //        foreach (var card in newCards)
        //            player.AddCard(card);
        //        foreach (var item in cards)
        //        {
        //            Console.WriteLine($"{item.Value}: {item.Colors.ElementAt(0)}\n");
        //        }
        //    }
        //    //data = TransmitMessageHelper.GetMessageUTF8(netStream);

        //    if (!isYourMove)
        //    {
        //        if (data.Contains("Your move..."))
        //        {
        //            isYourMove = true;
        //        }
        //        else
        //        {
        //            isYourMove = false;
        //        }

        //    }

        //    data = data.Replace("Your move...", "");
        //    if (!string.IsNullOrWhiteSpace(data))
        //    {
        //        data = data.Remove(data.Length - 2, 2);
        //        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
        //        {
        //            message = mooveSerializer.ReadObject(stream) as TransmitMessage;
        //            Console.WriteLine($"{message.SenderNickname}: dropped {message.TopCard.ToString()}");
        //        }
        //    }

        //    while (isYourMove)
        //    {
        //        Console.Write("Your moove: Enter index of a card.");
        //        string index = Console.ReadLine();
        //        TransmitMessageHelper.SendMessageUnicode(netStream, index);
        //        //TODO: ответ с сервера что карта положилась
        //        //IsCardDroppedAsync(netStream, cards, int.Parse(index));
        //        string mes = TransmitMessageHelper.GetMessageUnicode(netStream);
        //        if (mes.Contains("Success"))
        //        {
        //            var card = player.Cards.ElementAt(int.Parse(index));
        //            cards.Remove(card);

        //            if (card.Value == UNO.Enum.Value.ChooseColor || card.Value == UNO.Enum.Value.ChooseColorPlus4)
        //            {
        //                Console.WriteLine("Enter index of a color: ");
        //                index = Console.ReadLine();
        //                TransmitMessageHelper.SendMessageUnicode(netStream, index);
        //            }
        //            isYourMove = false;
        //            foreach (var item in cards)
        //            {
        //                Console.WriteLine($"{item.Value}: {item.Colors.ElementAt(0)}\n");
        //            }
        //        }
        //    }
        //    #endregion
        //}
    }
}