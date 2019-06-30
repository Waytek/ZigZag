using UnityEngine;
using Zenject;

namespace Game
{
  public class GameSignalsInstaller : Installer<GameSignalsInstaller>
  {
    public override void InstallBindings()
    {
      SignalBusInstaller.Install(Container);

      Container.DeclareSignal<GameStartSignal>();
      Container.DeclareSignal<GameEndSignal>();
      Container.DeclareSignal<TileSpawnSignal>();
      Container.DeclareSignal<CrystalPickUpSignal>();

      Container.BindSignal<GameStartSignal>().ToMethod(() => Debug.Log("GameStartSignal Fire"));
      Container.BindSignal<GameEndSignal>().ToMethod(() => Debug.Log("GameEndSignal Fire"));
      Container.BindSignal<CrystalPickUpSignal>().ToMethod(() => Debug.Log("CrystalPickUpSignal Fire"));
      
    }
  }
}
