namespace BookApp.Services;

public class LinearEquationSolver
{
    private const int MaxUnknowns = 5;
    private const double PivotTolerance = 1e-10;

    public double[]? SolutionVector { get; private set; }

    public double[] Solve(double[,] coefficients, double[] constants)
    {
        ArgumentNullException.ThrowIfNull(coefficients);
        ArgumentNullException.ThrowIfNull(constants);

        var rowCount = coefficients.GetLength(0);
        var columnCount = coefficients.GetLength(1);

        if (rowCount == 0 || columnCount == 0)
        {
            throw new ArgumentException("The coefficient matrix must not be empty.", nameof(coefficients));
        }

        if (rowCount != columnCount)
        {
            throw new ArgumentException("The coefficient matrix must be square.", nameof(coefficients));
        }

        if (rowCount != constants.Length)
        {
            throw new ArgumentException("The constants vector must match the number of equations.", nameof(constants));
        }

        if (rowCount > MaxUnknowns)
        {
            throw new ArgumentOutOfRangeException(nameof(coefficients), $"A maximum of {MaxUnknowns} unknowns is supported.");
        }

        var augmentedMatrix = BuildAugmentedMatrix(coefficients, constants);

        for (var pivotIndex = 0; pivotIndex < rowCount; pivotIndex++)
        {
            var pivotRow = FindPivotRow(augmentedMatrix, pivotIndex, rowCount);

            if (Math.Abs(augmentedMatrix[pivotRow, pivotIndex]) < PivotTolerance)
            {
                throw new InvalidOperationException("The system does not have a unique solution.");
            }

            if (pivotRow != pivotIndex)
            {
                SwapRows(augmentedMatrix, pivotRow, pivotIndex, rowCount + 1);
            }

            EliminateRowsBelow(augmentedMatrix, pivotIndex, rowCount);
        }

        SolutionVector = BackSubstitute(augmentedMatrix, rowCount);
        return (double[])SolutionVector.Clone();
    }

    private static double[,] BuildAugmentedMatrix(double[,] coefficients, double[] constants)
    {
        var size = constants.Length;
        var augmentedMatrix = new double[size, size + 1];

        for (var row = 0; row < size; row++)
        {
            for (var column = 0; column < size; column++)
            {
                augmentedMatrix[row, column] = coefficients[row, column];
            }

            augmentedMatrix[row, size] = constants[row];
        }

        return augmentedMatrix;
    }

    private static int FindPivotRow(double[,] augmentedMatrix, int pivotIndex, int size)
    {
        var pivotRow = pivotIndex;
        var maxValue = Math.Abs(augmentedMatrix[pivotIndex, pivotIndex]);

        for (var row = pivotIndex + 1; row < size; row++)
        {
            var candidateValue = Math.Abs(augmentedMatrix[row, pivotIndex]);
            if (candidateValue > maxValue)
            {
                maxValue = candidateValue;
                pivotRow = row;
            }
        }

        return pivotRow;
    }

    private static void SwapRows(double[,] matrix, int firstRow, int secondRow, int width)
    {
        for (var column = 0; column < width; column++)
        {
            (matrix[firstRow, column], matrix[secondRow, column]) =
                (matrix[secondRow, column], matrix[firstRow, column]);
        }
    }

    private static void EliminateRowsBelow(double[,] augmentedMatrix, int pivotIndex, int size)
    {
        for (var row = pivotIndex + 1; row < size; row++)
        {
            var factor = augmentedMatrix[row, pivotIndex] / augmentedMatrix[pivotIndex, pivotIndex];

            for (var column = pivotIndex; column <= size; column++)
            {
                augmentedMatrix[row, column] -= factor * augmentedMatrix[pivotIndex, column];
            }
        }
    }

    private static double[] BackSubstitute(double[,] augmentedMatrix, int size)
    {
        var solution = new double[size];

        for (var row = size - 1; row >= 0; row--)
        {
            var value = augmentedMatrix[row, size];

            for (var column = row + 1; column < size; column++)
            {
                value -= augmentedMatrix[row, column] * solution[column];
            }

            if (Math.Abs(augmentedMatrix[row, row]) < PivotTolerance)
            {
                throw new InvalidOperationException("The system does not have a unique solution.");
            }

            solution[row] = value / augmentedMatrix[row, row];
        }

        return solution;
    }
}
