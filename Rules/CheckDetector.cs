using ChakraView.Game;

namespace ChakraView.Rules
{
    public class CheckDetector
    {
        private readonly MoveGenerator
            moveGenerator;

        public CheckDetector()
        {
            moveGenerator =
                new MoveGenerator();
        }

        public bool IsSquareUnderAttack(
            Board board,
            int targetRow,
            int targetCol,
            PieceColor defenderColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece =
                        board.Squares[row, col];

                    // Ignore empty squares
                    if (piece.Type ==
                        PieceType.None)
                    {
                        continue;
                    }

                    // Ignore friendly pieces
                    if (piece.Color ==
                        defenderColor)
                    {
                        continue;
                    }

                    List<Move> enemyMoves =
                        moveGenerator
                        .GetLegalMoves(
                            board,
                            row,
                            col);

                    foreach (Move move
                             in enemyMoves)
                    {
                        if (move.EndRow
                            == targetRow &&
                            move.EndCol
                            == targetCol)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsKingInCheck(
           Board board,
           PieceColor kingColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece =
                        board.Squares[row, col];

                    bool isKing =
                        piece.Type ==
                        PieceType.King;

                    bool correctColor =
                        piece.Color ==
                        kingColor;

                    if (isKing &&
                        correctColor)
                    {
                        return IsSquareUnderAttack(
                            board,
                            row,
                            col,
                            kingColor);
                    }
                }
            }

            return false;
        }
    }
}