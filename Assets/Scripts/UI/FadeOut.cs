using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Sprite[] sprites;
    public float switchDelay = 1f;
    private Image img;
    private int currentIndex = 0;
    public bool reversed = false;

    private ScenesManager scenesManager;

    void Start()
    {
        img = GetComponent<Image>();
        img.enabled = false;
        scenesManager = ScenesManager.instance;
        if(reversed)
        {
            fade();
        }
    }

    public void fade()
    {
        img.enabled = true;
        if(reversed)
        {
            currentIndex = sprites.Length - 1;

            StartCoroutine(reverserun());
        }
        else StartCoroutine(SwitchSpritesWithDelay());


    }

    IEnumerator SwitchSpritesWithDelay()
    {
        while (currentIndex < sprites.Length - 1)
        {
            currentIndex++;
            img.sprite = sprites[currentIndex];
            yield return new WaitForSeconds(switchDelay);

        }

        scenesManager.LoadNextScene();


    }

    IEnumerator reverserun()
    {
        while (currentIndex > 0)
        {
            currentIndex--;
            img.sprite = sprites[currentIndex];
            yield return new WaitForSeconds(switchDelay);

        }
        img.enabled = false;
    }
}
