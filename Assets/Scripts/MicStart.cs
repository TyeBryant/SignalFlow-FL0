using System.Collections;
using System.Collections.Generic;
using nodeFunctionality;
using UnityEngine;

public class MicStart : MonoBehaviour
{
    public GameObject signalObjectPrefab;
    public GameObject signalObject;

    private GameObject signal;
    public Transform nodePoweredTransform;

	// Use this for initialization
	void Start ()
    {
        GameObject signal = Instantiate(signalObjectPrefab, this.transform.position, this.transform.rotation);

        signalObject = signal;

        signalObject.GetComponent<SignalFlowHolder>().signalFlowObjectType = this.GetComponent<Node>().signalObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (this.GetComponent<Node>().powering != null)
        {
            Debug.Log("powering");
            nodePoweredTransform = this.GetComponent<Node>().powering.transform;
            signalObject.transform.position = nodePoweredTransform.position;
        }
	}
}
