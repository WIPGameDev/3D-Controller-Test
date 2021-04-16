using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] GameObject axe;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject greatsword;

    [SerializeField] Animator theAnim;
    [SerializeField] AudioSource audioSource;
    private PlayerController playerController;

    bool hasSword = false, hasAxe = false, hasGreatsword = false;
    private int weaponActive = 0;
    private float attackCounter;
    [SerializeField] float swordAttackCD, axeAttackCD, greatswordAttackCD;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        SwitchWeapon();
        Attack();
        AttackCD();
    }

    //Counts down the attack CD
    private void AttackCD()
    {
        if (attackCounter >= 0)
        {
            attackCounter -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        //Prevent the player from attacking mid-air
        if (playerController.isGrounded() != true) return;

        if(Input.GetButtonDown("Fire1"))
        {
            if (attackCounter > 0)
            {
                Debug.Log("A whiff sound or some audio que should be here");
            }
            else if (weaponActive == 1)
            {
                theAnim.SetTrigger("swordAttack");
                attackCounter = swordAttackCD;
                sword.GetComponent<AudioSource>().Play();
            }
            else if (weaponActive == 2)
            {
                theAnim.SetTrigger("axeAttack");
                attackCounter = axeAttackCD;
                axe.GetComponent<AudioSource>().Play();
            }
            else if (weaponActive == 3)
            {
                theAnim.SetTrigger("greatswordAttack");
                attackCounter = greatswordAttackCD;
                greatsword.GetComponent<AudioSource>().Play();
            }
        }       
    }

    //Allows the player to select a weapon in their inventory
    private void SwitchWeapon()
    {
        //Safeguard so you can't switch weapons while attacking
        if (attackCounter > 0) return;


        if (hasSword && Input.GetKeyDown("1"))
        {
            axe.SetActive(false);
            greatsword.SetActive(false);

            sword.SetActive(true);
            weaponActive = 1;
        }

        if (hasAxe && Input.GetKeyDown("2"))
        {
            greatsword.SetActive(false);
            sword.SetActive(false);

            axe.SetActive(true);
            weaponActive = 2;
        }

        if (hasGreatsword && Input.GetKeyDown("3"))
        {
            axe.SetActive(false);
            sword.SetActive(false);

            greatsword.SetActive(true);
            weaponActive = 3;
        }
    }

    public void AddWeapon(int weapon)
    {
        switch (weapon)
        {
            case 1:
                audioSource.Play();
                hasSword = true;

                axe.SetActive(false);
                greatsword.SetActive(false);
                sword.SetActive(true);
                weaponActive = 1;
                break;
            case 2:
                audioSource.Play();
                hasAxe = true;          
                
                greatsword.SetActive(false);
                sword.SetActive(false);
                axe.SetActive(true);
                weaponActive = 2;
                break;
            case 3:
                audioSource.Play();
                hasGreatsword = true;

                axe.SetActive(false);                
                sword.SetActive(false);
                greatsword.SetActive(true);
                weaponActive = 3;
                break;
            default:
                Console.WriteLine("Unkown Weapon Picked Up");
                break;
        }
    }
}
