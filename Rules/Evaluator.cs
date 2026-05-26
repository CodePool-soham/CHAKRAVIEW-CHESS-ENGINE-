using ChakraView.Game;

namespace ChakraView.Rules
{
    public class Evaluator
    {
        public int EvaluateBoard(
            Board board)
        {
            int score = 0;

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

                    int pieceValue =
                        GetPieceValue(
                            piece.Type);

                    int positionBonus =
                        GetPositionBonus(
                            row,
                            col);

                    int totalValue =
                        pieceValue +
                        positionBonus;

                    if (piece.Color ==
                        PieceColor.White)
                    {
                        score += totalValue;
                    }
                    else if
                    (piece.Color ==
                     PieceColor.Black)
                    {
                        score -= totalValue;
                    }
                }
            }

            return score;
        }

        private int GetPieceValue(
            PieceType type)
        {
            return type switch
            {
                PieceType.Pawn => 1,
                PieceType.Knight => 3,
                PieceType.Bishop => 3,
                PieceType.Rook => 5,
                PieceType.Queen => 9,
                PieceType.King => 1000,

                _ => 0
            };
        }

        private int GetPositionBonus(
    int row,
    int col)
        {
            bool centerSquare =
                (row >= 3 && row <= 4) &&
                (col >= 3 && col <= 4);

            bool nearCenter =
                (row >= 2 && row <= 5) &&
                (col >= 2 && col <= 5);

            if (centerSquare)
            {
                return 2;
            }

            if (nearCenter)
            {
                return 1;
            }

            return 0;
        }
    }
}