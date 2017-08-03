using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;

public class aNode : MonoBehaviour
{

    /// <summary>
    /// This is a new Node script is intended to be a parent script
    /// for every nodetype possible.
    /// 
    /// This script should be able to:
    /// - Define what type of object this node is
    /// - Define what signals this node is a part of
    /// - Accept signal from specific types of nodes
    /// - Connect to those nodes on click
    /// - Handle both multiple inputs and outputs
    /// </summary>
    /// 

    //An enum used to define the type of node that this is
    public enum Type
    {
        ET_NULL,
        ET_MICROPHONE,
        ET_JUNCTIONBOX,
        ET_DIRECTOUT,
        ET_PREAMP,
        ET_CHANNELFADER,
        ET_MASTERFADER,
        ET_DAW,
        ET_PATCHBAY,
        ET_CHANNELS,
        ET_AUXSENDS,
        ET_AUXMASTERS,
        ET_MONITOR,
        ET_HEADPHONES,
        ET_HEADPHONEAMP,
        ET_OTHERPATCHBAYNODES,
        ET_OUTPUT,
        ET_INPUT,
        ET_MIXB,
        ET_MIXBMAINMIX,
        ET_MIXBTOMAINMIX,
        ET_MAINMIX,
        ET_LOCAL,
        ET_REVERB,
        ET_COMPRESSION,
        ET_DEAD
    }

    //An enum used to determine what channel I am a part of
    public enum Channel
    {
        EC_NULL,
        EC_ONE,
        EC_TWO,
        EC_THREE,
        EC_FOUR,
        EC_FIVE,
        EC_MULTI
    }

    [Tooltip("Determines the type of this node")]
    public Type nodeType;

    [Tooltip("Determines what types I can accept a signal from")]
    public Type[] acceptableTypes;

    [Tooltip("Determines the channel of this node")]
    public Channel nodeChannel;

    public int maximumInputs, maximumOutputs;

    public bool isPowered = false;

    public GameObject lineRenderPrefab;

    //[HideInInspector]
    public List<GameObject> inputs = new List<GameObject>(), outputs = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> connectionRenderers = new List<GameObject>();

    public GameObject signalObject;

    [HideInInspector]
    public float counter;

    [HideInInspector]
    public aConnectionManager connectionManager;

    public GameObject signalFlowHolder;

    public GameManager gameManager;

    // Use this for initialization
    public void Start()
    {

        ///Errors and Warnings
        //If the node type is not define, throw an error
        if (nodeType == Type.ET_NULL)
            Debug.LogError(this.gameObject.name + " Does not have a defined nodeType!");

        //If the node isn't a microphone and there isn't an acceptable type
        if (nodeType != Type.ET_MICROPHONE && acceptableTypes.Length == 0)
            Debug.LogError(this.gameObject.name + " does not hold a list of acceptable types");

        //If there's a connection manager
        if (FindObjectOfType<aConnectionManager>())
            connectionManager = FindObjectOfType<aConnectionManager>();
        //If there's no connection manager
        else
            Debug.LogError("There is no connection manager present on the scene");

        if (maximumInputs == 0 || maximumOutputs == 0)
            Debug.LogError(this.gameObject.name + " has no designated number of maximum outputs or inputs");

        counter = 0;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        //Only execute if this is above 0
        if (outputs.Count > 0)
        {
            ShowConnections();
        }

        if (connectionRenderers != null)
        {
            for (int index = 0; index < connectionRenderers.Count; index++)
            {
                counter += Time.deltaTime;
                if (counter > 0.5f)
                {
                    counter = 0;
                    GameObject tri1 = Instantiate(signalObject, this.gameObject.transform.position, Quaternion.identity);
                    tri1.GetComponent<LineShape>().positionA = connectionRenderers[index].GetComponent<LineRenderer>().GetPosition(0);
                    tri1.GetComponent<LineShape>().positionB = connectionRenderers[index].GetComponent<LineRenderer>().GetPosition(1);
                }
            }
        }
    }

    //When the sprite has been moused over
    public virtual void OnMouseOver()
    {
        //When the player clicks
        if (Input.GetMouseButtonDown(0))
        {
            //if this node is powered and the connection manager is carrying a signal
            if (!connectionManager.isCarryingSignal)
            {
                //  Manage picking up the signal
                if (outputs.Count < maximumOutputs)
                {
                    print("MaxOut: " + maximumOutputs);
                    connectionManager.CarrySignal(this.gameObject);
                }
            }
        }

        //Used for placing the signal exclusively
        if (Input.GetMouseButtonUp(0))
        {
            if (connectionManager.isCarryingSignal && connectionManager.inputFrom != this.gameObject)
            {
                if (!isPowered)
                {
                    //If check returns true, then run place signal on the connection amanger
                    if (Check())
                        connectionManager.inputFrom.GetComponent<aNode>().PlaceSignal(this.gameObject);
                }
                else if (isPowered)
                {
                    //If check returns true and I can still accept inputs
                    if (Check() && inputs.Count < maximumInputs)
                        connectionManager.inputFrom.GetComponent<aNode>().PlaceSignal(this.gameObject);
                }
            }
        }
    }

