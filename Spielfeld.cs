namespace N_Gewinnt
{
    internal class Spielfeld
    {
        public int paddingX = 20;
        public int paddingY = 150;

        public int rows { get; set; }
        public int cols { get; set; }
        private Chip[,] chips;


        public Spielfeld(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            chips = new Chip[cols, rows];
        }

    }
}
