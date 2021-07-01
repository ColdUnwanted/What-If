using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    /* Logic:
     * Save location for player respawn
     * Show text when reach this location
     */

    public GameObject respawnPoint;
    public GameObject toShow;
    public bool toDestroy;

    public GameObject checkpoint;

    AudioSource audioSource;
    public AudioClip clip;
    public AudioClip explode;

    bool playedAlready;

    GameObject levelDesign;
    MainMenu mainMenu;

    bool destroyAlready;
    bool spawnAlready;

    public bool stopMovement;
    public bool kaboom;
    public float endPosX;
    bool moveToEnd;
    bool atTheEnd;
    public GameObject kaboomParticles;

    public bool fresh;

    Player player;

    [Header("Choice")]
    public GameObject choiceToDelete;

    public bool theEnd;

    private void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        levelDesign = GameObject.Find("Level Design");
        mainMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainMenu>();

        if (choiceToDelete == null)
        {
            return;
        }

        if (!mainMenu.GetComponent<StoryChoices>().pregnant)
        {
            Destroy(choiceToDelete);
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        if (moveToEnd)
        {
            Vector3 end = new Vector3(endPosX, player.transform.position.y, player.transform.position.z);
            if (player.transform.position != end)
            {
                // Slowly move player
                player.transform.position = Vector3.MoveTowards(player.transform.position, end, .05f);
            }
            else
            {
                moveToEnd = false;
                atTheEnd = true;
            }
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        if (atTheEnd)
        {
            if (kaboom)
            {
                kaboom = !kaboom;
                StartCoroutine(KaboomEverything());
            }

            if (theEnd)
            {
                StartCoroutine(End());
            }
        }
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(5f);
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainMenu>().end.SetActive(true);
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainMenu>().newGame = true;
    }

    IEnumerator KaboomEverything()
    {
        yield return new WaitForSeconds(1.5f);

        // Find all blocks
        GameObject parent = transform.parent.gameObject;
        SpriteRenderer[] blockObj = parent.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer obj in blockObj)
        {
            if (obj.tag == "Blocks")
            {
                obj.enabled = false;
                Instantiate(kaboomParticles, obj.transform.position, Quaternion.identity, levelDesign.transform);
            }
        }

        Camera.main.GetComponent<CameraMovement>().lockMovement = true;
        player.fallingAlready = true;
        Destroy(parent);
        audioSource.PlayOneShot(explode, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        // Set checkpoint
        player = collision.GetComponent<Player>();
        player.rd.Clear();
        player.rd.Add(new RewindData(respawnPoint.transform.position, Vector3.one));
        
        if (!playedAlready)
        {
            audioSource.PlayOneShot(clip, 0.5f);
            playedAlready = true;
        }

        checkpoint.SetActive(true);

        if (toShow != null && !spawnAlready)
        {
            GameObject obj = Instantiate(toShow, levelDesign.transform);
            mainMenu.levels.Add(obj);
            spawnAlready = true;
        }

        if (toDestroy && !destroyAlready)
        {
            try
            {
                Destroy(mainMenu.levels[0]);
                mainMenu.levels.RemoveAt(0);
                destroyAlready = true;
            }
            catch
            {

            }
        }

        if (stopMovement)
        {
            player.lockMovement = true;

            // Slowly move player to the end pos
            moveToEnd = true;
        }

        if (fresh)
        {
            player.lockMovement = false;
            player.fallingAlready = false;
            Camera.main.GetComponent<CameraMovement>().lockMovement = false;
        }
    }
}
