using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStory : MonoBehaviour
{
    public GameObject whatToShow;
    Player player;

    public bool end;
    int whichEnd;
    public GameObject end1;
    public GameObject end2;
    public GameObject end3;
    public GameObject end4;
    public GameObject end5;

    public bool pregnantOnly;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        Player player = collision.GetComponent<Player>();

        if (player.rewinding)
        {
            return;
        }

        if (end)
        {
            switch (whichEnd)
            {
                case 1:
                    end1.SetActive(true);
                    break;
                case 2:
                    end2.SetActive(true);
                    break;
                case 3:
                    end3.SetActive(true);
                    break;
                case 4:
                    end4.SetActive(true);
                    break;
                case 5:
                    end5.SetActive(true);
                    break;
            }
        }
        else
        {

            whatToShow.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        Player player = collision.GetComponent<Player>();

        if (player.rewinding)
        {
            return;
        }

        if (end)
        {
            switch (whichEnd)
            {
                case 1:
                    end1.SetActive(true);
                    break;
                case 2:
                    end2.SetActive(true);
                    break;
                case 3:
                    end3.SetActive(true);
                    break;
                case 4:
                    end4.SetActive(true);
                    break;
                case 5:
                    end5.SetActive(true);
                    break;
            }
        }
        else
        {

            whatToShow.SetActive(true);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (end)
        {
            StoryChoices sc = GameObject.FindGameObjectWithTag("Canvas").GetComponent<StoryChoices>();
            whichEnd = sc.which;
        }

        if (!GameObject.FindGameObjectWithTag("Canvas").GetComponent<StoryChoices>().pregnant)
        {
            if (pregnantOnly)
            {
                Destroy(this);
            }
        }
    }

    private void Update()
    {
        if (whatToShow.activeSelf == false)
        {
            return;
        }

        if (player.rewinding)
        {
            if (whatToShow != null)
            {
                whatToShow.SetActive(false);
            }
            else
            {
                end1.SetActive(false);
                end2.SetActive(false);
                end3.SetActive(false);
                end4.SetActive(false);
                end5.SetActive(false);
            }
        }
    }
}
