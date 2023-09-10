using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitherWallsChecker : MonoBehaviour
{

    Shader ditherShader;
    void Start ()
    {
        ditherShader = Shader.Find("Custom/DitheredAlpha");
    }

    public Transform player;
    void Update()
    {
        Vector3 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;
        toPlayer.Normalize();
        var hits = Physics.SphereCastAll(new Ray(transform.position, toPlayer), .2f, dist);
        foreach(var hit in hits)
        {

            MeshRenderer renderer;
            if(renderer=hit.collider.GetComponent<MeshRenderer>())
            {
                if (hit.collider.tag != "DontDither")
                {
                    if (!hit.collider.GetComponent<DitherWalls>())
                    {
                        foreach (var mat in renderer.materials)
                        {
                            mat.shader = ditherShader;
                            hit.collider.gameObject.AddComponent<DitherWalls>();
                        }
                    }
                }
            }

            DitherWalls dwalls = null;
            if (dwalls=hit.collider.GetComponent<DitherWalls>())
            {
                dwalls.shouldDither = true;
            }
        }
    }
}
