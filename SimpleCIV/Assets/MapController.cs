using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private Tilemap map;

    private Vector3Int cellPosition;


    void Start()
    {
        map = GetComponentInChildren<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectTile();
        }
    }

    private void SelectTile()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = map.WorldToCell(mouseWorldPos);

        map.SetTileFlags(coordinate, TileFlags.None);

        if (map.GetColor(coordinate) == Color.white)
            map.SetColor(coordinate, Color.red);
        else
            map.SetColor(coordinate, Color.white);
    }


}
