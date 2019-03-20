using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    private Tilemap map;

    public Warning warning;

    public PlayerInfo info;

    public SpriteRenderer mouse;

    public List<Player> players;
    private Player playing;

    private Vector3Int cellPosition;
    private TileBase selectedTile;
    private AdvancedTile selectedInfo;

    public GameObject mainTile;
    private Dictionary<Vector3, AdvancedTile> advancedTiles;

    private Vector3Int coordinate;
    private List<Vector3> availablePlaces;
    
    private int Index = 0;

    private Buildables build;

    void Start()
    {
        map = GetComponentInChildren<Tilemap>();
        advancedTiles = new Dictionary<Vector3, AdvancedTile>();
        players = new List<Player>();

        LinkCellToInfo();

        players.Add(new Player("BLUE", 10, Color.blue));
        players.Add(new Player("RED", 10, Color.red));

        foreach (Player p in players)
        {
            int i = 0;
            while (i < 2)
            {
                Vector3Int v = ToVectorInt(availablePlaces[Random.Range(0, availablePlaces.Count)]);
                if (advancedTiles[v].owner == null)
                {
                    if (i > 0)
                    {
                        advancedTiles[v].build = new Buildables.Farm();
                        advancedTiles[v].ChangeIcon(advancedTiles[v].build.GetSprite());
                        p.Build(advancedTiles[v].build);
                    }
                    map.SetTileFlags(v, TileFlags.None);
                    map.SetColor(v, p.color);
                    advancedTiles[v].owner = p;
                    p.AddAdvancedTile(advancedTiles[v]);
                    i++;
                }
            }
        }
        NextPlayer();
    }

    private int money = 0;
    private int income = 0;
    void Update()
    {
        if (money != (int)playing.money || income != (int)playing.income)
        {
            money = (int)playing.money;
            income = (int)playing.income;
            info.ChangePlayer(playing);
        }

        if((Input.GetMouseButtonDown(0))&&(movingUnit || building))
        {
            FindClickedTile();
            if (building)
                Build();
            if (movingUnit)
                MoveUnit();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindClickedTile();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouse.gameObject.SetActive(false);
            if (building)
                building = false;
            if (movingUnit)
            {
                movingUnit = false;
                lastTile.MovingUnit(false);
                lastTile = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPlayer();
        }
        if (mouse.gameObject.activeSelf)
        {
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.transform.position = new Vector3(v.x, v.y, -1); 
        }
    }

    private void MoveUnit()
    {
        if(advancedTiles[coordinate].owner == playing && advancedTiles[coordinate].build is Buildables.Empty)
        {
            Debug.Log("Moved");
            advancedTiles[coordinate].build = build.Clone();
            advancedTiles[coordinate].ChangeIcon(build.GetSprite());
            advancedTiles[coordinate].build.moved = false;
            lastTile.UnitMoved();
            lastTile = null;
            mouse.gameObject.SetActive(false);
            movingUnit = false;
            return;
        }else
        if (advancedTiles[coordinate].owner != playing
            && advancedTiles[coordinate].build.GetDefenseLeve() < build.GetDefenseLeve())
        {
            playing.AddAdvancedTile(advancedTiles[coordinate]);

            if (advancedTiles[coordinate].owner != null)
                advancedTiles[coordinate].owner.RemoveAdvancedTile(advancedTiles[coordinate]);
            Debug.Log("Moved Agressive");

            map.SetTileFlags(ToVectorInt(coordinate), TileFlags.None);
            map.SetColor(ToVectorInt(coordinate), playing.color);
            advancedTiles[coordinate].owner = playing;
            advancedTiles[coordinate].build = build.Clone();
            advancedTiles[coordinate].ChangeIcon(build.GetSprite());
            advancedTiles[coordinate].build.moved = true;
            mouse.gameObject.SetActive(false);
            lastTile.UnitMoved();
            lastTile = null;
            movingUnit = false;
            return;
        }
        warning.ShowMessage(3f, "Cant Move There!");
    }
    private bool IsAdjscente()
    {
        bool r = true;
        bool f = false;
        int x = coordinate.x;
        int y = coordinate.y;
        int z = coordinate.z;
        if (y % 2 == 0)
        {
            Vector3 pos = new Vector3(x + 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            f = r;
            pos = new Vector3(x, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x - 1, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x - 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x - 1, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
        }
        else
        {
            Vector3 pos = new Vector3(x + 1, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x + 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x + 1, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x - 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner != playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
            pos = new Vector3(x, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                if (advancedTiles[pos].owner == playing)
                    r = false;
                else
                    r = true;
            }
            if (!f)
                f = r;
        }

        return f;
    }


    private bool building = false;
    private void Build()
    {
        if (!building)
            return;
        if (advancedTiles[coordinate].build.GetType() == typeof(Buildables.Empty) && advancedTiles[coordinate].owner == playing)
        {
            if (build.GetCost() <= playing.money )
            {
                if (playing.Build(build))
                {
                    Debug.Log("Build");
                    playing.money -= build.GetCost();
                    advancedTiles[coordinate].build = build.Clone();
                    advancedTiles[coordinate].ChangeIcon(build.GetSprite());
                    mouse.gameObject.SetActive(false);
                    building = false;
                    return;
                }
                warning.ShowMessage(3f, "Not enough Buildings");
                return;
            }
            warning.ShowMessage(3f, "Not enough Money");
            return;
        }
        else
        if(advancedTiles[coordinate].owner != playing && advancedTiles[coordinate].build.GetDefenseLeve()< build.GetDefenseLeve())
        {
            if (build.GetCost() <= playing.money )
            {
                if (playing.Build(build))
                {
                    Debug.Log("Build Agressive");
                    playing.money -= build.GetCost();
                    playing.AddAdvancedTile(advancedTiles[coordinate]);

                    foreach (Player p in players)
                    {
                        if (p.tilesOwned.Contains(advancedTiles[coordinate]))
                            p.tilesOwned.Remove(advancedTiles[coordinate]);
                    }
                    map.SetTileFlags(coordinate, TileFlags.None);
                    map.SetColor(coordinate, playing.color);
                    advancedTiles[coordinate].owner = playing;
                    advancedTiles[coordinate].build = build.Clone();
                    advancedTiles[coordinate].ChangeIcon(build.GetSprite());
                    mouse.gameObject.SetActive(false);
                    building = false;
                    return;
                }
                warning.ShowMessage(3f, "Not enough Buildings");
                return;
            }
            warning.ShowMessage(3f, "Not enough Money");
            return;
        }
        warning.ShowMessage(3f, "Invalid Location");
    }

    private bool movingUnit = false;
    private AdvancedTile lastTile;
    private void FindClickedTile()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        coordinate = map.WorldToCell(mouseWorldPos);
        Debug.Log(coordinate.x + "|" + coordinate.y + "|" + coordinate.z);
        if (advancedTiles[coordinate].owner == playing &&
            advancedTiles[coordinate].build.isMovable()
            && !advancedTiles[coordinate].build.moved
            && !movingUnit && !building)
        {
            Debug.Log("Moving");
            mouse.gameObject.SetActive(true);
            mouse.sprite = advancedTiles[coordinate].build.GetSprite();
            advancedTiles[coordinate].MovingUnit(true);
            build = advancedTiles[coordinate].build.Clone();
            movingUnit = true;
            lastTile = advancedTiles[coordinate];
        }

    }


    private void NextPlayer()
    {
        Debug.Log(Index);
        
        Index++;

        if (Index + 1 > players.Count)
        {
            Index = 0;
            foreach (Player p in players)
            {
                p.NexTurn();
            }
            foreach (var e in advancedTiles)
            {
                if (e.Value.build.isMovable())
                    e.Value.build.moved = false;
            }
        }

        playing = players[Index];
        money = (int)playing.money;
        income = (int)playing.income;
        info.ChangePlayer(playing);
    }
    private void LinkCellToInfo()
    {
        availablePlaces = new List<Vector3>();
        for (int n = map.cellBounds.xMin; n < map.cellBounds.xMax; n++)
        {
            for (int p = map.cellBounds.yMin; p < map.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)map.transform.position.y));
                if (map.HasTile(localPlace))
                {
                    availablePlaces.Add(localPlace);
                }

            }
        }
        availablePlaces.ForEach(vec =>
        {
            GameObject tile = Instantiate(mainTile, map.CellToWorld(ToVectorInt(vec)), transform.rotation);
            advancedTiles.Add(vec, tile.GetComponent<AdvancedTile>());
            Debug.Log(vec.x + "|" + vec.y);
        });
        map.SetColor(ToVectorInt(availablePlaces[0]), Color.blue);
        map.SetColor(ToVectorInt(availablePlaces[availablePlaces.Count - 1]), Color.red);

    }

    public void ClickPeasant()
    {
        if (!mouse.gameObject.activeSelf)
            mouse.gameObject.SetActive(true);
        build = new Buildables.Peasant();
        mouse.sprite = build.GetSprite();
        building = true;
    }
    public void ClickFarm()
    {
        if (!mouse.gameObject.activeSelf)
            mouse.gameObject.SetActive(true);
        build = new Buildables.Farm();
        mouse.sprite = build.GetSprite();
        building = true;
    }
    public void ClickKnight()
    {
        if (!mouse.gameObject.activeSelf)
            mouse.gameObject.SetActive(true);
        build = new Buildables.Knight();
        mouse.sprite = build.GetSprite();
        building = true;
    }
    public void ClickVillage()
    {
        if (!mouse.gameObject.activeSelf)
            mouse.gameObject.SetActive(true);
        build = new Buildables.Village();
        mouse.sprite = build.GetSprite();
        building = true;
    }
    public void ClickDuke()
    {
        if (!mouse.gameObject.activeSelf)
            mouse.gameObject.SetActive(true);
        build = new Buildables.Duke();
        mouse.sprite = build.GetSprite();
        building = true;
    }
    public void ClickCastle()
    {
        if (!mouse.gameObject.activeSelf)
            mouse.gameObject.SetActive(true);
        build = new Buildables.Castle();
        mouse.sprite = build.GetSprite();
        building = true;
    }

    private Vector3Int ToVectorInt(Vector3 v)
    {
        return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }
    private Vector3 ToVector(Vector3Int v)
    {
        return new Vector3Int(v.x, v.y, v.z);
    }

}
