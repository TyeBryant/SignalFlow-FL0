using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aConnectionManager : MonoBehaviour
{

    [HideInInspector]
    public bool isCarryingSignal = false;
    public GameObject inputFrom;
    public GameObject mousePointer;

    Camera camera;
    Vector3 mousePosition = new Vector3(0, 0, 0);

    public GameObject lineRenderPrefab;
    public GameObject pbLineRenderPrefab;
    public GameObject lineRendObj;
    LineRenderer lineRend;

    // Use this for initialization
    void Start()
    {
        //Retrieve the camera
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePointer.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        if (isCarryingSignal)
        {
            Debug.DrawLine(mousePointer.transform.position, inputFrom.transform.position, Color.blue);

            if (Input.GetMouseButtonDown(0) && lineRend == null)
            {
                bool zoomed = false;
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PatchBay"))
                    if (obj.GetComponent<PatchBay>().zoomed) {
                        lineRendObj = Instantiate(pbLineRenderPrefab);
                        zoomed = true;
                    }
                if(!zoomed)
                    lineRendObj = Instantiate(lineRenderPrefab);
                lineRend = lineRendObj.GetComponent<LineRenderer>();
                lineRend.SetPosition(0, inputFrom.transform.position);
            }
            else if (Input.GetMouseButtonDown(0) && lineRend != null)
                isCarryingSignal = true;

            lineRend.SetPosition(1, mousePointer.transform.position);

            //If the player right clicks, disconnect from the signal
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(lineRendObj);
                inputFrom = null;
                isCarryingSignal = false;
            }
        }
        else
        {
            Destroy(lineRendObj);
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
