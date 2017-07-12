using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;

    [SerializeField]
    private GameObject pausePanel;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }

            if (!isPaused)
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
