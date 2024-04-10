using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float thrusterFuelMax = 100f; // Maximum fuel for mid-flight adjustments
    private float thrusterFuel; // Current fuel
    private bool isMidFlight = false; // Is the player in mid-flight?   

    public float initialThrust = 5f;
    public float maxThrust = 50f;
    public float rotationSpeed = 100f;
    public float cooldownTime = 4f; // Cooldown time in seconds before the ship can launch again

    public AudioClip deathSound; // Your existing death sound
    public AudioClip alternateDeathSound; // New field for the alternate sound

    public AudioSource audioSource; // SOUNDS YAY!!



    [SerializeField]
    int health = 1;

    [SerializeField]
    Object destructableRef;
    
    //Probably a better way of accessing UI elements than this 
    public ThrustBar thrustMeter; //Thrust bar UI
    public LaunchButton launchButton; //Launch Button UI

    private Vector2 lastVelocity;
    private bool canLaunch = true;
    private float thrust;
    private float cooldownTimer;

    // Private field for thrust with a public property to access and modify it
    private float _thrust;
    public float Thrust
    {
        get { return _thrust; }
        set { _thrust = Mathf.Clamp(value, 0, maxThrust); } // Ensure thrust stays within expected bounds
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _thrust = initialThrust; // Initialize _thrust with initialThrust value
        thrusterFuel = thrusterFuelMax;
    }

    void Update()
    {
         //////
        //
        //
        // Check Launch (prelaunch)
        //
        //
        //////
        // Track the ship's most recent velocity for use in collision reflection
        lastVelocity = rb.velocity;

        // Cooldown timer to control re-launch availability
        if (!canLaunch)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                canLaunch = true;
                isMidFlight = false;
                thrusterFuel = thrusterFuelMax;
                launchButton.LaunchState(canLaunch); //Update launch button UI
                cooldownTimer = 0;
            }
        }
        //////
        //
        //
        // Mid-flight State
        //
        //
        //////
        if (isMidFlight && !canLaunch)
        {
            // Use WASD for omnidirectional control
            Vector2 adjustment = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * thrusterFuelMax * Time.deltaTime;
            
            if (adjustment != Vector2.zero && thrusterFuel > 0)
            {
                rb.AddForce(adjustment * 10, ForceMode2D.Force);
                thrusterFuel -= adjustment.magnitude; // Consume fuel based on the magnitude of adjustment
                //while (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Verticle") != 0) 
                //{
                   // thrusterFuel -=1;
                //}
                thrusterFuel = Mathf.Max(thrusterFuel, 0); // Ensure fuel doesn't go below 0
            }
            

            /*        Disabled stationary recharge 
            // Check if the player is stationary to recharge fuel and allow for the next launch
            if (rb.velocity.magnitude < 0.1f) // Threshold to consider the ship as 'stationary'
            {
                isMidFlight = false;
                canLaunch = true;
                thrusterFuel = thrusterFuelMax; // Recharge fuel
                launchButton.LaunchState(canLaunch); // Update launch button UI
            }

            */
        }

        // Only accept input if the ship has not been launched
        if (canLaunch)
        {
            // Increase thrust while holding down space, up to a maximum
            if (Input.GetKey(KeyCode.Space))
            {
                _thrust = Mathf.Min(_thrust + Time.deltaTime * initialThrust * 4, maxThrust);
                thrustMeter.updateMeter(_thrust); //Update the level of the UI meter to show thrust power
            }

            // Launch when space is released
            if (Input.GetKeyUp(KeyCode.Space))
            {
                canLaunch = false;
                isMidFlight = true;
                launchButton.LaunchState(canLaunch); //Update launch button UI
                thrustMeter.updateMeter(0); //Reset thrust meter display after launch
                rb.drag = 0.5f; // Adjust drag for gameplay
                rb.AddForce(transform.up * _thrust, ForceMode2D.Impulse);
                _thrust = initialThrust; // Reset _thrust after launch
            }

            // Smooth rotation control
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, -rotation);
        }
        else
        {
            // Adjust drag based on velocity to simulate space friction or resistance
            rb.drag = rb.velocity.magnitude > 0.1 ? 0.1f : 1f;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {               

        health -= 1;
        if (health <= 0)
        {
            // Randomly decide to play the death sound or the alternate sound
        float chance = Random.Range(0f, 1f); // Generates a random number between 0.0 and 1.0
        if (chance < 0.1f) // 10% chance
        {
            // Play the alternate death sound if it's assigned
            if (alternateDeathSound != null)
            {
                audioSource.clip = alternateDeathSound;
                audioSource.Play();
            }
        }
        else
        {
            // Play the regular death sound
            if (deathSound != null)
            {
                audioSource.clip = deathSound;
                audioSource.Play();
            }
        }
        ExplodeThisGameObject();
    }


        if (col.gameObject.tag == ("Wall"))
        {
            // Reflects the angle of the ship's velocity and simulates a bounce effect
            var speed = lastVelocity.magnitude;
            var direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 1); // Ensure there's always some movement after collision

            // Corrects ship's rotation to match the new direction
            AdjustRotationAfterCollision(direction);
        }
    }

    private void ExplodeThisGameObject()
    {
        Vector3 currRot = new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        GameObject destructable = (GameObject)Instantiate(destructableRef, transform.position, Quaternion.Euler(currRot));

        Destroy(gameObject);
    }

    void OnGUI()
    {
    // Create a simple GUI box to display debug information
    GUI.Box(new Rect(10, 10, 200, 55), "Debug Info");
    
    // Display the thruster fuel level
    GUI.Label(new Rect(20, 30, 180, 20), $"Thruster Fuel: {thrusterFuel.ToString("F2")}/{thrusterFuelMax}");

    // Display whether the player is in mid-flight
    GUI.Label(new Rect(20, 50, 180, 20), $"Is Mid-Flight: {isMidFlight}");
}


    private void AdjustRotationAfterCollision(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
