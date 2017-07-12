using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public void GameScene()
    {
        SceneManager.LoadScene("__MAIN__");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("__MENU__");
    }

    public void OptionsScene()
    {
        SceneManager.LoadScene("__OPTIONS__");
    }

    public void CreditsScene()
    {
        SceneManager.LoadScene("__CREDITS__");
    }

    public void Exit()
    {

    }
}
