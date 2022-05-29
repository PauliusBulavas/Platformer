using Platformer.Models;

public class Game
{
    private readonly string _gameFile;
    private Platform? _activePlatform;

    private int _maxPlatform;
    private int _points = 500;
    private int _totalJumps;

    public Game(string gameFile)
    {
        _gameFile = gameFile;
    }

    public void Run()
    {
        // TODO: Implement your mighty algorithm and jump to oblivion.

        var platforms = ReadPlatforms();

        _activePlatform = platforms.First();

        while (_activePlatform.Index < platforms.Count)
        {
            var nextPlatform = FindNextPlatform(platforms);
            JumpTo(nextPlatform);
        }
    }

    /// <summary>
    ///     Reads platforms from csv file and returns the as list.
    /// </summary>
    /// <returns>Platforms as list</returns>
    private List<Platform> ReadPlatforms()
    {
        var platforms = File.ReadAllLines(
                _gameFile)
            .Skip(1)
            .Select(v => FromCsv(v))
            .ToList();
        return platforms;
    }

    private static Platform FromCsv(string csvLine)
    {
        var platforms = csvLine.Split(',');
        var platform = new Platform
        {
            Index = int.Parse(platforms[0]),
            Cost = int.Parse(platforms[1])
        };
        return platform;
    }

    /// <summary>
    ///     Invoke this function to jump to next platform.
    /// </summary>
    /// <param name="platform">Platform that you are going to jump to.</param>
    public void JumpTo(Platform platform)
    {
        _activePlatform = platform;
    }

    private Platform FindNextPlatform(List<Platform> platforms)
    {
        var index = platforms.IndexOf(_activePlatform);

        if (platforms.Last() == _activePlatform)
        {
            Console.WriteLine($"Congratulations! It took {_totalJumps} jumps to reach the last platform.");
            Console.ReadLine();
            Environment.Exit(1);
        }

        var next = platforms[index + 1];

        switch (CanJump(platforms))
        {
            case true:

                if (_maxPlatform > _activePlatform.Index) JumpUpIfUnlocked(platforms);
                if (_points >= next.Cost) JumpUpIfNotUnlocked(platforms);

                return _activePlatform;

            case false:

                return JumpDown(platforms);
        }
    }

    private bool CanJump(List<Platform> platforms)
    {
        var index = platforms.IndexOf(_activePlatform);
        var next = platforms[index + 1];

        if (_points >= next.Cost || _maxPlatform > _activePlatform.Index) return true;
        return false;
    }

    private Platform JumpUpIfNotUnlocked(List<Platform> platforms)
    {
        var index = platforms.IndexOf(_activePlatform);

        _activePlatform = platforms[index + 1];
        _points -= _activePlatform.Cost;
        _maxPlatform++;
        _totalJumps++;
        return _activePlatform;
    }

    private Platform JumpUpIfUnlocked(List<Platform> platforms)
    {
        var index = platforms.IndexOf(_activePlatform);

        _activePlatform = platforms[index + 1];
        _points += _activePlatform.Cost;
        _totalJumps++;
        return _activePlatform;
    }

    private Platform JumpDown(List<Platform> platforms)
    {
        var index = platforms.IndexOf(_activePlatform);

        _activePlatform = platforms[index - 1];
        _points += _activePlatform.Cost;
        _totalJumps++;
        return _activePlatform;
    }
}