using UnityEngine;
using Zenject;

namespace Game
{
  // Main installer for our game
  public class GameInstaller : MonoInstaller
  {
    [Inject] private TileHelper.Settings _tile_settings;
    [Inject] private CrystalHelper.Settings _crystal_settings;
    [Inject] private PlayerHelper.Settings _player_settings;

    public override void InstallBindings()
    {
      GameSignalsInstaller.Install(Container);

      Container.BindInterfacesAndSelfTo<CrystalSpawnManager>().AsSingle();
      Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
      Container.BindInterfacesAndSelfTo<TileSpawnManager>().AsSingle();
      
      

      Container.BindFactory<Vector3, int, TileHelper.Directions, TileHelper, TileHelper.Factory>()
          .FromPoolableMemoryPool<Vector3, int, TileHelper.Directions, TileHelper, TileHelperPool>(TileBinder => TileBinder
              .WithInitialSize(10)
              .FromComponentInNewPrefab(_tile_settings.Prefab)
              .UnderTransformGroup("MapTiles"));

      Container.BindFactory<TileHelper, CrystalHelper, CrystalHelper.Factory>()
          .FromPoolableMemoryPool<TileHelper, CrystalHelper, CrystalHelperPool>(CrystalBinder => CrystalBinder
              .WithInitialSize(10)
              .FromComponentInNewPrefab(_crystal_settings.Prefab));

      Container.BindFactory<PlayerHelper, PlayerHelper.Factory>()
        .FromComponentInNewPrefab(_player_settings.Prefab);
    }

    class TileHelperPool : MonoPoolableMemoryPool<Vector3, int, TileHelper.Directions, IMemoryPool, TileHelper> { }
    class CrystalHelperPool : MonoPoolableMemoryPool<TileHelper, IMemoryPool, CrystalHelper> { }
  }
}
