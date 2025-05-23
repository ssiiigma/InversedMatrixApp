namespace InversedMatrix_ver3
{
    public static class Matrix
    {
        // Fill matrix with random els
        public static double[,] GenerateRandomMatrix(int size, double minValue, double maxValue)
        {
            Random rand = new Random();
            double[,] matrix = new double[size, size];
            for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                matrix[i, j] = (float)(minValue + (maxValue - minValue) * rand.NextDouble());
            
            return matrix;
        }

        // Outputting matrix
        public static void DisplayMatrix(DataGridView grid, double[,] matrix)
        {
            grid.Columns.Clear();
            grid.Rows.Clear();
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < cols; i++)
                grid.Columns.Add($"col{i}", "");

            for (int i = 0; i < rows; i++)
            {
                var row = new DataGridViewRow();
                row.CreateCells(grid);
                for (int j = 0; j < cols; j++)
                    row.Cells[j].Value = matrix[i, j];
                grid.Rows.Add(row);
            }
        }
    }
}