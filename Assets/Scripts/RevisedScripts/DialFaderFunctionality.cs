using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;
using UnityEngine.UI;
using Dial;

public class DialFaderFunctionality : MonoBehaviour
{
    public bool df_Select = false;

    public GameObject df_Holder;
    public GameObject dial;
    public GameObject fader;

    [HideInInspector]
    public float dValue;
    [HideInInspector]
    public float fValue;

    private GameObject gameManager;

    private GameObject thisObject;

    //Reference to connection manager to cursor
    aConnectionManager connectionManager;

    // Use this for initialization
    void Start ()
    {
        //Get connection manager
        connectionManager = FindObjectOfType<aConnectionManager>();        
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        thisObject = this.gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (dValue == 0 && fValue == 0)
        {
            thisObject.GetComponent<aNode>().enabled = false;
            GetComponent<aNode>().sendingSignal = false;
        }

        if (dValue > 0 || fValue > 0)
        {
            thisObject.GetComponent<aNode>().enabled = true;
            GetComponent<aNode>().sendingSignal = true;
        }

        if (dial != null)
        {
            if (df_Select == true)
            {
                df_Holder.SetActive(true);
            }

            if (df_Select == false)
            {
                df_Holder.SetActive(false);
            }

            if (gameManager.GetComponent<GameManager>().gamePaused == true)
            {
                df_Select = false;
            }
        }

        if (dial == null)
        {
            dValue = 1;
        }
        else
        {
            dValue = dial.GetComponent<DialRotation>().dialValue;
        }

        if (fader == null)
        {
            fValue = 1;
        }
        else
        {
            fValue = fader.GetComponent<Slider>().value;
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && gameManager.GetComponent<GameManager>().gamePaused == false)
        {
            df_Select = !df_Select;
        }
    }
}
