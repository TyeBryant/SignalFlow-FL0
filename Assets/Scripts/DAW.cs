using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAW : Node {

    public List<Color> signalColours = new List<Color>();
    public List<GameObject> colourPickers = new List<GameObject>();

	// Use this for initialization
	void Start () {
		base.Start();
        numInputs = 2;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
        //knobValue = 1;
        //faderValue = 1;
	}

    private void OnMouseDown() {
        base.OnMouseDown();
        signalColours.Clear();
        foreach (GameObject node in lReceiving) {
            if (!signalColours.Contains(node.GetComponent<Node>().signalColour))
                signalColours.Add(node.GetComponent<Node>().signalColour);
        }

        for (int i = 0; i < signalColours.Count; ++i) {
            if (signalColours[i] != null && colourPickers[i] != null) {
                colourPickers[i].SetActive(true);
                colourPickers[i].GetComponent<SignalColourPicker>().signalColour = signalColours[i];
                colourPickers[i].GetComponent<SpriteRenderer>().color = new Color(signalColours[i].r, signalColours[i].g, signalColours[i].b, 255); // Simply changing to the stored colour sets alpha to 0
            }
            else if (signalColours[i] != null)
                colourPickers[i].SetActive(false);
        }
        

    }
}
