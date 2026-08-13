using UnityEngine;
public class Elevation_Entry2 : MonoBehaviour
{
    public Collider2D[] ElevationColliders;
    public Collider2D[] EleBoundaryColliders;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (Collider2D ElevationCollider in ElevationColliders)
            {
                ElevationCollider.enabled = false;
            }
            foreach (Collider2D EleBoundaryCollider in EleBoundaryColliders)
            {
                EleBoundaryCollider.enabled = true;
            }
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
        }
    }
}