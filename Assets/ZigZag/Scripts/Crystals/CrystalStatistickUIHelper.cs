using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
  public class CrystalStatistickUIHelper : MonoBehaviour
  {
    [Inject] private SignalBus _signal_bus;
    [Inject] private GameManager _game_manager;

    [SerializeField]
    private string strPattern = "Score {0}";

    private Text _score;

    private void Start()
    {
      _score = GetComponent<Text>();
      if (!_score)
      {
        throw new System.ArgumentException($"Cant find Text component on CrystalStatistyckUIHelper {gameObject.name}");
      }
      _signal_bus.Subscribe<CrystalPickUpSignal>(_on_crystal_pick_up);
      _update_score();
    }

    private void _on_crystal_pick_up()
    {
      _update_score();
    }

    private void _update_score()
    {
      _score.text = string.Format(strPattern, _game_manager.CurrentScore);
    }
  }
}
