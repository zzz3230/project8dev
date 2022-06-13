using System.Linq;
using UnityEngine;

public class OverlapCheckerScript : MonoBehaviour
{
    Collider[] gameObjectColliders;
    void Awake()
    {
        gameObjectColliders = GetComponentsInChildren<Collider>();
    }

    public bool CheckOveralp()
    {
        Collider[] neighbours = new Collider[16];
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, neighbours);
        for (int j = 0; j < gameObjectColliders.Length; j++)
        {
            for (int i = 0; i < count; ++i)
            {
                var collider = neighbours[i];


                if (gameObjectColliders.Contains(collider))
                    continue; // skip ourself

                Vector3 otherPosition = collider.gameObject.transform.position;
                Quaternion otherRotation = collider.gameObject.transform.rotation;

                Vector3 direction;
                float distance;

                bool overlapped = Physics.ComputePenetration(
                    gameObjectColliders[j], gameObjectColliders[j].gameObject.transform.position, gameObjectColliders[j].gameObject.transform.rotation,
                    collider, otherPosition, otherRotation,
                    out direction, out distance
                );

                //Log.Ms(distance);

                if (overlapped && distance > 0.5f)
                    return true;
            }
        }
        return false;
    }
    public float radius;
    [SerializeField] bool drawSphere;
    private void OnDrawGizmos()
    {
        if (drawSphere)
            Gizmos.DrawSphere(transform.position, radius);
    }
}
