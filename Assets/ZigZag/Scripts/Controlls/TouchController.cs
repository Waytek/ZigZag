using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game
{
  public class TouchController : MonoBehaviour, IPointerDownHandler
  {
    [Inject] private SignalBus _signal_bus;

    public void OnPointerDown(PointerEventData eventData)
    {
      _signal_bus.Fire<Touch>();
    }
  }
}
