using UnityEngine;

public class GameController : MonoBehaviour
{
    private int _width = Settings.Width;
    private int _height = Settings.Height;
    private int _minesCounter = Settings.MinesCounter;
    private Game _minesweeper = new Game();
    private GameBoard _gameBoard;
    private Timer _timer;
    private MinesCounterScript _minesCounterScript;
    private GameOverMenu _gameOverMenu;
    private bool _isActionFirst = true;

    private void OnValidate()
    {
        _minesCounter = Mathf.Clamp(_minesCounter, 0, _width * _height);
    }

    private void Awake()
    {
        _gameBoard = GetComponentInChildren<GameBoard>();
        _timer = GetComponentInChildren<Timer>();
        _minesCounterScript = GetComponentInChildren<MinesCounterScript>();
        _gameOverMenu = GetComponentInChildren<GameOverMenu>();
    }

    private void Start()
    {
        _minesweeper.ApplySettings(width: _width, height: _height, minesCounter: _minesCounter);

        _minesweeper.NewGame();

        _gameOverMenu.GameOverHide();

        _minesCounterScript.SetMinesCounterText(
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

            _timer.EndTimer();
        }
        else if (!_minesweeper.IsGameOver && !PauseMenu.IsGamePaused)
        {
            Vector3Int cellPosition = _gameBoard.GetСoordinates();

            if (Input.GetMouseButtonDown(1))
            {
                _minesweeper.Flag(x: cellPosition.x, y: cellPosition.y);

                _minesCounterScript.SetMinesCounterText(
                    minesCounter: _minesweeper.MinesCounter - _minesweeper.CountFlags()
                );

                _gameBoard.Draw(_minesweeper.State);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (_isActionFirst)
                {
                    _isActionFirst = false;

                    _timer.BeginTimer();
                }

                _minesweeper.Open(x: cellPosition.x, y: cellPosition.y);

                _gameBoard.Draw(_minesweeper.State);
            }
            else if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                if (_isActionFirst)
                {
                    _isActionFirst = false;

                    _timer.BeginTimer();
                }

                _minesweeper.OpenArea(x: cellPosition.x, y: cellPosition.y);

                _gameBoard.Draw(_minesweeper.State);
            }
        }
        else if (!PauseMenu.IsGamePaused)
        {
            _timer.EndTimer();
        }

        if (_minesweeper.IsGameOver)
        {
            _gameOverMenu.GameOverShow(isGameWin: _minesweeper.IsGameWin);
        }
    }
}
