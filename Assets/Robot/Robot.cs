using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField]
    Transform PatrolLeftPosition, PatrolRightPosition;
    [SerializeField]
    float RobotSpeed;
    bool moveright = true;
    // Update is called once per frame
    void Update()
    {
        if (!GlobalManager.Instance.GameIsPaused)
            Move();
        TurnAround();
    }

    private void TurnAround()
    {
        if (transform.position.x > PatrolRightPosition.position.x) moveright = false;
        if (transform.position.x < PatrolLeftPosition.position.x) moveright = true;
    }

    private void Move()
    {
        var Direction = moveright ? 1 : -1;
        transform.Translate(new Vector3(Direction, 0, 0) * RobotSpeed * Time.deltaTime);
    }
}
