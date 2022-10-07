using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Models;

namespace UNO.Interfaces
{
    public interface ICardCorrectChecker
    {
        /// <summary>
        /// Check is the card correct (has the same color or value)
        /// </summary>
        /// <param name="card">Card to drop</param>
        /// <param name="game">Current game</param>
        /// <returns>Card is correct or not</returns>
        bool CheckCard(Card card, Game game);
    }
}
