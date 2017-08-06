using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GetScore : MonoBehaviour
{ 
    private float score;
	private string time;

    // Use this for initialization
    private void Start()
    {
        score = GameObject.Find("GameManager").GetComponent<GameManager>().GetScore();
		time = GameObject.Find("GameManager").GetComponent<GameManager>().GetNiceTime();
        SetText();
    }

    public void SetText()
    {
		string text = "Score: " + score + "\n Time: " + time;
		text.Replace("\\n", ("\n"));
        GetComponentInChildren<Text>().text = text;
    }
}
