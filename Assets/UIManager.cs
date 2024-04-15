using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject newButton;
    public GameObject Playerbutton;
    public ArrayList buttons = new ArrayList();
    public Transform canvas;
    [SerializeField] private Sprite BonemanSprite;
    [SerializeField] private Sprite ArmorSprite;
    [SerializeField] private Sprite CatapultSprite;
    // Start is called before the first frame update



    public void removeAll()
    {
        foreach(GameObject go in buttons)
        {
            Destroy(go);
        }
        buttons.Clear();
    }

    public void addbutton(string type, Transform transform)
    {

        
        GameObject ttemp;
        if (buttons.Count == 0)
        {
             ttemp = Playerbutton;
            print("Check 1");

        }
        else
        {
             ttemp = (GameObject)buttons[buttons.Count-1];

        }
        float objwidth = ttemp.transform.localScale.x;
        print(objwidth);
        Transform tsf = gameObject.transform;
        tsf.position = (ttemp.transform.position + new Vector3(objwidth + 40f, 0f, 0f));
        GameObject newbutt = Instantiate(newButton, canvas);
        newbutt.transform.position = tsf.position;


        if (type == "Basic Boneman")
        {
            newbutt.GetComponent<Image>().sprite = BonemanSprite;
        }
        else if (type == "Armored Boneman")
        {
            newbutt.GetComponent<Image>().sprite = ArmorSprite;
        }
        else if (type == "Bone Catipult")
        {
            newbutt.GetComponent<Image>().sprite = CatapultSprite;
        }

        newbutt.GetComponent<TargetTransform>().target = transform;



        buttons.Add(newbutt);

        
    }
}
