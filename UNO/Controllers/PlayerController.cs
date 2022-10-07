using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UNO.Helper;
using UNO.Interfaces;
using UNO.Models;

namespace UNO.Controllers
{
    public class PlayerController : IPlayerController
    {
        private IGameController _gameController;
        private Socket _socket;
        public PlayerController(IGameController gameController, Socket sokcet)
        {
            gameController = gameController;
            _socket = sokcet;
        }

        public Card MakeMove(Player player)
        {
            TransmitMessageHelper.SendMessage(player.Socket, "Your move...");
            string move = TransmitMessageHelper.GetMessage(_socket);
            if (int.TryParse(move, out int index))
                return player.Cards.ElementAt(index);
            throw new Exception("Received message was incorrect!!!");
        }
    }
}
