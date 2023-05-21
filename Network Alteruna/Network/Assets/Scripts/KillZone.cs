using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class KillZone : AttributesSync
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Alteruna.Avatar _avatar))
        {
            if (!_avatar.IsMe)
                return;


            PlayerDied(_avatar);
        }
    }

    void PlayerDied(Alteruna.Avatar _avatar)
    {
        _avatar.GetComponent<PlayerHealth>().MessageDeath(_avatar.Possessor.Index);
    }



}
