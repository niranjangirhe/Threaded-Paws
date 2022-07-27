using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGameActive;
    private bool isGamePaused;
    public GameObject pauseScreenPrefab;
    private GameObject pauseScreen;
    private Transform Canvas;

    // Start is called before the first frame update
    void Start()
    {
        pauseScreen = Instantiate(pauseScreenPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Canvas = GameObject.Find("Canvas").transform;
        pauseScreen.transform.position = pauseScreenPrefab.transform.position;
        pauseScreen.transform.SetParent(Canvas, false);
        pauseScreen.SetActive(false);
        isGameActive = true;
        isGamePaused = false;
        Time.timeScale = 1f;
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePause();
        }
    }

    public void ChangePause()
    {

        if (isGameActive)
        {
            if (!isGamePaused)
            {
                Time.timeScale = 0.0f;
                isGamePaused = true;
                pauseScreen.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                isGamePaused = false;
                pauseScreen.SetActive(false);
            }
        }
    }
}