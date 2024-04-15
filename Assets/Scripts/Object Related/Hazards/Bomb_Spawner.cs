using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bomb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Bomb>().expl)
        {
            Instantiate(bomb);
        }
    }
}
