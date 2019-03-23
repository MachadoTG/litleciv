using System;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public string nome;
    public Color color;

    public float money;
    public float income;

    public int farms;
    public int villages;
    public int castles;

    public int farmsUsed;
    public int villagesUsed;
    public int castlesUsed;

    public List<AdvancedTile> tilesOwned;
    public List<AdvancedTile> units;

    public AI myBrains;

    public Player(string s, float f, Color c, bool AI)
    {
        if (AI)
        {
            myBrains = new AI(this, 0);
        }
        nome = s;
        color = c;
        money = f;
        tilesOwned = new List<AdvancedTile>();
        units = new List<AdvancedTile>();
    }

    public void DoIncome()
    {
        income = 0;
        tilesOwned.ForEach(t => {
            income += t.GetBuildable().GetIncome();
            income -= t.GetBuildable().GetUpkeep();
        });
    }
    public bool Build(Buildables b)
    {
        Debug.Log(nome);
        Debug.Log(b.GetType().Name);
        Debug.Log(tilesOwned.Count);
        if (b is Buildables.Farm && money >= b.GetCost())
        {
            farms += 1;
            money -= b.GetCost();
            return true;
        }
        if (b is Buildables.Village && money >= b.GetCost())
        {
            villages += 1;
            money -= b.GetCost();
            return true;
        }
        if (b is Buildables.Castle && money >= b.GetCost())
        {
            castles += 1;
            money -= b.GetCost();
            return true;
        }
        if (b is Buildables.Peasant && money >= b.GetCost())
            if (farmsUsed < farms)
            {
                Debug.Log("FARMUSED");
                farmsUsed += 1;
                money -= b.GetCost();
                return true;
            }
        if (b is Buildables.Knight && money >= b.GetCost())
            if (villagesUsed < villages)
            {
                Debug.Log("VillageUsed");
                villagesUsed += 1;
                money -= b.GetCost();
                return true;
            }
        if (b is Buildables.Duke)
            if (castlesUsed < castles && money >= b.GetCost())
            {
                Debug.Log("CastleUsed");
                castlesUsed += 1;
                money -= b.GetCost();
                return true;
            }
        return false;
    }
    public bool RemoveBuild(Buildables b)
    {
        if (b is Buildables.Farm)
        {
            farms -= 1;
            return true;
        }
        if (b is Buildables.Village)
        {
            villages -= 1;
            return true;
        }
        if (b is Buildables.Castle)
        {
            castles -= 1;
            return true;
        }
        if (b is Buildables.Peasant)
        {
            farmsUsed -= 1;
            return true;
        }
        if (b is Buildables.Knight)
        {
            villagesUsed -= 1;
            return true;
        }
        if (b is Buildables.Duke)
        {
            castlesUsed -= 1;
            return true;
        }
        return false;
    }
    public void AddAdvancedTile(AdvancedTile t)
    {
        tilesOwned.Add(t);
        DoIncome();
    }
    public void RemoveAdvancedTile(AdvancedTile t)
    {
        tilesOwned.Remove(t);
        RemoveBuild(t.GetBuildable());
        DoIncome();
    }
    public void NexTurn()
    {
        units.Clear();
        tilesOwned.ForEach(tile => {
            if (tile.GetBuildable().isMovable())
            {
                tile.GetBuildable().moved = false;
                units.Add(tile);
            }
        });
        DoIncome();
        money += income;
        if (money < 0)
        {
            Debug.Log("LOW MONEY");
            money = 0;
            for(int i = tilesOwned.Count - 1; i >= 0; i--)
            {
                AdvancedTile t = tilesOwned[i];
                if (t.GetBuildable().isMovable())
                    t.KillUnit();
            }
            units.Clear();
        }
    }
    public bool HasTile(AdvancedTile tile)
    {
        return tilesOwned.Contains(tile);
    }
}
