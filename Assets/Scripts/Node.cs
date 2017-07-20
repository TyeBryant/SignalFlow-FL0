using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dial;

namespace nodeFunctionality
{
    public class Node : MonoBehaviour
    {

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
            ENT_HEADPHONES,
            ENT_HEADPHONEAMP,
            ENT_OTHERPATCHBAYNODES,
            ENT_OUTPUT,
            ENT_INPUT,
            ENT_MIXB,
            ENT_MIXBMAINMIX,
            ENT_MIXBTOMAINMIX,
            ENT_MAINMIX,
            ENT_DEAD
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

        [Tooltip("Determines whether the node can support multiple inputs")]
        public int numInputs = 1;

        [HideInInspector]
        public bool isPowered;

        //[HideInInspector]
        public GameObject powering, receiving;

        [HideInInspector]
        public List<GameObject> lPowering = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> lReceiving = new List<GameObject>();

        [HideInInspector]
        public ConnectionManager conMan;

        [HideInInspector]
        public LineRenderer lineRend;

        public DialRotation Knob;
        public Slider Fader;

        [HideInInspector]
        public float knobValue;
        [HideInInspector]
        public float faderValue;

       
        public GameObject signalObject;

        [HideInInspector]
        public float counter;

        [HideInInspector]
        public Vector2 endPos;

        public void Start()
        {


            //Set up the LineRenderer
            lineRend = this.gameObject.AddComponent<LineRenderer>();
            LineRendererSetup(lineRend);

            //Safety falses
            isPowered = false;

            //Stop playtesting if the channel or type is not set
            if (channel == SignalChannel.ESC_NULL || type == NodeType.ENT_NULL)
                Debug.LogError("A signal channel or type has not been defined for a node!");

            if (acceptedChannels.Length == 0 || acceptedTypes.Length == 0)
                Debug.LogError("The accepted channels or types have not been set for this object!");

            if (numInputs == 0 && type != NodeType.ENT_MICROPHONE)
                Debug.LogError("The number of acceptable inputs has not been set for this object!");

            //If I'm a microphone make me the starting node
            if (type == NodeType.ENT_MICROPHONE)
                isPowered = true;

            //Find the connection manager in the scene
            conMan = FindObjectOfType<ConnectionManager>();
            
            //For certain note(exp DAW) have multiple input
            if (type == NodeType.ENT_DAW)
                numInputs = 100;

            counter = 0;
        }

        public void Update()
        {
            if (Knob == null)
            {
                knobValue = 1;
            }
            else
            {
                knobValue = Knob.GetComponent<DialRotation>().dialValue;
            }

            if (Fader == null)
            {
                faderValue = 1;
            }
            else
            {
                faderValue = Fader.GetComponent<Slider>().value;
            }

            //If I'm connected, call Connected
            if (powering)
            {
                counter += Time.deltaTime;
                if (counter > 0.5f)
                {
                    counter = 0;
                    GameObject tri1 = Instantiate(signalObject, this.gameObject.transform.position, Quaternion.identity);
                    tri1.GetComponent<LineShape>().positionA = this.transform.position;
                    tri1.GetComponent<LineShape>().positionB = endPos;
                }
                Connected(powering);
            }
            if (type == NodeType.ENT_DEAD) {
                powering = null;
                lPowering.Clear();
            }
        }

        //The Sprite has been clicked
        public void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Allow the player to pick up a signal
                if (isPowered && !conMan.isCarryingSignal && knobValue > 0 && faderValue > 0)
                {
                    if (powering)
                    {
                        //MassDisconnect();
                    }
                    else
                        conMan.ProvideSignal(this.gameObject);
                }

                //Allow the player to place a signal down
                else if (!isPowered && conMan.isCarryingSignal)
                {
                    EstablishSignalConnection();
                }
                else if (isPowered && conMan.isCarryingSignal)
                {
                    print("Running Function");
                    EstablishSignalConnection();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (powering)
                {
                    MassDisconnect();
                }
            }
        }

        void EstablishSignalConnection()
        {
            if (TypeChannelCheck(conMan) && lReceiving.Count < numInputs)
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
            endPos = poweredObject.transform.position;



        }

        public void MassDisconnect()
        {
            //Check if this node is connected
            if (powering)
            {
                powering.GetComponent<Node>().lReceiving.Remove(this.gameObject);


                if (powering.GetComponent<Node>().lReceiving.Count == 0)
                {
                    powering.GetComponent<Node>().receiving = null;
                    powering.GetComponent<Node>().isPowered = false;
                    powering.GetComponent<Node>().lPowering.Clear();
                    lPowering.Remove(powering);

                    powering.GetComponent<Node>().MassDisconnect();
                    powering = null;


                }

                lineRend.SetPosition(1, this.transform.position);
                if (powering)
                    lPowering.Remove(powering);
                if (lPowering.Count == 0)
                    powering = null;
            }
        }

        public void DisconnectNode(GameObject poweredNode)
        {
            if (lPowering.Contains(poweredNode))
            {
                if (poweredNode.GetComponent<Node>().lReceiving.Count <= 1)
                    poweredNode.GetComponent<Node>().MassDisconnect();
                lPowering.Remove(poweredNode);
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
            foreach (NodeType c in acceptedTypes)
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

        //Set up everything to do with Line Renderers
        void LineRendererSetup(LineRenderer l)
        {
            l.startWidth = 0.5f;
            l.endWidth = 0.01f;
        }
    }
}

