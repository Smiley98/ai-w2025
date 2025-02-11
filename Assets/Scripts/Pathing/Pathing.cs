using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public int row;
    public int col;

    public static bool Equals(Cell a, Cell b)
    {
        return a.row == b.row && a.col == b.col;
    }

    public static Cell Invalid()
    {
        return new Cell { row = -1, col = -1 };
    }
}

public static class Pathing
{
    public static List<Cell> FloodFill(Cell start, Cell end, int[,] tiles, int iterations)
    {
        int rows = tiles.GetLength(0);
        int cols = tiles.GetLength(1);

        List<Cell> result = new List<Cell>();
        return result;
    }

    // Task 1:
    // Given a cell, output the left, right, up, and down neighbouring cells\
    // Ensure you don't return cells outside of the grid!
    public static List<Cell> Adjacent(Cell cell, int rows, int cols)
    {
        List<Cell> cells = new List<Cell>();
        Cell left = new Cell { row = cell.row, col = cell.col - 1 };
        Cell right = new Cell { row = cell.row, col = cell.col + 1 };

        if (left.col >= 0) cells.Add(left);
        if (right.col < cols) cells.Add(right);

        // TODO - add up of cell if within grid bounds
        // TODO - add down of cell if within grid bounds
        return cells;
    }
}
