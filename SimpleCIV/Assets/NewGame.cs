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
    public Slider aiSpeed;

    public Text aiSpeedT;
    public Text moneyT;
    public Text tilesT;

    public float AIPLAYSPEED;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        players = new List<Player>();
    }

    public void StartGame()
    {
        Player p = new GameObject().AddComponent<Player>();
        p.SetUP("HUMAN", startMoney, Color.blue, false);
        Debug.Log(p.myBrains != null ? "AI" : "HUMAN");
        players.Add(p);
        DontDestroyOnLoad(p);
        foreach (Toggle t in GetComponentsInChildren<Toggle>())
        {
            if (t.isOn)
            {
                Dropdown d = t.transform.parent.GetComponentInChildren<Dropdown>();
                Color c = GetColor(d.options[d.value].text);
                p = new GameObject().AddComponent<Player>();
                p.SetUP(d.options[d.value].text, startMoney, c, true);
                Debug.Log(p.myBrains != null ? "AI" : "HUMAN");
                DontDestroyOnLoad(p);
                players.Add(p);
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
    public void AiSpeedChange()
    {
        AIPLAYSPEED = aiSpeed.value;
        aiSpeedT.text = AIPLAYSPEED.ToString();
    }
    public void TilesChange()
    {
        startTiles = (int)tiles.value;
        if (startTiles > 100)
            tilesT.text = "DIVIDE THE MAP AMONG THE PLAYERS";
        else
        tilesT.text = AIPLAYSPEED.ToString("##.##");
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
