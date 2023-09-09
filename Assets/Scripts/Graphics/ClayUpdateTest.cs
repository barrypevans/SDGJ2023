using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayUpdateTest : MonoBehaviour
{
    public bool doRotate = false;
    public float offset = 0;
    void ClayUpdate()
    {
        if(doRotate)
        transform.Rotate(new Vector3(10, 6, 8));
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin((Time.time + offset)*5) *.05f, transform.position.z);
    }
}
