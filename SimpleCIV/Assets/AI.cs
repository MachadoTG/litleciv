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

    public AI(Player p, int Agression)
    {
        reverseDic = new Dictionary<AdvancedTile, Vector3>();
        this.Agression = Agression;
        mySelf = p;
    }
    public void PlayTurn()
    {
        if (map == null)
        {
            map = GameObject.FindObjectOfType<MapController>();
            advancedTiles = map.getAdvancedTiles();

            foreach (var v in advancedTiles)
            {
                reverseDic.Add(v.Value, v.Key);
            }
        }

        List<AdvancedTile> owned = new List<AdvancedTile>(mySelf.tilesOwned);
        for (int i = owned.Count; i >= 0; i--)
        {
            if ((mySelf.tilesOwned.Count / 10) > mySelf.castles && mySelf.money > Buildables.Castle.cost)
                BuildSpecific(new List<AdvancedTile>(owned), new Buildables.Castle());
            if ((int)(mySelf.tilesOwned.Count / 5) > mySelf.villages && mySelf.money > Buildables.Village.cost)
                BuildSpecific(new List<AdvancedTile>(owned), new Buildables.Village());
            if ((int)(mySelf.tilesOwned.Count / 5) > mySelf.farms && mySelf.money > Buildables.Farm.cost)
                BuildSpecific(new List<AdvancedTile>(owned), new Buildables.Farm());

            if (mySelf.castles > mySelf.castlesUsed && mySelf.money > Buildables.Duke.cost)
                BuildSpecific(new List<AdvancedTile>(owned), new Buildables.Duke());
            if (mySelf.villages > mySelf.villagesUsed && mySelf.money > Buildables.Knight.cost)
                BuildSpecific(new List<AdvancedTile>(owned), new Buildables.Knight());
            if (mySelf.farms > mySelf.farmsUsed && mySelf.money > Buildables.Peasant.cost)
                BuildSpecific(new List<AdvancedTile>(owned), new Buildables.Peasant());
        }
        List<AdvancedTile> units = new List<AdvancedTile>(mySelf.units);
        for (int i = units.Count - 1; i >= 0; i--)
        {
            AdvancedTile tile = units[i];
            if (tile.GetBuildable().isMovable())
                AtackRandom(tile);

        }

        map.NextPlayer();
    }
    private void BuildSpecific(List<AdvancedTile> owned, Buildables b)
    {
        while (owned.Count != 0)
        {
            int index = Random.Range(0, owned.Count - 1);
            AdvancedTile tile = owned[index];
            owned.Remove(tile);
            if (!(tile.GetBuildable() is Buildables.Empty))
            {
                if (b.isMovable())
                    if (BuildAgressive(tile, b))
                        return;
            }
            else
            if (mySelf.money >= b.GetCost())
            {
                if (mySelf.Build(b))
                {
                    Debug.Log("build");
                    tile.Build(b);
                    new WaitForSeconds(1f);
                    return;
                }
            }
        }
    }
    private bool BuildAgressive(AdvancedTile t, Buildables b)
    {
        List<AdvancedTile> adjacent = GetAdjacentTiles(map.ToVectorInt(reverseDic[t]),mySelf);
        while (adjacent.Count != 0)
        {
            int index = Random.Range(0, adjacent.Count - 1);
            AdvancedTile tile = adjacent[index];
            adjacent.Remove(tile);
            if (!(tile.GetBuildable() is Buildables.Empty))
                continue;
            if (mySelf.money >= b.GetCost())
            {
                if (mySelf.Build(b))
                {
                    Debug.Log("Agressive");
                    tile.ChangeOwner(mySelf);
                    tile.Build(b);
                    tile.GetBuildable().moved = true;
                    new WaitForSeconds(1f);
                    return true;
                }
            }
        }
        return false;
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
                                    Debug.Log("Atk");
                                    t.ChangeOwner(mySelf);
                                    t.Build(unit.GetBuildable());
                                    t.GetBuildable().moved = true;
                                    unit.Build(new Buildables.Empty());
                                    new WaitForSeconds(1f);
                                    return;
                                }
                    }
                }
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
