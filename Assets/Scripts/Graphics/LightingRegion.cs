using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingRegion : MonoBehaviour
{
    bool isActive = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isActive = true;
            if (giRoot)
                segi.followTransform = giRoot;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isActive = false;
        }
    }

    private List<DitherWalls> m_ditherables;
    public GameObject sceneRoot;
    public Transform giRoot;
    public SEGI segi;
    private void Awake()
    {
        m_ditherables = new List<DitherWalls>();

        GatherDitherables(m_ditherables, sceneRoot.transform);
        segi = GameObject.FindObjectOfType<SEGI>();
    }

    private void GatherDitherables(List<DitherWalls> list, Transform t)
    {
        if(t.GetComponent<DitherWalls>())
        {
            foreach(var peepee in t.GetComponents<DitherWalls>())
                list.Add(peepee);
        }

        int iChild = 0;
        while(iChild < t.childCount)
        {
            GatherDitherables(list, t.GetChild(iChild));
            iChild++;
        }   
    }

    private void Update()
    {
        if(!isActive)
        {
            foreach(var ditherable in m_ditherables)
            {
                ditherable.shouldDither = true;
            }
        }
    }
}
