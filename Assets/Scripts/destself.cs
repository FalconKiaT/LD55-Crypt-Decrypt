using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class destself : MonoBehaviour
{
    // Start is called before the first frame update
    TargetTransform targetTransform;
    void Start()
    {
        targetTransform = GetComponent<TargetTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetTransform.transform.gameObject.IsDestroyed())
        {
            Destroy(gameObject);
        }
    }
}
