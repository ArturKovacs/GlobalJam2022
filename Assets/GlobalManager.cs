using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;
    public bool GameIsPaused;
    public GameObject PauseMenu;
    [SerializeField]
    float BulletSpeedAtStart, BulletSpeed, DistanceOfChange, ValueOfChange, TimeChange, ReloadTime;

    [SerializeField]
    Transform Girl, GirlStartPosition;
    [SerializeField]
    bool ChangeByDistance = true;
    [SerializeField]
    GameObject BulletPrefab, RobotMuzzle;

    internal float GetBottomOfScreen()
    {
        var cam = Camera.main;
        return cam.transform.position.y - cam.orthographicSize;
    }
    internal float GetTopOfScreen()
    {
        var cam = Camera.main;
        return cam.transform.position.y + cam.orthographicSize;
    }

    void Update()
    {
        SetBulletSpeed();

        if (Input.GetButtonDown("Cancel"))
        {
            bool shouldPause = !PauseMenu.activeSelf;
            PauseMenu.SetActive(shouldPause);
            if (shouldPause) Time.timeScale = 0;
            else Time.timeScale = 1;
        }
    }

    internal float GetBulletSpeed() => BulletSpeed;

    private IEnumerator BulletShooter()
    {
        while (!GameIsPaused)
        {
            Shoot();
            yield return new WaitForSeconds(ReloadTime);
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(BulletPrefab, RobotMuzzle.transform.position, Quaternion.identity);
    }

    void Start()
    {
        Instance = this;
        BulletSpeed = BulletSpeedAtStart;
        if (ChangeByDistance) SetBulletSpeed();
        else StartCoroutine(SpeedUpTimer());
        StartCoroutine(BulletShooter());
    }

    public IEnumerator SpeedUpTimer()
    {
        while(true){
            if(!GameIsPaused)
            BulletSpeed += ValueOfChange;
            yield return new WaitForSeconds(TimeChange);
        }
    }

    private void SetBulletSpeed()
    {
        if (ChangeByDistance)
        {
            var DistanceFromStart = Vector2.Distance(Girl.position, GirlStartPosition.position);
            BulletSpeed = BulletSpeedAtStart + ((int)(DistanceFromStart / DistanceOfChange) * ValueOfChange);
        }
    }
}
