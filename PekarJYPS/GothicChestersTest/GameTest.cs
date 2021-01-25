using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GothicChesters;

namespace GothicChestersTest
{
    [TestClass]
    public class GameTest
    {
        private Game game;

        [TestInitialize]
        public void Initialize()
        {
            game = new Game(1, Players.Human, Players.AI);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //NONE
        }

        [TestMethod]
        public void Game()
        {
            //Difficulty mimo 1,2,3
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Game(0, Players.AI, Players.Human));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Game(4, Players.AI, Players.Human));

            Assert.AreNotEqual(game.WhitePlayer, game.BlackPlayer);
            Assert.AreNotEqual(game.PlayerOnMove, game.EnemyPlayer);
        }
    }
}
