using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
  public class CrystalSpawnManager : IInitializable
  {
    [Inject] private SignalBus _signal_bus;
    [Inject] private Settings _settings;
    [Inject] private CrystalHelper.Factory _crystal_factory;

    private int _tile_num = 0;
    private bool _spawn_this_order = false;

    public void Initialize()
    {
      _signal_bus.Subscribe<TileSpawnSignal>(_on_tile_spawn);
    }

    private void _on_tile_spawn(TileSpawnSignal args)
    {
      _tile_num++;

      if (_tile_num % _settings.SpawnOrder == 1)
      {
        _spawn_this_order = false;
      }

      switch (_settings.SpawnType)
      {
        case Settings.SpawnTypes.Random:
          if (_check_random_spawn())
          {
            _spawn_crystal(args.Tile);
            _spawn_this_order = true;
          }
          break;
        case Settings.SpawnTypes.InOrder:
          if(_tile_num% _settings.SpawnOrder == 1)
          {
            _spawn_crystal(args.Tile);
            _spawn_this_order = true;
          }
          break;
        default:
          throw new ArgumentException($"On spawned CrystalHelper with spawn type {_settings.SpawnType} not processed, default is empty");
      }
    }

    private bool _check_random_spawn()
    {
      if (_spawn_this_order)
      {
        return false;
      }
      
      if(_tile_num % _settings.SpawnOrder == 0)
      {
        return true;
      }

      var random = new System.Random();
      if (random.Next(100) < 100 / _settings.SpawnOrder)
      {
        return true;
      }

      return false;
    }

    private void _spawn_crystal(TileHelper tileToSpawn)
    {
      _crystal_factory.Create(tileToSpawn);
    }

    [Serializable]
    public class Settings
    {
      public enum SpawnTypes
      {
        Random,
        InOrder
      }

      public SpawnTypes SpawnType;
      public int SpawnOrder = 5;
    }
  }
}
