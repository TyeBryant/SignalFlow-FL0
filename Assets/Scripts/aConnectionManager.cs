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
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePointer.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        if (isCarryingSignal)
        {
            Debug.DrawLine(mousePointer.transform.position, inputFrom.transform.position, Color.blue);
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
        inputFrom.GetComponent<aNode>().Disconnect();
        inputFrom = null;
    }

    //Carry the signal from an object
    public void CarrySignal(GameObject _inputFrom)
    {
        inputFrom = _inputFrom;
        isCarryingSignal = true;
    }
}
