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

    public void ChangePlayer(Player p)
    {
        pName.text = p.nome;
        money.text = p.money.ToString();
        income.text = p.income.ToString();
        farms.text = "F:" + p.farmsUsed + "/" + p.farms;
        villages.text = "F:" + p.villagesUsed + "/" + p.villages;
        castle.text = "F:" + p.castlesUsed + "/" + p.castles;
    }
}
