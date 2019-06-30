using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
  public class TileHelper : MonoBehaviour, IPoolable<Vector3, int, TileHelper.Directions, IMemoryPool>
  {
    public enum Directions
    {
      Forward,
      Right
    }

    [Inject] private Settings _settings;

    [SerializeField]
    private Transform spawnPoint;

    public int Widt { get; private set; }
    public Directions Direction { get; private set; }

    public CrystalHelper Crystal;

    private IMemoryPool _pool;

    public void Destroy()
    {
      Timing.RunCoroutine(_despawn_coroutine());
      if (Crystal)
      {
        Crystal.Destroy();
      }
    }

    public void OnSpawned(Vector3 position, int width, Directions direction, IMemoryPool pool)
    {
      transform.position = position;
      _pool = pool;
      Widt = width;
      Direction = direction;
      transform.localScale = new Vector3(width, _settings.Height, _settings.Height);
      switch (direction)
      {
        case Directions.Forward:
          transform.rotation = Quaternion.Euler(_settings.ForwardRotation);
          break;
        case Directions.Right:
          transform.rotation = Quaternion.Euler(_settings.RightRotation);
          break;
        default:
          throw new ArgumentException($"On spawned TileHelper with direction {direction} used default");
      }
    }

    public void OnDespawned()
    {
      _pool = null;
      Crystal = null;
    }

    private IEnumerator<float> _despawn_coroutine()
    {
      float time = 0;
      while (time < _settings.DestroyTime)
      {
        transform.position -= transform.up * _settings.DestroyAnimationSpeed * Time.deltaTime;
        time += Time.deltaTime;
        yield return Timing.WaitForOneFrame;
      }
      _pool.Despawn(this);
    }

    [Serializable]
    public class Settings
    {
      public TileHelper Prefab;
      public float Height = 1f;
      public Vector3 ForwardRotation = new Vector3(0, 0, 0);
      public Vector3 RightRotation = new Vector3(90, 0, 0);
      public float DestroyTime = 3f;
      public float DestroyAnimationSpeed = 5f;
    }

    public class Factory : PlaceholderFactory<Vector3, int, Directions, TileHelper> { }
  }
}