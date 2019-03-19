using System;

public class Buildables
{
    public static int cost;
    public static int defenseLevel;
    public static int upkeep;
    public static int income;
    public static bool movable;
    public bool moved;

    public virtual int GetCost() { return cost; }
    public virtual int GetDefenseLeve() { return defenseLevel; }
    public virtual int GetUpkeep() { return upkeep; }
    public virtual int GetIncome() { return income; }
    public virtual bool isMovable() { return movable; }
    public virtual bool HasMoved() { return moved; }
    public virtual Buildables Clone() { return new Buildables(); }


    public class Empty : Buildables
    {
        public new static int cost = 0;
        public new static int defenseLevel = 0;
        public new static int upkeep = 0;
        public new static int income = 3;
        public new static bool movable = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return defenseLevel; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Buildables Clone()
        {
            return new Buildables.Empty();
        }
    }

    public class Peasant : Buildables
    {
        public new static int cost = 10;
        public new static int defenseLevel = 1;
        public new static int upkeep = 5;
        public new static bool movable = true;
        public new bool moved = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return defenseLevel; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Buildables Clone()
        {
            return new Buildables.Peasant();
        }
    }
    public class Farm : Buildables
    {
        public new static int cost = 20;
        public new static int defenseLevel = 2;
        public new static int income = 5;
        public new static bool movable = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return defenseLevel; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Buildables Clone()
        {
            return new Buildables.Farm();
        }
    }
    public class Knight : Buildables
    {
        public new static int cost = 40;
        public new static int defenseLevel = 3;
        public new static int upkeep = 20;
        public new static bool movable = true;
        public new bool moved = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return defenseLevel; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Buildables Clone()
        {
            return new Buildables.Knight();
        }
    }
    public class Village : Buildables
    {
        public new static int cost = 50;
        public new static int defenseLevel = 4;
        public new static int income = 10;
        public new static bool movable = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return defenseLevel; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Buildables Clone()
        {
            return new Buildables.Village();
        }
    }
    public class Duke : Buildables
    {
        public new static int cost = 100;
        public new static int defenseLevel = 5;
        public new static int upkeep = 50;
        public new static bool movable = true;
        public new bool moved = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return defenseLevel; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Buildables Clone()
        {
            return new Buildables.Duke();
        }
    }
    public class Castle: Buildables
    {
        public new static int cost = 200;
        public new static int defenseLevel = 5;
        public new static int upkeep = 100;
        public new static bool movable = false;

        public override int GetCost() { return cost; }
        public override int GetDefenseLeve() { return defenseLevel; }
        public override int GetUpkeep() { return upkeep; }
        public override int GetIncome() { return income; }
        public override bool isMovable() { return movable; }

        public override Buildables Clone()
        {
            return new Buildables.Castle();
        }
    }
}
