using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{

    private Text messge;

    void Start()
    {
        messge = GetComponent<Text>();   
    }
    public void ShowMessage(float time, string s)
    {
        StartCoroutine(Show(time, s));
    }
    IEnumerator Show(float f,string s)
    {
        messge.text = s.ToUpper();
        yield return new WaitForSeconds(f);
        messge.text = "";
    }
}
