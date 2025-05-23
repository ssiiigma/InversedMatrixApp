using System.ComponentModel;

namespace InversedMatrix_ver3
{
    public static class FileManager
    {
        // Save resulted inversed matrix into txt file
        public static void SaveMatrixToFile(string path, double[,] matrix, Action onComplete, Action<string> onError)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter((string)e.Argument))
                    {
                        int rows = matrix.GetLength(0);
                        int cols = matrix.GetLength(1);

                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < cols; j++)
                                sw.Write($"{matrix[i, j]:F3}\t");
                            sw.WriteLine();
                        }
                    }
                    e.Result = true;
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Result is Exception error)
                    onError?.Invoke($"Помилка збереження: {error.Message}");
                else
                    onComplete?.Invoke();
            };

            worker.RunWorkerAsync(path);
        }

        // Import original matrix
        public static void LoadMatrixFromFile(string path, Action<double[,]> onSuccess, Action<string> onError)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                try
                {
                    string[] lines = File.ReadAllLines((string)e.Argument);
                    if (lines.Length == 0)
                        throw new Exception("Файл порожній");

                    int rows = lines.Length;
                    int cols = lines[0].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

                    double[,] matrix = new double[rows, cols];


                    for (int i = 0; i < rows; i++)
                    {
                        string[] values = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length != cols)
                            throw new Exception($"Непослідовна кількість стовпців у рядку {i + 1}");

                        for (int j = 0; j < cols; j++)
                            if (!double.TryParse(values[j], out matrix[i, j]))
                                throw new Exception($"Невірне значення у рядку {i + 1}, стовпець {j + 1}");
                    }

                    e.Result = matrix;
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Result is Exception error)
                    onError?.Invoke($"Помилка завантаження: {error.Message}");
                else
                    onSuccess?.Invoke((double[,])e.Result);
            };

            worker.RunWorkerAsync(path);
        }
    }
}