using UnityEngine;
using System.Collections.Generic;
using Utils;

public class Grid : MonoBehaviour
{
    [SerializeField]
    GameObject tilePrefab;

    const int rows = 10;
    const int cols = 20;

    const int AIR = 0;
    const int ROCK = 1;
    const int WATER = 2;
    const int GRASS = 3;
    const int TILE_TYPE_COUNT = 4;

    [SerializeField]
    int iterations;

    int[,] tiles =
    {   //0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 <-- Columns
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 0
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 1
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 2
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 3
        { 1, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 1 }, // 4
        { 1, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 1 }, // 5
        { 1, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 1 }, // 6
        { 1, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 7
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // 8
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }  // 9
    };                                                             // Rows ^

    public List<List<GameObject>> tileObjects = new List<List<GameObject>>();

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

        // Dequeues based on lowest priority -- logs c, b, a
        PriorityQueue<int, float> pq = new PriorityQueue<int, float>();
        int a = 4;
        int b = 8;
        int c = 12;
        pq.Enqueue(a, 3.0f);
        pq.Enqueue(b, 2.0f);
        pq.Enqueue(c, 1.0f);
        while (pq.Count > 0)
        {
            int n = pq.Dequeue();
            Debug.Log(n);
        }
    }

    void Update()
    {
        //DrawGradient();
        DrawTiles();

        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Cell mouseCell = WorldToGrid(mouse);
        //Debug.Log("Row: " + mouseCell.row + " Col: " + mouseCell.col);

        if (!Cell.Equals(mouseCell, Cell.Invalid()))
        {
            DrawCell(mouseCell, Color.magenta);
            //foreach (Cell adj in Pathing.Adjacent(mouseCell, rows, cols))
            //    DrawCell(adj, Color.blue);
        }

        Cell start = new Cell { row = 7, col = 3 };
        Cell end = new Cell { row = 2, col = 16 };
        DrawCell(start, Color.green);
        DrawCell(end, Color.red);
        List<Cell> path = Pathing.FloodFill(start, end, tiles, iterations, this);
        foreach (Cell cell in path)
            DrawCell(cell, Color.blue);
    }

    public void DrawCell(Cell cell, Color color)
    {
        GameObject obj = tileObjects[cell.row][cell.col];
        obj.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    //public void DrawCell(Cell cell, int type)
    //{
    //    DrawCell(cell, colors[type]);
    //}

    public float TileCost(int type)
    {
        float[] costs = new float[TILE_TYPE_COUNT];
        costs[AIR] = 1.0f;
        costs[ROCK] = 100.0f;
        costs[WATER] = 25.0f;
        costs[GRASS] = 10.0f;
        return costs[type];
    }

    public Color TileColor(int type)
    {
        Color[] colors = new Color[TILE_TYPE_COUNT];
        colors[AIR] = Color.white;
        colors[ROCK] = Color.gray;
        colors[WATER] = Color.blue;
        colors[GRASS] = Color.green;
        return colors[type];
    }

    void DrawGradient()
    {
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

    void DrawTiles()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int type = tiles[row, col];
                Cell cell = new Cell { row = row, col = col };
                DrawCell(cell, TileColor(type));
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
