using System.Collections;
using System.Collections.Generic;
using nodeFunctionality;
using UnityEngine;

public class SignalColourPicker : MonoBehaviour {

    public GameObject parent;
    public Color signalColour;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown() {
        parent.GetComponent<Node>().signalColour = signalColour;
        parent.GetComponent<SpriteRenderer>().color = new Color(signalColour.r, signalColour.g, signalColour.b, 255);
    }
}
