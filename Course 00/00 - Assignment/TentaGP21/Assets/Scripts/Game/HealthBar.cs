using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject health;

    // Start is called before the first frame update
    void Start()
    {
        AddHealthBar(4);
        AddHealth(4);
    }

    public void AddHealthBar(int bars)
    {
        for (int i = 0; i < bars; i++)
        {
            AddHealthBar();
        }
    }

    public void AddHealthBar()
    {
        Instantiate(healthBar, transform);
    }

    public void AddHealth(int bars)
    {
        for (int i = 0; i < bars; i++)
        {
            AddHealth();
        }
    }

    public void AddHealth()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.childCount == 0)
            {
                Instantiate(health, child);
                break;
            }
        }
    }

    public void LoseHealth()
    {
        for (int i = transform.childCount - 1; i >= 0 ; i--)
        {
            Transform child = transform.GetChild(i);

            if (child.childCount != 0)
            {
                Debug.Log("Health Lost! at pos " + i);
                Destroy(child.GetChild(0).gameObject);

                if (i == 0)
                {
                    GameOver();
                }
                break;
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("You Died!");
        SceneManager.LoadScene("GameOver");
    }

}
