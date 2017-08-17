using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetterSceneChange : MonoBehaviour {

    public string LevelName;
    public AudioClip levelSelectSound;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LevelSelect()
    {
       // AudioManager.Instance.PlayClip(levelSelectSound, AudioManager.Instance.GetChannel("SFX"));
        SceneManager.LoadScene(LevelName);
    }
}
