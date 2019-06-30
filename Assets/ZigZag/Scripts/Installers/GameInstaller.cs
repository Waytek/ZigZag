using UnityEngine;
using Zenject;

namespace Game
{
  // Main installer for our game
  public class GameInstaller : MonoInstaller
    {
        [Inject] private TileHelper.Settings _tile_settings;

    public override void InstallBindings()
    {
      GameSignalsInstaller.Install(Container);

      Container.BindInterfacesAndSelfTo<TileSpawnManager>().AsSingle();
      Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
      


      Container.BindFactory<Vector3, int, TileHelper.Directions, TileHelper, TileHelper.Factory>()
          .FromPoolableMemoryPool<Vector3, int, TileHelper.Directions, TileHelper, TileHelperPool>(TileBinder => TileBinder
              .WithInitialSize(10)
              .FromComponentInNewPrefab(_tile_settings.Prefab)
              .UnderTransformGroup("MapTiles"));

      
    }

    class TileHelperPool : MonoPoolableMemoryPool<Vector3, int, TileHelper.Directions, IMemoryPool, TileHelper>
    {
    }
  }
}
