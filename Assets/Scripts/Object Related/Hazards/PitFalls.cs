using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitFalls : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity entity) && entity.CompareTag("Entity"))
        {
            entity.Death();
        }
    }
}
