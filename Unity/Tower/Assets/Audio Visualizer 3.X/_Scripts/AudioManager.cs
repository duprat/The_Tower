using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip Music1;
    public AudioClip Music2;
    public AudioClip Music3;
    public AudioClip Music4;
    public AudioClip Music5;
    public AudioClip Music6;
    public AudioClip Music7;
    public AudioClip Music8;
    public AudioClip Music9;
    public AudioClip Music10;

    public static AudioManager current;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        switch (UIManager.current.getIndexMusic())
        {
            case 0:
                UIManager.current.musicName.SetText("No Song Selected");
                break;
            case 1:
                GetComponent<AudioSource>().clip = Music1;
                UIManager.current.musicName.SetText(Music1.name);
                break;
            case 2:
                GetComponent<AudioSource>().clip = Music2;
                UIManager.current.musicName.SetText(Music2.name);
                break;
            case 3:
                GetComponent<AudioSource>().clip = Music3;
                UIManager.current.musicName.SetText(Music3.name);
                break;
            case 4:
                GetComponent<AudioSource>().clip = Music4;
                UIManager.current.musicName.SetText(Music4.name);
                break;
            case 5:
                GetComponent<AudioSource>().clip = Music5;
                UIManager.current.musicName.SetText(Music5.name);
                break;
            case 6:
                GetComponent<AudioSource>().clip = Music6;
                UIManager.current.musicName.SetText(Music6.name);
                break;
            case 7:
                GetComponent<AudioSource>().clip = Music7;
                UIManager.current.musicName.SetText(Music7.name);
                break;
            case 8:
                GetComponent<AudioSource>().clip = Music8;
                UIManager.current.musicName.SetText(Music8.name);
                break;
            case 9:
                GetComponent<AudioSource>().clip = Music9;
                UIManager.current.musicName.SetText(Music9.name);
                break;
            case 10:
                GetComponent<AudioSource>().clip = Music10;
                UIManager.current.musicName.SetText(Music10.name);
                break;
            default:
                //UIManager.current.musicName.SetText("No Song Selected");
                break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetComponent<AudioSource>().clip = Music1;
            UIManager.current.setIndexMusic(1);
            UIManager.current.musicName.SetText(Music1.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetComponent<AudioSource>().clip = Music2;
            UIManager.current.setIndexMusic(2);
            UIManager.current.musicName.SetText(Music2.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetComponent<AudioSource>().clip = Music3;
            UIManager.current.setIndexMusic(3);
            UIManager.current.musicName.SetText(Music3.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GetComponent<AudioSource>().clip = Music4;
            UIManager.current.setIndexMusic(4);
            UIManager.current.musicName.SetText(Music4.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GetComponent<AudioSource>().clip = Music5;
            UIManager.current.setIndexMusic(5);
            UIManager.current.musicName.SetText(Music5.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GetComponent<AudioSource>().clip = Music6;
            UIManager.current.setIndexMusic(6);
            UIManager.current.musicName.SetText(Music6.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GetComponent<AudioSource>().clip = Music7;
            UIManager.current.setIndexMusic(7);
            UIManager.current.musicName.SetText(Music7.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GetComponent<AudioSource>().clip = Music8;
            UIManager.current.setIndexMusic(8);
            UIManager.current.musicName.SetText(Music8.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GetComponent<AudioSource>().clip = Music9;
            UIManager.current.setIndexMusic(9);
            UIManager.current.musicName.SetText(Music9.name);
            playMusic();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GetComponent<AudioSource>().clip = Music10;
            UIManager.current.setIndexMusic(10);
            UIManager.current.musicName.SetText(Music10.name);
            playMusic();
        }
    }

    void loadMusic()
    {

    }

    public void playMusic()
    {
        //GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
    }

    public void pauseMusic()
    {
        GetComponent<AudioSource>().Pause();
    }

    public void stopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
}
