using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text title;
    public Button playBtn;
    public Button soundBtn;
    public Button continueBtn;
    public Sprite soundSprite;
    public Sprite noSoundSprite;
    public AudioSource audioSource;
    bool sound = true;

    public GameObject levelDesign;
    public GameObject starting;
    public GameObject player;
    public GameObject mainMenu;
    public GameObject rewind;
    public GameObject gameStuffs;
    public GameObject fallingText;

    [HideInInspector] public List<GameObject> levels;

    public PostProcessVolume ppv;
    public Color mainBackground;
    public PostProcessProfile mainProfile;
    public Color specialBackground;
    public PostProcessProfile specialProfile;
    public bool special;

    public GameObject end;
    public Button backToMenu;

    public bool newGame;

    private void Start()
    {
        // Animate title
        StartCoroutine(AnimateTitle());

        // Assigning button 
        playBtn.onClick.AddListener(Play);
        soundBtn.onClick.AddListener(Sound);
        continueBtn.onClick.AddListener(Continue);
        backToMenu.onClick.AddListener(Pause);

        newGame = true;
    }

    IEnumerator AnimateTitle()
    {
        string fullTitle = "What If";
        string currentText;

        for (int i = 0; i < fullTitle.Length + 1; i++)
        {
            currentText = fullTitle.Substring(0, i);
            title.text = currentText;
            yield return new WaitForSeconds(.1f);
        }
    }

    void Play()
    {
        if (newGame)
        {
            newGame = false;
        }

        // Reset all 
        Transform[] transform = levelDesign.GetComponentsInChildren<Transform>();
        bool first = false;

        foreach (Transform trans in transform)
        {
            if (!first)
            {
                first = true;
            }
            else
            {
                Destroy(trans.gameObject);
            }
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject objToDelete in objs)
        {
            Destroy(objToDelete);
        }

        GameObject obj = Instantiate(starting, levelDesign.transform);
        levels.Add(obj);
        Instantiate(player);

        mainMenu.SetActive(false);
        Time.timeScale = 1;
        gameStuffs.SetActive(true);

        ChangeToNormal();

        Camera.main.GetComponent<CameraMovement>().lockMovement = false;
    }

    void Sound()
    {
        if (sound)
        {
            sound = false;
            soundBtn.GetComponent<Image>().sprite = noSoundSprite;
            audioSource.mute = true;
        }
        else
        {
            sound = true;
            soundBtn.GetComponent<Image>().sprite = soundSprite;
            audioSource.mute = false;
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        mainMenu.SetActive(true);
        gameStuffs.SetActive(false);

        ChangeToNormal();
    }

    void Continue()
    {
        if (newGame)
        {
            newGame = false;
            Play();
            return;
        }

        Time.timeScale = 1;
        mainMenu.SetActive(false);
        gameStuffs.SetActive(true);

        if (special)
        {
            ChangeToSpecial();
        }
        else
        {
            ChangeToNormal();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Pause();
        }
    }

    public void ChangeToSpecial()
    {
        Camera.main.backgroundColor = specialBackground;
        ppv.profile = specialProfile;
        special = true;
    }

    public void ChangeToNormal()
    {
        Camera.main.backgroundColor = mainBackground;
        ppv.profile = mainProfile;
    }
}
