using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void OnButtonClickLoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void OnButtonClickLoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
