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

    public Player(string s, float f, Color c)
    {
        nome = s;
        color = c;
        money = f;
        tilesOwned = new List<AdvancedTile>();
    }

    

    public void DoIncome()
    {
        income = 0;
        tilesOwned.ForEach(t => {
            income += t.build.GetIncome();
            income -= t.build.GetUpkeep();
        });
    }
    public void AddAdvancedTile(AdvancedTile t)
    {
        tilesOwned.Add(t);
        DoIncome();
    }
    public void RemoveAdvancedTile(AdvancedTile t)
    {
        tilesOwned.Remove(t);
        DoIncome();
    }
    internal void NexTurn()
    {
        DoIncome();
        money += income;
    }
}
