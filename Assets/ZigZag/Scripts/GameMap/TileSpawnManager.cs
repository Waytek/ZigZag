using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
  public class TileSpawnManager : IInitializable
  {
    [Inject] private TileHelper.Factory _tile_factory;
    [Inject] private GameManager _game_manager;
    [Inject] private Settings _settings;
    [Inject] private TileHelper.Settings _tile_settings;
    [Inject] private SignalBus _signal_bus;

    private List<TileHelper> _tiles = new List<TileHelper>();
    private CoroutineHandle _spawn_coroutine_handle;

    public void Initialize()
    {
      _signal_bus.Subscribe<GameStartSignal>(_on_game_started);
      _signal_bus.Subscribe<GameEndSignal>(_on_game_end);
    }

    private void _on_game_started()
    {
      _remove_all_tiles();
      _spawn_start_place();
      _spawn_coroutine_handle = Timing.RunCoroutine(_spawn_coroutine());
    }

    private void _on_game_end()
    {
      _remove_all_tiles();
      Timing.KillCoroutines(_spawn_coroutine_handle);
    }

    private IEnumerator<float> _spawn_coroutine()
    {
      while (true)
      {
        yield return Timing.WaitForSeconds(_settings.SpawnTime);
        _spawn_tile();
      }
    }

    private void _spawn_start_place()
    {
      for (int i = 0; i < _settings.StartTileHeight; i++)
      {
        Vector3 spawnPos;
        if (i == 0)
        {
          spawnPos = Vector3.zero;
        }
        else
        {
          spawnPos = _tiles[i - 1].transform.position + new Vector3(0,0, _tile_settings.Height);
        }
        TileHelper spawnedTile = _tile_factory.Create(spawnPos, _settings.StartTileWidth, TileHelper.Directions.Forward);
        _tiles.Add(spawnedTile);
      }
    }

    private void _spawn_tile()
    {
      TileHelper tileToSpawn = _tiles[_tiles.Count - 1];
      TileHelper.Directions direcrtion = tileToSpawn.Direction;
      int width = _game_manager.CurrentDifficulty.TileWidth;
      float spawnPointMoveFactor = _tile_settings.Height;
      float sameRowMoveFactor = 0f;

      int sameRow = 0;
      for (int i = _tiles.Count - 1 - _settings.StartTileHeight; i > 0; i--)
      {
        if (_tiles[i].Direction == tileToSpawn.Direction)
        {
          sameRow++;
        }
        else
        {
          break;
        }
      }

      if (sameRow > width)
      {
        Array values = Enum.GetValues(typeof(TileHelper.Directions));
        System.Random random = new System.Random();
        direcrtion = (TileHelper.Directions)values.GetValue(random.Next(values.Length));
        if (direcrtion != tileToSpawn.Direction)
        {
          spawnPointMoveFactor = _tile_settings.Height + (_tile_settings.Height / 2 * (tileToSpawn.Widt - 1));
          sameRowMoveFactor -= _tile_settings.Height / 2 * (width - 1);
        }
      }

      Vector3 spawnPosition;
      switch (direcrtion)
      {
        case TileHelper.Directions.Forward:
          spawnPosition = tileToSpawn.transform.position + new Vector3(sameRowMoveFactor, 0, spawnPointMoveFactor);
          break;
        case TileHelper.Directions.Right:
          spawnPosition = tileToSpawn.transform.position + new Vector3(spawnPointMoveFactor, 0, sameRowMoveFactor);
          break;
        default:
          spawnPosition = tileToSpawn.transform.position;
          break;
      }

      TileHelper spawnedTile = _tile_factory.Create(spawnPosition, _game_manager.CurrentDifficulty.TileWidth, direcrtion);
      _tiles.Add(spawnedTile);

      if (_tiles.Count > _settings.SpawnedLenght)
      {
        _tiles[0].Destroy();
        _tiles.Remove(_tiles[0]);
      }

      _signal_bus.Fire<TileSpawnSignal>();
    }

    private void _remove_all_tiles()
    {
      foreach(var tile in _tiles)
      {
        tile.Destroy();
      }
      _tiles.Clear();
    }

    [Serializable]
    public class Settings
    {
      public int StartTileWidth = 3;
      public int StartTileHeight = 3;
      public int SpawnedLenght;
      public float SpawnTime = 1f;
    }
  }
}
