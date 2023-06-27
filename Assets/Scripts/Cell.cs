using UnityEngine;
/// <summary>
/// Structure that represents a minesweeper cell
/// </summary>
public struct Cell
{
    public enum Type
    {
        Invalid,
        Empty,
        Mine,
        Number,
    }

    public Type type { get; set; }
    public Vector3Int Position { get; set; }
    public int Number { get; set; }
    public bool IsOpend { get; set; }
    public bool IsFlagged { get; set; }
    public bool IsExploded { get; set; }
}
