using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefundBones : MonoBehaviour
{

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (collision.gameObject.GetComponentInChildren<Summon>().getTotalBones() != collision.gameObject.GetComponentInChildren<Summon>().bones)
            {
                transform.GetChild(0).gameObject.SetActive(true);

            }

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (Input.GetKeyDown(KeyCode.E) && collision.tag == "Player")
        {
            collision.gameObject.GetComponentInChildren<Summon>().refund();
            transform.GetChild(0).gameObject.SetActive(false);
            Camera.main.GetComponent<CameraFollow>().resetCamera();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);

        }
    }
}
