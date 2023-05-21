using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [SerializeField] private GameObject multiplayerManagerPrefab;
    [SerializeField] private GameObject roomMenuPrefab;

    private GameObject roomMenu;
    private Color startColor;
    private GameObject scrollView;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Instantiate(multiplayerManagerPrefab);
            roomMenu = Instantiate(roomMenuPrefab);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startColor = roomMenu.GetComponentInChildren<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowRooms()
    {
        roomMenu.GetComponentInChildren<Image>().color = startColor;

        if (scrollView == null)
            scrollView = GameObject.Find("Scroll View");

        roomMenu.GetComponent<CanvasScaler>().referenceResolution = new Vector2(400, 300);
        
        scrollView.SetActive(true);
    }

    public void HideRooms()
    {
        roomMenu.GetComponentInChildren<Image>().color = Color.clear;

        if(scrollView == null)
            scrollView = GameObject.Find("Scroll View");

        roomMenu.GetComponent<CanvasScaler>().referenceResolution = new Vector2(800, 600);
        scrollView.SetActive(false);
    }
}
