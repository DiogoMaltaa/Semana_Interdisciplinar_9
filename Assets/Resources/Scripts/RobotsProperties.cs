using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsProperties : MonoBehaviour
{
    public int life;
    public int energyCost;
    public float rechargeTime;
    public int attackPower;

    // Update is called once per frame
    void Update()
    {
        CheckDeath();
    }

    public void CheckDeath()
    {
        if(life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
