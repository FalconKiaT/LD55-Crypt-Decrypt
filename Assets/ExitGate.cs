using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGate: MonoBehaviour
{
    public Sprite[] sprites;
    public float switchDelay = 1f;
    private SpriteRenderer rend;
    private int currentIndex = 0;
    public int Trigdist;
    [SerializeField] private GameObject fadeout;
    [SerializeField] private GameObject Player;
    private float distance;
    private bool triggered = false;
    private ScenesManager sm;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
            sm = ScenesManager.instance;
    }
    private void Update()
    {
        if(!triggered)
        {
            opengate();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("LOG1");
        if(collision.gameObject.tag == "Player")
        {
            print("LOG2");
            StartCoroutine(BeginSwitch());
            
        }
    }

    IEnumerator BeginSwitch()
    {
        print("TEST");
        fadeout.GetComponent<FadeOut>().fade();
        yield return new WaitForSeconds(1.3f);
        sm.LoadNextScene();
    }
        private void opengate()
        {
        distance = CalculateDistance(transform.position, Player.transform.position);

        if (distance <= Trigdist && !triggered)
        {

            triggered = true;
            fade();
        }
    }

    float CalculateDistance(Vector3 obj1Position, Vector3 obj2Position)
    {
        float distance = Mathf.Abs(obj1Position.x - obj2Position.x);
        return distance;
    }


    public void fade()
    {
        StartCoroutine(SwitchSpritesWithDelay());

    }

    IEnumerator SwitchSpritesWithDelay()
    {
        while (currentIndex < sprites.Length -1 )
        {
            currentIndex++;
            rend.sprite = sprites[currentIndex];
            yield return new WaitForSeconds(switchDelay);

        }
    }
}
