using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    Coroutine m_clayUpdateRoutine;

    //string leveslToLoad[3] = {""};

    void Start()
    {
        m_clayUpdateRoutine = StartCoroutine(Co_ClayUpdate());
        LoadLevels();
    }

    void Update()
    {
        
    }

    void LoadLevels()
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
