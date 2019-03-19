using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private Tilemap map;

    public Sprite[] sprites;
    private int spriteIndex = 0;

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

    void Start()
    {
        map = GetComponentInChildren<Tilemap>();
        advancedTiles = new Dictionary<Vector3, AdvancedTile>();
        players = new List<Player>();

        LinkCellToInfo();

        players.Add(new Player("BLUE", 100, Color.blue));
        players.Add(new Player("RED", 100, Color.red));

        foreach (Player p in players)
        {
            Vector3Int v = ToVectorInt(availablePlaces[Random.Range(0, availablePlaces.Count)]);
            map.SetTileFlags(v, TileFlags.None);
            map.SetColor(v, p.color);
            advancedTiles[v].owner = p;
        }
        playing = players[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindClickedTile();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            spriteIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            spriteIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            spriteIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            spriteIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            spriteIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            spriteIndex = 5;
    }

    private void NextPlayer()
    {
        Debug.Log(Index);

        Index++;
        if (Index + 1 > players.Count)
            Index = 0;

        playing = players[Index];
        Debug.Log(playing.nome);
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
            GameObject tile = Instantiate(mainTile,map.CellToWorld(ToVectorInt(vec)),transform.rotation);
            advancedTiles.Add(vec, tile.GetComponent<AdvancedTile>());
            Debug.Log(vec.x + "|" + vec.y);
        });
        map.SetColor(ToVectorInt(availablePlaces[0]),Color.blue);
        map.SetColor(ToVectorInt(availablePlaces[availablePlaces.Count - 1]), Color.red);

    }

    

    private void FindClickedTile()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        coordinate = map.WorldToCell(mouseWorldPos);

        if (advancedTiles[coordinate].owner == playing)
            advancedTiles[coordinate].ChangeIcon(sprites[spriteIndex]);

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
