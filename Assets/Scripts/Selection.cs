using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    StoryChoices sc;
    GameObject levelDesign;

    public GameObject choiceParticles;
    public GameObject toDestroy;

    [Header("Choices")]
    public bool goodChildhood;
    public bool badChildhood;
    public bool study;
    public bool lazy;
    public bool stay;
    public bool leave;
    public bool continueStudies;
    public bool stopStudies;

    private void Start()
    {
        sc = GameObject.FindGameObjectWithTag("Canvas").GetComponent<StoryChoices>();
        levelDesign = GameObject.Find("Level Design");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        Player player = collision.GetComponent<Player>();

        // Destroy the other path
        if (toDestroy != null)
        {
            SpriteRenderer[] blockObj = toDestroy.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer obj in blockObj)
            {
                if (obj.tag == "Blocks")
                {
                    obj.enabled = false;
                    Instantiate(choiceParticles, obj.transform.position, Quaternion.identity, levelDesign.transform);
                }
            }

            Destroy(toDestroy);
        }

        if (goodChildhood)
            sc.goodChildhood = true;

        if (badChildhood)
            sc.badChildhood = true;

        if (study)
            sc.study = true;

        if (lazy)
            sc.lazy = true;

        if (stay)
            sc.stay = true;

        if (leave)
            sc.leave = true;

        if (continueStudies)
            sc.continueStudies = true;

        if (stopStudies)
            sc.stopStudies = true;
    }
}
