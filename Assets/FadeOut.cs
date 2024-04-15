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

    void Start()
    {
        img = GetComponent<Image>();
        img.enabled = false;
    }

    public void fade()
    {
        img.enabled = true;
        StartCoroutine(SwitchSpritesWithDelay());

    }

    IEnumerator SwitchSpritesWithDelay()
    {
        while (currentIndex < sprites.Length -1 )
        {
            currentIndex++;
            img.sprite = sprites[currentIndex];
            yield return new WaitForSeconds(switchDelay);

        }
    }
}
