using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : MonoBehaviour
{
    public float Speed;
    public float JumpStrength;
    public float fastestReJumpTime;
    public bool IsContollerTarget;

    public List<GameObject> RemainigHearts;

    new Rigidbody2D rigidbody;

    ReloadManager reloadManager;

    HashSet<Platform> standingOnPlatforms;
    float lastJumpTime = 0;
    bool wasJumpDown = true;

    const int totalJumpAirJumpCount = 1;
    int remainingAirJumps = totalJumpAirJumpCount;

    //Coroutine currentRespawnAnimation;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        standingOnPlatforms = new HashSet<Platform>();
    }

    private void Start()
    {
        reloadManager = FindObjectOfType<ReloadManager>();
        //remainigLife = UiHearts.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ControlMovement();
        CheckFallen();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var platform = collision.collider.GetComponent<Platform>();
        if (platform != null)
        {
            float highestContactY = float.NegativeInfinity;
            foreach (var contact in collision.contacts)
            {
                if (contact.point.y > highestContactY) highestContactY = contact.point.y;
            }
            if (highestContactY < transform.position.y)
            {
                standingOnPlatforms.Add(platform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var platform = collision.collider.GetComponent<Platform>();
        if (platform != null)
        {
            standingOnPlatforms.Remove(platform);
        }
    }

    /// <summary>
    /// Check whether the girl has fallen off the screen
    /// </summary>
    protected void CheckFallen()
    {
        var mainCam = Camera.main;

        float camVerticalSize = mainCam.orthographicSize;

        float camBottomY = mainCam.transform.position.y - camVerticalSize;
        if (transform.position.y < camBottomY)
        {
            // Has fallen. Deal damage or re-start game
            if (TakeDamage())
            {
                ResetPosition();
            }
        }
    }

    /// <summary>
    /// Returns true iff the girl is still alive
    /// </summary>
    /// <returns></returns>
    protected bool TakeDamage()
    {
        if (RemainigHearts.Count == 0)
        {
            reloadManager.Died();
            return false;
        }
        else
        {
            var rightmostHeart = RemainigHearts[RemainigHearts.Count - 1];
            rightmostHeart.SetActive(false);
            RemainigHearts.RemoveAt(RemainigHearts.Count - 1);
            return true;
        }
    }

    protected void ResetPosition()
    {
        // Find the lowermost platform on the screen
        var allPlatforms = FindObjectsOfType<Platform>();
        var bottomOfScreen = GlobalManager.Instance.GetBottomOfScreen();
        Platform bottomMostPlatform = null;
        foreach (var item in allPlatforms)
        {
            float currY = item.transform.position.y;
            if (currY > bottomOfScreen)
            {
                if (bottomMostPlatform == null || bottomMostPlatform.transform.position.y > currY)
                {
                    bottomMostPlatform = item;
                }
            }
        }
        if (bottomMostPlatform == null)
        {
            Debug.Log("Could not find a platform on the screen. You died");
            reloadManager.Died();
        }
        else
        {
            // The multiplier should be the height of the girl
            transform.position = bottomMostPlatform.transform.position + Vector3.up * 1;
            StartCoroutine(AfterRespawnAnimation());
        }
    }

    protected void ControlMovement()
    {
        bool touchingGround = standingOnPlatforms.Count > 0;
        if (touchingGround)
        {
            remainingAirJumps = totalJumpAirJumpCount;
        }

        float horizontal = 0;
        bool isJumpDown = false;
        if (IsContollerTarget)
        {
            horizontal = Input.GetAxis("Horizontal");
            isJumpDown = Input.GetButton("Submit");
        }

        // F=m*a
        // a = ds/dt
        var targetSpeed = horizontal * Speed;
        var speedDiff = targetSpeed - rigidbody.velocity.x;
        rigidbody.AddForce(new Vector3(speedDiff, 0, 0) / Time.fixedDeltaTime);

        if (touchingGround || remainingAirJumps > 0)
        {
            var elapsedSinceLastJump = Time.time - lastJumpTime;
            if (isJumpDown && !wasJumpDown && elapsedSinceLastJump > fastestReJumpTime)
            {
                var vel = rigidbody.velocity;
                vel.y = JumpStrength;
                rigidbody.velocity = vel;
                lastJumpTime = Time.time;
                if (!touchingGround)
                {
                    remainingAirJumps -= 1;
                }
            }
        }

        wasJumpDown = isJumpDown;
    }

    IEnumerator AfterRespawnAnimation()
    {
        var renderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 7; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        renderer.enabled = true;
    }
}
