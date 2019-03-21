using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class AdvancedTile : MonoBehaviour
{
    public int targetWidht = 80;
    public int targetHeight = 80;

    private Buildables build;
    private Buildables temp;

    private SpriteRenderer img;

    private Player owner = null;

    private Vector3Int tilePos;
    private MapController map;

    private void Awake()
    {
        img = GetComponentInChildren<SpriteRenderer>();
        map = GameObject.FindObjectOfType<MapController>();
        build = new Buildables.Empty();
    }

    public void Build(Buildables b)
    {
        build = b.Clone();
        build.moved = b.moved;
        ChangeSprite(build.GetSprite());
    }
    public void ChangeSprite(Sprite s)
    {
        if (s == null)
        {
            Color c = img.color;
            c.a = 0;
            img.color = c;
        }
        else
        {
            Color c = img.color;
            c.a = 1;
            img.color = c;
            img.sprite = s;
        }
    }
    public Sprite GetSprite() { return img.sprite; }
    public void UnitMoveTo(AdvancedTile t)
    {
        Debug.Log(owner.nome);
        if (t.owner != null)
            Debug.Log(t.owner.nome);
        if (owner != t.owner)
            t.ChangeOwner(owner);
        Debug.Log(temp.GetType());
        t.Build(temp);
    }
    public void UnitMoving()
    {
        temp = build.Clone();
        build = new Buildables.Empty();
        ChangeSprite(build.GetSprite());
    }
    public void CancelUnitMove()
    {
        build = temp.Clone();
    }
    public void ChangeOwner(Player p)
    {
        if (owner != null)
            if (owner.HasTile(this))
                owner.RemoveAdvancedTile(this);
        owner = p;
        if (!owner.HasTile(this))
            owner.AddAdvancedTile(this);
        map.ChangeTileColor(tilePos, p.color);
    }
    public Player GetPlayer() { return owner; }
    public Buildables GetBuildable() { return build; }
    public void SetTilePos(Vector3Int t) { tilePos = t; }
}
