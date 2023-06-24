using UnityEngine;

public class GameController : MonoBehaviour 
{
    public int width { get; set; } = Settings.width;
    public int height { get; set; } = Settings.height;
    public int numberOfMines { get; set; } = Settings.mineCounter;

    private Game _minesweeper = new Game();
    private GameBoard _gameBoard;
    private bool _isActionFirst = true;

    private void OnValidate() {
        numberOfMines = Mathf.Clamp(numberOfMines, 0, width * height);
    }

    private void Awake() {
        _gameBoard = GetComponentInChildren<GameBoard>();
    }

    private void Start() {
        _minesweeper.ApplySettings(width: width, height: height, numberOfMines: numberOfMines);

        _minesweeper.NewGame();

        GameOverMenu.instance.GameOverHide();

        MinesCounterScript.instance.SetMinesCounterText(minesCounter: _minesweeper._numberOfMines - _minesweeper.CountFlags());

        Camera.main.transform.position = new Vector3(_minesweeper._width / 2.0f, _minesweeper._height / 2.0f, -10.0f);
        Camera.main.orthographicSize = _minesweeper._height / 2.0f;
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
        else if (!_minesweeper.IsGameOver && !PauseMenu.isGamePaused) 
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = _gameBoard.Map.WorldToCell(worldPosition);

            if (Input.GetMouseButtonDown(1)) 
            {
                _minesweeper.Flag(x: cellPosition.x, y: cellPosition.y);

                MinesCounterScript.instance.SetMinesCounterText(minesCounter: _minesweeper._numberOfMines - _minesweeper.CountFlags());

                _gameBoard.Draw(_minesweeper.State);
            } 
            else if (Input.GetMouseButtonDown(0)) 
            {
                if (_isActionFirst) {
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
        else if (!PauseMenu.isGamePaused) 
        {
            Timer.instance.EndTimer();
        }

        if (_minesweeper.IsGameOver)
        {
            GameOverMenu.instance.GameOverShow(isGameWin: _minesweeper.IsGameWin);
        }
    }
}