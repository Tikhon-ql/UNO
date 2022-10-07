using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using UNO.Enum;
using UNO.Helper;
using UNO.Models.CardModels;

namespace UNO.Models
{
    [DataContract]
    [KnownType(typeof(NumericCard))]
    [KnownType(typeof(AddTwoCard))]
    [KnownType(typeof(ChooseColorCard))]
    [KnownType(typeof(ChooseColorAndAddFourCard))]
    [KnownType(typeof(ReverseCard))]
    [KnownType(typeof(SkipCard))]
    public abstract class Card
    {
        [DataMember]
        public readonly Guid Id;
        [DataMember]
        protected List<Color> _colors = new List<Color>();

        public Card()
        {
            Id = Guid.NewGuid();
        }

        public Card(Value value, List<Color> colors)
        {
            Id = Guid.NewGuid();
            Value = value;
            _colors = colors;
        }
        public void SetColor(Color color)
        {
            _colors[0] = color;
        }

        public IEnumerable<Color> Colors => _colors;
        [DataMember]
        public Value Value { get; private set; }
        public override string ToString()
        {
            return $"{Value}: {Colors.ElementAt(0)}";
        }

        public abstract void MakeAction(Game game);


        public static void SendCardsToPlayer(Player player, IEnumerable<Card> cards)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<Card>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, cards);
                TransmitMessageHelper.SendMessageUTF8(player.Stream, Encoding.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}