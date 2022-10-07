using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Models;

namespace UNO.Interfaces
{
    public interface IPlayerController : IController<Player>
    {
        Card MakeMove(Player player);
    }
}
