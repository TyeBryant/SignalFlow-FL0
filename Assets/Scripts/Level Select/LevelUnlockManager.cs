using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockManager : MonoBehaviour {

    public int CurrentLevel;
    public List<GameObject> levelconnecters = new List<GameObject>();

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetInt("CurrentLevel") < CurrentLevel)
        {
            PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
        }
        else 
        {
           CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        levelconnecters.AddRange(GameObject.FindGameObjectsWithTag("LevelConnecters"));
        foreach (GameObject levelconnecter in levelconnecters)
        {
            if(levelconnecter.GetComponent<LevelConnecters>().LevelConnecterNum > CurrentLevel -1)
            {
                levelconnecter.SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("m"))
        {
            PlayerPrefs.DeleteKey("CurrentLevel");
        }
	}
}
