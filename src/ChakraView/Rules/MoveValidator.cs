using ChakraView.Game;

namespace ChakraView.Rules
{
    public class MoveValidator
    {
        private readonly CheckDetector
            checkDetector;

        public MoveValidator()
        {
            checkDetector =
                new CheckDetector();
        }

        public bool IsMoveLegal(
            Board board,
            Move move,
            PieceColor playerColor)
        {
            MoveGenerator generator =
                new MoveGenerator();

            List<Move> legalMoves =
                generator.GetLegalMoves(
                    board,
                    move.StartRow,
                    move.StartCol);

            bool moveExists =
                legalMoves.Any(m =>
                    m.EndRow ==
                    move.EndRow &&
                    m.EndCol ==
                    move.EndCol);

            if (!moveExists)
            {
                return false;
            }

            // Clone board
            Board tempBoard =
                board.Clone();

            // Simulate move
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

            // King safety
            return !checkDetector
                .IsKingInCheck(
                    tempBoard,
                    playerColor);
        }

        public List<Move> GetValidMoves(
    Board board,
    int row,
    int col)
        {
            List<Move> validMoves =
                new List<Move>();

            MoveGenerator generator =
                new MoveGenerator();

            Piece piece =
                board.Squares[row, col];

            List<Move> possibleMoves =
                generator.GetLegalMoves(
                    board,
                    row,
                    col);

            foreach (Move move
                     in possibleMoves)
            {
                bool isLegal =
                    IsMoveLegal(
                        board,
                        move,
                        piece.Color);

                if (isLegal)
                {
                    validMoves.Add(move);
                }
            }

            return validMoves;
        }
    }
}