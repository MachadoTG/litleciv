using UnityEngine;
using UnityEngine.UI;

public class AdvancedTile : MonoBehaviour
{
    public bool empty;
    public int defenseLevel;
    public int income;

    private Image img;

    public Player owner;


    private void Awake()
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        img = GetComponentInChildren<Image>();
        income = 1;
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
            img.sprite = s;
            Color c = img.color;
            c.a = 1;
            img.color = c;
        }
    }
}
