using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using UNO.Helper;
using UNO.Interfaces;
using UNO.Models;
using UNO.Models.CardModels;

namespace UNO.Controllers
{
    public class GameController : IGameController
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        private readonly ICardCorrectChecker checker;

        private Game game;
        private string ip;
        private int port;
        private Socket socket;

        public GameController(string ip, int port, Game game, ICardCorrectChecker checker)
        {
            this.ip = ip;
            this.port = port;
            this.game = game;
            this.checker = checker;
        }

        public void FullFillDeck()
        {
            try
            {


                for (int i = 0; i < 4; i++)
                {
                    game.AddCard(new NumericCard(0, new List<Enum.Color> { (Enum.Color)i }));
                    game.AddCard(new ChooseColorCard((Enum.Value)13, new List<Enum.Color> { Enum.Color.Red, Enum.Color.Green, Enum.Color.Blue, Enum.Color.Yellow }));
                    game.AddCard(new ChooseColorAndAddFourCard((Enum.Value)14, new List<Enum.Color> { Enum.Color.Red, Enum.Color.Green, Enum.Color.Blue, Enum.Color.Yellow }));
                }
                for (int i = 1; i <= 9; i++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            game.AddCard(new NumericCard((Enum.Value)i, new List<Enum.Color> { (Enum.Color)j }));
                        }
                    }
                }
                for (int k = 0; k < 2; k++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        game.AddCard(new AddTwoCard(Enum.Value.Plus2, new List<Enum.Color> { (Enum.Color)j }));
                    }
                }
                for (int k = 0; k < 2; k++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        game.AddCard(new SkipCard(Enum.Value.Skip, new List<Enum.Color> { (Enum.Color)j }));
                    }
                }
                for (int k = 0; k < 2; k++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        game.AddCard(new ReverseCard(Enum.Value.Reverse, new List<Enum.Color> { (Enum.Color)j }));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool GiveRandomCardToPlayer(Player player)
        {
            try
            {
                int cardIndex = rnd.Next(0, game.DeckSize);
                var card = game.Deck.ElementAt(cardIndex);
                if (card == null)
                    throw new ArgumentException($"Something wrong with card taking");
                player.AddCard(card);
                game.RemoveCard(card.Id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        private Player StartGame()
        {
            try
            {
                #region Checking
                if (game == null)
                    throw new ArgumentNullException(nameof(game));
                if (game.PlayersCount < 2 || game.PlayersCount > 10)
                    throw new ArgumentException("Player's count should be in the range.");
                #endregion
                FullFillDeck();
                #region Give cards for each player
                for (int i = 0; i < game.PlayersCount; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        GiveRandomCardToPlayer(game.Players.ElementAt(i));
                    }
                    game.SendDeckToPlayer(game.Players.ElementAt(i));
                    TransmitMessageHelper.GetMessageUnicode(game.Players.ElementAt(i).Stream);
                }
                #endregion

                while (true)
                {
                    Card card;
                    Player player;
                    while (true)
                    {
                        player = game.Players.ElementAt(game.CurrentPlayerIndex);
                        card = player.MakeMove();
                        if (checker.CheckCard(card, game))
                        {
                            TransmitMessageHelper.SendMessageUTF8(player.Stream, "Success");
                            break;
                        }
                    }
                    //game.SetTopCard(card, player);
                    game.TopCard = card;
                    player.RemoveCard(card.Id);
                    if (player.CardsCount == 0)
                        return player;
                    if (player.TookCardsCount != 0)
                    {
                        SendRandomCardsToPlayer(player, player.TookCardsCount);
                    }
                    game.TopCard.MakeAction(game);

                    BroadcastMove(player.Nickname, game.TopCard);
                    game.IncreaseIndex();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void WaitForPlayers()
        {
            try
            {
                var endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(endPoint);
                while (game.PlayersCount < 2)
                {
                    socket.Listen(900);
                    var listnerSocket = socket.Accept();
                    string message = TransmitMessageHelper.GetMessage(listnerSocket);
                    if (message == "EndWait")
                        break;
                    Console.WriteLine($"{message} has been connected.");
                    var player = new Player(message, this, listnerSocket);
                    //player.Socket = listnerSocket;
                    game.AddPlayer(player);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //finally
            //{
            //    //socket.Shutdown(SocketShutdown.Both);
            //    //socket.Close();
            //}
        }

        public void SendRandomCardsToPlayer(Player player, int cardCount)
        {
            var cards = new List<Card>();
            for (int i = 0; i < cardCount; i++)
            {
                cards.Add(game.GiveRandomCardToPlayer(player));
            }
            TransmitMessageHelper.SendMessageUTF8(player.Stream, "AddCards");

            TransmitMessageHelper.GetMessageUTF8(player.Stream);

            Card.SendCardsToPlayer(player, cards);

            TransmitMessageHelper.GetMessageUTF8(player.Stream);
        }

        public void Run()
        {
            try
            {
                Console.WriteLine("Waiting for players...");
                WaitForPlayers();
                Console.WriteLine("Game started...");
                var player = StartGame();
                if (player == null)
                    return;
                string message = $"Player {player.Nickname} won!!!";
                Console.WriteLine(message);
                BroadcastMessage(game.Id, message);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void BroadcastMessage(Guid senderId, string message)
        {
            foreach (var player in game.Players)
            {
                if (player.Id != senderId)
                {
                    TransmitMessageHelper.SendMessageUnicode(player.Stream, message);
                }
            }
        }

        public void BroadcastMove(string senderNickname, Card card)
        {
            try
            {
                foreach (var player in game.Players)
                {
                    if (player.Nickname != senderNickname)
                    {
                        TransmitMessageHelper.SendMessageUTF8(player.Stream, "PreviousPlayerMove");
                        var message = new TransmitMessage { SenderNickname = senderNickname, TopCard = card };
                        var json = new DataContractJsonSerializer(typeof(TransmitMessage));
                        using (var stream = new MemoryStream())
                        {
                            //fdghbmnhgfd
                            json.WriteObject(stream, message);
                            TransmitMessageHelper.SendMessageUnicode(player.Stream, Encoding.Unicode.GetString(stream.ToArray()));
                        }
                        TransmitMessageHelper.GetMessageUTF8(player.Stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
