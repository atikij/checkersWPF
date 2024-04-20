using NUnit.Framework;
using CheckersCore.Core;
using CheckersCore.Core.Player;
using System.Collections.Generic;

namespace TestProject1
{
    [TestFixture]
    public class tests
    {
        // Тест на проверку получения всех черных шашек
        [Test]
        public void GetCheckersByColor_ReturnsAllBlackCheckers()
        {
            Board board = new Board();

            List<Checker> blackCheckers = board.GetCheckersByColor(Color.Black);

            foreach (var checker in blackCheckers)
            {
                Assert.AreEqual(Color.Black, checker.Color);
            }
        }

        // Тест на проверку получения всех белых шашек
        [Test]
        public void GetCheckersByColor_ReturnsAllWhiteCheckers()
        {
            Board board = new Board();

            List<Checker> whiteCheckers = board.GetCheckersByColor(Color.White);

            foreach (var checker in whiteCheckers)
            {
                Assert.AreEqual(Color.White, checker.Color);
            }
        }
        
        // Тест на проверку индексатора, который должен возвращать правильную шашку
        [Test]
        public void Indexer_ReturnsCorrectChecker()
        {
            Board board = new Board();
            Checker expectedChecker = new Checker(Color.Black);
    
            board[0, 0] = expectedChecker;
            Checker actualChecker = board[0, 0];
    
            Assert.AreEqual(expectedChecker, actualChecker);
        }

        // Тест на проверку индексатора, который должен удалять шашку, если передано значение null.
        [Test]
        public void Indexer_SetNullChecker_RemovesChecker()
        {
            Board board = new Board();
            Checker checker = new Checker(Color.Black);
    
            board[0, 0] = checker;
            board[0, 0] = null;
            Checker actualChecker = board[0, 0];
    
            Assert.IsNull(actualChecker);
        }
    }
}