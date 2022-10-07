using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using UNO.Helper;

namespace UNO.Models
{
    public class Game
    {
        public static Random rnd = new Random(DateTime.Now.Millisecond);

        public readonly Guid Id;
        private List<Player> _players = new List<Player>();
        private List<Card> _deck = new List<Card>(108);
        //private string _ip;
        //private int _port;

        public Game()
        {
            Id = Guid.NewGuid();
        }

        public Game(List<Player> players, List<Card> deck)
        {
            Id = Guid.NewGuid();
            _players = players;
            _deck = deck;
        }

        public Card this[int index]
        {
            get
            {
                return _deck[index];
            }
        }

        public IEnumerable<Player> Players => _players;
        public IEnumerable<Card> Deck => _deck;
        public int DeckSize => _deck.Count;
        public int PlayersCount => _players.Count;

        public int CurrentPlayerIndex = 0;
        public Player CurrentPlayer => _players[CurrentPlayerIndex];

        public int MooveDirection { get; set; } = 1;
        //public string Ip => _ip;
        //public int Port => _port;
        public Card TopCard { get; set; }

        public bool AddPlayer(Player player)
        {
            try
            {
                if (player == null)
                    throw new ArgumentNullException(nameof(player));
                _players.Add(player);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool RemovePlayer(Guid id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                    throw new ArgumentNullException(nameof(id), "Id cannot be empty or null!!!");
                var player = _players.FirstOrDefault(x => x.Id == id);
                if (player == null)
                    throw new ArgumentException($"Player with id: {id} doesn't exist");
                _players.Remove(player);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool AddCard(Card card)
        {
            try
            {
                if (card == null)
                    throw new ArgumentNullException(nameof(card));
                _deck.Add(card);
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
                var card = _deck.FirstOrDefault(x => x.Id == id);
                if (card == null)
                    throw new ArgumentException($"Card with id: {id} doesn't exist");
                _deck.Remove(card);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public Card GiveRandomCardToPlayer(Player player)
        {
            int index = rnd.Next(0, _deck.Count() - 1);
            var card = _deck[index];
            player.AddCard(card);
            RemoveCard(card.Id);
            return card;
        }

        public void IncreaseIndex()
        {
            if (MooveDirection == 1)
                CurrentPlayerIndex = CurrentPlayerIndex == PlayersCount - 1 ? 0 : CurrentPlayerIndex + MooveDirection;
            else if (MooveDirection == -1)
                CurrentPlayerIndex = CurrentPlayerIndex == 0 ? PlayersCount - 1 : CurrentPlayerIndex + MooveDirection;
        }

        public void SendDeckToPlayer(Player player)
        {
            Card.SendCardsToPlayer(player, player.Cards);
        }

        public void SetTopCard(Card card, Player player)
        {
         
        }
    }
}
