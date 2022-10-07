using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Models;

namespace UNO.Interfaces
{
    public interface IGameController : IController<Game>
    {
        //void WaitForPlayers();
        //void StartGame();
        void Run();
        bool GiveRandomCardToPlayer(Player player);
        void FullFillDeck();
        void BroadcastMove(string senderNickname, Card card);
        void SendRandomCardsToPlayer(Player player, int cardCount);
        //void WaitForDisconnect();
    }
}
