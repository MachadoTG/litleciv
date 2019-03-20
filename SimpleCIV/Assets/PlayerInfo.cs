using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{

    public Text pName;
    public Text money;
    public Text income;
    public Text farms;
    public Text villages;
    public Text castle;

    public Player player;
    private void Update()
    {
        UpdateInfo();
    }
    public void UpdateInfo()
    {
        pName.text = player.nome;
        money.text = player.money.ToString();
        income.text = player.income.ToString();
        farms.text = "F:" + player.farmsUsed + "/" + player.farms;
        villages.text = "V:" + player.villagesUsed + "/" + player.villages;
        castle.text = "C:" + player.castlesUsed + "/" + player.castles;
    }

    public void ChangePlayer(Player p)
    {
        player = p;
        UpdateInfo();
    }
}
