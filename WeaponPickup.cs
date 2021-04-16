using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    //Initialised at 1 and limited by range to weapons already in the game
    [SerializeField][Range(1,3)] int whichWeapon = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<WeaponController>().AddWeapon(whichWeapon);
            Destroy(gameObject);
        }
    }
}
