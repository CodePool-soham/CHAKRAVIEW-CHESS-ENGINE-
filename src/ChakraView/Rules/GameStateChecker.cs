using ChakraView.Game;

namespace ChakraView.Rules
{
    public class GameStateChecker
    {
        private readonly CheckDetector
            checkDetector;

        private readonly MoveValidator
            moveValidator;

        public GameStateChecker()
        {
            checkDetector =
                new CheckDetector();

            moveValidator =
                new MoveValidator();
        }

        public bool IsCheckmate(
            Board board,
            PieceColor playerColor)
        {
            // Must be in check first
            bool inCheck =
                checkDetector
                .IsKingInCheck(
                    board,
                    playerColor);

            if (!inCheck)
            {
                return false;
            }

            // Search all pieces
            for (int row = 0;
                 row < 8;
                 row++)
            {
                for (int col = 0;
                     col < 8;
                     col++)
                {
                    Piece piece =
                        board.Squares[row,
                                      col];

                    // Skip empty squares
                    if (piece.Type ==
                        PieceType.None)
                    {
                        continue;
                    }

                    // Skip enemy pieces
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

                    // Found escape move
                    if (validMoves.Count
                        > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsStalemate(
    Board board,
    PieceColor playerColor)
        {
            // Cannot be in check
            bool inCheck =
                checkDetector
                .IsKingInCheck(
                    board,
                    playerColor);

            if (inCheck)
            {
                return false;
            }

            // Search all player pieces
            for (int row = 0;
                 row < 8;
                 row++)
            {
                for (int col = 0;
                     col < 8;
                     col++)
                {
                    Piece piece =
                        board.Squares[row,
                                      col];

                    // Skip empty squares
                    if (piece.Type ==
                        PieceType.None)
                    {
                        continue;
                    }

                    // Skip enemy pieces
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

                    // Found legal move
                    if (validMoves.Count
                        > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}