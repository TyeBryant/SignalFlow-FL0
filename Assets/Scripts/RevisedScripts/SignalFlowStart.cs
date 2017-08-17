using System.Collections;
using System.Collections.Generic;
using gameManagement;
using System.Linq;
using UnityEngine;

public class SignalFlowStart : MonoBehaviour
{ 
    public GameManager gameManager;
    public GameObject signalObjectPrefab;

    [HideInInspector]
    public GameObject signalObject;

    //Win state things
    public List<aNode.Type> signalRequirements;

    public List<aNode.Type> banList;

    [HideInInspector]
    //Win state tracking
    public List<aNode.Type> previousNodeListTypes;

    [HideInInspector]
    public List<GameObject> previousNodeList;

    [HideInInspector]
    public List<GameObject> signalObjects;

    public int signalNumber;

    [HideInInspector]
    private List<aNode> Nodes;

    [HideInInspector]
    public List<GameObject> nodeObjects;

    public bool win;

    [HideInInspector]
    public bool notOkay;

    public bool winChecked;

    public GameObject DAW;
    public GameObject PatchBay;

    // Use this for initialization
    void Start ()
    {
        winChecked = false;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        GameObject signal = Instantiate(signalObjectPrefab, this.transform.position, this.transform.rotation);

        signalObject = signal;

        signalObject.GetComponent<SignalFlowObject>().signalFlowObjectType = this.GetComponent<aNode>().signalObject;

        signalObject.GetComponent<SignalFlowObject>().currentNode = this.gameObject;
        signalObject.GetComponent<SignalFlowObject>().StartingNode = this.gameObject;
        signalObject.GetComponent<SignalFlowObject>().objectSignalNumber = signalNumber;

        Nodes = Object.FindObjectsOfType<aNode>().ToList();

        for (int index = 0; index < Nodes.Count; index++)
        {
            nodeObjects.Add(Nodes[index].gameObject);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(signalObject != null)
        {
            if (signalObject.GetComponent<SignalFlowObject>().currentNode.GetComponent<aNode>().nodeType == aNode.Type.ET_PATCHBAY)
            {
                CheckWin();
            }
            else if (winChecked == true && signalObject.GetComponent<SignalFlowObject>().currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_PATCHBAY)
            {
                winChecked = false;
                gameManager.winCount -= 1;
            }

            if (previousNodeList.Contains(DAW) && !DAW.GetComponent<aDAW>().signalNumbers.Contains(signalNumber))
            {
                previousNodeList.Remove(DAW);
                previousNodeListTypes.Remove(DAW.GetComponent<aNode>().nodeType);
            }

            if (previousNodeList.Contains(PatchBay) && !PatchBay.GetComponent<aPatchBay>().signalNumbers.Contains(signalNumber))
            {
                previousNodeList.Remove(PatchBay);
                previousNodeListTypes.Remove(PatchBay.GetComponent<aNode>().nodeType);
            }
        }

    }

    public void CheckNodes()
    {
        for (int index = 0; index < Nodes.Count; ++index)
        {
            if (nodeObjects[index].GetComponent<aNode>().nodeSignalNumber == signalNumber)
            {
                if (!previousNodeList.Contains(nodeObjects[index]))
                {
                    previousNodeList.Add(nodeObjects[index]);
                    previousNodeListTypes.Add(nodeObjects[index].GetComponent<aNode>().nodeType);
                }
            }
            if (previousNodeList.Contains(nodeObjects[index]) && nodeObjects[index].GetComponent<aNode>().nodeSignalNumber != signalNumber)
            {
                previousNodeList.Remove(nodeObjects[index]);
                previousNodeListTypes.Remove(nodeObjects[index].GetComponent<aNode>().nodeType);
            }
        }

        if (DAW != null && DAW.GetComponent<aDAW>().signalNumbers.Contains(signalNumber))
        {
            previousNodeList.Add(DAW);
            previousNodeListTypes.Add(DAW.GetComponent<aNode>().nodeType);
        }

        if (PatchBay != null && PatchBay.GetComponent<aPatchBay>().signalNumbers.Contains(signalNumber))
        {
            previousNodeList.Add(PatchBay);
            previousNodeListTypes.Add(PatchBay.GetComponent<aNode>().nodeType);
        }
    }

    public void CheckWin()
    {
        notOkay = banList.Any(i => previousNodeListTypes.Contains(i));
        if (notOkay)
        {
            Debug.Log("no");
        }
        if (!notOkay && winChecked == false)
        {
            win = signalRequirements.All(i => previousNodeListTypes.Contains(i));
            gameManager.winCount += 1;
            winChecked = true;
        }
    }
}
