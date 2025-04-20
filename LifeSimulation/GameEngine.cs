namespace LifeSimulation
{
    class GameEngine
    {
        private bool[,] _field;
        private int _rows;
        private int _cols;
        Random random = new Random();

        public int CountGeneration { get; private set; }

        public GameEngine(int rows, int cols, int density)
        {
            _rows = rows;
            _cols = cols;

            _field = new bool[_cols, _rows];

            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    _field[x, y] = random.Next(density) == 0;
                }
            }
        }

        public void NextGeneration()
        {
            var newField = new bool[_cols, _rows];

            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    var neighbours = CountNeighbours(x, y);
                    var isLive = _field[x, y];

                    if (!isLive && neighbours == 3)
                        newField[x, y] = true;
                    else if (isLive && (neighbours > 3 || neighbours < 2))
                        newField[x, y] = false;
                    else
                        newField[x, y] = _field[x, y];
                }
            }

            CountGeneration++;

            _field = newField;
        }

        public bool[,] GetCurrentField()
        {
            var newField = new bool[_cols,_rows];
            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    newField[x, y] = _field[x, y];
                }
            }
            return newField;
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + _cols) % _cols;
                    var row = (y + j + _rows) % _rows;

                    var isSelfChecking = col == x && row == y;
                    var isLive = _field[col, row];

                    if (isLive && !isSelfChecking)
                        count++;
                }
            }

            return count;
        }

        private bool CellValidatePosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _cols && y < _rows;
        }

        private void UpdateCell(int x, int y, bool state)
        {
            if (CellValidatePosition(x, y))
                _field[x, y] = state;
        }

        public void AddCell(int x, int y, bool state)
        {
            UpdateCell(x,y,state);
        }
        public void RemoveCell(int x, int y, bool state)
        {
            UpdateCell(x, y, state);
        }
    }
}
