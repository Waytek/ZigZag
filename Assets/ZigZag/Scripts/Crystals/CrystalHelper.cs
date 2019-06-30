using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
  public class CrystalHelper : MonoBehaviour, IPoolable<TileHelper, IMemoryPool>
  {
    [Inject] private SignalBus _signal_bus;

    private IMemoryPool _pool;
    private TileHelper _tile_helper;

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<PlayerHelper>() != null)
      {
        _signal_bus.Fire<CrystalPickUpSignal>();
        Destroy();
      }
    }                                                                                                                                                         

    public void OnDespawned()
    {
      _pool = null;
      _tile_helper = null;
    }

    public void OnSpawned(TileHelper tileToSpawn, IMemoryPool pool)
    {
      transform.localScale = Vector3.one;
      _pool = pool;
      _tile_helper = tileToSpawn;
      transform.position = tileToSpawn.transform.position;
      transform.parent = tileToSpawn.transform;
      tileToSpawn.Crystal = this;
    }

    public void Destroy()
    {
      _tile_helper.Crystal = null;
      _pool.Despawn(this);
      
    }

    [Serializable]
    public class Settings
    {
      public CrystalHelper Prefab;
    }

    public class Factory : PlaceholderFactory<TileHelper, CrystalHelper> { }
  }
}
