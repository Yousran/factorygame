using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        StartCoroutine(LoadAsynchronously(level));
    }
    IEnumerator LoadAsynchronously(int level)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/.9f);
            Debug.Log(progress);
            yield return null;
        }
    }
}
