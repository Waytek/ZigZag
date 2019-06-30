using Zenject;

namespace Game
{

  public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
  {
    public GameManager.Settings GameManager;
    public TileHelper.Settings TileHelper;
    public TileSpawnManager.Settings TileSpawner;
    public CrystalHelper.Settings CrystalHelper;
    public CrystalSpawnManager.Settings CrystalSpawner;
    public PlayerHelper.Settings PlayerSettings;



    public override void InstallBindings()
    {
      Container.BindInstance(GameManager);
      Container.BindInstance(TileSpawner);
      Container.BindInstance(TileHelper);
      Container.BindInstance(CrystalHelper);
      Container.BindInstance(CrystalSpawner);
      Container.BindInstance(PlayerSettings);
    }
  }
}

