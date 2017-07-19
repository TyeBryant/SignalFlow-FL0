using System.Collections;
using System.Collections.Generic;
using nodeFunctionality;
using gameManagement;
using UnityEngine;

public class SignalFlowHolder : MonoBehaviour
{
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
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I'm in the same spot!");
    }
}
