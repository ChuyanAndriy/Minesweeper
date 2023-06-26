using UnityEngine;

public class GameController : MonoBehaviour
{
    private int _width = Settings.Width;
    private int _height = Settings.Height;
    private int _minesCounter = Settings.MinesCounter;
    private Game _minesweeper = new Game();
    private GameBoard _gameBoard;
    private bool _isActionFirst = true;

    private void OnValidate()
    {
        _minesCounter = Mathf.Clamp(_minesCounter, 0, _width * _height);
    }

    private void Awake()
    {
        _gameBoard = GetComponentInChildren<GameBoard>();
    }

    private void Start()
    {
        _minesweeper.ApplySettings(width: _width, height: _height, numberOfMines: _minesCounter);

        _minesweeper.NewGame();

        GameOverMenu.instance.GameOverHide();

        MinesCounterScript.instance.SetMinesCounterText(
            minesCounter: _minesweeper.MinesCounter - _minesweeper.CountFlags()
        );

        _gameBoard.Draw(_minesweeper.State);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Start();

            _isActionFirst = true;

            Timer.instance.EndTimer();
        }
        else if (!_minesweeper.IsGameOver && !PauseMenu.IsGamePaused)
        {
            Vector3Int cellPosition = _gameBoard.GetСoordinates();

            if (Input.GetMouseButtonDown(1))
            {
                _minesweeper.Flag(x: cellPosition.x, y: cellPosition.y);

                MinesCounterScript.instance.SetMinesCounterText(
                    minesCounter: _minesweeper.MinesCounter - _minesweeper.CountFlags()
                );

                _gameBoard.Draw(_minesweeper.State);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (_isActionFirst)
                {
                    _isActionFirst = false;

                    Timer.instance.BeginTimer();
                }

                _minesweeper.Open(x: cellPosition.x, y: cellPosition.y);

                _gameBoard.Draw(_minesweeper.State);
            }
            else if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                if (_isActionFirst)
                {
                    _isActionFirst = false;

                    Timer.instance.BeginTimer();
                }

                _minesweeper.OpenArea(x: cellPosition.x, y: cellPosition.y);

                _gameBoard.Draw(_minesweeper.State);
            }
        }
        else if (!PauseMenu.IsGamePaused)
        {
            Timer.instance.EndTimer();
        }

        if (_minesweeper.IsGameOver)
        {
            GameOverMenu.instance.GameOverShow(isGameWin: _minesweeper.IsGameWin);
        }
    }
}
