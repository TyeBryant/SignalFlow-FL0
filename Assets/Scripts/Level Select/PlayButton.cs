using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayButton : MonoBehaviour {

    public LevelUnlockManager levelUnlockManager;
    public int myLevel;
    public Button button; 
   

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (myLevel <= levelUnlockManager.CurrentLevel)
        {
            button.interactable = true;
        }
	}
}
