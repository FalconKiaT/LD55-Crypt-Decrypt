using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] List<GameObject> healthNodes;

    public void UpdateUI(int damage)
    {
        foreach (GameObject healthN in healthNodes)
        {
            if (damage <= 0)
                return;

            if (healthN.activeSelf)
            {
                healthN.SetActive(false);
                damage--;
            }
        }
    }
}
