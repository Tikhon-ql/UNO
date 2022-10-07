using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UNO.Enum;

namespace UNO.Models.CardModels
{
    [DataContract]
    public class ReverseCard : Card
    {
        public ReverseCard(Value value, List<Color> colors) : base(value, colors)
        {
        }
        public override void MakeAction(Game game)
        {
            //game.IncreaseIndex();
            game.MooveDirection *= -1;
        }
    }
}
