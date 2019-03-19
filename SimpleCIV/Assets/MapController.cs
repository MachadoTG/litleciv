using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private Tilemap map;

    public List<Player> players;
    private Player playing;

    private Vector3Int cellPosition;
    private TileBase selectedTile;
    private AdvancedTile selectedInfo;

    private Dictionary<Vector3, AdvancedTile> advancedTiles;

    private int Index = 0;

    void Start()
    {
        map = GetComponentInChildren<Tilemap>();
        advancedTiles = new Dictionary<Vector3, AdvancedTile>();
        players = new List<Player>();

        LinkCellToInfo();

        players.Add(new Player("BLUE", Color.blue));
        players.Add(new Player("RED", Color.red));

        playing = players[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectTile();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPlayer();
        }
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
        List<Vector3> availablePlaces = new List<Vector3>();
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
            AdvancedTile tile = new AdvancedTile();
            advancedTiles.Add(vec, tile);
            Debug.Log(vec.x + "|" + vec.y);
        });
        map.SetColor(ToVectorInt(availablePlaces[0]),Color.blue);
        map.SetColor(ToVectorInt(availablePlaces[availablePlaces.Count - 1]), Color.red);

    }

    

    private void SelectTile()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = map.WorldToCell(mouseWorldPos);

        map.SetTileFlags(coordinate, TileFlags.None);

        selectedTile = map.GetTile(coordinate);
        advancedTiles.TryGetValue(coordinate, out selectedInfo);

        if (map.GetColor(coordinate) != playing.color)
            map.SetColor(coordinate, playing.color);

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
