using UnityEngine;

public class Game
{
    public int Width { get; private set; } = 9;
    public int Height { get; private set; } = 9;
    public int MinesCounter { get; private set; } = 10;
    public Cell[,] State { get; private set; }
    public bool IsGameOver { get; private set; } = false;
    public bool IsGameWin { get; private set; } = false;

    public void ApplySettings(int width, int height, int numberOfMines)
    {
        Width = width;
        Height = height;
        MinesCounter = numberOfMines;
    }

    public void NewGame()
    {
        State = new Cell[Width, Height];

        GenerateCells();
        GenerateMines();
        GenerateNumbers();

        IsGameOver = false;
        IsGameWin = false;
    }

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
