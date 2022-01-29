using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : MonoBehaviour
{
    public float Speed;
    public float JumpStrength;
    public float fastestReJumpTime;
    public bool IsContollerTarget;

    new Rigidbody2D rigidbody;

    ReloadManager reloadManager;

    HashSet<Platform> standingOnPlatforms;
    float lastJumpTime = 0;
    bool wasJumpDown = true;

    const int totalJumpAirJumpCount = 1;
    int remainingAirJumps = totalJumpAirJumpCount;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        standingOnPlatforms = new HashSet<Platform>();
    }

    private void Start()
    {
        reloadManager = FindObjectOfType<ReloadManager>();
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
            reloadManager.Died();
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
}
