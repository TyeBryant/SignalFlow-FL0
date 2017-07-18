using System.Collections;
using System.Collections.Generic;
using nodeFunctionality;
using gameManagement;
using UnityEngine;

public class MicStart : MonoBehaviour
{
    public GameObject signalObjectPrefab;
    public GameObject signalObject;

    //Can be used by gameobject to check bools
    public GameObject currentNode;
    public GameObject previousNode;

    private GameObject signal;
    public Transform nodePoweredTransform;

    public List<GameObject> previousNodeList;

    public GameManager gameManager;

    // Use this for initialization
    void Start ()
    {
        GameObject signal = Instantiate(signalObjectPrefab, this.transform.position, this.transform.rotation);

        signalObject = signal;

        signalObject.GetComponent<SignalFlowHolder>().signalFlowObjectType = this.GetComponent<Node>().signalObject;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        currentNode = this.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (currentNode.GetComponent<Node>().powering != null)
        {
            Debug.Log("powering");
            nodePoweredTransform = currentNode.GetComponent<Node>().powering.transform;
            signalObject.transform.position = nodePoweredTransform.position;

            GameObject currentNodeHolder = currentNode.GetComponent<Node>().powering;
            currentNode = currentNodeHolder;
            previousNode = currentNode.GetComponent<Node>().receiving;
            previousNodeList.Add(previousNode);
        }

        if (previousNode.GetComponent<Node>().powering != currentNode)
        {
            int index = previousNodeList.Count - 1;
            int removeIndex = previousNodeList.Count;
            currentNode = previousNodeList[index];
            signalObject.transform.position = currentNode.transform.position;

            previousNodeList.RemoveAt(index);
        }
        //Set up for multiple inputs
        Debug.Log(previousNodeList.Count);

        //Setting the material of the signal
        currentNode.GetComponent<Node>().signalObject = signalObject.GetComponent<SignalFlowHolder>().signalFlowObjectType;


        if (currentNode.GetComponent<Node>().type == Node.NodeType.ENT_DAW)
        {
            gameManager.DAWConnected = true;
        }

        if (currentNode.GetComponent<Node>().type == Node.NodeType.ENT_MONITOR)
        {
            gameManager.monitorConnected = true;
        }

        if (currentNode.GetComponent<Node>().type == Node.NodeType.ENT_DAW)
        {
            gameManager.DAWConnected = true;
        }
    }
}
