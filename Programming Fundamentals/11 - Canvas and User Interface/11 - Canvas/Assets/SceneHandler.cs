using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    bool playerEnable = false;
    bool tankEnable = false;
    bool marioEnable = false;

    public void OnButtonClickLoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void OnButtonClickLoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }


    public void PlayerScript()
    {
        playerEnable = !playerEnable;
        GameObject varGameObject = GameObject.Find("Spaceship");
        varGameObject.GetComponent<PlayerContoller>().enabled = playerEnable;
    }

    public void TankScript()
    {
        tankEnable = !tankEnable;
        GameObject varGameObject = GameObject.Find("Tank");
        varGameObject.GetComponent<TankController>().enabled = tankEnable;
        varGameObject.GetComponentInChildren<TurretController>().enabled = tankEnable;
        varGameObject.GetComponentInChildren<Fire>().enabled = tankEnable;
    }

    public void MarioScript()
    {
        marioEnable = !marioEnable;
        GameObject varGameObject = GameObject.Find("Mario");
        varGameObject.GetComponent<MarioController>().enabled = marioEnable;
    }

}
