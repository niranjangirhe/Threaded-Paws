using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGameActive;
    private bool isGamePaused;
    [SerializeField] private GameObject pauseScreenPrefab;
    private GameObject pauseScreen;
    private Transform Canvas;
    private AudioSource[] allAudioSources;

    // Start is called before the first frame update
    void Start()
    {
        //make instance of pause screen
        pauseScreen = Instantiate(pauseScreenPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
        Canvas = GameObject.Find("Canvas").transform;
        
        //set parent as canvas for pause screen
        pauseScreen.transform.position = pauseScreenPrefab.transform.position;
        pauseScreen.transform.SetParent(Canvas, false);


        pauseScreen.SetActive(false);

        //get all audio sources
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        //start game
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
            //if game is not paused
            if (!isGamePaused)
            {
                Time.timeScale = 0.0f;
                isGamePaused = true;
                pauseScreen.SetActive(true);
                foreach (AudioSource audioS in allAudioSources)
                {
                    if (audioS.isActiveAndEnabled)
                        audioS.Pause();
                }
            }
            else
            {
                Time.timeScale = 1.0f;
                isGamePaused = false;
                pauseScreen.SetActive(false);
                foreach (AudioSource audioS in allAudioSources)
                {
                    
                    if(audioS.isActiveAndEnabled)
                        audioS.Play(0);
                }
            }
        }
    }
}