using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMainMenu : MonoBehaviour
{
    private GameObject muteMusicSprite;
    private GameObject unmuteMusicSprite;
    private AudioSource music;
    private void Start()
    {
        music = GameObject.Find("Logging").GetComponent<AudioSource>();
        unmuteMusicSprite = GameObject.Find("On");
        muteMusicSprite = GameObject.Find("Off");
    }
    public void MainMenu()
	{
		
		SceneManager.LoadScene("MainMenu");
	}

    public void MuteAudio()
    {

        music.mute = true;
        muteMusicSprite.SetActive(true);
        unmuteMusicSprite.SetActive(false);

    }
    public void UnmuteAudio()
    {
        music.mute = false;
        muteMusicSprite.SetActive(false);
        unmuteMusicSprite.SetActive(true);
    }
   }
