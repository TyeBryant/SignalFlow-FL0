using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelect : MonoBehaviour {

    public bool dialSelect = false;

    public GameObject dial;

    private void Update()
    {
        if (dialSelect == true)
        {
            dial.SetActive(true);
        }

        if (dialSelect == false)
        {
            dial.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dialSelect = !dialSelect;
        }
    }

}
