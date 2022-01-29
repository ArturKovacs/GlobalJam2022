using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public bool GameIsPaused;
    [SerializeField]
    float BulletSpeedAtStart, BulletSpeed, DistanceOfChange, ValueOfChange, TimeChange, ReloadTime;

    [SerializeField]
    Transform Girl, GirlStartPosition, BottomScreen;
    [SerializeField]
    bool ChangeByDistance = true;
    [SerializeField]
    GameObject BulletPrefab, Robot;

    internal float GetBottomOfScreen() => BottomScreen.position.y;

    void Update()
    {
        SetBulletSpeed();
        
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
        var bullet = Instantiate(BulletPrefab, Robot.transform.position, Quaternion.identity);
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
