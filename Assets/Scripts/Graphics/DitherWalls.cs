using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DitherWalls : MonoBehaviour
{
    public bool shouldDither;
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        shouldDither = false;
    }

    private void LateUpdate()
    {
        if(shouldDither)
        {
            Color c = meshRenderer.material.color;
            c.a -= Time.deltaTime * 10;
            if(c.a <0)
            {
                c.a = 0.0f;
            }
            meshRenderer.material.SetColor("_Color", c);
        }
        else
        {
            Color c = meshRenderer.material.color;
            c.a += Time.deltaTime * 10;
            if (c.a > 1)
            {
                c.a = 1;
            }
            meshRenderer.material.SetColor("_Color", c);

        }
    }
}
