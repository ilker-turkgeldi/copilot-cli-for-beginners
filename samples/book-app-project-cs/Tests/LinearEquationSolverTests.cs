using BookApp.Services;

namespace BookApp.Tests;

public class LinearEquationSolverTests
{
    [Fact]
    public void Solve_TwoByTwoSystem_ReturnsExpectedSolutionVector()
    {
        var solver = new LinearEquationSolver();
        var coefficients = new double[,]
        {
            { 2, 1 },
            { 1, -1 }
        };
        var constants = new[] { 5d, 1d };

        var solution = solver.Solve(coefficients, constants);

        Assert.Equal([2d, 1d], solution, new DoubleComparer(6));
        Assert.Equal(solution, solver.SolutionVector, new DoubleComparer(6));
    }

    [Fact]
    public void Solve_ThreeByThreeSystem_UsesPartialPivotingAndReturnsSolution()
    {
        var solver = new LinearEquationSolver();
        var coefficients = new double[,]
        {
            { 0, 2, 1 },
            { 1, -2, -3 },
            { 3, -1, 2 }
        };
        var constants = new[] { 8d, -11d, -3d };

        var solution = solver.Solve(coefficients, constants);

        Assert.Equal([1d, 2d, 4d], solution, new DoubleComparer(6));
    }

    [Fact]
    public void Solve_MoreThanFiveUnknowns_ThrowsArgumentOutOfRangeException()
    {
        var solver = new LinearEquationSolver();
        var coefficients = new double[,]
        {
            { 1, 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 1 }
        };
        var constants = new[] { 1d, 2d, 3d, 4d, 5d, 6d };

        Assert.Throws<ArgumentOutOfRangeException>(() => solver.Solve(coefficients, constants));
    }

    [Fact]
    public void Solve_MismatchedDimensions_ThrowsArgumentException()
    {
        var solver = new LinearEquationSolver();
        var coefficients = new double[,]
        {
            { 1, 2 },
            { 3, 4 }
        };
        var constants = new[] { 5d };

        Assert.Throws<ArgumentException>(() => solver.Solve(coefficients, constants));
    }

    [Fact]
    public void Solve_SingularMatrix_ThrowsInvalidOperationException()
    {
        var solver = new LinearEquationSolver();
        var coefficients = new double[,]
        {
            { 1, 2 },
            { 2, 4 }
        };
        var constants = new[] { 3d, 6d };

        Assert.Throws<InvalidOperationException>(() => solver.Solve(coefficients, constants));
    }

    private sealed class DoubleComparer(int precision) : IEqualityComparer<double>
    {
        private readonly double _tolerance = Math.Pow(10, -precision);

        public bool Equals(double x, double y) => Math.Abs(x - y) <= _tolerance;

        public int GetHashCode(double obj) => 0;
    }
}
