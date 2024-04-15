using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public LineRenderer lineRenderer;
    private Vector3 beginning;
    private Vector3 end;

    public GameObject first;
    public GameObject last;
    public float offset;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (first == null || last == null)
            return;

        beginning = new Vector3(first.transform.position.x, (first.transform.position.y + offset));
        end = new Vector3(last.transform.position.x, (last.transform.position.y + offset));
        lineRenderer.SetPosition(0, beginning);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, end);
    }
}
