using UnityEngine;

public class GroundCollisionCheckerScript : MonoBehaviour
{
    public bool onGround { get { return contactCount > 0; } }
    public int contactCount;

    public Vector3 groundNormal;
    [SerializeField] BasePlayerScript _playerScript;
    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        groundNormal = collision.contacts[0].normal;

        for (int i = 1; i < collision.contactCount; i++)
        {
            groundNormal.Scale(collision.contacts[i].normal);
        }
        contactCount++;
        //groundNormal = collision.contacts[0].normal;
    }
    private void OnCollisionExit(Collision collision)
    {
        contactCount--;
    }
}
