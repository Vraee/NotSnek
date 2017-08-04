using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndTextController : MonoBehaviour {
    public GameObject endText;

    public void Instantiate()
    {
        GameObject instance = Instantiate(endText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector3(0,0,0));
        instance.transform.SetParent(this.transform, false);
        instance.transform.SetAsFirstSibling();
        instance.transform.position = screenPosition;
    }
}
