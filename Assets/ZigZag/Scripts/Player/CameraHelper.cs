using System;
using UnityEngine;
using Zenject;

namespace Game
{
  public class CameraHelper : MonoBehaviour
  {
    [Inject] private SignalBus _signal_bus;

    [SerializeField]
    private float speed;

    private Transform _target;

    private void Start()
    {
      _target = transform.parent;
      if (!_target)
      {
        throw new ArgumentException($"Cant get target for CameraHelper {gameObject.name}");
      }
      transform.parent = null;
      _signal_bus.Subscribe<GameOverSignal>(_on_game_over);
    }

    private void _on_game_over()
    {
      _signal_bus.Unsubscribe<GameOverSignal>(_on_game_over);
      Destroy(this.gameObject);
    }

    private void Update()
    {
      if (_target)
      {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
      }
    }
  }
}
