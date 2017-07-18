using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;

public class OnSelect : MonoBehaviour {

    public bool dialSelect = false;

    public GameObject dial;

    private GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

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

        if (gameManager.GetComponent<GameManager>().gamePaused == true)
        {
            dialSelect = false; 
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && gameManager.GetComponent<GameManager>().gamePaused == false)
        {
            dialSelect = !dialSelect;
        }
    }

}
