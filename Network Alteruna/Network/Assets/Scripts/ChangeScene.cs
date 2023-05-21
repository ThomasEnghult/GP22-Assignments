using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Alteruna;

public class ChangeScene : MonoBehaviour
{

    public void LoadScene(string name)
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if(playerHealth != null)
        {
            int index = playerHealth.GetComponent<Alteruna.Avatar>().Possessor.Index;

            playerHealth.MessageDeath(index);
        }

        SceneManager.LoadScene(name);
    }

    public void ToggleRooms(bool enabled)
    {
        if (enabled)
            MainMenuManager.Instance.ShowRooms();
        else
            MainMenuManager.Instance.HideRooms();
    }
}
