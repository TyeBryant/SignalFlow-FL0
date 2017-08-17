using System.Collections;
using System.Collections.Generic;
using nodeFunctionality;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace gameManagement
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public bool gamePaused;

        [HideInInspector]
        public SignalFlowStart[] starting;

        [HideInInspector]
        public List<GameObject> startingNodes;

        public int winCount;

        public bool levelWin;

        public string LevelName;
        public AudioClip levelWinSound;

        // Use this for initialization
        void Start()
        {
            starting = GameObject.FindObjectsOfType<SignalFlowStart>();
            for (int index = 0; index < starting.Length; index++)
            {
                startingNodes.Add(starting[index].gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (winCount == startingNodes.Count)
            {
                levelWin = true;
            }
            if (levelWin == true)
            {
                StartCoroutine(LoadNextLevel());
            }
        }

        IEnumerator LoadNextLevel()
        {
            AudioManager.Instance.PlayClip(levelWinSound, AudioManager.Instance.GetChannel("SFX"));
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(LevelName);
        }
    }
}
