using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    Coroutine m_clayUpdateRoutine;

    string[] levelsToLoad = { "level2" , "level1.5", "levelHallway", "JacobTestScene", "title-ui" };

    void Awake()
    {
        m_clayUpdateRoutine = StartCoroutine(Co_ClayUpdate());
        LoadLevels();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void LoadLevels()
    {
        foreach (string level in levelsToLoad)
        {
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
        }
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
