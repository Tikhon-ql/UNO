
using UNO.Controllers;
using UNO.Models;
using UNO.Services;

class Program
{
    //При добавлении 2-х карт приходит сразу и Your move и карта, положенная предыдущим игроком.
    static void Main()
    {
        var checker = new CardCorrectChecker();

        var game = new Game();
        var gameController = new GameController("127.0.0.1",8080, game, checker);
        gameController.Run();
    }
}