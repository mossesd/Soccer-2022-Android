using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    [HideInInspector]
    public bool isKicked = false;

    private Vector3 initialPosition;
    public Transform ownerPlayer;
    private float lastTime = 0;

    [HideInInspector]
    public string lastOwnerTag = "";
    internal bool isFree;

    void Awake()
    {
        gameObject.name = "Ball";
    }

    void Start()
    {
        ownerPlayer = null;
        initialPosition = transform.position;
    }

    public void PlaceOnInitialPositon()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        isKicked = false;
        ownerPlayer = null;
        lastTime = Time.time;
        lastOwnerTag = "";
        transform.position = initialPosition;
    }

    public void SetFree()
    {
        lastTime = Time.time;
        ownerPlayer = null;
    }

    void Update()
    {

        if (GetComponent<Rigidbody>().velocity.magnitude > 20f)
        {
            Vector3 vel = GetComponent<Rigidbody>().velocity.normalized;
            GetComponent<Rigidbody>().velocity = vel * 20f;
        }

        if (ownerPlayer != null)
            if (GameManager.SharedObject().IsGameReady == false && ownerPlayer.tag != "Hand")
                SetFree();

        if (GameManager.SharedObject().OpponentMadeFoul || GameManager.SharedObject().PlayerMadeFoul || GameManager.SharedObject().IsGameReady == false)
        {
            if (ownerPlayer)
                transform.position = ownerPlayer.position;
        }
        else
        {
            if (ownerPlayer)
            {
                transform.position = new Vector3(0, transform.position.y, 0) + new Vector3(ownerPlayer.position.x, 0, ownerPlayer.position.z) + ownerPlayer.forward * 0.5f;

                if (ownerPlayer.tag == "Player" && ownerPlayer.GetComponent<Player>().isMoving)
                    transform.RotateAround(transform.position, ownerPlayer.right, 350 * Time.deltaTime);
                else if (ownerPlayer.tag == "ComputerPlayer" && ownerPlayer.GetComponent<AI_DefenderScript>().isMoving)
                    transform.RotateAround(transform.position, ownerPlayer.right, 350 * Time.deltaTime);
            }
        }

        if (transform.position.y < -0.15f)
            transform.position = new Vector3(transform.position.x, 0.15f, transform.position.z);

        // Detect shooting input
        if (Input.GetKeyDown(KeyCode.M) && ownerPlayer != null)
        {
            StartCoroutine(ShootBall());
        }
    }


    public void PassBall()
    {
        // Implement passing logic
        if (ownerPlayer != null)
        {
            // Set the ball as free and apply a force in the forward direction of the player
            isFree = true;
            ownerPlayer = null;
            GetComponent<Rigidbody>().velocity = transform.forward * 10; // Adjust the force as needed
        }
    }

    public IEnumerator ShootBall()
    {
        // Implement shooting logic
        if (ownerPlayer != null)
        {
            Debug.Log("Shooting ball...");

            // Check if the ownerPlayer has an Animation component
            Animation playerAnimation = ownerPlayer.GetComponent<Animation>();
            if (playerAnimation != null)
            {
                Debug.Log("Playing shoot animation...");
                playerAnimation.Play("tiro", PlayMode.StopAll);
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                Debug.LogWarning("Owner player does not have an Animation component.");
            }

            // Set the ball as free and apply a force in the forward direction of the player
            isFree = true;

            // Calculate the shooting force with an upward component
            Vector3 shootDirection = ownerPlayer.forward + new Vector3(0, 0.5f, 0); // Adjust the upward component as needed
            GetComponent<Rigidbody>().velocity = shootDirection.normalized * 500; // Adjust the force magnitude as needed

            ownerPlayer = null;
        }
        else
        {
            Debug.LogWarning("No owner player assigned to the ball.");
        }
    }


    public void TackleBall()
    {
        // Implement tackling logic
        if (ownerPlayer != null)
        {
            // Set the ball as free and apply a force in a specific direction
            isFree = true;
            ownerPlayer = null;
            GetComponent<Rigidbody>().velocity = transform.forward * 5; // Adjust the force as needed
        }
    }

    public void PassBallTo(Transform nearestPlayer)
    {
        // Implement passing to a specific player logic
        if (nearestPlayer != null)
        {
            // Set the ball as free and apply a force towards the nearest player
            isFree = true;
            ownerPlayer = null;
            Vector3 direction = (nearestPlayer.position - transform.position).normalized;
            float forceMagnitude = 10f; // Adjust the force as needed
            GetComponent<Rigidbody>().velocity = direction * forceMagnitude;
        }
    }


    public void SetOwnerIfPossible(Transform owner)
    {
        if (ownerPlayer != null && owner.tag == "Hand")
            return;

        isKicked = false;

        if (ownerPlayer == null && Time.time - lastTime > 1f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ownerPlayer = owner;

            lastOwnerTag = ownerPlayer.tag;

            Debug.Log("Owner player set to: " + ownerPlayer.name);
        }
    }

    public void SetOwner(Transform owner)
    {
        if (ownerPlayer != null && owner.tag == "Hand")
            return;

        isKicked = false;

        if (Time.time - lastTime > 2f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ownerPlayer = owner;

            lastOwnerTag = ownerPlayer.tag;
            lastTime = Time.time;

            Debug.Log("Owner player set to: " + ownerPlayer.name);
        }
    }
}
