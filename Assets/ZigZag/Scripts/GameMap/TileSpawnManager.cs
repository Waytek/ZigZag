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
    private List<TileHelper> _spawn_place_tiles = new List<TileHelper>();
    private CoroutineHandle _spawn_coroutine_handle;
    private System.Random random = new System.Random();

    public void Initialize()
    {
      _signal_bus.Subscribe<GameStartSignal>(_on_game_started);
      _signal_bus.Subscribe<GameOverSignal>(_on_game_over);
      _spawn_start_place();
    }

    private void _on_game_started()
    {
      _spawn_coroutine_handle = Timing.RunCoroutine(_spawn_coroutine());
    }

    private void _on_game_over()
    {
      Timing.KillCoroutines(_spawn_coroutine_handle);
      _remove_all_tiles();
      _spawn_start_place();
    }

    private IEnumerator<float> _spawn_coroutine()
    {
      while (true)
      {        
        _spawn_tile();
        yield return Timing.WaitForSeconds(_settings.SpawnTime);
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
          spawnPos = _spawn_place_tiles[i - 1].transform.position + new Vector3(0,0, _tile_settings.Height);
        }
        TileHelper spawnedTile = _tile_factory.Create(spawnPos, _settings.StartTileWidth, TileHelper.Directions.Forward);
        _spawn_place_tiles.Add(spawnedTile);
      }

      _tiles.Add(_spawn_place_tiles[_spawn_place_tiles.Count - 1]);
      _spawn_place_tiles.Remove(_spawn_place_tiles[_spawn_place_tiles.Count - 1]);

      for(int i=0; i< _settings.SpawnOnStart; i++)
      {
        _spawn_tile();
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
      for (int i = _tiles.Count - 1; i >= 0; i--)
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
        _spawn_place_tiles.ForEach(tile => tile.Destroy());
        _spawn_place_tiles.Clear();
      }

      _signal_bus.Fire(new TileSpawnSignal() { Tile = spawnedTile });
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
      public int SpawnOnStart = 10;
      public int SpawnedLenght = 20;
      public float SpawnTime = 1f;
    }
  }
}
