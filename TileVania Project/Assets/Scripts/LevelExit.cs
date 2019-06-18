using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    [SerializeField] float levelLoadDelay = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        Time.timeScale = 1f;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Destroy(FindObjectOfType<ScenePersist>()); // stops problem w/coins not loading between levels
        SceneManager.LoadScene(currentSceneIndex + 1);
            
    }
}
