using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerT : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }

    public float speed = 5;


    private Rigidbody2D rb;
    private Vector2 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    private void FixedUpdate()
    {
        rb.velocity = movementDirection * speed;
    }
    private void Awake()
    {
        currentHealth = startingHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
            //Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        Debug.Log(currentHealth);
    }
}
