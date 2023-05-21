using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject map;
    public GameObject player;
    public GameObject[] players;
        
    [SerializeField] [Range(0, 10)]   private float moveSpeed = 2;
    [SerializeField] [Range(0, 10)]   private float zoomSpeed = 1.5f;
    [SerializeField] [Range(-10, 10)] private float zoomOffset = 1;
    [SerializeField] [Range(0, 10)]   private float zoomScale = 2.8f;

    [SerializeField] [Range(0, 10)]   private float minOrthograpic = 4;
    [SerializeField] [Range(0, 100)]  private float maxOrthograpic = 15;

    private float xMin, xMax, zMin, zMax;
    private Camera cam;
    private int numOfPlayers = 0;

    void Awake()
    {
        players = new GameObject[4];
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        //Lock the camera to the map
        if (map.TryGetComponent<BoxCollider>(out BoxCollider mapBounds))
        {
            if(mapBounds.bounds.extents.x/cam.aspect > mapBounds.bounds.extents.z)
            {
                xMin = -mapBounds.bounds.extents.x + mapBounds.bounds.center.x;
                xMax = mapBounds.bounds.extents.x + mapBounds.bounds.center.x;
                zMin = xMin / cam.aspect;
                zMax = xMax / cam.aspect;

                //Debug.Log("Camera limited by Width");
            }
            else
            {
                zMin = -mapBounds.bounds.extents.z + mapBounds.bounds.center.z;
                zMax = mapBounds.bounds.extents.z + mapBounds.bounds.center.z;

                xMin = zMin * cam.aspect;
                xMax = zMax * cam.aspect;
                //Debug.Log("Camera limited by height");
            }


            //xMin = -mapBounds.bounds.extents.x;
            //xMax = mapBounds.bounds.extents.x;
            //yMin = -mapBounds.bounds.extents.y;
            //yMax = mapBounds.bounds.extents.y;

            //Set the max cam size to view whole map
            maxOrthograpic = zMax;
        }
        else
        {
            Debug.Log("Couldn't locate bounding box");
        }
        AddCameraTracking(player);
    }

    void Update()
    {
        if(numOfPlayers != 0)
            MoveCameraOrthographic(players);
        if(numOfPlayers == 1)
        {
            moveSpeed = 8;
        }
    }

    public void AddCameraTracking(GameObject player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                players[i] = player;
                Debug.Log("Added " + player.name + " to camera tracking");
                numOfPlayers++;
                return;
            }
        }
        Debug.Log("Tracking list full! Couldn't add " + player.name + " to camera tracking");
    }

    public void RemoveCameraTracking(GameObject player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == player)
            {
                players[i] = null;
                Debug.Log("Removed " + player.name + " from camera tracking");
                numOfPlayers--;
                return;
            }
        }
        Debug.Log("Can't Remove " + player.name + " in camera tracking list, does not exist in tracking");
    }


    void MoveCameraOrthographic(GameObject[] players)
    {

        //Set Size
        float currentSize = cam.orthographicSize;
        float targetSize = zoomOffset + GetCameraTargetSize(players);
        float increment = (targetSize - currentSize) * Time.deltaTime * zoomSpeed;

        cam.orthographicSize = Mathf.Clamp(currentSize + increment, minOrthograpic, maxOrthograpic);

        //Move camera
        Vector3 middle = GetCameraDestination(players);
        middle.z = 0;
        transform.position += middle * Time.deltaTime * moveSpeed;

        if(map != null)
            transform.position = ClampCameraToMap();
    }

    Vector3 ClampCameraToMap()
    {
        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        float camX = Mathf.Clamp(transform.position.x, xMin + width, xMax - width);
        float camY = Mathf.Clamp(transform.position.y, zMin + height, zMax - height);
        return new Vector3(camX, camY, -10);
    }

    float GetCameraTargetSize(GameObject[] players)
    {
        float aspect = GetComponent<Camera>().aspect;
        List<Vector3> playerPositions = new List<Vector3>();

        //Scale distance in proportions to the camera aspect
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i] != null)
                playerPositions.Add(new Vector3(players[i].transform.position.x, players[i].transform.position.z * aspect));
        }

        float maxDistance = minOrthograpic;
        //Calculate the max distance of all player pairs
        foreach (Vector3 position in playerPositions)
        {
            for (int i = 1; i < playerPositions.Count; i++)
            {
                float distance = Vector2.Distance(position, playerPositions[i]);
                if (maxDistance < distance)
                {
                    maxDistance = distance;
                }
            }
        }

        return maxDistance/zoomScale;
    }

    Vector3 GetCameraDestination(GameObject[] players)
    {
        Vector3 destination = Vector2.zero;
        foreach (GameObject player in players)
        {
            if(player != null)
                destination += (player.transform.position - transform.position);
        }

        destination = destination / players.Length;

        return destination;
    }

    void MoveCameraPerspective(Vector3 player, Vector3 cube)
    {
        Vector3 middle = ((player - transform.position) + (cube - transform.position)) / 2;

        transform.position += middle * Time.deltaTime * moveSpeed;
        transform.position = Vector3.Lerp(transform.position, new(transform.position.x, transform.position.y, -(zoomOffset + Vector2.Distance(player, cube))), Time.deltaTime);
    }
}
