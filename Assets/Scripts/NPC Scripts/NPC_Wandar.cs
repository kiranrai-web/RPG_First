using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NPC_Wandar : MonoBehaviour
{
    [Header("Wander Area")]
    public float wanderWidth = 5f;
    public float wanderHeight = 5f;
    public Vector2 startingPosition;
    public float speed = 2f;
    public Vector2 target;
    private Rigidbody2D rb;
    public float pauseDuration = 1f;
    private bool isPaused;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(PauseAndPickNewDestination());
    }

    private void Update() 
    {
        if (isPaused)
        { 
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if(Vector2.Distance(transform.position, target) < 0.1f)
        {
            StartCoroutine(PauseAndPickNewDestination());
        }
        Move();
    }


    private void Move()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        if(direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        rb.linearVelocity = direction * speed;
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        anim.Play("Idle");
        yield return new WaitForSeconds(pauseDuration);
        target = GetRandomTarget();
        isPaused = false;
        anim.Play("Walk");
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(PauseAndPickNewDestination());
    }

    private Vector2 GetRandomTarget()
    {
        float halfWidth = wanderWidth / 2f;
        float halfHeight = wanderHeight / 2f;
        int edge = Random.Range(0, 4);

        return edge switch
        {
            0 => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y + halfHeight), // Top edge
            1 => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y - halfHeight), // Bottom edge
            2 => new Vector2(startingPosition.x - halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)), // Left edge
            _ => new Vector2(startingPosition.x + halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)), // Right edge
        };
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startingPosition, new Vector3(wanderWidth, wanderHeight, 0f));
    }
}
