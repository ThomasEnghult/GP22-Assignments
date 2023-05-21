using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using TMPro;

public class PlayerHealth : AttributesSync
{
    [SynchronizableField] float maxHealth = 10;
    [SynchronizableField] public float health = 10;
    [SynchronizableField] bool isDead = false;
    [SynchronizableField] int placement;
    [SerializeField] private GameObject playerCanvas;

    Vector3 spawnPoint;

    private Alteruna.Avatar _avatar;


    private void Awake()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
    }
    // Start is called before the first frame update
    void Start()
    {
        string displayName = _avatar.Possessor.Name;

        playerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = displayName;

        if (!_avatar.IsMe)
        {
            enabled = false;
            return;
        }

        spawnPoint = transform.position;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoseHealth(1f);
        }
    }

    public void MessageDeath(int playerIndex)
    {
        InvokeRemoteMethod(nameof(RecieveDeath), UserId.AllInclusive, playerIndex);
    }

    [SynchronizableMethod]
    void RecieveDeath(int playerIndex)
    {
        if (Multiplayer.Me.Index == playerIndex)
        {
            OnDeath();
        }

        GameManager.Instance.PlayerDied();
        CheckIfWin();
    }

    void CheckIfWin()
    {
        if (GameManager.Instance.alivePlayers == 1 && !isDead)
        {
            GameManager.Instance.PlayerWon();
        }
    }



    public void GainHealth(int heal)
    {
        if (heal < 0)
        {
            Debug.LogWarning("Used GainHealth to add Negative HP, use LoseHealth instead");
        }
        ChangeHealth(heal);
        Debug.Log(gameObject.name + " healed " + heal + "HP.");
    }

    public void LoseHealth(float damage)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (damage < 0)
            {
                Debug.LogWarning("Used LoseHealth to add Negative damage, use GainHealth instead");
            }
            ChangeHealth(-damage);
            Debug.Log(gameObject.name + " lost " + damage + "HP.");
        }
    }

    private void ChangeHealth(float healthChange)
    {
        if (isDead)
        {
            Debug.Log("Target is already dead!");
            return;
        }
        health += healthChange;

        if (health > maxHealth)
        {
            health = maxHealth;
            Debug.Log(gameObject.name + " has full health!");
        }

        if (health <= 0)
        {
            OnDeath();
            health = 0;
            Debug.Log(gameObject.name + " reached 0 health!");
        }
    }
    public void OnDeath()
    {
        OnDeath(false, 5);
    }

    public void OnDeath(bool respawn, float respawntimer)
    {
        placement = GameManager.Instance.alivePlayers;
        isDead = true;
        GameManager.Instance.SetAliveText("placement " + placement);


        GetComponent<PlayerMovement>().enabled = false;
        transform.eulerAngles = new Vector3(0, 0, 90);

        if(respawn)
            Invoke(nameof(Respawn), respawntimer);

        if (!GameManager.Instance.inGame)
        {
            Respawn();
        }

    }

    public void Respawn()
    {
        if (!isDead)
        {
            return;
        }

        health = maxHealth;
        isDead = false;
        GameManager.Instance.alivePlayers++;
        GetComponent<CharacterController>().enabled = false;
        transform.position = Multiplayer.AvatarSpawnLocations[Multiplayer.Me.Index].position;
        GetComponent<CharacterController>().enabled = true;

        GetComponent<PlayerMovement>().ResetMovement();
        GetComponent<PlayerMovement>().enabled = true;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
