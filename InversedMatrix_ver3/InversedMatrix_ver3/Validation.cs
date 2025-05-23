namespace InversedMatrix_ver3;

public static class Validation
{
    // Messages in input-out-of-range-case 
    public static String GetErrorInputMsg(int size, double minValue, double maxValue)
    {
        if (size < 2 || size > 10)
            return "Розмір матриці має бути в межах від 2 до 10.";
        
        if (minValue < -100 || minValue > 0)
            return "Мінімальне значення має бути в межах від -100 до 0.";

        if (maxValue < 1 || maxValue > 100)
            return "Максимальне значення має бути в межах від 1 до 100.";

        if (minValue >= maxValue)
            return "Мінімальне значення повинно бути меншим за максимальне.";

        return String.Empty;
    }
    
    // Validating manual input
    public static bool IsInputValid(double[,] matrix, bool isManual)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        double minValue = matrix[0, 0];
        double maxValue = matrix[0, 0];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix[i, j] < minValue)
                    minValue = matrix[i, j];
                if (matrix[i, j] > maxValue)
                    maxValue = matrix[i, j];
            }
        }
            
        if (isManual && (minValue < -100 || maxValue > 100))
        {
            MessageBox.Show("Ваші значення за межами діапазону!", "Помилка", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        if (!isManual)
        {
            String errorMsg = GetErrorInputMsg(rows, minValue, maxValue);
            if (errorMsg.Length > 0)
            {
                MessageBox.Show(errorMsg, "Помилка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
            
        return true;
    }
    
    // Checking if cells aren't empty + last el aren't 0
    public static double[,] CheckCellContent(DataGridView grid, bool isLUP)
    {
        int rows = grid.Rows.Count;
        int cols = grid.Columns.Count;
        double[,] matrix = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var cellValue = grid.Rows[i].Cells[j].Value?.ToString();
                if (cellValue == null)
                    throw new ArgumentException("Для початку обчислень необхідно повністю заповнити матрицю!");

                if (!double.TryParse(cellValue.Replace('.', ','), out matrix[i, j]))
                    throw new ArgumentException($"Невірне значення у клітинці [{i},{j}]");
            }
        }
            
        if (!isLUP && matrix[rows - 1, cols - 1] == 0)
            throw new ArgumentException("Значення в останній комірці матриці не може бути нулем, " +
                                        "інакше вихідна матриця не буде оборотною.");
        
        return matrix;
    }
}