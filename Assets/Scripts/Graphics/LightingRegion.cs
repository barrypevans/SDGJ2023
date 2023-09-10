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
    public Transform sunlight;
    public string sceneRootName;
    public GameObject sceneRoot;
    public Transform giRoot;
    public Vector3 sunRotation;
    public float softSun;
    public float skyContribution;
    public SEGI segi;
    private void Awake()
    {
        StartCoroutine(Co_SetupSecenRoot());
        
        m_ditherables = new List<DitherWalls>();

        //GatherDitherables(m_ditherables, sceneRoot.transform);
        segi = GameObject.FindObjectOfType<SEGI>();
    }

    IEnumerator Co_SetupSecenRoot()
    {
        yield return new WaitForEndOfFrame();
        sceneRoot = GameObject.Find(sceneRootName);
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
        if (sceneRoot)
        {
            sceneRoot.SetActive(isActive);
            if (isActive)
            {
                sunlight.rotation = Quaternion.Euler(sunRotation);
                segi.softSunlight = softSun;
                segi.skyIntensity = skyContribution;
            }
        }
    }
}
