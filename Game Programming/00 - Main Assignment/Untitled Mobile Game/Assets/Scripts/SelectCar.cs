using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCar : MonoBehaviour
{
    public List<GameObject> cars;

    int showCarIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject car in cars)
        {
            car.SetActive(false);
        }

        cars[showCarIndex].SetActive(true);
    }

    public void ChangeCar()
    {
        Debug.Log("Changed car");
        cars[showCarIndex].SetActive(false);
        showCarIndex++;
        if(showCarIndex >= cars.Count)
        {
            showCarIndex = 0;
        }
        cars[showCarIndex].SetActive(true);
    }

    public void ConfirmCar()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
