using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;

public class SignalFlowObject : MonoBehaviour
{
    public List<aNode.Type> signalRequirements;

    //[HideInInspector]
    public GameObject currentNode;

    //[HideInInspector]
    public GameObject previousNode;

    public Transform nodePoweredTransform;

    //[HideInInspector]
    public List<GameObject> previousNodeList;

    public GameObject signalFlowObjectType;

    public GameManager gameManager;

    // Use this for initialization
    void Start ()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (currentNode.GetComponent<aNode>().outputs == null)
        {
            return;
        }

        if (currentNode.GetComponent<aNode>().outputs != null)
        {
            nodePoweredTransform = currentNode.GetComponent<aNode>().outputs[0].transform;
            this.transform.position = nodePoweredTransform.position;

            previousNode = currentNode;
            previousNodeList.Add(previousNode);
            GameObject currentNodeHolder = currentNode.GetComponent<aNode>().outputs[0];
            currentNode = currentNodeHolder;
        }

        if (currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_MICROPHONE)
        {
            if (previousNode.GetComponent<aNode>().outputs[0] != currentNode)
            {
                int index = previousNodeList.Count - 1;
                int removeIndex = previousNodeList.Count;
                currentNode = previousNodeList[index];
                this.transform.position = currentNode.transform.position;

                previousNodeList.RemoveAt(index);
            }
        }

        //If list is empty and current node doesn't equal mic, delete yourself
        if (previousNodeList.Count == 0 && currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_MICROPHONE)
        {
            Destroy(this.gameObject);
        }

        //Setting the material of the signal
        currentNode.GetComponent<aNode>().signalObject = this.GetComponent<SignalFlowHolder>().signalFlowObjectType;

        if (previousNodeList.Count == signalRequirements.Count)
        {
            bool winning = false;
            for (int i = 0; i < signalRequirements.Count; ++i)
            {
                if (previousNodeList[i].GetComponent<aNode>().nodeType == signalRequirements[i])
                {
                    winning = true;
                }
                else
                {
                    winning = false;
                    break;
                }
            }
            if (winning)
                gameManager.win = true;
        }
    }
}
