using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
	}

	public void LoadScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void LoadPanel(GameObject panel)
    {
		panel.SetActive(true);
    }

	public void UnloadPanel(GameObject panel)
    {
		panel.SetActive(false);
	}
}
