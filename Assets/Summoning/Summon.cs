using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class Summon : MonoBehaviour
{

    public TextMeshProUGUI txt;
    public GameObject objectToInstantiate;
    public GameObject player;
    public float minDistanceToCollider = 0.5f;
    [SerializeField]private int cost;
     public int bones = 0;
    private int totalbones;
    private UIManager uiManager;
    public float maxDistance = 5f;
    public LayerMask layerMask;
    public bool placing;
    public Texture2D m1;
    public Texture2D m2;

    public Vector3 mouse;

    public bool canplace = false;
    public ArrayList summons = new ArrayList();

    void Start()
    {
        setSkele(objectToInstantiate, 1);
        uiManager = GetComponent<UIManager>();
        player = gameObject.transform.parent.gameObject;
    }

    public void refund()
    {
        foreach(GameObject item in summons)
        {
            
            Destroy(item);
        }
        uiManager.removeAll();
        summons.Clear();
        bones = totalbones;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && placing)
        {
          spawn(objectToInstantiate, cost);
        }
        else
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        txt.text = bones.ToString();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, minDistanceToCollider, layerMask);
        if (colliders.Length == 0 && Vector2.Distance(mousePosition, player.transform.position) <= maxDistance)
        {
            canplace = true;
            Cursor.SetCursor(m1, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            canplace = false;
            Cursor.SetCursor(m2, Vector2.zero, CursorMode.Auto);

        }


    }

    public void setSkele(GameObject temp, int tempcost)
    {
        setSkele(temp);
        setSkeleCost(tempcost);
    }

    public void setSkele(GameObject skele)
    {
        objectToInstantiate = skele;
    }

    public void setSkeleCost(int tempcost)
    {
        cost = tempcost;
    }

    private void spawn(GameObject objectToInstantiate, int cost)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure the object is instantiated at z=0
        if (canplace)
        {
            if (removeBones(cost))
            {
                GameObject temp = Instantiate(objectToInstantiate, mousePosition, Quaternion.identity);
                summons.Add(temp);



                if(objectToInstantiate.name == "Basic Boneman" || objectToInstantiate.name == "Basic Boneman(Clone)")
                {
                    uiManager.addbutton("Basic Boneman", temp.transform);
                }
                else if(objectToInstantiate.name == "Armored Boneman" || objectToInstantiate.name == "Armored Boneman(Clone)")
                {
                    uiManager.addbutton("Armored Boneman", temp.transform);
                }
                else if (objectToInstantiate.name == "Bone Catipult" || objectToInstantiate.name == "Bone Catipult(Clone)")
                {
                    uiManager.addbutton("Bone Catipult", temp.transform);
                }


            }
            else
            {
                print("broke ass");
            }
            
        }
        else
        {
            Debug.Log("Too close to a collider, object not instantiated.");
        }
    }    
    private bool removeBones(int bonesToTake)
    {
        if(bones >= bonesToTake)
        {
            bones -= bonesToTake;
            print("T");
            return true;
        }    
        else
        {
            print("TOO MANY BONES");
            return false;
        }
    }    

    public void AddOneBone()
    {
        bones++;
        totalbones++;
    }

    public int getTotalBones()
    {
        return totalbones;
    }
}
