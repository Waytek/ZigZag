using System;
using UnityEngine;
using Zenject;

namespace Game
{
  public class GameManager : IInitializable, ITickable
  {
    [Inject] private Settings _settings;
    [Inject] private SignalBus _signal_bus;
    [Inject] private PlayerHelper.Factory _player_factory;

    public Difficulty CurrentDifficulty
    {
      get
      {
        int difficulties = Mathf.Clamp((int)_settings._Difficulties_Curve.Evaluate(_game_time), 0, _settings.Difficulties.Length - 1);
        return _settings.Difficulties[difficulties];
      }
    }

    public int CurrentScore{ get; private set;}

    private float _game_time = 0f;

    public void Initialize()
    {
      _signal_bus.Subscribe<GameOverSignal>(_on_game_end);
      _signal_bus.Subscribe<CrystalPickUpSignal>(() => CurrentScore++);
      _player_factory.Create();
      _signal_bus.Subscribe<Touch>(_on_touch);
    }

    private void _on_touch()
    {
      _signal_bus.Unsubscribe<Touch>(_on_touch);
      _signal_bus.Fire<GameStartSignal>();
      _game_time = 0f;
    }

    private void _on_game_end()
    {
      _player_factory.Create();
      _signal_bus.Subscribe<Touch>(_on_touch);
      CurrentScore = 0;
      _game_time = 0f;
    }

    public void Tick()
    {
      _game_time += Time.deltaTime;
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
      public AnimationCurve _Difficulties_Curve;
    }
  }
}