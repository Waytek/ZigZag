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
      Container.DeclareSignal<GameOverSignal>();
      Container.DeclareSignal<TileSpawnSignal>();
      Container.DeclareSignal<CrystalPickUpSignal>();
      Container.DeclareSignal<Touch>();
      
    }
  }
}
