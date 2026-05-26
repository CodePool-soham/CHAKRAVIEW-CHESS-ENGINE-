namespace ChakraView.Game
{
    public class Move
    {
        public int StartRow { get; set; }
        public int StartCol { get; set; }

        public int EndRow { get; set; }
        public int EndCol { get; set; }

        public Move(int startRow, int startCol, int endRow, int endCol)
        {
            StartRow = startRow;
            StartCol = startCol;
            EndRow = endRow;
            EndCol = endCol;
        }
    }
}