using UnityEngine;
using System.Collections.Generic;

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

public class Grid : MonoBehaviour
{
    [SerializeField]
    GameObject tilePrefab;

    const int rows = 10;
    const int cols = 20;

    int[,] tiles =
    {   //0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 <-- Columns
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 0
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 1
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 2
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 3
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 4
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 5
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 6
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 7
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 8
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }  // 9
    };                                                             // Rows ^

    List<List<GameObject>> tileObjects = new List<List<GameObject>>();

    void Start()
    {
        float y = 9.5f;
        for (int row = 0; row < rows; row++)
        {
            List<GameObject> rowObjects = new List<GameObject>();
            float x = 0.5f;
            for (int col = 0; col < cols; col++)
            {
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.position = new Vector3(x, y);
                rowObjects.Add(tile);
                x += 1.0f;
            }
            tileObjects.Add(rowObjects);
            y -= 1.0f;
        }
    }

    void Update()
    {
        DrawGradient();

        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Cell cell = WorldToGrid(mouse);
        Debug.Log("Row: " + cell.row + " Col: " + cell.col);

        // If our cell is on the grid (meaning its not invalid):
        if (!Cell.Equals(cell, Cell.Invalid()))
        {
            // Colour the cell that our cursor is inside of magenta.
            GameObject cellObj = tileObjects[cell.row][cell.col];
            cellObj.GetComponent<SpriteRenderer>().color = Color.magenta;

            // Colour all adjacent cells blue!
            List<Cell> cells = Pathing.Adjacent(cell, rows, cols);
            for (int i = 0; i < cells.Count; i++)
            {
                // If you did task 1 correctly, you should see a blue "plus" around your cursor!
                Cell adj = cells[i];
                GameObject adjObj = tileObjects[adj.row][adj.col];
                adjObj.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    void DrawGradient()
    {
        float y = 9.5f;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject go = tileObjects[row][col];
                Vector2 pos = go.transform.position;
                float u = pos.x / (float)cols;
                float v = pos.y / (float)rows;
                Color color = new Color(u, v, 0.0f);
                go.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }

    Cell WorldToGrid(Vector3 pos)
    {
        if (pos.x < 0.0f || pos.x > cols || pos.y < 0.0f || pos.y > rows)
            return Cell.Invalid();

        Cell cell = new Cell();
        cell.col = (int)pos.x;
        cell.row = (rows - 1) - (int)pos.y;
        return cell;
    }
}
