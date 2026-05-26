using ChakraView.Game;

namespace ChakraView.Rules
{
    public class CheckmateDetector
    {
        private MoveValidator
            moveValidator;

        public CheckmateDetector()
        {
            moveValidator =
                new MoveValidator();
        }

        public bool IsCheckmate(
            Board board,
            PieceColor playerColor)
        {
            CheckDetector
                checkDetector =
                new CheckDetector();

            // Must be in check
            if (!checkDetector
                .IsKingInCheck(
                    board,
                    playerColor))
            {
                return false;
            }

            // Any legal move?
            for (int row = 0;
                 row < 8;
                 row++)
            {
                for (int col = 0;
                     col < 8;
                     col++)
                {
                    Piece piece =
                        board.Squares[
                            row, col];

                    if (piece.Color !=
                        playerColor)
                    {
                        continue;
                    }

                    List<Move>
                        validMoves =
                        moveValidator
                        .GetValidMoves(
                            board,
                            row,
                            col);

                    if (validMoves
                        .Count > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}