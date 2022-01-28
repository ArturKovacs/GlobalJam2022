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

    float lastJumpTime = 0;
    bool wasJumpDown = true;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (IsContollerTarget)
        {
            float horizontal = Input.GetAxis("Horizontal");

            var deltaPos = new Vector3(horizontal, 0, 0) * Speed * Time.deltaTime;
            transform.Translate(deltaPos);

            // Jump
            bool isJumpDown = Input.GetButton("Submit");
            var elapsedSinceLastJump = Time.time - lastJumpTime;
            if (isJumpDown && !wasJumpDown && elapsedSinceLastJump > fastestReJumpTime)
            {
                rigidbody.AddForce(Vector3.up * JumpStrength, ForceMode2D.Impulse);
                lastJumpTime = Time.time;
            }

            wasJumpDown = isJumpDown;
        }
    }
}
