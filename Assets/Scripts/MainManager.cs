using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    

    public void OnClickMove(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene("03_play");
    }

    public void OnClickOption()
    {


    }


    public void OnClickQuit()
    {
        Application.Quit();
    }

}
