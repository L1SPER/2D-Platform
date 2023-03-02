using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Continue()
    {
        Time.timeScale=1.0f;
    }
    public void Pause()
    {
        Time.timeScale=0.0f;
    }
    //private void Settings() Ýlerde eklenecek
    //{

    //}
    public void Quit()
    {
        Application.Quit();
    }
}
