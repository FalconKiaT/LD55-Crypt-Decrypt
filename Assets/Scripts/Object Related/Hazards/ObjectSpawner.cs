using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject obj;

    private GameObject trackedObject;

    // Update is called once per frame
    void Update()
    {
        if (trackedObject == null)
            SpawnObject();
    }

    void SpawnObject()
    {
        trackedObject = Instantiate(obj, transform.position, Quaternion.identity);
    }
}
