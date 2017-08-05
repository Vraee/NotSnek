using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GetScore : MonoBehaviour
{ 
    private float score;

    // Use this for initialization
    private void Start()
    {
        score = GameObject.Find("GameManager").GetComponent<GameManager>().GetScore();
        SetText();
    }

    public void SetText()
    {
        GetComponentInChildren<Text>().text = "Score: " + score;
    }
}
