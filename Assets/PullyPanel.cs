using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PullyPanel : MonoBehaviour
{

    public int weight = 0;

    ArrayList gameobjects = new ArrayList();

    private void Start()
    {
        gameobjects.Clear();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WeightedObject>())
        {
            gameobjects.Add(collision.gameObject);
            
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WeightedObject>())
        {
            gameobjects.Remove(collision.gameObject);
            
        }

    }

    private void Update()
    {

        print(gameobjects.Count);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            print("TEST1");
            CheckWeight();
        }
        print(weight);
    }


    public void CheckWeight()
    {
        print("checking");
        foreach (GameObject skele in gameobjects)
        {
            //weight += skele.gameObject.GetComponent<HasWeight>().weight;
        }
    }



}
