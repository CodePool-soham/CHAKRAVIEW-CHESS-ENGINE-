namespace ChakraView.Game
{
    public class Board
    {
        public Piece[,] Squares { get; private set; }

        public Board()
        {
            Squares = new Piece[8, 8];
            InitializeBoard();
        }

        public Board Clone()
        {
            Board clonedBoard = new Board();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece originalPiece =
                        Squares[row, col];

                    clonedBoard.Squares[row, col] =
                        new Piece(
                            originalPiece.Type,
                            originalPiece.Color);
                }
            }

            return clonedBoard;
        }

        private void InitializeBoard()
        {
            // Fill board with empty pieces
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Squares[row, col] =
                        new Piece(PieceType.None, PieceColor.None);
                }
            }

            // Black pawns
            for (int col = 0; col < 8; col++)
            {
                Squares[1, col] =
                    new Piece(PieceType.Pawn, PieceColor.Black);
            }

            // White pawns
            for (int col = 0; col < 8; col++)
            {
                Squares[6, col] =
                    new Piece(PieceType.Pawn, PieceColor.White);
            }
            // Black rooks
            Squares[0, 0] =
                new Piece(PieceType.Rook, PieceColor.Black);

            Squares[0, 7] =
                new Piece(PieceType.Rook, PieceColor.Black);

            // White rooks
            Squares[7, 0] =
                new Piece(PieceType.Rook, PieceColor.White);

            Squares[7, 7] =
                new Piece(PieceType.Rook, PieceColor.White);

            // Black knights
            Squares[0, 1] =
                new Piece(PieceType.Knight, PieceColor.Black);

            Squares[0, 6] =
                new Piece(PieceType.Knight, PieceColor.Black);

            // White knights
            Squares[7, 1] =
                new Piece(PieceType.Knight, PieceColor.White);

            Squares[7, 6] =
                new Piece(PieceType.Knight, PieceColor.White);

            // Black bishops
            Squares[0, 2] =
                new Piece(PieceType.Bishop, PieceColor.Black);

            Squares[0, 5] =
                new Piece(PieceType.Bishop, PieceColor.Black);

            // White bishops
            Squares[7, 2] =
                new Piece(PieceType.Bishop, PieceColor.White);

            Squares[7, 5] =
                new Piece(PieceType.Bishop, PieceColor.White);

            // Black queen
            Squares[0, 3] =
                new Piece(PieceType.Queen, PieceColor.Black);

            // Black king
            Squares[0, 4] =
                new Piece(PieceType.King, PieceColor.Black);

            // White queen
            Squares[7, 3] =
                new Piece(PieceType.Queen, PieceColor.White);

            // White king
            Squares[7, 4] =
                new Piece(PieceType.King, PieceColor.White);
        }

        public void PrintBoard()
        {
            Console.WriteLine();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = Squares[row, col];

                    string symbol = piece.Type switch
                    {
                        PieceType.Pawn =>
                            piece.Color == PieceColor.White ? "P" : "p",

                        PieceType.Rook =>
                            piece.Color == PieceColor.White ? "R" : "r",

                        PieceType.Knight =>
                            piece.Color == PieceColor.White ? "N" : "n",

                        PieceType.Bishop =>
                            piece.Color == PieceColor.White ? "B" : "b",

                        PieceType.Queen =>
                            piece.Color == PieceColor.White ? "Q" : "q",

                        PieceType.King =>
                            piece.Color == PieceColor.White ? "K" : "k",

                        _ => "."
                    };

                    Console.Write(symbol + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}