using UnityEngine;
using Zenject;

namespace Game
{
  // Main installer for our game
  public class GameInstaller : MonoInstaller
  {
    [Inject] private TileHelper.Settings _tile_settings;
    [Inject] private CrystalHelper.Settings _crystal_settings;

    public override void InstallBindings()
    {
      GameSignalsInstaller.Install(Container);

      Container.BindInterfacesAndSelfTo<TileSpawnManager>().AsSingle();
      Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
      Container.BindInterfacesAndSelfTo<CrystalSpawnManager>().AsSingle();

      Container.BindFactory<Vector3, int, TileHelper.Directions, TileHelper, TileHelper.Factory>()
          .FromPoolableMemoryPool<Vector3, int, TileHelper.Directions, TileHelper, TileHelperPool>(TileBinder => TileBinder
              .WithInitialSize(10)
              .FromComponentInNewPrefab(_tile_settings.Prefab)
              .UnderTransformGroup("MapTiles"));

      Container.BindFactory<TileHelper, CrystalHelper, CrystalHelper.Factory>()
          .FromPoolableMemoryPool<TileHelper, CrystalHelper, CrystalHelperPool>(CrystalBinder => CrystalBinder
              .WithInitialSize(10)
              .FromComponentInNewPrefab(_crystal_settings.Prefab));
    }

    class TileHelperPool : MonoPoolableMemoryPool<Vector3, int, TileHelper.Directions, IMemoryPool, TileHelper> { }
    class CrystalHelperPool : MonoPoolableMemoryPool<TileHelper, IMemoryPool, CrystalHelper> { }
  }
}
