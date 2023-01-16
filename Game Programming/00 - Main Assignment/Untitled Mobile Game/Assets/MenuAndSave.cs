using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuAndSave : MonoBehaviour
{

    public TMP_InputField nameInput;

    string name;
    // Start is called before the first frame update
    void Start()
    {
        name = PlayerPrefs.GetString("PlayerName");

        nameInput.text = name;

        nameInput.onValueChanged.AddListener(delegate { NameChanged(); });
    }

    void NameChanged()
    {
        PlayerPrefs.SetString("PlayerName", nameInput.text);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
