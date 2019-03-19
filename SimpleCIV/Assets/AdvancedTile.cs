using UnityEngine;
using UnityEngine.UI;

public class AdvancedTile : MonoBehaviour
{
    public Buildables build;
    public int targetWidht = 80;
    public int targetHeight = 80;
    private SpriteRenderer img;

    public Player owner = null;


    private void Awake()
    {
        img = GetComponentInChildren<SpriteRenderer>();
        build = new Buildables.Empty();
    }

    private void Update()
    {
        
    }

    public void ChangeIcon(Sprite s)
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
    public void MovingUnit(bool b)
    {
        if (b)
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
        }
    }
    public void UnitMoved()
    {
        build = new Buildables.Empty();
        Color c = img.color;
        c.a = 0;
        img.color = c;
    }
}
