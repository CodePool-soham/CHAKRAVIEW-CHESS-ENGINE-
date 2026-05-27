using ChakraView.Game;

namespace ChakraView.Rules
{
    public class StalemateDetector
    {
        private MoveValidator
            moveValidator;

        public StalemateDetector()
        {
            moveValidator =
                new MoveValidator();
        }

        public bool IsStalemate(
            Board board,
            PieceColor playerColor)
        {
            CheckDetector
                checkDetector =
                new CheckDetector();

            // Cannot be in check
            if (checkDetector
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