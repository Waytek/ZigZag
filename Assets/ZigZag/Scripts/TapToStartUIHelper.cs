using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game {
  public class TapToStartUIHelper : MonoBehaviour
  {
    [Inject] private SignalBus _signal_bus;

    private void Start()
    {
      _signal_bus.Subscribe<GameStartSignal>(_on_game_start);
      _signal_bus.Subscribe<GameOverSignal>(_on_game_over);
    }

    private void _on_game_start()
    {
      gameObject.SetActive(false);
    }

    private void _on_game_over()
    {
      gameObject.SetActive(true);
    }
  }
}