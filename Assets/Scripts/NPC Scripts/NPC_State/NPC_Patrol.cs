using UnityEngine;
using System.Collections;

public class NPC_Patrol : MonoBehaviour
{
    public Vector2[] patrolPoints;
    private int currentPatrolIndex;

    public float pauseDuration = 1f;
    private bool isPaused;
    private Vector2 target;
    public float speed = 2f;
    private Rigidbody2D rb;
    private Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("NPC_Patrol: patrolPoints not set or empty on " + gameObject.name + ". Disabling patrol.");
            enabled = false;
            return;
        }

        currentPatrolIndex = 0;
        StartCoroutine(SetPatrolPoint());
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        if (rb == null)
            return;

        Vector2 currentPos = transform.position;
        Vector2 direction = target - currentPos;

        if (direction.sqrMagnitude > 0.0001f)
        {
            Vector2 vel = direction.normalized * speed;
            if(direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0) 
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            rb.linearVelocity = vel;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (Vector2.Distance(currentPos, target) < 0.1f)
        {
            StartCoroutine(SetPatrolPoint());
        }
    }

    IEnumerator SetPatrolPoint()
    {
        isPaused = true;
        anim.Play("Idle");
        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        target = patrolPoints[currentPatrolIndex];
        isPaused = false;
        anim.Play("Walk");
    }

}
