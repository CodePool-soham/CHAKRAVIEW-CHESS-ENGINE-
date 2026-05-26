using ChakraView.Game;
using ChakraView.Rules;

namespace ChakraView.AI
{
    public class ChessAI
    {
        private readonly
            Evaluator evaluator;

        private readonly
            MoveValidator
            moveValidator;

        public ChessAI()
        {
            evaluator =
                new Evaluator();

            moveValidator =
                new MoveValidator();
        }

        public List<Move> GetAllLegalMoves(
        Board board,
        PieceColor playerColor)
        {
            List<Move> allMoves =
                new List<Move>();

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

                    allMoves.AddRange(
                        validMoves);
                }
            }

            return allMoves;
        }
        private Board SimulateMove(
        Board board,
        Move move)
        {
            Board tempBoard =
                board.Clone();

            Piece movingPiece =
                tempBoard.Squares[
                    move.StartRow,
                    move.StartCol];

            tempBoard.Squares[
                move.EndRow,
                move.EndCol] =
                movingPiece;

            tempBoard.Squares[
                move.StartRow,
                move.StartCol] =
                new Piece(
                    PieceType.None,
                    PieceColor.None);

            return tempBoard;
        }
        private int Minimax(
        Board board,
        int depth,
        bool maximizingPlayer)
        {
            // Base case
            if (depth == 0)
            {
                return evaluator
                    .EvaluateBoard(
                        board);
            }

            PieceColor currentPlayer =
                maximizingPlayer
                ? PieceColor.White
                : PieceColor.Black;

            List<Move> legalMoves =
                GetAllLegalMoves(
                    board,
                    currentPlayer);

            // No moves
            if (legalMoves.Count == 0)
            {
                return evaluator
                    .EvaluateBoard(
                        board);
            }

            if (maximizingPlayer)
            {
                int bestScore =
                    int.MinValue;

                foreach (Move move
                         in legalMoves)
                {
                    Board tempBoard =
                        SimulateMove(
                            board,
                            move);

                    int score =
                        Minimax(
                            tempBoard,
                            depth - 1,
                            false);

                    bestScore =
                        Math.Max(
                            bestScore,
                            score);
                }

                return bestScore;
            }
            else
            {
                int bestScore =
                    int.MaxValue;

                foreach (Move move
                         in legalMoves)
                {
                    Board tempBoard =
                        SimulateMove(
                            board,
                            move);

                    int score =
                        Minimax(
                            tempBoard,
                            depth - 1,
                            true);

                    bestScore =
                        Math.Min(
                            bestScore,
                            score);
                }

                return bestScore;
            }
        }

        public Move GetBestMove(
        Board board,
        PieceColor aiColor)
        {
            List<Move> legalMoves =
                GetAllLegalMoves(
                    board,
                    aiColor);

            Move bestMove =
                legalMoves[0];

            int bestScore =
                aiColor ==
                PieceColor.White
                ? int.MinValue
                : int.MaxValue;

            foreach (Move move
                     in legalMoves)
            {
                Board tempBoard =
                    SimulateMove(
                        board,
                        move);

                int score =
                    Minimax(
                        tempBoard,
                        2,
                        aiColor ==
                        PieceColor.Black);

                bool betterMove =
                    aiColor ==
                    PieceColor.White
                    ? score > bestScore
                    : score < bestScore;

                if (betterMove)
                {
                    bestScore =
                        score;

                    bestMove =
                        move;
                }
            }

            return bestMove;
        }
    }
}