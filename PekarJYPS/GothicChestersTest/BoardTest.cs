using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GothicChesters;

namespace GothicChestersTest
{
    [TestClass]
    public class BoardTest
    {
        private Board board;

        [TestInitialize]
        public void Initialize()
        {
            board = new Board();
        }

        [TestCleanup]
        public void Cleanup()
        {
            //NONE
        }

        [TestMethod]
        public void Clone()
        {
            Board clonedBoard = (Board)board.Clone();
            Assert.AreEqual(clonedBoard.WhiteDead, ((Board)board.Clone()).WhiteDead);
            Assert.AreEqual(clonedBoard.BlackDead, ((Board)board.Clone()).BlackDead);
            Assert.AreNotSame(clonedBoard, board.Clone());
            Assert.AreNotSame(clonedBoard.Boxes, ((Board)board.Clone()).Boxes);
        }
    }
}
