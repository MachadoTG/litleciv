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
    public Player playing;

    private Vector3Int cellPosition;
    private TileBase selectedTile;
    private AdvancedTile selectedInfo;

    public GameObject mainTile;
    private Dictionary<Vector3, AdvancedTile> advancedTiles;

    private Vector3Int coordinate;
    private List<Vector3> availablePlaces;
    
    private int Index = -1;

    private Buildables build;
    void Start()
    {
        map = GetComponentInChildren<Tilemap>();
        advancedTiles = new Dictionary<Vector3, AdvancedTile>();
        players = new List<Player>();

        LinkCellToInfo();

        players.Add(new Player("BLUE", 30, Color.blue,false));
        players.Add(new Player("RED", 30, Color.red,true));
        players.Add(new Player("CYAN", 30, Color.cyan,true));
        players.Add(new Player("YELLOW", 30, Color.yellow,true));
        players.Add(new Player("MAGENTA", 30, Color.magenta,true));
        foreach (Player p in players)
        {
            Vector3Int v = ToVectorInt(availablePlaces[Random.Range(0, availablePlaces.Count-1)]);
            if (advancedTiles[v].GetPlayer() == null)
            {
                advancedTiles[v].ChangeOwner(p);
                advancedTiles[v].Build(new Buildables.Farm());
                p.Build(new Buildables.Farm());
            }
        }
        NextPlayer();
    }
    void Update()
    {
        if (lockInput)
            return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(ShowCells());
        }
        if (Input.GetMouseButtonDown(2))
        {
            FindClickedTile();
            IsAdjscente();
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
                lastTile.CancelUnitMove();
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

    IEnumerator ShowCells()
    {
        availablePlaces = new List<Vector3>();
        for (int n = map.cellBounds.xMin; n < map.cellBounds.xMax; n++)
        {
            for (int p = map.cellBounds.yMin; p < map.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)map.transform.position.y));
                if (map.HasTile(localPlace))
                {
                    Debug.Log(n+"|"+p+"|"+0);
                    map.SetTileFlags(localPlace, TileFlags.None);
                    if (map.GetColor(localPlace) != Color.yellow)
                        map.SetColor(localPlace, Color.yellow);
                    else
                        map.SetColor(localPlace, Color.black);
                }
                else
                    continue;
                yield return new WaitForSeconds(2f);
            }
        }
    }
    private void MoveUnit()
    {
        if (building || !movingUnit)
            return;
        AdvancedTile tile = advancedTiles[coordinate];
        if (tile.GetPlayer() == playing)
        {
            if (tile.GetBuildable() is Buildables.Empty)
            {
                Debug.Log("Move");
                lastTile.UnitMoveTo(tile);
                lastTile = null;
                mouse.gameObject.SetActive(false);
                movingUnit = false;
                Debug.Log(tile.GetBuildable().GetType().Name);
                return;
            }
            else
            {
                warning.ShowMessage(3f, "Tile is Occupied!");
                return;
            }
        }
        if (IsAdjscente())
        {
            if (GetProtectionLevel() < build.GetDefenseLeve())
            {
                if (tile.GetBuildable().GetDefenseLeve() < build.GetDefenseLeve())
                {
                    Debug.Log("Agressive Move");
                    Debug.Log(build.GetType().Name);
                    tile.ChangeOwner(playing);
                    tile.Build(build);
                    tile.GetBuildable().moved = true;
                    mouse.gameObject.SetActive(false);
                    movingUnit = false;
                    Debug.Log(tile.GetBuildable().GetType().Name);
                    return;
                }
                else
                {
                    warning.ShowMessage(3f, "The Defense There Is Too Strong!");
                    return;
                }
            }
            else
            {
                warning.ShowMessage(3f, "There Is A Tile Defending It!");
                return;
            }
        }
        else
        {
            warning.ShowMessage(3f, "Too Far From The Border!");
            return;
        }
          
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
            Vector3Int pos = new Vector3Int(x + 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            f = r;
            pos = new Vector3Int(x, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x - 1, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x - 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x - 1, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
        }
        else
        {
            Vector3Int pos = new Vector3Int(x + 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x + 1, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x, y + 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x + 1, y - 1, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
            pos = new Vector3Int(x - 1, y, z);
            if (advancedTiles.ContainsKey(pos))
            {
                r = CheckTileOwner(pos, playing);
            }
            if (!f)
                f = r;
        }
        return f;
    }
    private bool CheckTileOwner(Vector3Int v, Player p)
    {
        if (advancedTiles[v].GetPlayer() == playing)
            return true;
        else
            return false;
    }

    private int GetTileDefense(Vector3Int v)
    {
        return advancedTiles[v].GetBuildable().GetDefenseLeve();
    }
    private int GetProtectionLevel()
    {
        int r = 0;
        int f = 0;
        int x = coordinate.x;
        int y = coordinate.y;
        int z = coordinate.z;

        if (y % 2 == 0)
        {
            Vector3Int pos = new Vector3Int(x + 1, y, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x, y + 1, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x - 1, y + 1, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x - 1, y, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x - 1, y - 1, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x, y - 1, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
        }
        else
        {
            Vector3Int pos = new Vector3Int(x + 1, y, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x + 1, y + 1, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x, y + 1, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x, y - 1, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x + 1, y - 1, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
            pos = new Vector3Int(x - 1, y, z);
            if (advancedTiles.ContainsKey(pos) && advancedTiles[coordinate].GetPlayer() == advancedTiles[pos].GetPlayer())
            {
                r = GetTileDefense(pos);
            }
            if (f < r)
                f = r;
        }
        return f;
    }

    private bool building = false;
    private void Build()
    {
        if (!building || movingUnit)
            return;
        AdvancedTile tile = advancedTiles[coordinate];
        if (tile.GetPlayer() == playing)
        {
            if (tile.GetBuildable() is Buildables.Empty)
            {
                if (build.GetCost() <= playing.money)
                {
                    if (playing.Build(build))
                    {
                        Debug.Log("Build");
                        tile.Build(build);
                        Debug.Log(tile.GetBuildable().GetType().Name);
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            mouse.gameObject.SetActive(false);
                            building = false;
                        }
                        return;
                    }
                    else
                    {
                        warning.ShowMessage(3f, "Not enough Buildings!");
                        return;
                    }
                }
                else
                {
                    warning.ShowMessage(3f, "Not enough Money!");
                    return;
                }
            }
            else
            {
                warning.ShowMessage(3f, "Tile Is Occupied!");
                return;
            }
        }
        if (build.isMovable())
        {
            if (IsAdjscente())
            {
                if (GetProtectionLevel() < build.GetDefenseLeve())
                {
                    if (tile.GetBuildable().GetDefenseLeve() < build.GetDefenseLeve())
                    {
                        if (build.GetCost() <= playing.money)
                        {
                            if (playing.Build(build))
                            {
                                Debug.Log("Agressive Build");
                                tile.ChangeOwner(playing);
                                tile.Build(build);
                                tile.GetBuildable().moved = true;
                                Debug.Log(tile.GetBuildable().GetType().Name);
                                if (!Input.GetKey(KeyCode.LeftShift))
                                {
                                    mouse.gameObject.SetActive(false);
                                    building = false;
                                }
                                return;
                            }
                            else
                            {
                                warning.ShowMessage(3f, "Not Enough Buildings!");
                                return;
                            }
                        }
                        else
                        {
                            warning.ShowMessage(3f, "Not Enough Money!");
                            return;
                        }
                    }
                    else
                    {
                        warning.ShowMessage(3f, "There Is A Tile Defending It!");
                        return;
                    }
                }
                else
                {
                    warning.ShowMessage(3f, "Too Far From The Border!");
                    return;
                }
            }
            else
            {
                warning.ShowMessage(3f, "Too Far From The Border!");
                return;
            }

        }
        else
        {
            warning.ShowMessage(3f, "Only Units Can Conquer Tiles!");
            return;
        }
    }

    private bool movingUnit = false;
    private AdvancedTile lastTile;
    private void FindClickedTile()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        coordinate = map.WorldToCell(mouseWorldPos);
        if (map.HasTile(coordinate))
            SelectTile();
    }
    private void SelectTile()
    {
        AdvancedTile tile = advancedTiles[coordinate];
        if (building)
        {
            return;
        }
        if (movingUnit)
        {
            return;
        }
        if(tile.GetPlayer() == playing)
        {
            if (tile.GetBuildable().isMovable())
            {
                if (!tile.GetBuildable().HasMoved())
                {
                    build = advancedTiles[coordinate].GetBuildable().Clone();
                    ChangeMouseIcon(advancedTiles[coordinate].GetBuildable(),false);
                    advancedTiles[coordinate].UnitMoving();
                    lastTile = advancedTiles[coordinate];
                    movingUnit = true;
                }
                else
                {
                    warning.ShowMessage(3f, "This Unit Has Alredy Moved!");
                    return;
                }
            }
            else
            {
                warning.ShowMessage(3f, "This Is A Build It Cant Move!");
                return;
            }
        }
        else
        {
            warning.ShowMessage(3f, "That Tile Isnt Yours!");
            return;
        }
    }

    bool lockInput = false;
    public void NextPlayer()
    {   
        Index++;

        if (Index + 1 > players.Count)
        {
            Index = 0;
        }
        playing = players[Index];
        Debug.Log(playing.nome);
        playing.NexTurn();
        info.ChangePlayer(playing);

        if (playing.myBrains != null)
        {
            lockInput = true;
            playing.myBrains.PlayTurn();
            new WaitForSeconds(2f);
        }
        else
        {
            lockInput = false;
        }   
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
            advancedTiles[vec].SetTilePos(ToVectorInt(vec));
        });
        map.SetColor(ToVectorInt(availablePlaces[0]), Color.blue);
        map.SetColor(ToVectorInt(availablePlaces[availablePlaces.Count - 1]), Color.red);

    }

    public void ChangeMouseIcon(Buildables buildable , bool b)
    {
        if (!mouse.gameObject.activeSelf)
            mouse.gameObject.SetActive(true);
        build = buildable;
        mouse.sprite = buildable.GetSprite();
        building = b;
    }

    public void ClickPeasant()
    {
        ChangeMouseIcon(new Buildables.Peasant(),true);
    }
    public void ClickFarm()
    {
        ChangeMouseIcon(new Buildables.Farm(), true);
    }
    public void ClickKnight()
    {
        ChangeMouseIcon(new Buildables.Knight(), true);
    }
    public void ClickVillage()
    {
        ChangeMouseIcon(new Buildables.Village(), true);
    }
    public void ClickDuke()
    {
        ChangeMouseIcon(new Buildables.Duke(), true);
    }
    public void ClickCastle()
    {
        ChangeMouseIcon(new Buildables.Castle(), true);
    }

    public void ChangeTileColor(Vector3Int v, Color c)
    {
        map.SetTileFlags(v, TileFlags.None);
        map.SetColor(v, c);
    }

    public Vector3Int ToVectorInt(Vector3 v)
    {
        return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }
    public Vector3 ToVector(Vector3Int v)
    {
        return new Vector3Int(v.x, v.y, v.z);
    }
    public Dictionary<Vector3,AdvancedTile> getAdvancedTiles() { return advancedTiles; }
    public bool IsAdjscente(Vector3Int v)
    {
        coordinate = v;
        return IsAdjscente();
    }
    public int GetProtectionLevel(Vector3Int v)
    {
        coordinate = v;
        return GetProtectionLevel();
    }

}
