using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;

public class SignalFlowObject : MonoBehaviour
{
    [HideInInspector]
    public GameObject currentNode;

    [HideInInspector]
    public GameObject previousNode;

    public Transform nodePoweredTransform;

    [HideInInspector]
    public List<GameObject> previousNodeList;

    public GameObject signalFlowObjectType;

    public GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNode.GetComponent<aNode>().outputs == null)
        {
            return;
        }

        if (previousNodeList == null)
        {
            return;
        }

        if (currentNode.GetComponent<aNode>().outputs != null)
        {
            nodePoweredTransform = currentNode.GetComponent<aNode>().outputs[0].transform;
            this.transform.position = nodePoweredTransform.position;

            previousNode = currentNode;
            previousNodeList.Add(previousNode);

            //Some game manager stuff
            gameManager.previousNodeList.Add(previousNode);
            aNode.Type type = previousNode.GetComponent<aNode>().nodeType;
            gameManager.previousNodeListTypes.Add(type);

            GameObject currentNodeHolder = currentNode.GetComponent<aNode>().outputs[0];
            currentNode = currentNodeHolder;
        }

        if (currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_MICROPHONE && previousNodeList.Count != 0)
        {
            if (previousNode.GetComponent<aNode>().outputs[0] != currentNode)
            {
                int index = previousNodeList.Count - 1;
                int removeIndex = previousNodeList.Count;
                currentNode = previousNodeList[index];
                this.transform.position = currentNode.transform.position;

                GameObject pn = previousNodeList[index];
                aNode.Type pnType = pn.GetComponent<aNode>().nodeType;
                previousNodeList.RemoveAt(index);

                gameManager.previousNodeList.Remove(pn);
                gameManager.previousNodeListTypes.Remove(pnType);
            }
        }

        //If list is empty and current node doesn't equal mic, kill yourself
        if (previousNodeList.Count == 0 && currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_MICROPHONE)
        {
            Destroy(this.gameObject);
        }

        //Setting the material of the signal
        currentNode.GetComponent<aNode>().signalObject = this.GetComponent<SignalFlowHolder>().signalFlowObjectType;
    }
}