    public void ShowConnections()
    {
        outputs.ForEach((GameObject g) => Debug.DrawLine(this.gameObject.transform.position, g.transform.position, Color.blue));
    }

    public void Disconnect(GameObject outObj)
    {


        /*//For all object's getting a signal from me
        int tempCount = outputs.Count;
        for (int i = 0; i < tempCount; ++i)
        {
            aNode outN = outputs[0].GetComponent<aNode>();

            //Remove this node from it's input list
            outN.inputs.Remove(this.gameObject);
            print(outN.inputs.Count);
            //If there is no more inputs from anything,this node is no longer powered
            if (outN.inputs.Count == 0)
                outN.isPowered = false;

            //Remove the object from the outputs list
            outputs.Remove(outputs[0]);

            //Destroy(connectionRenderers[0]);
            connectionRenderers.Remove(connectionRenderers[0]);
        }*/
    }

    //Checks whether or not a connection can be established
    public bool Check()
    {
        bool ret = false;
        bool ret2 = false;
        //Search through all types
        foreach (Type t in acceptableTypes)
        {
            //If the type is the same as accepted turn ret to true
            if (t == connectionManager.inputFrom.GetComponent<aNode>().nodeType)
            {
                ret = true;
                print("checked");
            }
        }

        //If the node's channel is null, or the nodes channel is the same as mine, return true if not, return false
        if (connectionManager.inputFrom.GetComponent<aNode>().nodeChannel == Channel.EC_NULL)
            ret2 = true;
        else if (connectionManager.inputFrom.GetComponent<aNode>().nodeChannel == nodeChannel)
            ret2 = true;
        else
            ret2 = false;

        return (ret && ret2);
    }

    //Place the signal down on the outputting node
    public virtual void PlaceSignal(GameObject _outputTo)
    {
        if (!outputs.Contains(_outputTo))
        {
            //If this node's channel is null, and the output's node is not null or multiple
            if (nodeChannel == Channel.EC_NULL && (_outputTo.GetComponent<aNode>().nodeChannel != Channel.EC_NULL || _outputTo.GetComponent<aNode>().nodeChannel != Channel.EC_MULTI))
            {
                nodeChannel = _outputTo.GetComponent<aNode>().nodeChannel;
            }
            else
                _outputTo.GetComponent<aNode>().nodeChannel = nodeChannel;

            connectionManager.inputFrom = null;
            connectionManager.isCarryingSignal = false;

            _outputTo.GetComponent<aNode>().isPowered = true;
            _outputTo.GetComponent<aNode>().inputs.Add(this.gameObject);
            outputs.Add(_outputTo);

            GameObject lineRendObj = Instantiate(lineRenderPrefab, transform.position, Quaternion.identity);
            lineRendObj.GetComponent<LineRendCol>().startPoint = transform.position;
            lineRendObj.GetComponent<LineRendCol>().endPoint = _outputTo.transform.position;
            lineRendObj.GetComponent<LineRendCol>().outNode = _outputTo;
            lineRendObj.GetComponent<LineRendCol>().node = this.GetComponent<aNode>();
            connectionRenderers.Add(lineRendObj);

            LineRenderer lineRend = lineRendObj.GetComponent<LineRenderer>();

            if (lineRendObj.name == "PB_LineRendererPrefab" || lineRendObj.name == "PB_LineRendererPrefab(Clone)")
                lineRend.SetPositions(new Vector3[] { new Vector3(transform.position.x, transform.position.y, 0), new Vector3(_outputTo.transform.position.x, _outputTo.transform.position.y, 0) });
            else
                lineRend.SetPositions(new Vector3[] { transform.position, _outputTo.transform.position });

            if (outputs.Count > 1)
            {
                GameObject signal = Instantiate(signalFlowHolder, _outputTo.transform.position, _outputTo.transform.rotation);
                signal.GetComponent<SignalFlowObject>().previousNode = this.gameObject;
                signal.GetComponent<SignalFlowObject>().currentNode = _outputTo;

                gameManager.signalNodes.Add(signal);

                signal.GetComponent<SignalFlowObject>().signalFlowObjectType = this.gameObject.GetComponent<aNode>().signalObject;
                _outputTo.GetComponent<aNode>().signalObject = signal.GetComponent<SignalFlowObject>().signalFlowObjectType;
            }
        }
    }
}
