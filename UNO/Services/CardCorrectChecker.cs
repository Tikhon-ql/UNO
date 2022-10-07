using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Interfaces;
using UNO.Models;

namespace UNO.Services
{
    public class CardCorrectChecker : ICardCorrectChecker
    {
        public bool CheckCard(Card card, Game game)
        {
            if (card.Value == Enum.Value.None)
                return false;
            if (game.TopCard == null)
                return true;
            if (card.Value == Enum.Value.Plus2 || card.Value == Enum.Value.ChooseColorPlus4)
            {
                int nextPlayerIndex = 0;
                if (game.MooveDirection == 1)
                    nextPlayerIndex = game.CurrentPlayerIndex == game.PlayersCount - 1 ? 0 : game.CurrentPlayerIndex + game.MooveDirection;
                else if (game.MooveDirection == -1)
                    nextPlayerIndex = game.CurrentPlayerIndex == 0 ? game.PlayersCount - 1 : game.CurrentPlayerIndex + game.MooveDirection;
                game.Players.ElementAt(nextPlayerIndex).TookCardsCount += game.CurrentPlayer.TookCardsCount;
                game.CurrentPlayer.TookCardsCount = 0;
            }
            if (card.Value == Enum.Value.ChooseColor || card.Value == Enum.Value.ChooseColorPlus4)
                return true;
            if (game.TopCard.Value == card.Value || game.TopCard.Colors.ElementAt(0) == card.Colors.ElementAt(0))
                return true;
            return false;
        }
    }
}
