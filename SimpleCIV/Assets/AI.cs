using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private Player mySelf;

    public int Agression;

    private MapController map;

    private Dictionary<Vector3, AdvancedTile> advancedTiles;
    private Dictionary<AdvancedTile, Vector3> reverseDic;

    public AI(MapController map, Player p, int Agression)
    {
        reverseDic = new Dictionary<AdvancedTile, Vector3>();

        this.Agression = Agression;
        this.map = map;
        mySelf = p;
        advancedTiles = map.getAdvancedTiles();

        foreach (var v in advancedTiles)
        {
            reverseDic.Add(v.Value, v.Key);
        }
    }
    public void PlayTurn()
    {
        List<AdvancedTile> owned = new List<AdvancedTile>(mySelf.tilesOwned);
        for (int i = owned.Count - 1; i >= 0; i--)
        {
            AdvancedTile tile = owned[i];
            if (tile.GetBuildable().isMovable())
                AtackRandom(tile);
            BuildBestUnit(new List<AdvancedTile>(owned));
            BuildBestStructure(new List<AdvancedTile>(owned));
        }
        map.NextPlayer();
    }
    private void BuildBestUnit(List<AdvancedTile> owned)
    {
        while (owned.Count != 0)
        {
            int index = Random.Range(0, owned.Count - 1);
            AdvancedTile tile = owned[index];
            owned.Remove(tile);
            if (tile.GetBuildable().isMovable())
                continue;
            if (!(tile.GetBuildable() is Buildables.Empty))
                continue;
            if (mySelf.money >= Buildables.Knight.cost)
            {
                if (mySelf.Build(new Buildables.Knight()))
                    tile.Build(new Buildables.Knight());
            }
            if (mySelf.money >= Buildables.Peasant.cost)
            {
                if (mySelf.Build(new Buildables.Peasant()))
                    tile.Build(new Buildables.Peasant());
            }
        }
    }
    private void AtackRandom(AdvancedTile unit)
    {
        List<AdvancedTile> ours = new List<AdvancedTile>(mySelf.tilesOwned);
        for (int i = ours.Count - 1; i >= 0; i--)
        {
            AdvancedTile tile = ours[i];
            List<AdvancedTile> agressiveMove = GetAdjacentTiles(map.ToVectorInt(reverseDic[tile]), mySelf);

            while (unit.GetBuildable().isMovable() && agressiveMove.Count != 0)
            {
                int index = Random.Range(0, agressiveMove.Count - 1);
                AdvancedTile t = agressiveMove[index];
                agressiveMove.Remove(t);

                if (advancedTiles.ContainsValue(t))
                {
                    if (map.IsAdjscente(map.ToVectorInt(reverseDic[t])))
                    {
                        if (t.GetBuildable().GetDefenseLeve() < unit.GetBuildable().GetDefenseLeve())
                            if (map.GetProtectionLevel(map.ToVectorInt(reverseDic[t])) < unit.GetBuildable().GetDefenseLeve())
                                if (t.GetPlayer() != mySelf)
                                {
                                    t.Build(unit.GetBuildable());
                                    t.ChangeOwner(mySelf);
                                    t.GetBuildable().moved = true;
                                    unit.Build(new Buildables.Empty());
                                    return;
                                }
                    }
                }
            }
        }
    }
    private void BuildBestStructure(List<AdvancedTile> owned)
    {
        while (owned.Count != 0)
        {
            int index = Random.Range(0, owned.Count - 1);
            AdvancedTile tile = owned[index];
            owned.Remove(tile);

            if (tile.GetBuildable().isMovable())
                continue;
            if (!(tile.GetBuildable() is Buildables.Empty))
                continue;
            if (mySelf.money >= Buildables.Village.cost)
            {
                if (mySelf.Build(new Buildables.Village()))
                    tile.Build(new Buildables.Village());
            }
            if (mySelf.money >= Buildables.Farm.cost)
            {
                if (mySelf.Build(new Buildables.Farm()))
                    tile.Build(new Buildables.Farm());
            }
        }
    }
    private List<AdvancedTile> GetAdjacentTiles(Vector3Int coordinate, Player playing)
    {
        List<AdvancedTile> resultSet = new List<AdvancedTile>();
        int x = coordinate.x;
        int y = coordinate.y;
        int z = coordinate.z;

        if (y % 2 == 0)
        {
            Vector3Int pos = new Vector3Int(x + 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x - 1, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x - 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x - 1, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
        }
        else
        {
            Vector3Int pos = new Vector3Int(x + 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x + 1, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x + 1, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
            pos = new Vector3Int(x - 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (GetTileIfDiferentOwner(pos, playing) != null)
                    resultSet.Add(GetTileIfDiferentOwner(pos, playing));
            }
        }
        return resultSet;
    }
    private AdvancedTile GetTileIfDiferentOwner(Vector3Int v, Player p)
    {
        return advancedTiles[v];
    }

}
