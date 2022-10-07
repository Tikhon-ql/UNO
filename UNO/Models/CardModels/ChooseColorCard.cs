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
    public class ChooseColorCard : Card
    {
        public ChooseColorCard(Value value, List<Color> colors) : base(value, colors)
        {
        }
        public override void MakeAction(Game game)
        {
            var player = game.CurrentPlayer;
            //TransmitMessageHelper.SendMessageUnicode(player.Stream, "Choose color");
            var color = (Color)int.Parse(TransmitMessageHelper.GetMessageUnicode(player.Stream));
            game.TopCard.SetColor(color);
        }
    }
}
