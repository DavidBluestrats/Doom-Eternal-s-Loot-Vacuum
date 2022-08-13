using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullAblePickup : MonoBehaviour
{
    //Variables to determine Drop Type
    GunHelper gunType;
    PlayerStats playerStats;
    public enum ammoType { Crossbow=0, SSG=1, Nailgun=2, Rocket=3,Flame=4,Tesla=5,Health=6,Armor=7};
    public ammoType type;
    public float ammoReward = 1f;

    //Variables for movement
    Transform playerTransform;
    public float suckSpeed = 35f;
    public float initialSuckSpeed = 20f;
    public float rotateSpeed = 200f;
    public float suckRadius = 10f;
    float dist;
    Vector3 rotateAmount;
    Rigidbody rb;
    bool hasFollowed;


    //TODO: Get a reference to the gun, so that we are able to know its type, current ammo and max ammo.
    private void Start()
    {
        //hasFollowed = false;
        //rb = GetComponent<Rigidbody>();
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //gunType = playerTransform.gameObject.GetComponent<SwitchWeapons>().guns[(int)type].GetComponent<GunHelper>();
        //Debug.Log("Ammo type: " + type.ToString());
       // gunType = GameObject.FindGameObjectWithTag(type.ToString()).GetComponent<GunHelper>();
    }
    private void FixedUpdate()
    {
        if (ShouldFollow() && CanGrabPickup())
        {
            rb.useGravity = false;
            GetComponent<SphereCollider>().isTrigger = true;
            hasFollowed = true;
            //rb.freezeRotation = false;
            FollowPlayer();
        }
        else
        {
            rb.useGravity = true;
            //rb.freezeRotation = true;
            GetComponent<SphereCollider>().isTrigger = false;
            transform.rotation = Quaternion.identity;
            initialSuckSpeed = 20f;
            if(hasFollowed)
                rb.velocity = new Vector3(0f, -9.81f, 0f);
        }
    }

    public void PreparePikcup(int index)
    {
        //TODO: Add a new parameter for ammo reward.
        hasFollowed = false;
        type = (ammoType)index;
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Jester").transform;
        if (index <= 5)
        {
            gunType = playerTransform.gameObject.GetComponent<SwitchWeapons>().guns[index].GetComponent<GunHelper>();
            Debug.Log("Gun Type: " + gunType.name);
            playerStats = null;
        }
        else
        {
            playerStats = playerTransform.gameObject.GetComponent<PlayerStats>();
            gunType = null;
        }
    }

    bool ShouldFollow()
    {
        dist = Vector3.Distance(playerTransform.position, rb.position);
        return (dist < suckRadius ? true : false);
        
    }
    void FollowPlayer()
    {
         
       Vector3 direction = playerTransform.position - rb.position;
       direction.Normalize();
       rotateAmount = Vector3.Cross(transform.up, direction);
       rb.angularVelocity = rotateAmount * rotateSpeed;
       if(dist <= 6f)
       {
            rb.velocity = direction * initialSuckSpeed;
       }
       else
       {
            rb.velocity = transform.up * initialSuckSpeed;
       }
        initialSuckSpeed += Time.fixedDeltaTime * 50f;
        initialSuckSpeed = Mathf.Clamp(initialSuckSpeed, 20f, suckSpeed);
    }

    bool CanGrabPickup()
    {
        if (gunType != null)
        {
            return gunType.CanGrabAmmo();
        }else if (playerStats != null)
        {
            switch (type)
            {
                case ammoType.Health:
                    {
                        return playerStats.CanPickHealth();
                    }
                case ammoType.Armor:
                    {
                        return playerStats.CanPickArmor();
                    }
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Jester"))
        {
            if (CanGrabPickup())
            {
                //Debug.Log("Rewarded player with " + ammoReward + " ammo.");
                //gunType.GiveAmmo(ammoReward);
                
                switch (type)
                {
                    case ammoType.Health:
                        {
                            playerStats.Heal(ammoReward);
                            break;
                        }
                    case ammoType.Armor:
                        {
                            playerStats.GiveArmor(ammoReward);
                            break;
                        }
                    default:
                        {
                            gunType.GiveAmmo(ammoReward);
                            break;
                        }
                }
                
                Destroy(gameObject);


            }
        }
        
    }

}


