using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class aPatchBay : aNode, IPointerClickHandler {
    public List<GameObject> inputNodes = new List<GameObject>();
    public List<GameObject> outputNodes = new List<GameObject>();
    public List<GameObject> settingNodes = new List<GameObject>();

    public GameObject subNodeObject;
    public float nodeRadiusSpacing;

    //This shouldn't really be set beforehand
    public List<GameObject> subNodes = new List<GameObject>(); // Used for selecting signal
    //Signal things
    public List<GameObject> cubeInputs = new List<GameObject>();

    public List<Vector3> inputNodePos = new List<Vector3>();
    public List<GameObject> signalObjs = new List<GameObject>();

    List<GameObject> activeInNodes = new List<GameObject>();
    List<GameObject> activeOuNodes = new List<GameObject>();

    public int selectedIndex;

    public bool zoomed, zooming;
    float zoomSpeed = 3;
    public int pbCounter;

    public new int signalNumber;

    public List<int> signalNumbers;

    private new void Start()
    {
        base.Start();
        for (int index = 0; index < inputNodes.Count; ++index) {
            inputNodes[index].SetActive(false);
            outputNodes[index].SetActive(false);
            settingNodes[index].SetActive(false);
            //inputNodePos.Add(inputNodes[index].transform.position);
            //subNodes[index].SetActive(false);
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        if (eventData.clickCount >= 2 && !zoomed) {
            // Zoom In, show patch bay
            zooming = true;
            zoomed = true;

            for (int index = 0; index < inputs.Count; ++index) {
                inputNodes[index].SetActive(true);
                inputNodes[index].GetComponent<aNode>().inputs.Add(inputs[index]);
                outputNodes[index].SetActive(true);
                settingNodes[index].SetActive(true);
                Debug.Log(inputs.Count);
            }

            GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (eventData.clickCount < 2 && !zoomed) {
            // Show Outputs
            if (outputNodes[0].activeSelf)
                foreach (GameObject node in outputNodes)
                    node.SetActive(false);
            else
            {

                pbCounter++;
                foreach (GameObject node in inputNodes)
                    node.SetActive(false);
                //foreach (GameObject node in subNodes)
                //    Destroy(node);
                //subNodes.Clear();
                activeInNodes.Clear();
                //for (int i = 0; i < inputs.Count; ++i)
                //{
                    
                //    Vector3 spawnPos = (inputs[i].transform.position - transform.position).normalized * nodeRadiusSpacing;
                //    GameObject obj = Instantiate(subNodeObject, spawnPos, Quaternion.identity);
                //    obj.GetComponent<subPB>().pb = this;
                //    obj.GetComponent<subPB>().selectedIndex = i;
                //    subNodes.Add(obj);
                //}
            }

        }
        
        
    }

    public override void OnMouseOver() {
        base.OnMouseOver();

        if (Input.GetMouseButtonUp(0)) {
            foreach (GameObject obj in subNodes)
                Destroy(obj);
            subNodes.Clear();
            Debug.Log(inputs.Count);
            for (int i = 0; i < inputs.Count; ++i) {
                Vector3 spawnPos = (inputs[i].transform.position - transform.position).normalized * nodeRadiusSpacing;
                GameObject obj = Instantiate(subNodeObject, transform.position + spawnPos, Quaternion.identity);
                obj.GetComponent<subPB>().pb = this;
                obj.GetComponent<subPB>().selectedIndex = i;
                subNodes.Add(obj);
            }
        }

    }

    new void Update() {

        signalNumber = 0;

        //Win checking stuff
        if (isPowered == false)
        {
            signalNumber = 0;
        }

        if (outputs.Count > 0)
            ShowConnections();

        if (connectionRenderers != null) {
            for (int index = 0; index < connectionRenderers.Count; ++index) {
                counter += Time.deltaTime;
                if (counter > 0.5f && signalObjs.Count > 0) {
                    counter = 0;

                    signalObject = signalObjs[index];
                    GameObject tri1 = Instantiate(signalObject, this.gameObject.transform.position, Quaternion.identity);
                    tri1.GetComponent<LineShape>().positionA = connectionRenderers[index].GetComponent<LineRenderer>().GetPosition(0);
                    tri1.GetComponent<LineShape>().positionB = connectionRenderers[index].GetComponent<LineRenderer>().GetPosition(1);
                }
            }
        }

        if (zooming && zoomed) {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), Time.deltaTime * zoomSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 0.5f, Time.deltaTime * zoomSpeed);
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z)) < 0.1f)
                zooming = false;
        }

        else if (zooming && !zoomed) {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, 0, Camera.main.transform.position.z), Time.deltaTime * zoomSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5, Time.deltaTime * zoomSpeed);
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(0, 0, Camera.main.transform.position.z)) < 0.1f)
                zooming = false;
        }

        // Zoom Out
        if (Input.GetMouseButtonDown(0) && zoomed && !zooming &&
            Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) > 11.01f) {
            Debug.Log(Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position));
            zooming = true;
            zoomed = false;
            GetComponent<CircleCollider2D>().enabled = true;
            foreach (GameObject node in inputNodes)
                node.SetActive(false);
            foreach (GameObject node in outputNodes)
                node.SetActive(false);
            foreach (GameObject node in settingNodes)
                node.SetActive(false);
        }

        pbCounter = inputs.Count;

    }

    public override void PlaceSignal(GameObject _outputTo) 
    {
        Debug.Log("Done a thing from placesignal");
        if (inputs.Count > 0)
            signalObjs.Add(inputs[selectedIndex].GetComponent<aNode>().signalObject);
        base.PlaceSignal(_outputTo);
    }

    //void SetPositions(float distance = 0.75f, bool type = false) {
    //    List<GameObject> activeSubNodes = new List<GameObject>();
    //    foreach (GameObject node in subNodes)
    //        if (node.activeSelf)
    //            activeSubNodes.Add(node);

    //    float range = 180;
    //    float interval = range / (activeSubNodes.Count + 1);
    //    for (int i = 0; i < activeSubNodes.Count; ++i) {
    //        float position;
    //        position = i * interval + interval;
    //        if (position < 270)
    //            position += 270;
    //        else
    //            position -= 90;
    //        Vector3 newPos = new Vector3((Mathf.Sin(position * Mathf.Deg2Rad)) * distance, (Mathf.Cos(position * Mathf.Deg2Rad)) * distance, 2);
    //        activeSubNodes[i].transform.localPosition = newPos;
    //        if (type) {
    //            activeOuNodes[i].transform.localPosition = new Vector3(newPos.x, -newPos.y, newPos.z);
    //            activeOuNodes[i].SetActive(true);
    //        }

    //        activeSubNodes[i].SetActive(true);
    //    }
    //}
}
