using System.Collections.Generic;
using UnityEngine;
using Utils;

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
    public float cost;// How expensive it is to move to this node
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

        List<Cell> debugCells = new List<Cell>();

        // Search until there's nothing left to explore
        //while (open.Count > 0)

        // Easier to visualize if we drag our iterations slider to see flood-fill step by step
        for (int i = 0; i < iterations; i++)
        {
            // Examing the front of the queue ("first in line")
            Cell front = open.Dequeue();

            // Add to current cell to debug render (and prevent duplicates by ensuring its unexplored)
            if (grid.IsDebugDraw() && closed[front.row, front.col] == false)
                debugCells.Add(front);

            // Prevent the explored cell from being revisited (prevents infinite loop)
            closed[front.row, front.col] = true;

            // Stop searching if we've reached our goal
            if (Cell.Equals(front, end))
            {
                found = true;
                break;
            }

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

        if (!found)
        {
            foreach (Cell cell in debugCells)
                grid.DrawCell(cell, Color.magenta);
        }

        // If we've found the end, retrace our steps. Otherwise, there's no solution so return an empty list.
        List<Cell> result = found ? Retrace(nodes, start, end) : new List<Cell>();
        return result;
    }

    public static List<Cell>Dijkstra(Cell start, Cell end, int[,] tiles, int iterations, Grid grid)
    {
        int rows = tiles.GetLength(0);
        int cols = tiles.GetLength(1);
        Node[,] nodes = new Node[rows, cols];   // <-- Connections between cells (each cell and what came before each cell)
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                nodes[row, col].curr = new Cell { row = row, col = col };
                nodes[row, col].prev = Cell.Invalid();
                nodes[row, col].cost = float.MaxValue;
            }
        }

        PriorityQueue<Cell, float> open = new PriorityQueue<Cell, float>();
        open.Enqueue(start, 0.0f);
        nodes[start.row, start.col].cost = 0.0f;

        bool found = false;
        HashSet<Cell> debugCells = new HashSet<Cell>();
        for (int i = 0; i < iterations; i++)
        {
            // Examine the cell with the highest priority (lowest cost)
            Cell front = open.Dequeue();

            // Stop searching if we've reached our goal
            if (Cell.Equals(front, end))
            {
                found = true;
                break;
            }

            if (grid.IsDebugDraw())
                debugCells.Add(front);

            // Update cell cost and add it to open list if the new cost is cheaper than the old cost
            foreach (Cell adj in Adjacent(front, rows, cols))
            {
                float prevCost = nodes[adj.row, adj.col].cost;
                float currCost = nodes[front.row, front.col].cost + grid.TileCost(grid.TileType(adj));
                if (currCost < prevCost)
                {
                    open.Enqueue(adj, currCost);
                    nodes[adj.row, adj.col].cost = currCost;
                    nodes[adj.row, adj.col].prev = front;
                }
            }
        }

        if (!found)
        {
            foreach (Cell cell in debugCells)
                grid.DrawCell(cell, Color.magenta);
        }

        // If we've found the end, retrace our steps. Otherwise, there's no solution so return an empty list.
        List<Cell> result = found ? Retrace(nodes, start, end) : new List<Cell>();
        return result;
    }

    // Looks like task 2 has also been done for you... Enjoy a free lab I guess
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
            path.Add(curr);

            // 2. Set curr equal to prev
            curr = prev;

            // 3. Set prev equal to the cell that came before curr (query the node grid just like when we defined prev)
            prev = nodes[curr.row, curr.col].prev;

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
