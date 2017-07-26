using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;

public class SignalFlowStart : MonoBehaviour
{
    public GameObject signalObjectPrefab;

    //[HideInInspector]
    public GameObject signalObject;

    private GameManager gameManager;

    // Use this for initialization
    void Start ()
    {
        GameObject signal = Instantiate(signalObjectPrefab, this.transform.position, this.transform.rotation);

        signalObject = signal;

        signalObject.GetComponent<SignalFlowObject>().signalFlowObjectType = this.GetComponent<aNode>().signalObject;

        signalObject.GetComponent<SignalFlowObject>().currentNode = this.gameObject;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
}
