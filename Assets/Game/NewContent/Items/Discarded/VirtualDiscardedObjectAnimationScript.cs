using UnityEngine;

public class VirtualDiscardedObjectAnimationScript : MonoBehaviour
{
    [SerializeField] private Vector3 destinationPoint;
    [SerializeField] private float smoothing;

    Vector3 startPoint;
    Vector3 endPoint;
    private void Start()
    {
        startPoint = transform.position;
        endPoint = transform.position;
        endPoint.y += 0.1f;
        destinationPoint = endPoint;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationPoint, smoothing * Time.deltaTime);
        if (transform.position == destinationPoint)
        {
            destinationPoint = destinationPoint == startPoint ? endPoint : startPoint;
        }
    }
}
