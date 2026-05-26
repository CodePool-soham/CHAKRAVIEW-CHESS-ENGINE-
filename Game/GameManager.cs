using ChakraView.AI;
using ChakraView.Rules;

namespace ChakraView.Game
{
    public class GameManager
    {
        private Board board;

        private ChessAI ai;

        private ChessTimer timer;

        private GameStateChecker
            stateChecker;

        private PieceColor
            currentPlayer;

        public GameManager()
        {
            board =
                new Board();

            ai =
                new ChessAI();

            timer =
                new ChessTimer();

            stateChecker =
                new GameStateChecker();

            currentPlayer =
                PieceColor.White;
        }

        public void StartGame()
        {
            while (true)
            {
                Console.Clear();

                board.PrintBoard();

                Console.WriteLine(
                    $"\nTurn: " +
                    $"{currentPlayer}");

                timer.PrintTime();

                timer.Tick(
                    currentPlayer);

                Thread.Sleep(1000);

                // Switch turn
                currentPlayer =
                    currentPlayer ==
                    PieceColor.White
                    ? PieceColor.Black
                    : PieceColor.White;
            }
        }
    }
}