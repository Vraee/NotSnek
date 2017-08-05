using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class HighlightOnMouseOver : MonoBehaviour {

    private void OnMouseOver()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(this.gameObject, null);
    }
}
