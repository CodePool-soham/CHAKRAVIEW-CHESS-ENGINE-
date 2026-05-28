using System.Linq;
using ChakraView.AI;
using ChakraView.Game;
using ChakraView.Rules;

namespace ChakraView.GUI
{
    public partial class Form1 : Form
    {
        private Button[,] squares;

        private Dictionary<string, Image> pieceImages = new();



        private Board board;

        private ChessAI ai;

        private Label checkLabel;


        private List<Panel> moveHints = new();



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

        private FlowLayoutPanel
            blackCapturedPanel;

        private FlowLayoutPanel
            whiteCapturedPanel;

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
            LoadPieceImages();

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
                    1100,
                    640);

            Text =
                "ChakraView Chess";

            BackColor =
               Color.FromArgb(
                   24, 30, 40);



            FormBorderStyle =
                FormBorderStyle.FixedSingle;

            MaximizeBox =
                false;
        }

        private void LoadPieceImages()
        {
            pieceImages["White_King"] =
                Image.FromFile(
                    "Assets/Pieces/white_king.png");

            pieceImages["White_Queen"] =
                Image.FromFile(
                    "Assets/Pieces/white_queen.png");

            pieceImages["White_Rook"] =
                Image.FromFile(
                    "Assets/Pieces/white_rook.png");

            pieceImages["White_Bishop"] =
                Image.FromFile(
                    "Assets/Pieces/white_bishop.png");

            pieceImages["White_Knight"] =
                Image.FromFile(
                    "Assets/Pieces/white_knight.png");

            pieceImages["White_Pawn"] =
                Image.FromFile(
                    "Assets/Pieces/white_pawn.png");

            pieceImages["Black_King"] =
                Image.FromFile(
                    "Assets/Pieces/black_king.png");

            pieceImages["Black_Queen"] =
                Image.FromFile(
                    "Assets/Pieces/black_queen.png");

            pieceImages["Black_Rook"] =
                Image.FromFile(
                    "Assets/Pieces/black_rook.png");

            pieceImages["Black_Bishop"] =
                Image.FromFile(
                    "Assets/Pieces/black_bishop.png");

            pieceImages["Black_Knight"] =
                Image.FromFile(
                    "Assets/Pieces/black_knight.png");

            pieceImages["Black_Pawn"] =
                Image.FromFile(
                    "Assets/Pieces/black_pawn.png");
        }

        private void    CreateBoard()
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

                    square.FlatStyle =
                        FlatStyle.Flat;

                    square.FlatAppearance
                        .BorderSize = 0;

                    square.Margin =
                        Padding.Empty;

                    square.Padding =
                        Padding.Empty;

                    square.ImageAlign =
                        ContentAlignment.MiddleCenter;

                    square.BackColor =
                        (row + col) % 2 == 0
                        ? Color.FromArgb(
                            125, 155, 190) // light blue
                        : Color.FromArgb(
                            70, 97, 135);  // dark blue

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
            int panelX = 760;

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

            whiteTimerLabel.ForeColor =
                Color.WhiteSmoke;

            whiteTimerLabel.BackColor =
                Color.Transparent;

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
                    16,
                    FontStyle.Bold);

            turnLabel.ForeColor =
                Color.Gainsboro;

            turnLabel.BackColor =
                Color.Transparent;

            turnLabel.AutoSize =
                true;

            turnLabel.Left =
                panelX;

            turnLabel.Top =
                150;

            Controls.Add(
                turnLabel);

            // Check Label
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
                Color.FromArgb(
                    255, 90, 90);

            checkLabel.BackColor =
                Color.Transparent;

            checkLabel.AutoSize =
                true;

            checkLabel.Left =
                panelX;

            checkLabel.Top =
                185;

            Controls.Add(
                checkLabel);

            // Captured title
            Label capturedTitle =
                new Label();

            capturedTitle.Text =
                "Captured Pieces";

            capturedTitle.ForeColor =
                Color.White;

            capturedTitle.BackColor =
                Color.Transparent;

            capturedTitle.Font =
                new Font(
                    "Segoe UI",
                    14,
                    FontStyle.Bold);

            capturedTitle.Left =
                panelX;

            capturedTitle.Top =
                220;

            capturedTitle.AutoSize =
                true;

            Controls.Add(
                capturedTitle);

            // Captured panel
            blackCapturedPanel =
                new FlowLayoutPanel();

            blackCapturedPanel.Left =
                panelX;

            blackCapturedPanel.Top =
                260;

            blackCapturedPanel.Width =
                280;

            blackCapturedPanel.Height =
                80;

            blackCapturedPanel.BackColor =
                Color.FromArgb(
                    155, 175, 205);

            Controls.Add(
                blackCapturedPanel);

            // Lost title
            Label lostTitle =
                new Label();

            lostTitle.Text =
                "Lost Pieces";

            lostTitle.ForeColor =
                Color.White;

            lostTitle.BackColor =
                Color.Transparent;

            lostTitle.Font =
                new Font(
                    "Segoe UI",
                    14,
                    FontStyle.Bold);

            lostTitle.Left =
                panelX;

            lostTitle.Top =
                360;

            lostTitle.AutoSize =
                true;

            Controls.Add(
                lostTitle);

            // Lost panel
            whiteCapturedPanel =
                new FlowLayoutPanel();

            whiteCapturedPanel.Left =
                panelX;

            whiteCapturedPanel.Top =
                400;

            whiteCapturedPanel.Width =
                280;

            whiteCapturedPanel.Height =
                80;

            whiteCapturedPanel.BackColor =
                Color.FromArgb(
                    155, 175, 205);

            Controls.Add(
                whiteCapturedPanel);

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

            restartButton.FlatStyle =
                FlatStyle.Flat;

            restartButton.FlatAppearance
                .BorderSize = 0;

            restartButton.BackColor =
                Color.FromArgb(
                    44, 55, 72);

            restartButton.ForeColor =
                Color.White;

            restartButton.Font =
                new Font(
                    "Segoe UI",
                    12,
                    FontStyle.Bold);

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

            withdrawButton.FlatStyle =
                FlatStyle.Flat;

            withdrawButton.FlatAppearance
                .BorderSize = 0;

            withdrawButton.BackColor =
                Color.FromArgb(
                    90, 45, 45);

            withdrawButton.ForeColor =
                Color.White;

            withdrawButton.Font =
                new Font(
                    "Segoe UI",
                    12,
                    FontStyle.Bold);

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

                    Button square =
                        squares[
                            row, col];

                    square.Text =
                        "";

                    square.Image =
                        null;

                    if (piece.Type ==
                        PieceType.None)
                    {
                        continue;
                    }

                    string key =
                        $"{piece.Color}_" +
                        $"{piece.Type}";

                    Image original =
                        pieceImages[key];

                    Bitmap fittedImage =
                        new Bitmap(
                            74,
                            74);

                    using (Graphics g =
                           Graphics.FromImage(
                               fittedImage))
                    {
                        g.InterpolationMode =
                            System.Drawing
                            .Drawing2D
                            .InterpolationMode
                            .HighQualityBicubic;

                        g.DrawImage(
                            original,
                            new Rectangle(
                                0,
                                0,
                                74,
                                74));
                    }

                    square.Image =
                        fittedImage;

                    square.UseVisualStyleBackColor = false;


                    square.BackColor =
                        (row + col) % 2 == 0
                        ? Color.FromArgb(
                            125, 155, 190)
                        : Color.FromArgb(
                            70, 97, 135);

                    square.ImageAlign =
                        ContentAlignment
                        .MiddleCenter;
                }
            }
        }

        private async void SquareClicked(
            int row,
            int col)
        {
            if (gameOver ||
                aiThinking ||
                timer.IsTimeOver(
                    PieceColor.White))
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

                selectedRow = row;


                selectedCol = col;


                ShowLegalMoves(row, col);



                squares[row, col].BackColor = Color.FromArgb(120, 170, 255);





                return;
            }
            ClearMoveHints();
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
                    ClearMoveHints();
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

                if (gameOver || timer.IsTimeOver(PieceColor.White))
                {
                    return;
                }

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
                    ClearMoveHints();
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
            ClearMoveHints();
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
            if (gameOver)
            {
                return;
            }

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
                gameOver = true;

                gameTimer.Stop();

                turnLabel.Text =
                    "Game Over";

                MessageBox.Show(
                    "Time Over!\nBlack Wins!");
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
            gameOver = true;

            gameTimer.Stop();

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
                        ? Color.FromArgb(
                            125, 155, 190)
                        : Color.FromArgb(
                            70, 97, 135);
                }
            }
        }

        private void HighlightKingInCheck(PieceColor color)

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
                            row,
                            col];

                    if (piece.Type ==
                        PieceType.King &&
                        piece.Color ==
                        color)
                    {
                        squares[row, col]
                            .BackColor =
                            Color.Red;

                        return;
                    }
                }
            }
        }

        private void ShowLegalMoves(
    int row,
    int col)
        {
            ClearMoveHints();

            for (int targetRow = 0;
                 targetRow < 8;
                 targetRow++)
            {
                for (int targetCol = 0;
                     targetCol < 8;
                     targetCol++)
                {
                    Move move =
                        new Move(
                            row,
                            col,
                            targetRow,
                            targetCol);

                    bool legal =
                        moveValidator
                        .IsMoveLegal(
                            board,
                            move,
                            PieceColor.White);

                    if (legal)
                    {
                        Panel hint =
                            new Panel();

                        hint.Width = 18;
                        hint.Height = 18;

                        hint.BackColor =
                            Color.Gray;

                        hint.Left =
                            squares[targetRow,
                                targetCol].Left
                            + 31;

                        hint.Top =
                            squares[targetRow,
                                targetCol].Top
                            + 31;

                        hint.Region =
                            Region.FromHrgn(
                                NativeMethods
                                .CreateRoundRectRgn(
                                    0,
                                    0,
                                    hint.Width,
                                    hint.Height,
                                    18,
                                    18));

                        hint.Enabled =
                            false;

                        moveHints
                            .Add(hint);

                        Controls.Add(
                            hint);

                        hint.BringToFront();
                    }
                }
            }
        }

        private void ClearMoveHints()
        {
            foreach (var hint
                in moveHints)
            {
                Controls.Remove(
                    hint);

                hint.Dispose();
            }

            moveHints.Clear();
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
            blackCapturedPanel.Controls
                .Clear();

            whiteCapturedPanel.Controls
                .Clear();

            foreach (Piece piece
                in capturedByWhite)
            {
                string key =
                    $"{piece.Color}_" +
                    $"{piece.Type}";

                PictureBox picture =
                    new PictureBox();

                picture.Image =
                    pieceImages[key];

                picture.Width = 36;
                picture.Height = 36;

                picture.SizeMode =
                    PictureBoxSizeMode
                    .Zoom;

                blackCapturedPanel
                    .Controls
                    .Add(picture);
            }

            foreach (Piece piece
                in capturedByBlack)
            {
                string key =
                    $"{piece.Color}_" +
                    $"{piece.Type}";

                PictureBox picture =
                    new PictureBox();

                picture.Image =
                    pieceImages[key];

                picture.Width = 36;
                picture.Height = 36;

                picture.SizeMode =
                    PictureBoxSizeMode
                    .Zoom;

                whiteCapturedPanel
                    .Controls
                    .Add(picture);
            }
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

            ResetBoardColors();

            if (whiteInCheck)
            {
                checkLabel.Text =
                    "CHECK!";

                HighlightKingInCheck(
                    PieceColor.White);
            }
            else if (blackInCheck)
            {
                checkLabel.Text =
                    "AI IN CHECK!";

                HighlightKingInCheck(
                    PieceColor.Black);
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

internal static class NativeMethods
{
    [System.Runtime.InteropServices.DllImport(
        "gdi32.dll")]
    public static extern IntPtr
        CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse);
}