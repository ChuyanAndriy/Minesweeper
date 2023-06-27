using NUnit.Framework;

public class Test
{
    [Test]
    public void GameOverTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 1);
        game.NewGame();

        game.Open(x: 0, y: 0);

        Assert.AreEqual(game.IsGameOver, true);
    }

    [Test]
    public void GameOverLoseTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 1);
        game.NewGame();

        game.Open(x: 0, y: 0);

        Assert.AreEqual(game.IsGameWin, false);
    }

    [Test]
    public void WinGameTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 0);
        game.NewGame();

        game.Open(x: 0, y: 0);

        Assert.AreEqual(game.IsGameWin, true);
    }

    [Test]
    public void WinGameOverTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 0);
        game.NewGame();

        game.Open(x: 0, y: 0);

        Assert.AreEqual(game.IsGameOver, true);
    }

    [Test]
    public void OpenNeighboursTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 9, height: 9, minesCounter: 0);
        game.NewGame();

        game.State[5, 5].type = Cell.Type.Mine;

        game.Open(x: 0, y: 0);

        int counter = 0;
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (game.State[x, y].IsOpend)
                {
                    counter++;
                }
            }
        }

        Assert.AreEqual(counter, 80);
    }

    [Test]
    public void NewGameTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 9, height: 9, minesCounter: 1);
        game.NewGame();

        bool check = true;
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (game.State[x, y].type == Cell.Type.Invalid)
                {
                    check = false; 
                    break;
                }
            }
        }

        Assert.AreEqual(check, true);
    }

    [Test]
    public void OpenEmptyTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 0);
        game.NewGame();

        game.Open(x: 0, y: 0);

        Assert.AreEqual(game.State[0, 0].IsOpend, true);
    }

    [Test]
    public void OpenMineTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 1);
        game.NewGame();

        game.Open(x: 0, y: 0);

        Assert.AreEqual(game.State[0, 0].IsExploded, true);
    }

    [Test]
    public void OpenFlaggedTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 0);
        game.NewGame();

        game.Flag(x: 0, y: 0);

        game.Open(x: 0, y: 0);

        Assert.AreEqual(game.State[0, 0].IsOpend, false);
    }

    [Test]
    public void PutFlagTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 0);
        game.NewGame();

        game.Flag(x: 0, y: 0);

        Assert.AreEqual(game.State[0, 0].IsFlagged, true);
    }

    [Test]
    public void RemoveFlagTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 1, height: 1, minesCounter: 0);
        game.NewGame();

        game.Open(x: 0, y: 0);

        game.Flag(x: 0, y: 0);
        game.Flag(x: 0, y: 0);

        Assert.AreEqual(game.State[0, 0].IsFlagged, false);
    }

    [Test]
    public void CountFlagsTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 9, height: 9, minesCounter: 0);
        game.NewGame();

        game.Flag(x: 0, y: 0);
        game.Flag(x: 1, y: 0);
        game.Flag(x: 2, y: 0);

        int counter = game.CountFlags();

        Assert.AreEqual(counter, 3);
    }

    [Test]
    public void ApplySettingsTest()
    {
        Game game = new Game();

        game.ApplySettings(width: 100, height: 100, minesCounter: 54);

        int width = game.Width;

        Assert.AreEqual(width, 100);
    }

    [Test]
    public void InvalidValuesApplySettingsTest()
    {
        Game game = new Game();

        game.ApplySettings(width: -100, height: 100, minesCounter: 54);

        int width = game.Width;

        Assert.AreEqual(width, 9);
    }
}
