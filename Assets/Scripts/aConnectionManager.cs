using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aConnectionManager : MonoBehaviour {

    [HideInInspector]
    public bool isCarryingSignal = false;
    public GameObject inputFrom;
    public GameObject mousePointer;

    Camera camera;
    Vector3 mousePosition = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
        //Retrieve the camera
        if (!GetComponent<Camera>())
            Debug.LogError("The connection manager script is not connected to the Main Camera!");
        else
            camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isCarryingSignal)
        {
            //If the player right clicks, disconnect from the signal
            if (Input.GetMouseButtonDown(1))
                DisconnectSignal();
        }
	}

    //Disconnect the signal from the mouse pointer
    public void DisconnectSignal()
    {
        //Change signal to false and make input object null
        isCarryingSignal = false;
        inputFrom = null;
    }

    //Carry the signal from an object
    public void CarrySignal(GameObject _inputFrom)
    {
        inputFrom = _inputFrom;
        isCarryingSignal = true;
    }

    //Place the signal down on the outputting node
    public void PlaceSignal(GameObject _outputTo)
    {
        _outputTo.GetComponent<aNode>().inputs.Add(inputFrom);
        inputFrom.GetComponent<aNode>().outputs.Add(_outputTo);
    }
}
