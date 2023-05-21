using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class StartLevelZone : AttributesSync
{

    public int minimumPlayers = 2;
    int playersInside = 0;
    

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryStartLevel(8);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Alteruna.Avatar _avatar))
        {
            if (!_avatar.IsMe)
                return;

            InvokeRemoteMethod(nameof(ReceivePlayerSteppedInside), UserId.AllInclusive);

            TryStartLevel(playersInside);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Alteruna.Avatar _avatar))
        {
            if (!_avatar.IsMe)
                return;

            InvokeRemoteMethod(nameof(ReceivePlayerSteppedOutside), UserId.AllInclusive);
        }
    }

    [SynchronizableMethod]
    void ReceivePlayerSteppedInside()
    {
        playersInside++;
    }

    [SynchronizableMethod]
    void ReceivePlayerSteppedOutside()
    {
        playersInside--;
    }

    void TryStartLevel(int userCount)
    {
        if (userCount < minimumPlayers)
        {
            Debug.Log("Minimum players requirement not met:" + userCount + "/" + minimumPlayers);
            return;
        }

        InvokeRemoteMethod(nameof(SetEnabled), UserId.AllInclusive, false);
        GameManager.Instance.PlayerStarted();
    }

    [SynchronizableMethod]
    public void SetEnabled(bool enabled)
    {
        if (!enabled)
            playersInside = 0;

        gameObject.SetActive(enabled);
    }
}
