using System.Collections;
using System.Collections.Generic;
using nodeFunctionality;
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

	// Use this for initialization
	void Start ()
    {
        GameObject signal = Instantiate(signalObjectPrefab, this.transform.position, this.transform.rotation);

        signalObject = signal;

        signalObject.GetComponent<SignalFlowHolder>().signalFlowObjectType = this.GetComponent<Node>().signalObject;

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
        }

        //Check to make sure previous and current node are still connected

        //Set up for multiple inputs
	}
}
