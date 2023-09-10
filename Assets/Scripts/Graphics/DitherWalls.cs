using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DitherWalls : MonoBehaviour
{
    public bool shouldDither;
    MeshRenderer meshRenderer;
    public float debuga;
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
            if(c.a <.25)
            {
                c.a = 0.25f;
            }
            foreach(var mat in meshRenderer.materials)
                mat.SetColor("_Color", c);
        }
        else
        {
            Color c = meshRenderer.material.color;
            c.a += Time.deltaTime * 10;
            if (c.a > 1)
            {
                c.a = 1;
            }
            foreach (var mat in meshRenderer.materials)
                mat.SetColor("_Color", c);
        }
        debuga = meshRenderer.material.GetColor("_Color").a;
    }
}
