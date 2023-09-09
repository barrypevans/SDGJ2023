using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollisionBroadcaster : MonoBehaviour
{
    public delegate void MyEventHandler(GameObject sender, Collider col);
    public Action<Collision> CollisionEnter;

    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter.Invoke(collision);
    }
}