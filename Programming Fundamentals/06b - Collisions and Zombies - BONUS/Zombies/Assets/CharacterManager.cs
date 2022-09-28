using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject human_prefab;
    public GameObject zombie_prefab;

    MyHuman human;
    // Start is called before the first frame update
    void Start()
    {
        //character = new MyCharacter(prefab);
        human = new MyHuman(human_prefab);

    }

    // Update is called once per frame
    void Update()
    {

        //Move with arrow/WASD keys
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");

        Vector3 aim = new(xMove, yMove);

        //Move with mouse
        if (Input.GetMouseButton(0))
        {
            aim = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }

        human.MoveCharacter(human.GetGameObject(), aim);
    }
}
