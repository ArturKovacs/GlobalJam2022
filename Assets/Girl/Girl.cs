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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        bool touchingGround = standingOnPlatforms.Count > 0;
        if (touchingGround)
        {
            remainingAirJumps = totalJumpAirJumpCount;
        }

        if (IsContollerTarget)
        {
            float horizontal = Input.GetAxis("Horizontal");

            // F=m*a
            // a = ds/dt
            var targetSpeed = horizontal * Speed;
            var speedDiff = targetSpeed - rigidbody.velocity.x;
            //var deltaPos = new Vector3(horizontal, 0, 0) * Speed * Time.deltaTime;
            //transform.Translate(deltaPos);
            rigidbody.AddForce(new Vector3(speedDiff, 0, 0) / Time.fixedDeltaTime);

            bool isJumpDown = Input.GetButton("Submit");

            if (touchingGround || remainingAirJumps > 0)
            {
                var elapsedSinceLastJump = Time.time - lastJumpTime;
                if (isJumpDown && !wasJumpDown && elapsedSinceLastJump > fastestReJumpTime)
                {
                    var vel = rigidbody.velocity;
                    vel.y = JumpStrength;
                    //rigidbody.AddForce(Vector3.up * JumpStrength, ForceMode2D.Impulse);
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
}
