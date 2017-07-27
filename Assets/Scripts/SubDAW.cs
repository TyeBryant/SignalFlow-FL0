using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDAW : MonoBehaviour
{
    //[HideInInspector]
    public int selectedIndex;

    [HideInInspector]
    public aDAW hubDaw;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            hubDaw.selectedIndex = selectedIndex;
        }
    }
}