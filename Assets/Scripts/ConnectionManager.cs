using System.Collections;
using System.Collections.Generic;
using nodeFunctionality;
using UnityEngine;

public class ConnectionManager : MonoBehaviour {

    [HideInInspector]
    public bool isCarryingSignal;
    public GameObject powerFrom;
    public GameObject child;

    Camera cam;
    Vector3 mousePos = new Vector3(0,0,0); 

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        isCarryingSignal = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isCarryingSignal)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            child.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            powerFrom.GetComponent<Node>().Connected(child);

            //If the player right clicks, drop the current signal
            if (Input.GetMouseButtonDown(1))
                DisconnectSignal();
        }
	}

    //Called when the player is forcefully disconnected from a signal
    public void DisconnectSignal()
    {
        isCarryingSignal = false;
        powerFrom.GetComponent<Node>().lineRend.SetPosition(1, powerFrom.transform.position);
        powerFrom = null;
    }

    //Provides a signal to the player using the given node as a base
    public void ProvideSignal(GameObject providingNode)
    {
        powerFrom = providingNode;
        isCarryingSignal = true;
    }

    //Connects the base node to the given node
    public void ConnectToNode(GameObject connectingNode)
    {
        powerFrom.GetComponent<Node>().powering = connectingNode;
        connectingNode.GetComponent<Node>().receiving = powerFrom;
        powerFrom.GetComponent<Node>().lPowering.Add(connectingNode);
        connectingNode.GetComponent<Node>().lReceiving.Add(powerFrom);
    }
}
