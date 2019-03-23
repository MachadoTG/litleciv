using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    [HideInInspector]
    public List<Player> players;

    [HideInInspector]
    public int startTiles = 1;
    private int startMoney = 30;

    public Slider money;
    public Slider tiles;

    public Text moneyT;
    public Text tilesT;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        players = new List<Player>();
        players.Add(new Player("HUMAN", startMoney, Color.blue, false));
    }

    public void StartGame()
    {
        foreach(Toggle t in GetComponentsInChildren<Toggle>())
        {
            if (t.isOn)
            {
                Dropdown d = t.transform.parent.GetComponentInChildren<Dropdown>();
                Color c = GetColor(d.options[d.value].text);
                players.Add(new Player(d.options[d.value].text, startMoney, c, true));
            }
        }
        SceneManager.LoadScene(1);
    }
    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
    public void MoneyChange()
    {
        startMoney = (int)money.value;
        moneyT.text = startMoney.ToString();
    }
    public void TilesChange()
    {
        startTiles = (int)tiles.value;
        tilesT.text = startTiles.ToString();
    }
    private Color GetColor(string s)
    {
        switch (s)
        {
            case "RED":
                return new Color(1, 0, 0, 1 );
            case "GREEN":
                return new Color(0, 1, 0, 1);
            case "ORANGE":
                return new Color(1, .5f, 0, 1);
            case "YELLOW":
                return new Color(1, .9f, 0, 1);
            case "PURPLE":
                return new Color(1, 0, .9f, 1);
            case "CYAN":
                return new Color(0, 1, 1, 1);
            case "BROWN":
                return new Color(.5f, .02f, 0, 1);
            default:
                return new Color(0,.2f,.5f,1);
        }
    }
}
