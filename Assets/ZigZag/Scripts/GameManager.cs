using System;
using Zenject;

namespace Game {
  public class GameManager : IInitializable
  {
    [Inject] private Settings _settings;
    [Inject] private SignalBus _signal_bus;

    public Difficulty CurrentDifficulty
    {
      // TODO
      get
      {
        return _settings.Difficulties[2];
      }
    }

    public int CurrentScore{ get; private set;}

    public void Initialize()
    {
      _signal_bus.Fire<GameStartSignal>();
      _signal_bus.Subscribe<GameEndSignal>(_on_game_end);
      _signal_bus.Subscribe<CrystalPickUpSignal>(() => CurrentScore++);
    }

    private void _on_game_end()
    {
      
    }

    [Serializable]
    public class Difficulty
    {
      public enum Difficulties
      {
        Easy,
        Medium,
        Hard
      }

      public Difficulties difficulty;
      public int TileWidth;
    }

    [Serializable]
    public class Settings
    {
      public Difficulty[] Difficulties;
    }
  }
}