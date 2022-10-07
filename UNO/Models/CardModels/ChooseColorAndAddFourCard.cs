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
    public class ChooseColorAndAddFourCard : Card
    {
        public ChooseColorAndAddFourCard(Value value, List<Color> colors) : base(value, colors)
        {
        }
        public override void MakeAction(Game game)
        {
            int nextPlayerIndex = game.CurrentPlayerIndex == game.PlayersCount - 1 ? 0 : game.CurrentPlayerIndex + game.MooveDirection;
            var nextPlayer = game.Players.ElementAt(nextPlayerIndex);

            var cards = new List<Card>();


            cards.Add(game.GiveRandomCardToPlayer(nextPlayer));
            cards.Add(game.GiveRandomCardToPlayer(nextPlayer));
            cards.Add(game.GiveRandomCardToPlayer(nextPlayer));
            cards.Add(game.GiveRandomCardToPlayer(nextPlayer));

            nextPlayer.TookCardsCount += 4;

            //TransmitMessageHelper.SendMessageUTF8(nextPlayer.Stream, "AddCards");
            //SendCardsToPlayer(nextPlayer, cards);

            var player = game.CurrentPlayer;
            var color = (Color)int.Parse(TransmitMessageHelper.GetMessageUnicode(player.Stream));
            game.TopCard.SetColor(color);
        }
    }
}
