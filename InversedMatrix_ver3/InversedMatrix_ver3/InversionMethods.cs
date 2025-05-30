namespace InversedMatrix_ver3
{
    public static class InversionMethods
    {
        public static int BorderingOperations { get; private set; }
        public static int BorderingIterations { get; private set; }
        public static int LupDecompositions { get; private set; }
        public static int LupBackSubstitutions { get; private set; }

        // To count iterations 
        public static void ResetCounters()
        {
            BorderingOperations = 0;
            BorderingIterations = 0;
            LupDecompositions = 0;
            LupBackSubstitutions = 0;
        }

        // BORDERING METHOD (Окаймлення)
        public static double[,] InverseByBordering(double[,] A)
        {
            ResetCounters();
            int n = A.GetLength(0);
            if (n != A.GetLength(1))
                throw new ArgumentException("Матриця повинна бути квадратною");

            // Start with the inverse of the top-left 1x1 matrix
            double[,] A1 = new double[1, 1];
            A1[0, 0] = 1.0 / A[0, 0];
            BorderingOperations++; // Division

            // Iterative bordering process
            for (int k = 1; k < n; k++)
            {
                BorderingIterations++; // (k+1)x(k+1) submatrix A_k
                
                // Extract the current inverse B of the k x k block
                double[,] B = new double[k, k];
                for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                    B[i, j] = A1[i, j];

                // Extract the last column (u) and last row (v) needed for the bordering formula
                double[] u = new double[k], v = new double[k];
                for (int i = 0; i < k; i++)
                {
                    u[i] = A[i, k]; // Column vector from A
                    v[i] = A[k, i]; // Row vector from A
                }

                // Compute alpha
                double alpha = A[k, k];
                for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                {
                    alpha -= v[i] * B[i, j] * u[j];
                    BorderingOperations += 3;
                }

                if (Math.Abs(alpha) < 1e-10)
                    throw new InvalidOperationException("Матриця вироджена");

                double[,] newB = new double[k + 1, k + 1];
                for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                    newB[i, j] = B[i, j] + OuterProduct(B, v, u, alpha, i, j);

                // Last column and last row
                for (int i = 0; i < k; i++)
                {
                    newB[i, k] = -1 * SumVector(B, u, i) / alpha;
                    newB[k, i] = -1 * SumVector(B, v, i) / alpha;
                    BorderingOperations += 4;
                }

                newB[k, k] = 1.0 / alpha;
                BorderingOperations++;

                // Update A1 to the new inverse
                A1 = newB;
            }

            // Final resulted inverse matrix
            return A1;
        }

        private static double OuterProduct(double[,] B, double[] v, double[] u, double alpha, int i, int j)
        {
            double sum = 0;
            for (int l = 0; l < v.Length; l++)
            for (int m = 0; m < u.Length; m++)
            {
                sum += B[i, l] * v[l] * u[m] * B[m, j];
                BorderingOperations += 4;
            }
            return sum / alpha;
        }

        // Product B * vec for row i
        private static double SumVector(double[,] B, double[] vec, int i)
        {
            double sum = 0;
            for (int j = 0; j < vec.Length; j++)
            {
                sum += B[i, j] * vec[j];
                BorderingOperations += 2;
            }
            return sum;
        }

        // LUP decomposition (LU decomposition with partial pivoting)
        public static double[,] InverseByLup(double[,] A)
        {
            ResetCounters();
            int n = A.GetLength(0);
            if (n != A.GetLength(1))
                throw new ArgumentException("Матриця повинна бути квадратною");

            int[] P; // Permutation vector
            var (L, U, perm) = LupDecompose(A, out P);
            LupDecompositions++;

            double[,] inv = new double[n, n]; // To hold the inverse matrix

            // According to formula:
            // Solve A * X = I column by column
            for (int i = 0; i < n; i++)
            {
                double[] e = new double[n];
                e[i] = 1;

                // Solve L * y = P * e using forward substitution
                double[] y = ForwardSubstitution(L, Permute(e, P));

                // Solve U * x = y using backward substitution
                double[] x = BackwardSubstitution(U, y);
                LupBackSubstitutions++;

                // Store the solution x as the i-th column of the inverse
                for (int j = 0; j < n; j++)
                    inv[j, i] = x[j];
            }

            return inv;
        }

        // Performs LUP decomposition on matrix A, returning L, U, and the permutation vector P
        private static (double[,], double[,], int[]) LupDecompose(double[,] A, out int[] P)
        {
            int n = A.GetLength(0);
            double[,] LU = (double[,])A.Clone(); // Copy of A 
            P = new int[n]; 
            for (int i = 0; i < n; i++) P[i] = i; // Initialize permutation to identity

            for (int k = 0; k < n; k++)
            {
                // Find pivot row
                double max = 0.0;
                int kPrime = k;
                for (int i = k; i < n; i++)
                {
                    if (Math.Abs(LU[i, k]) > max)
                    {
                        max = Math.Abs(LU[i, k]);
                        kPrime = i;
                    }
                }

                // If no pivot found, the matrix is singular
                if (max < 1e-10)
                    throw new InvalidOperationException("Матриця вироджена");

                // Swap rows in LU and P to reflect the pivot
                if (k != kPrime)
                {
                    for (int j = 0; j < n; j++)
                    {
                        double tmp = LU[k, j];
                        LU[k, j] = LU[kPrime, j];
                        LU[kPrime, j] = tmp;
                    }

                    int tmpP = P[k];
                    P[k] = P[kPrime];
                    P[kPrime] = tmpP;
                }

                for (int i = k + 1; i < n; i++)
                {
                    LU[i, k] /= LU[k, k]; // Compute multiplier
                    for (int j = k + 1; j < n; j++)
                        LU[i, j] -= LU[i, k] * LU[k, j]; // Subtract the row multiple
                }
            }

            // Separate LU --> L and U
            double[,] L = new double[n, n];
            double[,] U = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                L[i, i] = 1.0; // Diagonal of L is 1
                for (int j = 0; j <= i; j++)
                    U[j, i] = LU[j, i]; // Upper triangular part
                for (int j = 0; j < i; j++)
                    L[i, j] = LU[i, j]; // Lower triangular part below the diagonal
            }

            return (L, U, P);
        }

        // P --> to a vector b (P * b)
        private static double[] Permute(double[] b, int[] P)
        {
            int n = b.Length;
            double[] bp = new double[n];
            for (int i = 0; i < n; i++)
                bp[i] = b[P[i]];
            return bp;
        }

        // Solves L * y = b using forward substitution (lower triangular)
        private static double[] ForwardSubstitution(double[,] L, double[] b)
        {
            int n = b.Length;
            double[] y = new double[n];
            for (int i = 0; i < n; i++)
            {
                y[i] = b[i];
                for (int j = 0; j < i; j++)
                    y[i] -= L[i, j] * y[j];
            }
            return y;
        }

        // Solves U * x = y using backward substitution (upper triangular)
        private static double[] BackwardSubstitution(double[,] U, double[] y)
        {
            int n = y.Length;
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = y[i];
                for (int j = i + 1; j < n; j++)
                    x[i] -= U[i, j] * x[j];
                x[i] /= U[i, i]; // Divide by the pivot element
            }
            return x;
        }

        // Calculating errors for testing
        public static double CalculateError(double[,] A, double[,] A_inv)
        {
            int n = A.GetLength(0);
            double error = 0.0;

            for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                double sum = 0.0;
                for (int k = 0; k < n; k++)
                    sum += A[i, k] * A_inv[k, j];

                error += Math.Pow(sum - (i == j ? 1.0 : 0.0), 2);
            }

            return Math.Sqrt(error);
        }
    }
}