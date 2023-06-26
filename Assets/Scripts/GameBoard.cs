using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour
{
    public Tilemap Map { get; private set; }

    [SerializeField]
    private Tile _unclickedTile;

    [SerializeField]
    private Tile _emptyTile;

    [SerializeField]
    private Tile _mineTile;

    [SerializeField]
    private Tile _explodedTile;

    [SerializeField]
    private Tile _flaggedTile;

    [SerializeField]
    private List<Tile> _numberTile;

    public Vector3Int GetСoordinates()
    {
        return Map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        Camera.main.transform.position = new Vector3(width / 2.0f, height / 2.0f, -10.0f);
        Camera.main.orthographicSize = height / 2.0f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                Map.SetTile(cell.Position, GetTile(cell: cell));
            }
        }
    }

    public Tile GetTile(Cell cell)
    {
        if (cell.IsOpend)
        {
            return GetOpendTile(cell: cell);
        }
        else if (cell.IsFlagged)
        {
            return _flaggedTile;
        }
        else
        {
            return _unclickedTile;
        }
    }

    private void Awake()
    {
        Map = GetComponent<Tilemap>();
    }

    private Tile GetOpendTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty:
                return _emptyTile;
            case Cell.Type.Mine:
                return cell.IsExploded ? _explodedTile : _mineTile;
            case Cell.Type.Number:
                return GetNumberTile(cell: cell);
            default:
                return null;
        }
    }

    private Tile GetNumberTile(Cell cell)
    {
        return _numberTile[cell.Number - 1];
    }
}
