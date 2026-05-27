using System.Linq;
using ChakraView.AI;
using ChakraView.Game;
using ChakraView.Rules;

namespace ChakraView.GUI
{
    public partial class Form1 : Form
    {
        private Button[,] squares;

        private Board board;

        private ChessAI ai;

        private Label
        checkLabel;

        private bool aiThinking = false;


        private MoveValidator
            moveValidator;

        private ChessTimer
            timer;

        private System.Windows.Forms.Timer
            gameTimer;

        private Label
            whiteTimerLabel;

        private Label
            turnLabel;

        private Label
            blackCapturedLabel;

        private Label
            whiteCapturedLabel;

        private Button
            restartButton;

        private Button
            withdrawButton;

        private List<Piece>
            capturedByWhite =
            new();

        private List<Piece>
            capturedByBlack =
            new();

        private CheckmateDetector
        checkmateDetector;

        private StalemateDetector
            stalemateDetector;

        private bool
            gameOver = false;

        private int
            selectedRow = -1;

        private int
            selectedCol = -1;

        public Form1()
        {
            InitializeComponent();

            board =
                new Board();

            ai =
                new ChessAI();

            moveValidator =
                new MoveValidator();

            timer =
                new ChessTimer();

            checkmateDetector =
            new CheckmateDetector();

            stalemateDetector =
                new StalemateDetector();

            gameTimer =
                new System.Windows.Forms.Timer();

            gameTimer.Interval =
                1000;

            gameTimer.Tick +=
                UpdateTimer;

            gameTimer.Start();

            CreateBoard();

            DrawBoard();

            CreateSidebar();

            ClientSize =
                new Size(
                    900,
                    640);

            Text =
                "ChakraView Chess";

            FormBorderStyle =
                FormBorderStyle.FixedSingle;

            MaximizeBox =
                false;
        }

        private void CreateBoard()
        {
            squares =
                new Button[8, 8];

            int size = 80;

            for (int row = 0;
                 row < 8;
                 row++)
            {
                for (int col = 0;
                     col < 8;
                     col++)
                {
                    Button square =
                        new Button();

                    square.Width =
                        size;

                    square.Height =
                        size;

                    square.Left =
                        col * size;

                    square.Top =
                        row * size;

                    square.Font =
                        new Font(
                            "Segoe UI Symbol",
                            28);

                    square.BackColor =
                        (row + col) % 2 == 0
                        ? Color.Beige
                        : Color.SaddleBrown;

                    int currentRow =
                        row;

                    int currentCol =
                        col;

                    square.Click +=
                        (sender, e) =>
                        SquareClicked(
                            currentRow,
                            currentCol);

                    Controls.Add(
                        square);

                    squares[
                        row, col] =
                        square;
                }
            }
        }

        private void CreateSidebar()
        {
            int panelX = 690;

            // Timer
            whiteTimerLabel =
                new Label();

            whiteTimerLabel.Text =
                "10:00";

            whiteTimerLabel.Font =
                new Font(
                    "Segoe UI",
                    28,
                    FontStyle.Bold);

            whiteTimerLabel.AutoSize =
                true;

            whiteTimerLabel.Left =
                panelX;

            whiteTimerLabel.Top =
                70;

            Controls.Add(
                whiteTimerLabel);

            // Turn label
            turnLabel =
                new Label();

            turnLabel.Text =
                "Your Move";

            turnLabel.Font =
                new Font(
                    "Segoe UI",
                    16);

            turnLabel.AutoSize =
                true;

            turnLabel.Left =
                panelX;

            turnLabel.Top =
                150;
            checkLabel =
    new Label();

            checkLabel.Text =
                "";

            checkLabel.Font =
                new Font(
                    "Segoe UI",
                    16,
                    FontStyle.Bold);

            checkLabel.ForeColor =
                Color.Red;

            checkLabel.AutoSize =
                true;

            checkLabel.Left =
                panelX;

            checkLabel.Top =
                185;

            Controls.Add(
                checkLabel);

            Controls.Add(
                turnLabel);

            // Captured by player
            blackCapturedLabel =
                new Label();

            blackCapturedLabel.Text =
                "Captured:\n";

            blackCapturedLabel.Font =
                new Font(
                    "Segoe UI Symbol",
                    16);

            blackCapturedLabel.AutoSize =
                true;

            blackCapturedLabel.Left =
                panelX;

            blackCapturedLabel.Top =
                220;

            Controls.Add(
                blackCapturedLabel);

            // Lost pieces
            whiteCapturedLabel =
                new Label();

            whiteCapturedLabel.Text =
                "Lost:\n";

            whiteCapturedLabel.Font =
                new Font(
                    "Segoe UI Symbol",
                    16);

            whiteCapturedLabel.AutoSize =
                true;

            whiteCapturedLabel.Left =
                panelX;

            whiteCapturedLabel.Top =
                330;

            Controls.Add(
                whiteCapturedLabel);

            // Restart button
            restartButton =
                new Button();

            restartButton.Text =
                "Restart";

            restartButton.Width =
                160;

            restartButton.Height =
                50;

            restartButton.Left =
                panelX;

            restartButton.Top =
                470;

            restartButton.Click +=
                RestartGame;

            Controls.Add(
                restartButton);

            // Withdraw button
            withdrawButton =
                new Button();

            withdrawButton.Text =
                "Withdraw";

            withdrawButton.Width =
                160;

            withdrawButton.Height =
                50;

            withdrawButton.Left =
                panelX;

            withdrawButton.Top =
                540;

            withdrawButton.Click +=
                WithdrawGame;

            Controls.Add(
                withdrawButton);
        }

