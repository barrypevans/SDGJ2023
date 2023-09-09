using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitherWallsChecker : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.Normalize();
        var hits = Physics.SphereCastAll(new Ray(transform.position, toPlayer), 1);
        foreach(var hit in hits)
        {
            DitherWalls dwalls = null;
            if (dwalls=hit.collider.GetComponent<DitherWalls>())
            {
                dwalls.shouldDither = true;
            }
        }
    }
}
