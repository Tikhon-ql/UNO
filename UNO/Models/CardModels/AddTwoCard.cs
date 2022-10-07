using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UNO.Enum;
using UNO.Helper;

namespace UNO.Models.CardModels
{
    [DataContract]
    public class AddTwoCard : Card
    {
        public AddTwoCard(Value value, List<Color> colors) : base(value, colors)
        {
        }
        public override void MakeAction(Game game)
        {
            int nextPlayerIndex = game.CurrentPlayerIndex == game.PlayersCount - 1 ? 0 : game.CurrentPlayerIndex + game.MooveDirection;
            var nextPlayer = game.Players.ElementAt(nextPlayerIndex);

            var cards = new List<Card>();

            cards.Add(game.GiveRandomCardToPlayer(nextPlayer));
            cards.Add(game.GiveRandomCardToPlayer(nextPlayer));
            nextPlayer.TookCardsCount += 2;
            //TransmitMessageHelper.SendMessageUTF8(nextPlayer.Stream, "AddCards");

            //SendCardsToPlayer(nextPlayer, cards);

            //TransmitMessageHelper.GetMessageUTF8(nextPlayer.Stream);
        }
    }
}
