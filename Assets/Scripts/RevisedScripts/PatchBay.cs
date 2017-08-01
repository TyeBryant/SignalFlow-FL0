using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatchBay : aNode, IPointerClickHandler {
    public List<GameObject> inputNodes = new List<GameObject>();
    public List<GameObject> outputNodes = new List<GameObject>();
    public List<GameObject> settingNodes = new List<GameObject>();
    List<Vector3> inputNodePos = new List<Vector3>();

    List<GameObject> activeInNodes = new List<GameObject>();
    List<GameObject> activeOuNodes = new List<GameObject>();

    public bool zoomed, zooming;
    float zoomSpeed = 3;
    public int pbCounter;

    private new void Start()
    {
        base.Start();
        for (int index = 0; index < inputNodes.Count; ++index) {
            inputNodes[index].SetActive(false);
            outputNodes[index].SetActive(false);
            settingNodes[index].SetActive(false);
            inputNodePos.Add(inputNodes[index].transform.position);
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
                inputNodes[index].transform.position = inputNodePos[index];
                outputNodes[index].SetActive(true);
                settingNodes[index].SetActive(true);
            }

            //foreach (GameObject node in inputNodes)
            //    node.SetActive(true);
            //foreach (GameObject node in outputNodes)
            //    node.SetActive(true);
            //foreach (GameObject node in settingNodes)
            //    node.SetActive(true);
            //for (int i = 0; i <= inputs.Count - 1; i++)
            //{
            //    inputNodes[i].SetActive(true);
            //    outputNodes[i].SetActive(true);

            //    inputNodes[i].GetComponent<aNode>().inputs.Add(inputs[i]);

            //    //foreach (GameObject nodes in inputNodes)
            //    //{
            //    //    nodes.GetComponent<aNode>().maximumInputs = 1;
            //    //    nodes.GetComponent<aNode>().maximumOutputs = 1;
            //    //}
            //}

            GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (eventData.clickCount < 2 && !zoomed) {
            // Show Outputs
            if (outputNodes[0].activeSelf)
                foreach (GameObject node in outputNodes)
                    node.SetActive(false);
            else
            //foreach (GameObject node in outputNodes) {
            //    node.SetActive(true);
            {
                pbCounter++;
                foreach (GameObject node in inputNodes)
                    node.SetActive(false);
                activeInNodes.Clear();
                for (int i = 0; i < pbCounter-1; ++i)
                {
                    activeInNodes.Add(inputNodes[i]);
                    inputNodes[i].SetActive(true);
                    SetPositions(0.75f);
                }
            }

        }
        
        
    }

    new void Update() {
        base.Update();

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

        ////Dismiss the middle ground if there is no input
        //if(inputs.Count == 0)
        //{
        //    foreach (GameObject node in settingNodes)
        //        node.SetActive(false);
        //}

        //original code for spawning the node outside the patch bay

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    pbCounter++;
        //    foreach (GameObject node in inputNodes)
        //        node.SetActive(false);
        //    activeInNodes.Clear();
        //    for (int i = 0; i < pbCounter; ++i)
        //    {
        //        activeInNodes.Add(inputNodes[i]);
        //        inputNodes[i].SetActive(true);
        //        SetPositions(0.75f);
        //    }
        //}

        pbCounter = inputs.Count;

    }

    public override void PlaceSignal(GameObject _outputTo) 
    {
        if (!outputs.Contains(_outputTo)) {
            //If this node's channel is null, and the output's node is not null or multiple
            if (nodeChannel == Channel.EC_NULL && (_outputTo.GetComponent<aNode>().nodeChannel != Channel.EC_NULL || _outputTo.GetComponent<aNode>().nodeChannel != Channel.EC_MULTI)) {
                nodeChannel = _outputTo.GetComponent<aNode>().nodeChannel;
            }
            else
                _outputTo.GetComponent<aNode>().nodeChannel = nodeChannel;

            connectionManager.inputFrom = null;
            connectionManager.isCarryingSignal = false;

            _outputTo.GetComponent<aNode>().isPowered = true;
            _outputTo.GetComponent<aNode>().inputs.Add(this.gameObject);
            outputs.Add(_outputTo);

            GameObject lineRendObj = Instantiate(lineRenderPrefab);
            connectionRenderers.Add(lineRendObj);

            LineRenderer lineRend = lineRendObj.GetComponent<LineRenderer>();

            Vector3 startPos = transform.position;
            startPos.z = 0;

            Vector3 endPos = _outputTo.transform.position;
            endPos.z = 0;

            lineRend.SetPositions(new Vector3[] { startPos, endPos });

            if (outputs.Count > 1) {
                GameObject signal = Instantiate(signalFlowHolder, _outputTo.transform.position, _outputTo.transform.rotation);
                signal.GetComponent<SignalFlowObject>().previousNode = this.gameObject;
                signal.GetComponent<SignalFlowObject>().currentNode = _outputTo;

                gameManager.signalNodes.Add(signal);

                signal.GetComponent<SignalFlowObject>().signalFlowObjectType = this.gameObject.GetComponent<aNode>().signalObject;
                _outputTo.GetComponent<aNode>().signalObject = signal.GetComponent<SignalFlowObject>().signalFlowObjectType;
            }
        }
    }

    void SetPositions(float distance = 0.75f, bool type = false) {

        float range = 180;
        float interval = range / (activeInNodes.Count + 1);
        for (int i = 0; i < activeInNodes.Count; ++i) {
            float position;
            position = i * interval + interval;
            if (position < 270)
                position += 270;
            else
                position -= 90;
            Vector3 newPos = new Vector3((Mathf.Sin(position * Mathf.Deg2Rad)) * distance, (Mathf.Cos(position * Mathf.Deg2Rad)) * distance, 2);
            activeInNodes[i].transform.localPosition = newPos;
            if (type) {
                activeOuNodes[i].transform.localPosition = new Vector3(newPos.x, -newPos.y, newPos.z);
                activeOuNodes[i].SetActive(true);
            }

            activeInNodes[i].SetActive(true);
        }
    }
}
