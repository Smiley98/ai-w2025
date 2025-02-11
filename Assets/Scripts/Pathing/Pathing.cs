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

public struct Node
{
    public Cell curr; // Current cell
    public Cell prev; // Parent (cell before current)
}

public static class Pathing
{
    public static List<Cell> FloodFill(Cell start, Cell end, int[,] tiles, int iterations, Grid grid)
    {
        int rows = tiles.GetLength(0);
        int cols = tiles.GetLength(1);
        bool[,] closed = new bool[rows, cols];  // <-- Cells we've already explored (can't explore again otherwise infinite loop)
        Node[,] nodes = new Node[rows, cols];   // <-- Connections between cells (each cell and what came before each cell)
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                closed[row, col] = tiles[row, col] == 1;
                nodes[row, col].curr = new Cell { row = row, col = col };
                nodes[row, col].prev = Cell.Invalid();
            }
        }

        // List of cells we've discovered and want to explore -- *first in, first out*
        Queue<Cell> open = new Queue<Cell>();
        open.Enqueue(start);
        bool found = false;

        // Search until there's nothing left to explore
        //while (open.Count > 0)

        // Easier to visualize if we drag our iterations slider to see flood-fill step by step
        for (int i = 0; i < iterations; i++)
        {
            // Examing the front of the queue ("first in line")
            Cell front = open.Dequeue();

            // Prevent the explored cell from being revisited (prevents infinite loop)
            closed[front.row, front.col] = true;

            // Stop searching if we've reached our goal
            if (Cell.Equals(front, end))
            {
                found = true;
                break;
            }

            grid.DrawCell(front, Color.magenta);

            // Otherwise, continue our search by enqueuing adjacent tiles!
            foreach (Cell adj in Adjacent(front, rows, cols))
            {
                // Ensure we haven't already searched this cell
                if (!closed[adj.row, adj.col])
                {
                    open.Enqueue(adj);
                    nodes[adj.row, adj.col].prev = front;
                }
            }
        }

        // If we've found the end, retrace our steps. Otherwise, there's no solution so return an empty list.
        List<Cell> result = found ? Retrace(nodes, start, end) : new List<Cell>();
        return result;
    }

    static List<Cell> Retrace(Node[,] nodes, Cell start, Cell end)
    {
        List<Cell> path = new List<Cell>();

        // Start at the end, and work backwards until we reach the start!
        Cell curr = end;

        // Prev is the cell that came before the current cell
        Cell prev = nodes[curr.row, curr.col].prev;

        // Search until nothing came before the previous cell, meaning we've reached start!
        // (Note that this will halt your program if you run this without any code inside the loop)
        //while (!Cell.Equals(prev, Cell.Invalid()))
        for (int i = 0; i < 32; i++)
        {
            // 1. Add curr to path
            // 2. Set curr equal to prev
            // 3. Set prev equal to the cell that came before curr (query the node grid just like when we defined prev)

            // If the previous cell is invalid, meaning there's no previous cell, then we've reached the start!
            if (Cell.Equals(prev, Cell.Invalid()))
                break;
        }

        return path;
    }

    // Task 1 has been done for you. Enjoy a free 3%!
    public static List<Cell> Adjacent(Cell cell, int rows, int cols)
    {
        List<Cell> cells = new List<Cell>();
        Cell left = new Cell { row = cell.row, col = cell.col - 1 };
        Cell right = new Cell { row = cell.row, col = cell.col + 1 };
        Cell up = new Cell { row = cell.row - 1, col = cell.col };
        Cell down = new Cell { row = cell.row + 1, col = cell.col };

        if (left.col >= 0) cells.Add(left);
        if (right.col < cols) cells.Add(right);
        if (up.row >= 0) cells.Add(up);
        if (down.row < rows) cells.Add(down);
        return cells;
    }
}
