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

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<PlayerHelper>() != null)
      {
        _signal_bus.Fire<CrystalPickUpSignal>();
        _pool.Despawn(this);
      }
    }                                                                                                                                                         

    public void OnDespawned()
    {
      _pool = null;      
    }

    public void OnSpawned(TileHelper tileToSpawn, IMemoryPool pool)
    {
      _pool = pool;
      transform.position = tileToSpawn.transform.position;
      transform.parent = tileToSpawn.transform;
      tileToSpawn.Crystal = this;
    }

    public void Destroy()
    {
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
