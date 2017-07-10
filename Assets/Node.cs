using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public enum NodeType
    {
        ENT_NULL,
        ENT_MICROPHONE,
        ENT_JUNCTIONBOX,
        ENT_DIRECTOUT,
        ENT_PREAMP,
        ENT_CHANNELFADER,
        ENT_MASTERFADER,
        ENT_DAW,
        ENT_PATCHBAY,
        ENT_CHANNELS,
        ENT_AUXSENDS,
        ENT_AUXMASTERS,
        ENT_MONITOR,
        ENT_HEADPHONES
    }

    public enum SignalChannel
    {
        ESC_NULL,
        ESC_ONE,
        ESC_TWO,
        ESC_THREE,
        ESC_FOUR,
        ESC_ALL
    }

    [Tooltip("Determines my node type")]
    public NodeType type;

    [Tooltip("Determines what type of node I can accept power from")]
    public NodeType[] acceptedTypes;

    [Tooltip("Determines the signals that I can accept")]
    public SignalChannel channel;

    [Tooltip("Determines what signals I can accept from")]
    public SignalChannel[] acceptedChannels;

    [Tooltip("Determines the colour of the signal")]
    public Color signalColour;

    [HideInInspector]
    public bool isPowered;

    [HideInInspector]
    public GameObject powering, receiving;

    [HideInInspector]
    public ConnectionManager conMan;

    [HideInInspector]
    public LineRenderer lineRend;

    public void Start()
    {
        //Set up the LineRenderer
        lineRend = this.gameObject.AddComponent<LineRenderer>();
        lineRend.startWidth = 0.5f;
        lineRend.endWidth = 0.01f;
        lineRend.startColor = signalColour;
        lineRend.endColor = signalColour;

        //Safety falses
        isPowered = false;

        //Stop playtesting if the channel or type is not set
        if (channel == SignalChannel.ESC_NULL || type == NodeType.ENT_NULL)
            Debug.LogError("A signal channel or type has not been defined for a node!");      

        //If I'm a microphone make me the starting node
        if (type == NodeType.ENT_MICROPHONE)
            isPowered = true;

        //Find the connection manager in the scene
        conMan = FindObjectOfType<ConnectionManager>();    
    }

    public void Update()
    {
        //If I'm connected, call Connected
        if (powering)
        {
            Connected(powering);
        }
    }

    //The Sprite has been clicked
    private void OnMouseDown()
    {   
        //Allow the player to pick up a signal
        if(isPowered && !conMan.isCarryingSignal)
        {
            if (powering)
                MassDisconnect();
            else
                conMan.ProvideSignal(this.gameObject);
        }
        //Allow the player to place a signal down
        else if(!isPowered && conMan.isCarryingSignal)
        {
            EstablishSignalConnection();
        }
        else if(isPowered && conMan.isCarryingSignal)
        {
            EstablishSignalConnection();
        }
    }

    void EstablishSignalConnection()
    {
        if (TypeChannelCheck(conMan))
        {
            conMan.ConnectToNode(this.gameObject);
            conMan.DisconnectSignal();
            isPowered = true;
        }
    }

    //If the node is connected then draw a line to the other node
    public void Connected(GameObject poweredObject)
    {
        lineRend.SetPosition(0, this.gameObject.transform.position);
        lineRend.SetPosition(1, poweredObject.transform.position); 
    }    

    public void MassDisconnect()
    {
        //Check if this node is connected
        if (powering)
    {
            powering.GetComponent<Node>().receiving = null;
            powering.GetComponent<Node>().isPowered = false;
            powering.GetComponent<Node>().MassDisconnect();          
            powering = null;
            lineRend.SetPosition(1, this.transform.position);
        }
    }

    bool TypeChannelCheck(ConnectionManager connectionManager)
    {
        if (CheckType(connectionManager.powerFrom.GetComponent<Node>().type) && CheckChannel(connectionManager.powerFrom.GetComponent<Node>().channel))
            return true;    

        return false;
    }

    bool CheckType(NodeType givenType)
    {
        //Search through the array of possible type inputs
        foreach(NodeType c in acceptedTypes)
        {
            //If the given type is the same as c, then return true
            if (givenType == c)
                return true;
        }
        
        return false;
    }

    bool CheckChannel(SignalChannel givenChannel)
    {
        //If the given signal is for all or I am an all, return true
        if (givenChannel == SignalChannel.ESC_ALL || channel == SignalChannel.ESC_ALL)
            return true;

        //Otherwise, search through the given array of possible inputs
        foreach (SignalChannel c in acceptedChannels)
        {
            //If there is a channel that is the same as the given channel, return true
            if (givenChannel == c)
                return true;
        }

        //Otherwise return false
        return false;
    }
}
