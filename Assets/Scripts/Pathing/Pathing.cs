using System.Collections.Generic;
using UnityEngine;

public static class Pathing
{
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
