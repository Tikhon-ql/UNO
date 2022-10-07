using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UNO.Controllers;
using UNO.Helper;
using UNO.Interfaces;
using UNO.Models.CardModels;

namespace UNO.Models
{
    public class Player
    {
        public readonly Guid Id;

        private IGameController gameController;

        private List<Card> _cards = new List<Card>();

        public Player()
        {
            Id = Guid.NewGuid();
        }
        public Player(string nickname, IGameController gameController, Socket socket)
        {
            Id = Guid.NewGuid();
            Nickname = nickname;
            this.gameController = gameController;
            //Socket = socket;
            Stream = new NetworkStream(socket);
        }
        public Player(string nickname, List<Card> cards)
        {
            Id = Guid.NewGuid();
            Nickname = nickname;
            _cards = cards;
        }

        public string Nickname { get; private set; }
        public IEnumerable<Card> Cards => _cards;
        public Socket Socket { get; set; }
        public int CardsCount => _cards.Count;
        public NetworkStream Stream { get; }
        //public Game Game { get; }

        public int TookCardsCount { get; set; } = 0;


        public bool AddCard(Card card)
        {
            try
            {
                if (card == null)
                    throw new ArgumentNullException(nameof(card));
                _cards.Add(card);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool RemoveCard(Guid id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                    throw new ArgumentNullException(nameof(id), "Id cannot be empty or null!!!");
                var card = _cards.FirstOrDefault(x => x.Id == id);
                if (card == null)
                    throw new ArgumentException($"Card with id: {id} doesn't exist");
                _cards.Remove(card);
                return true;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        //TODO: Make moove class
        public Card MakeMove()
        {
            TransmitMessageHelper.SendMessageUTF8(Stream, "Your move...");
            string move = TransmitMessageHelper.GetMessageUnicode(Stream);
            if (move == "-1")
            {
                gameController.SendRandomCardsToPlayer(this, 1);
                return new NumericCard(Enum.Value.None, new List<Enum.Color>() { Enum.Color.Red });
            }
            if (int.TryParse(move, out int index))
                return Cards.ElementAt(index);
            throw new Exception("Received message was incorrect!!!");
        }
    }
}
