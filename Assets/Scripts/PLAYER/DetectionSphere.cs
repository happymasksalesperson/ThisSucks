using UnityEngine;

public class DetectionSphere : MonoBehaviour
{
    public float detectionRadius;
    private Collider[] hitColliders = new Collider[25];

    private void FixedUpdate()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            INPC npc = hitColliders[i].GetComponent<INPC>();
            if (npc != null)
            {
                npc.OnPlayerEnterRange();
            }
        }
    }
}