        private void DrawBoard()
        {
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

                    squares[row, col]
                        .Text =
                        GetPieceSymbol(
                            piece);
                }
            }
        }

        private async void SquareClicked(
            int row,
            int col)
        {
            if (gameOver ||
                aiThinking)
            {
                return;
            }
            // First click
            if (selectedRow == -1)
            {
                Piece piece =
                    board.Squares[
                        row, col];

                // Ignore empty
                if (piece.Type ==
                    PieceType.None)
                {
                    return;
                }

                // Only white pieces
                if (piece.Color !=
                    PieceColor.White)
                {
                    return;
                }

                selectedRow =
                    row;

                selectedCol =
                    col;

                squares[row, col]
                    .BackColor =
                    Color.Yellow;

                return;
            }

            Move move =
                new Move(
                    selectedRow,
                    selectedCol,
                    row,
                    col);

            bool legal =
                moveValidator
                .IsMoveLegal(
                    board,
                    move,
                    PieceColor.White);

            if (legal)
            {
                // Capture BEFORE move
                Piece capturedPiece =
                    board.Squares[
                        row,
                        col];

                if (capturedPiece.Type !=
                    PieceType.None)
                {
                    capturedByWhite
                        .Add(
                            capturedPiece);

                    UpdateCapturedUI();
                }

                // Human move
                Piece movingPiece =
                    board.Squares[
                        selectedRow,
                        selectedCol];

                board.Squares[
                    row,
                    col] =
                    movingPiece;

                board.Squares[
                    selectedRow,
                    selectedCol] =
                    new Piece(
                        PieceType.None,
                        PieceColor.None);

                PromotePawn();

                DrawBoard();
                UpdateCheckStatus();
                if (CheckGameOver(
                    PieceColor.Black))
                {
                    ResetBoardColors();

                    selectedRow = -1;
                    selectedCol = -1;

                    DrawBoard();
                    UpdateCheckStatus();

                    return;
                }
                aiThinking = true;
                turnLabel.Text =
                    "AI Thinking...";

                // Small delay
                await Task.Delay(200);

                // AI thinking
                Move aiMove =
                    await Task.Run(() =>
                        ai.GetBestMove(
                            board,
                            PieceColor.Black));

                Piece aiPiece =
                    board.Squares[
                        aiMove.StartRow,
                        aiMove.StartCol];

                // AI capture BEFORE move
                Piece aiCapturedPiece =
                    board.Squares[
                        aiMove.EndRow,
                        aiMove.EndCol];

                if (aiCapturedPiece.Type !=
                    PieceType.None)
                {
                    capturedByBlack
                        .Add(
                            aiCapturedPiece);

                    UpdateCapturedUI();
                }

                // AI move
                board.Squares[
                    aiMove.EndRow,
                    aiMove.EndCol] =
                    aiPiece;

                board.Squares[
                    aiMove.StartRow,
                    aiMove.StartCol] =
                    new Piece(
                        PieceType.None,
                        PieceColor.None);
                PromotePawn();

                if (CheckGameOver(
                    PieceColor.White))
                {
                    ResetBoardColors();

                    selectedRow = -1;
                    selectedCol = -1;

                    DrawBoard();
                    UpdateCheckStatus();

                    return;
                }

                DrawBoard();
                UpdateCheckStatus();

                turnLabel.Text =
                    "Your Move";
                aiThinking = false;
            }

            ResetBoardColors();

            selectedRow = -1;
            selectedCol = -1;

            DrawBoard();
            UpdateCheckStatus();
        }

        private void UpdateTimer(
            object? sender,
            EventArgs e)
        {
            timer.Tick(
                PieceColor.White);

            int minutes =
                timer.WhiteTimeSeconds / 60;

            int seconds =
                timer.WhiteTimeSeconds % 60;

            whiteTimerLabel.Text =
                $"{minutes:D2}:{seconds:D2}";

            if (timer
                .IsTimeOver(
                    PieceColor.White))
            {
                gameTimer.Stop();

                MessageBox.Show(
                    "Time Over!\nYou Lost.");
            }
        }

        private void RestartGame(
            object? sender,
            EventArgs e)
        {
            gameOver = false;

            gameTimer.Start();
            board =
                new Board();

            capturedByWhite
                .Clear();

            capturedByBlack
                .Clear();

            UpdateCapturedUI();

            timer =
                new ChessTimer();

            whiteTimerLabel.Text =
                "10:00";

            selectedRow = -1;
            selectedCol = -1;

            turnLabel.Text =
                "Your Move";

            DrawBoard();
            UpdateCheckStatus();
        }

        private void WithdrawGame(
            object? sender,
            EventArgs e)
        {
            MessageBox.Show(
                "You withdrew.\nBlack wins!");
        }

        private void ResetBoardColors()
        {
            for (int row = 0;
                 row < 8;
                 row++)
            {
                for (int col = 0;
                     col < 8;
                     col++)
                {
                    squares[row, col]
                        .BackColor =
                        (row + col) % 2 == 0
                        ? Color.Beige
                        : Color.SaddleBrown;
                }
            }
        }

        private string GetPieceSymbol(
            Piece piece)
        {
            return piece.Type switch
            {
                PieceType.King =>
                    piece.Color ==
                    PieceColor.White
                    ? "♔" : "♚",

                PieceType.Queen =>
                    piece.Color ==
                    PieceColor.White
                    ? "♕" : "♛",

                PieceType.Rook =>
                    piece.Color ==
                    PieceColor.White
                    ? "♖" : "♜",

                PieceType.Bishop =>
                    piece.Color ==
                    PieceColor.White
                    ? "♗" : "♝",

                PieceType.Knight =>
                    piece.Color ==
                    PieceColor.White
                    ? "♘" : "♞",

                PieceType.Pawn =>
                    piece.Color ==
                    PieceColor.White
                    ? "♙" : "♟",

                _ => ""
            };
        }

        private void UpdateCapturedUI()
        {
            blackCapturedLabel.Text =
                "Captured:\n" +
                string.Join(
                    " ",
                    capturedByWhite
                    .Select(p =>
                        GetPieceSymbol(p)));

            whiteCapturedLabel.Text =
                "Lost:\n" +
                string.Join(
                    " ",
                    capturedByBlack
                    .Select(p =>
                        GetPieceSymbol(p)));
        }

        private bool CheckGameOver(
    PieceColor currentPlayer)
        {
            // Checkmate
            if (checkmateDetector
                .IsCheckmate(
                    board,
                    currentPlayer))
            {
                gameOver = true;

                gameTimer.Stop();

                string winner =
                    currentPlayer ==
                    PieceColor.White
                    ? "Black Wins!"
                    : "You Win!";

                MessageBox.Show(
                    $"Checkmate!\n{winner}");

                return true;
            }

            // Stalemate
            if (stalemateDetector
                .IsStalemate(
                    board,
                    currentPlayer))
            {
                gameOver = true;

                gameTimer.Stop();

                MessageBox.Show(
                    "Draw by Stalemate!");

                return true;
            }

            return false;
        }

        private void UpdateCheckStatus()
        {
            CheckDetector
                detector =
                new CheckDetector();

            bool whiteInCheck =
                detector
                .IsKingInCheck(
                    board,
                    PieceColor.White);

            bool blackInCheck =
                detector
                .IsKingInCheck(
                    board,
                    PieceColor.Black);

            if (whiteInCheck)
            {
                checkLabel.Text =
                    "CHECK!";
            }
            else if (blackInCheck)
            {
                checkLabel.Text =
                    "AI IN CHECK!";
            }
            else
            {
                checkLabel.Text =
                    "";
            }
        }

        private void PromotePawn()
        {
            // White promotion
            for (int col = 0;
                 col < 8;
                 col++)
            {
                Piece piece =
                    board.Squares[
                        0,
                        col];

                if (piece.Type ==
                    PieceType.Pawn &&
                    piece.Color ==
                    PieceColor.White)
                {
                    board.Squares[
                        0,
                        col] =
                        new Piece(
                            PieceType.Queen,
                            PieceColor.White);
                }
            }

            // Black promotion
            for (int col = 0;
                 col < 8;
                 col++)
            {
                Piece piece =
                    board.Squares[
                        7,
                        col];

                if (piece.Type ==
                    PieceType.Pawn &&
                    piece.Color ==
                    PieceColor.Black)
                {
                    board.Squares[
                        7,
                        col] =
                        new Piece(
                            PieceType.Queen,
                            PieceColor.Black);
                }
            }
        }
    }
}