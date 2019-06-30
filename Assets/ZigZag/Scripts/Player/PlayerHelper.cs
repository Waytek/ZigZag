using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
  public class PlayerHelper : MonoBehaviour
  {
    [Inject] private SignalBus _signal_bus;
    [Inject] private Settings _settings;
        
    private bool _forward = true;
    private CoroutineHandle move_handle;
    private Rigidbody _rigidbody;

    [Inject]
    public void Construct()
    {
      _signal_bus.Subscribe<GameStartSignal>(_on_game_started);
      _rigidbody = GetComponent<Rigidbody>();
      if (!_rigidbody)
      {
        throw new Exception($"Cant find Rigidbody component on PLayerHelper in construct");
      }
    }

    private void _on_game_started()
    {
      _signal_bus.Subscribe<Touch>(_on_touch);
      _signal_bus.Subscribe<GameOverSignal>(_on_game_over);
      move_handle = Timing.RunCoroutine(_move_coroutine());
    }

    private void _on_game_over()
    {
      _signal_bus.Unsubscribe<Touch>(_on_touch);
      _signal_bus.Unsubscribe<GameOverSignal>(_on_game_over);
      Timing.KillCoroutines(move_handle);
      Destroy(this.gameObject);
    }

    private void _on_touch()
    {
      _forward = !_forward;
    }

    private IEnumerator<float> _move_coroutine()
    {
      while (true)
      {
        _rigidbody.MovePosition(_forward ?
          transform.position + Vector3.forward * _settings.Speed * Time.deltaTime :
          transform.position + Vector3.right * _settings.Speed * Time.deltaTime);
        if (transform.position.y < -5)
        {
          _signal_bus.Fire<GameOverSignal>();
        }

        yield return Timing.WaitForOneFrame;
      }
    }

    private void OnDestroy()
    {
      if (Application.isPlaying)
      {
        _signal_bus.Unsubscribe<GameStartSignal>(_on_game_started);
      }
    }

    [Serializable]
    public class Settings
    {
      public PlayerHelper Prefab;
      public float Speed = 1f;
    }

    public class Factory : PlaceholderFactory<PlayerHelper> { }
  }
}