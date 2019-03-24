using System;
using UnityEngine;

[Serializable]
public class Buildables
{
    public static int cost;
    public static int level;
    public static int upkeep;
    public static int income;
    public static bool movable;
    public static Sprite img;
    public bool moved;

    public virtual int GetCost() { return cost; }
    public virtual int GetDefenseLeve() { return level; }
    public virtual int GetUpkeep() { return upkeep; }
    public virtual int GetIncome() { return income; }
    public virtual Sprite GetSprite() { return img; }
    public virtual bool isMovable() { return movable; }
    public virtual bool HasMoved() { return moved; }
    public virtual Buildables Clone() { return new Buildables(); }


    public class Empty : Buildables
    {
        public new static int cost = 0;
        public new static int level = 0;
        public new static int upkeep = 0;
        public new static int income = 1;
        public new static Sprite img;
        public new static bool movable = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return level; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }
        public override Sprite GetSprite()
        {
            return img;
        }

        public override Buildables Clone()
        {
            return new Buildables.Empty();
        }
    }

    public class Peasant : Buildables
    {
        public new static int cost = 5;
        public new static int level = 1;
        public new static int upkeep = 5;
        public new static Sprite img = Resources.Load<Sprite>("Sprite/peasant");
        public new static bool movable = true;
        public new bool moved = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return level; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Sprite GetSprite()
        {
            return img;
        }

        public override Buildables Clone()
        {
            return new Buildables.Peasant();
        }
    }
    public class Farm : Buildables
    {
        public new static int cost = 10;
        public new static int level = 2;
        public new static int income = 4;
        public new static Sprite img = Resources.Load<Sprite>("Sprite/farm");
        public new static bool movable = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return level; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Sprite GetSprite() {
            return img; }

        public override Buildables Clone()
        {
            return new Buildables.Farm();
        }
    }
    public class Knight : Buildables
    {
        public new static int cost = 20;
        public new static int level = 3;
        public new static int upkeep = 10;
        public new static Sprite img = Resources.Load<Sprite>("Sprite/knigth");
        public new static bool movable = true;
        public new bool moved = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return level; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }
        public override Sprite GetSprite()
        {
            return img;
        }

        public override Buildables Clone()
        {
            return new Buildables.Knight();
        }
    }
    public class Village : Buildables
    {
        public new static int cost = 40;
        public new static int level = 4;
        public new static int income = 6;
        public new static Sprite img = Resources.Load<Sprite>("Sprite/City");
        public new static bool movable = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return level; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Sprite GetSprite()
        {
            return img;
        }

        public override Buildables Clone()
        {
            return new Buildables.Village();
        }
    }
    public class Duke : Buildables
    {
        public new static int cost = 50;
        public new static int level = 5;
        public new static int upkeep = 40;
        public new static Sprite img = Resources.Load<Sprite>("Sprite/Duke");
        public new static bool movable = true;
        public new bool moved = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return level; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Sprite GetSprite()
        {
            return img;
        }

        public override Buildables Clone()
        {
            return new Buildables.Duke();
        }
    }
    public class Castle: Buildables
    {
        public new static int cost = 80;
        public new static int level = 4;
        public new static Sprite img = Resources.Load<Sprite>("Sprite/castle");
        public new static bool movable = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return level; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Sprite GetSprite()
        {
            return img;
        }

        public override Buildables Clone()
        {
            return new Buildables.Castle();
        }
    }
}
