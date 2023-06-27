using UnityEngine;

/// <summary>
/// Class that represents the logic of the game.
/// </summary>
public class Game
{
    public int Width { get; private set; } = 9; // Width of the game field.
    public int Height { get; private set; } = 9; // Height of the game field.
    public int MinesCounter { get; private set; } = 10; // Number of mines.
    public Cell[,] State { get; private set; } // Game field.
    public bool IsGameOver { get; private set; } = false;
    public bool IsGameWin { get; private set; } = false;

    /// <summary>
    /// Method method that sets preferences.
    /// </summary>
    /// <param name="width">Width of the game field.</param>
    /// <param name="height">Height of the game field.</param>
    /// <param name="minesCounter">Number of mines.</param>
    public void ApplySettings(int width, int height, int minesCounter)
    {
        if (width > 0 && height > 0 && minesCounter >= 0 && minesCounter <= width * height)
        {
            Width = width;
            Height = height;
            MinesCounter = minesCounter;
        }
    }

    /// <summary>
    /// Method for setting a new game.
    /// </summary>
    public void NewGame()
    {
        State = new Cell[Width, Height]; // game field

        GenerateCells();
        GenerateMines();
        GenerateNumbers();

        IsGameOver = false;
        IsGameWin = false;
    }

    /// <summary>
    /// Method that opens a cells around a cell with a number.
    /// </summary>
    /// <param name="x">Cell x coordinate.</param>
    /// <param name="y">Cell y coordinate.</param>
    public void OpenArea(int x, int y)
    {
        Cell cell = GetCell(x: x, y: y);

        if (cell.type == Cell.Type.Invalid || !cell.IsOpend)
        {
            return;
        }

        if (CountFlagsInArea(x: x, y: y) == cell.Number)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Open(x: x + i, y: y + j);
                }
            }
        }
    }

    /// <summary>
    /// Method that sets a flag on a cell.
    /// </summary>
    /// <param name="x">Cell x coordinate.</param>
    /// <param name="y">Cell y coordinate.</param>
    public void Flag(int x, int y)
    {
        Cell cell = GetCell(x: x, y: y);

        if (cell.type == Cell.Type.Invalid || cell.IsOpend)
        {
            return;
        }

        cell.IsFlagged = !cell.IsFlagged;
        State[x, y] = cell;
    }


    /// <summary>
    /// Method that opens a cell.
    /// </summary>
    /// <param name="x">Cell x coordinate.</param>
    /// <param name="y">Cell y coordinate.</param>
    public void Open(int x, int y)
    {
        Cell cell = GetCell(x: x, y: y);

        if (cell.type == Cell.Type.Invalid || cell.IsFlagged || cell.IsOpend)
        {
            return;
        }

        switch (cell.type)
        {
            case Cell.Type.Mine:
                Explode(cell);
                break;

            case Cell.Type.Empty:
                OpenNeighbours(cell);
                CheckWin();
                break;

            default:
                cell.IsOpend = true;
                State[x, y] = cell;
                CheckWin();
                break;
        }
    }

    /// <summary>
    /// Method that counts the number of flags on the game field.
    /// </summary>
    /// <returns>Number of flags on the game field.</returns>
    public int CountFlags()
    {
        int flagCounter = 0;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (State[x, y].IsFlagged)
                {
                    flagCounter++;
                }
            }
        }

        return flagCounter;
    }

    /// <summary>
    /// Method that generates cells
    /// </summary>
    private void GenerateCells()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cell cell = new Cell();
                cell.Position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                State[x, y] = cell;
            }
        }
    }

    /// <summary>
    /// Method that places mines.
    /// </summary>
    private void GenerateMines()
    {
        if (MinesCounter >= 0 && MinesCounter <= Height * Width)
        {
            for (int i = 0; i < MinesCounter; i++)
            {
                int x = Random.Range(0, Width);
                int y = Random.Range(0, Height);

                if (State[x, y].type == Cell.Type.Mine)
                {
                    i--;
                    continue;
                }

                State[x, y].type = Cell.Type.Mine;
            }
        }
    }

    /// <summary>
    /// Method that places numbers.
    /// </summary>
    private void GenerateNumbers()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (State[x, y].type == Cell.Type.Mine)
                {
                    continue;
                }

                State[x, y].Number = CountMines(x: x, y: y);

                if (State[x, y].Number > 0)
                {
                    State[x, y].type = Cell.Type.Number;
                }
            }
        }
    }

    /// <summary>
    /// Method that counts mines around cell.
    /// </summary>
    /// <param name="x">Cell x coordinate.</param>
    /// <param name="y">Cell y coordinate.</param>
    /// <returns>Number of mines around cell.</returns>
    private int CountMines(int x, int y)
    {
        int minesCounter = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (GetCell(x: x + i, y: y + j).type == Cell.Type.Mine)
                {
                    minesCounter++;
                }
            }
        }

        return minesCounter;
    }

    /// <summary>
    /// Method that counts flags around cell.
    /// </summary>
    /// <param name="x">Cell x coordinate.</param>
    /// <param name="y">Cell x coordinate.</param>
    /// <returns>Number of flags around cell.</returns>
    private int CountFlagsInArea(int x, int y)
    {
        int flagsCounter = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (GetCell(x: x + i, y: y + j).IsFlagged)
                {
                    flagsCounter++;
                }
            }
        }

        return flagsCounter;
    }

    /// <summary>
    /// Method that opens neighbours of a cell.
    /// </summary>
    /// <param name="cell">Ñell whose neighbors are open.</param>
    private void OpenNeighbours(Cell cell)
    {
        if (cell.IsOpend)
        {
            return;
        }
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid)
        {
            return;
        }

        cell.IsOpend = true;
        State[cell.Position.x, cell.Position.y] = cell;

        if (cell.type == Cell.Type.Empty)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    OpenNeighbours(GetCell(x: cell.Position.x + i, y: cell.Position.y + j));
                }
            }
        }
    }

    /// <summary>
    /// Methot that explodes cell.
    /// </summary>
    /// <param name="cell">Ñell that explodes.</param>
    private void Explode(Cell cell)
    {
        IsGameOver = true;

        cell.IsOpend = true;
        cell.IsExploded = true;
        State[cell.Position.x, cell.Position.y] = cell;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                cell = State[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.IsOpend = true;
                    State[x, y] = cell;
                }
            }
        }
    }

    /// <summary>
    /// Method that checks the win status.
    /// </summary>
    private void CheckWin()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cell cell = State[x, y];

                if (cell.type != Cell.Type.Mine && !cell.IsOpend)
                {
                    return;
                }
            }
        }

        IsGameOver = true;
        IsGameWin = true;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cell cell = State[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.IsFlagged = true;
                    State[x, y] = cell;
                }
            }
        }
    }

    /// <summary>
    /// Method that returns a cell by coordinates.
    /// </summary>
    /// <param name="x">Cell x coordinate.</param>
    /// <param name="y">Cell y coordinate.</param>
    /// <returns>Cell.</returns>
    private Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return State[x, y];
        }
        else
        {
            return new Cell();
        }
    }
}
