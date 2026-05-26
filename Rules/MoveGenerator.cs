using ChakraView.Game;

namespace ChakraView.Rules
{
    public class MoveGenerator
    {
        public List<Move> GetLegalMoves(
        Board board,
        int row,
        int col)
        {
            Piece piece = board.Squares[row, col];

            return piece.Type switch
            {
                PieceType.Pawn =>
                    GetPawnMoves(board, row, col),

                PieceType.Knight =>
                    GetKnightMoves(board, row, col),

                PieceType.Rook =>
                    GetRookMoves(board, row, col),

                PieceType.Bishop =>
                    GetBishopMoves(board, row, col),

                PieceType.Queen =>
                    GetQueenMoves(board, row, col),

                PieceType.King =>
                    GetKingMoves(board, row, col),


                _ => new List<Move>()
            };
        }
        public List<Move> GetPawnMoves(Board board, int row, int col)
        {
            List<Move> moves = new List<Move>();

            Piece piece = board.Squares[row, col];

            // Make sure it's a pawn
            if (piece.Type != PieceType.Pawn)
                return moves;

            int direction =
                piece.Color == PieceColor.White ? -1 : 1;

            int nextRow = row + direction;

            // One-square move
            if (IsInsideBoard(nextRow, col) &&
                board.Squares[nextRow, col].Type == PieceType.None)
            {
                moves.Add(new Move(row, col, nextRow, col));

                // Two-square first move
                bool startingPosition =
                    (piece.Color == PieceColor.White && row == 6) ||
                    (piece.Color == PieceColor.Black && row == 1);

                int doubleMoveRow = row + (2 * direction);

                if (startingPosition &&
                    IsInsideBoard(doubleMoveRow, col) &&
                    board.Squares[doubleMoveRow, col].Type
                        == PieceType.None)
                {
                    moves.Add(
                        new Move(row, col,
                                 doubleMoveRow, col));
                }

             
            }
            // Diagonal captures
            int[] captureCols = { col - 1, col + 1 };

            foreach (int captureCol in captureCols)
            {
                if (IsInsideBoard(nextRow, captureCol))
                {
                    Piece targetPiece =
                        board.Squares[nextRow, captureCol];

                    if (targetPiece.Type != PieceType.None &&
                        targetPiece.Color != piece.Color)
                    {
                        moves.Add(
                            new Move(row, col,
                                     nextRow, captureCol));
                    }
                }
            }
            return moves;
        }

        public List<Move> GetKnightMoves(
        Board board,
        int row,
        int col)
        {
            List<Move> moves = new List<Move>();

            Piece piece = board.Squares[row, col];

            int[,] knightOffsets =
            {
            { -2, -1 },
            { -2,  1 },
            { -1, -2 },
            { -1,  2 },
            {  1, -2 },
            {  1,  2 },
            {  2, -1 },
            {  2,  1 }
        };

            for (int i = 0; i < 8; i++)
            {
                int newRow =
                    row + knightOffsets[i, 0];

                int newCol =
                    col + knightOffsets[i, 1];

                if (!IsInsideBoard(newRow, newCol))
                    continue;

                Piece targetPiece =
                    board.Squares[newRow, newCol];

                // Empty square or enemy piece
                if (targetPiece.Type == PieceType.None ||
                    targetPiece.Color != piece.Color)
                {
                    moves.Add(
                        new Move(row, col,
                                 newRow, newCol));
                }
            }

            return moves;
        }

        public List<Move> GetRookMoves(
    Board board,
    int row,
    int col)
        {
            List<Move> moves = new List<Move>();

            Piece piece = board.Squares[row, col];

            int[,] directions =
            {
                { -1, 0 }, // up
                { 1, 0 },  // down
                { 0, -1 }, // left
                { 0, 1 }   // right
    };

            for (int d = 0; d < 4; d++)
            {
                int rowDirection =
                    directions[d, 0];

                int colDirection =
                    directions[d, 1];

                int currentRow =
                    row + rowDirection;

                int currentCol =
                    col + colDirection;

                while (IsInsideBoard(currentRow,
                                     currentCol))
                {
                    Piece targetPiece =
                        board.Squares[currentRow,
                                      currentCol];

                    // Empty square
                    if (targetPiece.Type ==
                        PieceType.None)
                    {
                        moves.Add(
                            new Move(row, col,
                                     currentRow,
                                     currentCol));
                    }
                    else
                    {
                        // Enemy piece capture
                        if (targetPiece.Color !=
                            piece.Color)
                        {
                            moves.Add(
                                new Move(row, col,
                                         currentRow,
                                         currentCol));
                        }

                        // Stop moving in this direction
                        break;
                    }

                    currentRow += rowDirection;
                    currentCol += colDirection;
                }
            }

            return moves;
        }



        public List<Move> GetBishopMoves(
        Board board,
        int row,
        int col)
        {
            List<Move> moves = new List<Move>();

            Piece piece = board.Squares[row, col];

            int[,] directions =
            {
                { -1, -1 }, // up-left
                { -1,  1 }, // up-right
                {  1, -1 }, // down-left
                {  1,  1 }  // down-right
    };

            for (int d = 0; d < 4; d++)
            {
                int rowDirection =
                    directions[d, 0];

                int colDirection =
                    directions[d, 1];

                int currentRow =
                    row + rowDirection;

                int currentCol =
                    col + colDirection;

                while (IsInsideBoard(currentRow,
                                     currentCol))
                {
                    Piece targetPiece =
                        board.Squares[currentRow,
                                      currentCol];

                    // Empty square
                    if (targetPiece.Type ==
                        PieceType.None)
                    {
                        moves.Add(
                            new Move(row, col,
                                     currentRow,
                                     currentCol));
                    }
                    else
                    {
                        // Enemy capture
                        if (targetPiece.Color !=
                            piece.Color)
                        {
                            moves.Add(
                                new Move(row, col,
                                         currentRow,
                                         currentCol));
                        }

                        break;
                    }

                    currentRow += rowDirection;
                    currentCol += colDirection;
                }
            }

            return moves;
        }

        public List<Move> GetKingMoves(
        Board board,
        int row,
        int col)
        {
            List<Move> moves = new List<Move>();

            Piece piece = board.Squares[row, col];

            int[,] kingOffsets =
            {
                { -1, -1 },
                { -1,  0 },
                { -1,  1 },

                {  0, -1 },
                {  0,  1 },

                {  1, -1 },
                {  1,  0 },
                {  1,  1 }
    };

            for (int i = 0; i < 8; i++)
            {
                int newRow =
                    row + kingOffsets[i, 0];

                int newCol =
                    col + kingOffsets[i, 1];

                if (!IsInsideBoard(newRow, newCol))
                    continue;

                Piece targetPiece =
                    board.Squares[newRow, newCol];

                // Empty square or enemy piece
                if (targetPiece.Type == PieceType.None ||
                    targetPiece.Color != piece.Color)
                {
                    moves.Add(
                        new Move(row, col,
                                 newRow, newCol));
                }
            }

            return moves;
        }

        public List<Move> GetQueenMoves(
        Board board,
        int row,
        int col)
        {
            List<Move> moves = new List<Move>();

            moves.AddRange(
                GetRookMoves(board, row, col));

            moves.AddRange(
                GetBishopMoves(board, row, col));

            return moves;
        }

        private bool IsInsideBoard(int row, int col)
        {
            return row >= 0 &&
                   row < 8 &&
                   col >= 0 &&
                   col < 8;
        }
    }
}