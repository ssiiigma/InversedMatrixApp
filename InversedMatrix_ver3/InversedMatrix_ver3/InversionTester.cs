namespace InversedMatrix_ver3
{
    public static class InversionTester
    {
        // For counting iterations, operations and errors
        public static void RunTests()
        {
            int[] sizes = { 3, 5, 7, 10 };
            string borderFile = "BorderingResults.txt";
            string lupFile = "LupResults.txt";

            using StreamWriter borderWriter = new StreamWriter(borderFile);
            using StreamWriter lupWriter = new StreamWriter(lupFile);

            borderWriter.WriteLine("Розмірність;\tЕлементарні операції;\tКількість ітерацій;\tПохибка");
            lupWriter.WriteLine("Розмірність;\tКількість розкладів;\tКількість обернень;\tПохибка");

            foreach (int size in sizes)
            {
                double[,] A = GenerateMatrix(size);
                double[,] A_inv;

                // Bordering method
                A_inv = InversionMethods.InverseByBordering((double[,])A.Clone());
                double errB = InversionMethods.CalculateError(A, A_inv);
                borderWriter.WriteLine($"{size}x{size};{InversionMethods.BorderingOperations};" +
                                       $"{InversionMethods.BorderingIterations};{errB:E5}");

                // LUP method
                A_inv = InversionMethods.InverseByLup((double[,])A.Clone());
                double errL = InversionMethods.CalculateError(A, A_inv);
                lupWriter.WriteLine($"{size}x{size};{InversionMethods.LupDecompositions};" +
                                    $"{InversionMethods.LupBackSubstitutions};{errL:E5}");
            }
        }

        private static double[,] GenerateMatrix(int size)
        {
            Random rand = new Random();
            double[,] A = new double[size, size];
            for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                A[i, j] = rand.NextDouble() * 200 - 100;
            return A;
        }
    }
}