using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
    public int width { get; set; } = Settings.width;
    public int height { get; set; } = Settings.height;
    public int numberOfMines { get; set; } = Settings.mineCounter;

    private GameBoard _gameBoard;
    private Cell[,] _state;
    private bool _isGameOver = false;
    private bool _isActionFirst = true;

    private void OnValidate() 
    {
        numberOfMines = Mathf.Clamp(numberOfMines, 0, width * height);
    }

    private void Awake() 
    {
        _gameBoard = GetComponentInChildren<GameBoard>();
    }

    private void Start() 
    {
        NewGame();

        GenerateCells();
        GenerateMines();
        GenerateNumbers();

        Camera.main.transform.position = new Vector3(width / 2.0f, height / 2.0f, -10.0f);
        Camera.main.orthographicSize = height / 2.0f;
        _gameBoard.Draw(_state);
    }

    private void NewGame() 
    {
        _state = new Cell[width, height];
        _isGameOver = false;
    }

    private void GenerateCells() 
    {
        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                Cell cell = new Cell();
                cell.Position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                _state[x, y] = cell;
            }
        }
    }

    private void GenerateMines() 
    {
        for (int i = 0; i < numberOfMines; i++) 
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (_state[x, y].type == Cell.Type.Mine) 
            {
                i--;
                continue;
            }

            _state[x, y].type = Cell.Type.Mine;
        }
    }

    private void GenerateNumbers() 
    {
        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                if (_state[x, y].type == Cell.Type.Mine) 
                {
                    continue;
                }

                _state[x, y].Number = CountMines(x: x, y: y);

                if (_state[x, y].Number > 0) 
                {
                    _state[x, y].type = Cell.Type.Number;
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

    private int CountFlags(int x, int y) 
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

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Start();

            _isActionFirst = true;
        } 
        else if (!_isGameOver && !PauseMenu.isGamePaused) 
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = _gameBoard.Map.WorldToCell(worldPosition);

            if (Input.GetMouseButtonDown(1)) 
            {
                Flag(x: cellPosition.x, y: cellPosition.y);

                _gameBoard.Draw(_state);
            } 
            else if (Input.GetMouseButtonDown(0)) 
            {
                if (_isActionFirst)
                {
                    _isActionFirst = false;

                    Timer.instance.BeginTimer();
                }

                Open(x: cellPosition.x, y: cellPosition.y);

                _gameBoard.Draw(_state);
            } 
            else if (Input.GetMouseButton(0) && Input.GetMouseButton(1)) 
            {
                if (_isActionFirst)
                {
                    _isActionFirst = false;

                    Timer.instance.BeginTimer();
                }

                OpenArea(x: cellPosition.x, y: cellPosition.y);

                _gameBoard.Draw(_state);
            }
        }
        else if (!PauseMenu.isGamePaused)
        {
            Timer.instance.EndTimer();
        }
    }

    private void OpenArea(int x, int y) 
    {
        Cell cell = GetCell(x: x, y: y);

        if (cell.type == Cell.Type.Invalid || !cell.IsOpend) 
        {
            return;
        }

        if (CountFlags(x: x, y: y) == cell.Number) 
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

    private void Flag(int x, int y) 
    {
        Cell cell = GetCell(x: x, y: y);

        if (cell.type == Cell.Type.Invalid || cell.IsOpend) 
        {
            return;
        }

        cell.IsFlagged = !cell.IsFlagged;
        _state[x, y] = cell;
    }

    private void Open(int x, int y) 
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
                break;

            default:
                cell.IsOpend = true;
                _state[x, y] = cell;
                CheckWin();
                break;
        }
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
        _state[cell.Position.x, cell.Position.y] = cell;

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
        Debug.Log("Game Over!");
        _isGameOver = true;

        cell.IsOpend = true;
        cell.IsExploded = true;
        _state[cell.Position.x, cell.Position.y] = cell;

        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                cell = _state[x, y];

                if (cell.type == Cell.Type.Mine) 
                {
                    cell.IsOpend = true;
                    _state[x, y] = cell;
                }
            }
        }
    }

    private void CheckWin() 
    {
        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                Cell cell = _state[x, y];

                if (cell.type != Cell.Type.Mine && !cell.IsOpend) 
                {
                    return;
                }
            }
        }

        Debug.Log("Win!");
        _isGameOver = true;

        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                Cell cell = _state[x, y];

                if (cell.type == Cell.Type.Mine) 
                {
                    cell.IsFlagged = true;
                    _state[x, y] = cell;
                }
            }
        }
    }

    private Cell GetCell(int x, int y) 
    {
        if (x >= 0 && x < width && y >= 0 && y < height) 
        {
            return _state[x, y];
        } 
        else 
        {
            return new Cell();
        }
    }
}
