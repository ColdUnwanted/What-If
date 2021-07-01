using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ParticleSystem dust;
    public ParticleSystem jumpDust;
    public ParticleSystem falling;
    public GameObject deathParticles;

    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpForce = 6;
    public Rigidbody2D rb;
    public Animator animator;

    bool isGrounded = false;
    bool jumpAllowed = false;
    public Transform isGroundCheckerLeft;
    public Transform isGroundCheckerRight;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    bool previousRight;
    float maxY = -500f;
    bool death;
    bool deathParticlesCreated;
    public bool rewinding;

    [HideInInspector] public List<RewindData> rd = new List<RewindData>();
    GameObject rewindPanel;

    public Color32 baseColor;
    public Color32 rewindColor;

    public bool deathByTrap;

    AudioSource audioSource;
    public AudioClip clipJump;
    public AudioClip clipDie;

    public bool lockMovement;
    public bool fallingAlready;

    GameObject fallingText;
    bool animationPlayed;

    GameObject jumpThing;
    GameObject jumpThingEmpty;

    void Start()
    {
        // Set player in CameraMovement Script
        Camera.main.GetComponent<CameraMovement>().player = transform;
        rewindPanel = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainMenu>().rewind;
        fallingText = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainMenu>().fallingText;
        audioSource = Camera.main.GetComponent<AudioSource>();

        animator.Play("Jump", -1, 1f);
        jumpThing = GameObject.FindGameObjectWithTag("Jump");
        jumpThingEmpty = GameObject.FindGameObjectWithTag("JumpEmpty");
    }

    private void Update()
    {
        Controls();
        
        if (!lockMovement)
        {
            OutOfBoundCheck();
        }


        if (fallingAlready)
        {
            falling.Play();
            // Lock Y pos when reaching a certain Y
            float toLock = 25;
            maxY = -100;
            if (transform.position.y <= toLock && !animationPlayed)
            {
                // Lock it
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                // Ready to show text
                fallingText.SetActive(true);

                if (fallingText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    // Animation is done
                    animationPlayed = true;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    transform.position = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
                    fallingText.SetActive(false);
                    GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainMenu>().ChangeToSpecial();
                }
            }
        }
        else
        {
            if (falling.IsAlive())
            {
                falling.Stop();
            }
        }

        if (jumpAllowed)
        {
            jumpThing.SetActive(true);
            jumpThingEmpty.SetActive(false);
        }
        else
        {
            jumpThing.SetActive(false);
            jumpThingEmpty.SetActive(true);
        }

        if (rewinding)
        {
            jumpThing.SetActive(false);
            jumpThingEmpty.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        RewindHandler();

        if (rewinding)
        {
            Rewind();
        }
    }

    void Controls()
    {
        /* Keys:
         * - WAD
         * - Up Arrow, Left Arrow & Right Arrow
         * - Spacebar
         * 
         * Summary:
         * - Left: A & Left Arrow
         * - Right: D & Right Arrow
         * -
         */

        // Move left and right
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Jump");

        if (rewinding || lockMovement)
        {
            x = 0;
            y = 0;
        }

        if (x != 0 && x != 1 && x != -1)
        {
            x *= 1.2f;
        }
        else if (y != 0 && y != 1 && y != -1)
        {
            y *= 1.2f;
        }

        if (x > 1)
        {
            x = 1;
        }
        else if (x < -1)
        {
            x = -1;
        }

        if (y > 1)
        {
            y = 1;
        }
        else if (y < -1)
        {
            y = -1;
        }

        if (x < 0)
        {
            dust.transform.localScale = new Vector3(-1, 1, 1);
            if (previousRight == true)
            {

                CreateDust();
            }

            previousRight = false;
        }
        else if (x > 0)
        {
            dust.transform.localScale = new Vector3(1, 1, 1);
            if (previousRight == false)
            {
                CreateDust();
            }

            previousRight = true;
        }

        float moveBy = x * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);

        // Jump
        CheckIfGrounded();

        if (isGrounded)
        {
            jumpAllowed = true;
        }

        if (y > 0 && jumpAllowed && !death)
        {
            // Button pressed
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            audioSource.PlayOneShot(clipJump, 0.5f);
            animator.Play("Jump", -1, 0f);

            if (x != 0)
            {
                CreateDust();
            }
            else
            {
                CreateJumpDust();
            }

            jumpAllowed = false;
        }
    }

    void OutOfBoundCheck()
    {
        if (isGrounded)
        {
            maxY = transform.position.y - 9;
        }

        if ((transform.position.y <= maxY && !death && !rewinding) || deathByTrap)
        {
            // Death
            death = true;

            if (deathByTrap)
            {
                deathByTrap = false;
            }
        }

        if (death && !deathParticlesCreated)
        {
            // Disable renderer, show particles then after particles end move player back to respawn
            audioSource.PlayOneShot(clipDie, 0.5f);
            GetComponent<SpriteRenderer>().enabled = false;
            CreateDeathParticles();
            deathParticlesCreated = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        //transform.position = new Vector3(1, 1, transform.position.z);
        GetComponent<SpriteRenderer>().enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rewinding = true;
        death = false;
        deathParticlesCreated = false;
    }

    void Rewind()
    {
        if (rd.Count > 0)
        {
            Time.timeScale = 25;
            Camera.main.GetComponent<CameraMovement>().rewind = true;
            GetComponent<SpriteRenderer>().color = rewindColor;
            rewindPanel.SetActive(true);
            animator.enabled = false;
            RewindData data = rd[0];
            transform.position = data.position;
            transform.localScale = data.scale;
            rd.RemoveAt(0);
        }
        else
        {
            Time.timeScale = 1;
            rewinding = false;
            rewindPanel.SetActive(false);
            GetComponent<SpriteRenderer>().color = baseColor;
            Camera.main.GetComponent<CameraMovement>().rewind = false;
            animator.enabled = true;
        }
    }

    void CreateDeathParticles()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }

    void CheckIfGrounded()
    {
        Collider2D colliderLeft = Physics2D.OverlapCircle(isGroundCheckerLeft.position, checkGroundRadius, groundLayer);
        Collider2D colliderRight = Physics2D.OverlapCircle(isGroundCheckerRight.position, checkGroundRadius, groundLayer);

        if (colliderLeft != null || colliderRight != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void CreateDust()
    {
        dust.Play();
    }

    void CreateJumpDust()
    {
        jumpDust.Play();
    }

    void RewindHandler()
    {
        if (!rewinding)
        {
            // Save positions
            if (rd.Count > 0)
            {
                if (rd[0].position != transform.position || rd[0].scale != transform.localScale)
                {
                    RewindData data = new RewindData(transform.position, transform.localScale);
                    rd.Insert(0, data);
                }
            }
            else
            {
                RewindData data = new RewindData(transform.position, transform.localScale);
                rd.Insert(0, data);
            }
        }
    }

    public void Bounce(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
        audioSource.PlayOneShot(clipJump, 0.5f);
        animator.Play("Jump", -1, 0f);
        CreateJumpDust();
    }
}

[System.Serializable]
public class RewindData
{
    public Vector3 position;
    public Vector3 scale;

    public RewindData(Vector3 position, Vector3 scale)
    {
        this.position = position;
        this.scale = scale;
    }
}
