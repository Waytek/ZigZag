namespace Game
{

  public struct GameStartSignal { }

  public struct GameOverSignal { }

  public struct TileSpawnSignal
  {
    public TileHelper Tile;
  }

  public struct CrystalPickUpSignal { }

  public struct Touch { }

}
