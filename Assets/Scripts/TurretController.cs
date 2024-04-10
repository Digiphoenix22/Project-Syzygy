using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target;
    public AudioSource audioSource;
    public GameObject projectile;
    public Transform spawnPos;
    public float launchSpeed;
    public float maxCoolDown;
    
    private float cooldownTime;
    private bool coolingDown = false;
    private bool active = false;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform; //sets player as target
    }


    // Update is called once per frame
    void Update()
    {
        if (target != null) //make sure player exists
        {
            Vector2 pos = transform.position; //easier position reference
            Vector2 direction = new Vector2(target.position.x - pos.x, target.position.y - pos.y); //direction towards player
            RaycastHit2D lookingForPlayer = Physics2D.Raycast(transform.position, direction); //raycast in direction of player

            //check if raycast hit player
            if (lookingForPlayer.collider.tag == "Player")
            {
                active = true; //set turret active
            }
            else { active = false; } //set turret deactive

            //only do if turret is active
            if (active)
            {

                transform.up = direction; //look towards target

                if (!coolingDown)
                {
                    {
                        Debug.Log("fired");
                        audioSource.Play();
                        GameObject bullet = Instantiate(projectile, spawnPos.position, transform.rotation); //spawn projectile

                        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * launchSpeed); //addforce to projectile

                        coolingDown = true;
                        cooldownTime = maxCoolDown;
                    }
                }
                else
                {
                    cooldownTime -= Time.deltaTime;
                    if (cooldownTime <= 0)
                    {
                        coolingDown = false;
                    }
                }
            }
        }
    }

}
