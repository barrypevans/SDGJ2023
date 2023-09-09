using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    Coroutine m_clayUpdateRoutine;

    void Start()
    {
        m_clayUpdateRoutine = StartCoroutine(Co_ClayUpdate());
    }

    void Update()
    {
        
    }

    IEnumerator Co_ClayUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f / 15.0f);

            var gameObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (var obj in gameObjects)
            {
                obj.BroadcastMessage("ClayUpdate",SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
