namespace ChakraView.Game
{
    public class ChessTimer
    {
        public int WhiteTimeSeconds
        {
            get;
            private set;
        }

        public int BlackTimeSeconds
        {
            get;
            private set;
        }

        public ChessTimer()
        {
            // 10 minutes each
            WhiteTimeSeconds = 600;
            BlackTimeSeconds = 600;
        }

        public void Tick(
            PieceColor currentPlayer)
        {
            if (currentPlayer ==
                PieceColor.White)
            {
                if (WhiteTimeSeconds > 0)
                {
                    WhiteTimeSeconds--;
                }
            }
            else
            {
                if (BlackTimeSeconds > 0)
                {
                    BlackTimeSeconds--;
                }
            }
        }

        public bool IsTimeOver(
            PieceColor player)
        {
            return player ==
                PieceColor.White
                ? WhiteTimeSeconds <= 0
                : BlackTimeSeconds <= 0;
        }

        public void PrintTime()
        {
            Console.WriteLine(
                $"White: " +
                $"{WhiteTimeSeconds}s");

            Console.WriteLine(
                $"Black: " +
                $"{BlackTimeSeconds}s");
        }
    }
}