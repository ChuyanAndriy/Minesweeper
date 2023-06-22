using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour 
{
    public Tilemap Map { get; private set; }

    [SerializeField] private Tile _unclickedTile;
    [SerializeField] private Tile _emptyTile;
    [SerializeField] private Tile _mineTile;
    [SerializeField] private Tile _explodedTile;
    [SerializeField] private Tile _flaggedTile;
    [SerializeField] private List<Tile> _numberTile;

    private void Awake() 
    {
        Map = GetComponent<Tilemap>();
    }

    public void Draw(Cell[,] state) 
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

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

    private Tile GetOpendTile(Cell cell) 
    {
        switch (cell.type) 
        {
            case Cell.Type.Empty: return _emptyTile;
            case Cell.Type.Mine: return cell.IsExploded ? _explodedTile : _mineTile; 
            case Cell.Type.Number: return GetNumberTile(cell: cell);
            default: return null;
        }
    }

    private Tile GetNumberTile (Cell cell) 
    {
        return _numberTile[cell.Number - 1];
    }
}
