using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct CollisionStayInfo
{
    public GameObject otherGroup;
    public GameObject[] thisObjects;
    public GameObject[] otherObjects;
    public Vector3 impulse;
}
public class BaseBuildingGroup : MonoBehaviour
{
    protected Vector3 lastImpulse;
    protected List<CollisionStayInfo> collisionStays = new List<CollisionStayInfo> { };
    private void OnCollisionStay(Collision collision)
    {
        //lastImpulse = collision.impulse;

        
        
        var thisObj = collision.contacts.Select((c) => c.thisCollider.gameObject);
        var otherObj = collision.contacts.Select((c) => c.otherCollider.gameObject);

        var info = new CollisionStayInfo
        {
            otherGroup = collision.gameObject,
            thisObjects = thisObj.ToArray(),
            otherObjects = otherObj.ToArray(),
            impulse = collision.impulse
        };

        if (collisionStays.Exists((c) => c.otherGroup == collision.gameObject))
            collisionStays[collisionStays.FindIndex((c) => c.otherGroup == collision.gameObject)] = info;
        else
            collisionStays.Add(info);
    }
}
