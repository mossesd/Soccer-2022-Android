using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static float ballGetableDistance = 0.5f;
    public OtherDialoguesActive ode;
    public GameObject matchComplete, halfComplete;
    public Texture barTexture;
    public Texture sprintButton;
    public Texture passButton;
    public Texture shootButton;
    public Texture tackleButton;
    public Texture sprintButtonSel;
    public Texture passButtonSel;
    public Texture shootButtonSel;
    public Texture tackleButtonSel;

    private bool sprintButtonPressed;
    private bool passButtonPressed;
    private bool shootButtonPressed;
    private bool tackleButtonPressed;

    private Rect sprintButtonRect;
    private Rect passButtonRect;
    private Rect shootButtonRect;
    private Rect tackleButtonRect;

    int moveSpeed = 5;
    [HideInInspector]
    public bool isMoving = false;

    private float progress = 0;
    private Rect touchRect;
    private GameObject theBall;
    private BallScript theBallScript;
    private Rigidbody ballRigidbody;

    public Transform[] players;
    private Transform highlight;

    public Vector3 initialPosition, targetPosition;

    public enum PlayerType { AttackerLeft, AttackerRight, DefenderLeft, DefenderRight, MidFielderLeft, MidFielderRight };
    public PlayerType playerType = PlayerType.AttackerLeft;

    private float sprintStamina = 10f;
    float lastTackleTime = 0f;

    public static bool noControls = false;
    internal object number;
    internal object position;

    void Start()
    {
        ode = GameObject.Find("Main Camera").GetComponent<OtherDialoguesActive>();
        lastTackleTime = Time.time;

        sprintButtonRect = new Rect(Screen.width - GetValue(150), Screen.height - GetValue(150), GetValue(130), GetValue(130));
        shootButtonRect = new Rect(Screen.width - GetValue(150) - GetValue(130), Screen.height - GetValue(150), GetValue(110), GetValue(110));
        tackleButtonRect = new Rect(Screen.width - GetValue(150) - GetValue(130), Screen.height - GetValue(150), GetValue(110), GetValue(110));
        passButtonRect = new Rect(Screen.width - GetValue(150), Screen.height - GetValue(150) - GetValue(130), GetValue(110), GetValue(110));

        initialPosition = transform.position;
        targetPosition = initialPosition;

        highlight = GameObject.Find("Highlight").transform;
        touchRect = new Rect(Screen.width / 3 * 2, 0, Screen.width / 3, Screen.height);
        theBall = GameObject.FindGameObjectWithTag("TheSoccerBall");

        ballRigidbody = theBall.GetComponent<Rigidbody>(); // Initialize Rigidbody

        GameObject[] playersT = GameObject.FindGameObjectsWithTag("Player");
        players = new Transform[playersT.Length];
        for (int i = 0; i < playersT.Length; i++)
            players[i] = playersT[i].transform;

        theBallScript = theBall.GetComponent<BallScript>();
        GameObject ball = GameObject.FindGameObjectWithTag("TheSoccerBall");
        if (ball != null)
        {
            ballRigidbody = ball.GetComponent<Rigidbody>();
        }
    }

    Transform ControllablePlayer()
    {
        if (HasTheBall()) return transform;

        Transform idealPlayer = transform;
        foreach (Transform player in players)
        {
            if (Vector3.Distance(theBall.transform.position, player.position) < Vector3.Distance(theBall.transform.position, idealPlayer.position))
                idealPlayer = player;
        }

        return idealPlayer;
    }

    bool HasTheBall()
    {
        return (theBall.GetComponent<BallScript>().ownerPlayer == transform);
    }

    void Update()
    {
        if (noControls)
        {
            if (GetComponent<Animation>()["reposo"].enabled == false)
                GetComponent<Animation>().Play("reposo", PlayMode.StopAll);
            return;
        }

        if (GameManager.SharedObject().OpponentMadeFoul || GameManager.SharedObject().PlayerMadeFoul)
        {
            gameObject.GetComponent<Player>().enabled = false;
            gameObject.GetComponent<PlayerFoulHandler>().enabled = true;
            return;
        }

        if (GameManager.SharedObject().PlayerGotCornerKick || GameManager.SharedObject().OpponentGotCornerKick)
        {
            gameObject.GetComponent<Player>().enabled = false;
            gameObject.GetComponent<PCornerKickHandler>().enabled = true;
            return;
        }

        if (Vector3.Distance(theBall.transform.position, transform.position) < ballGetableDistance && GameManager.SharedObject().IsGameReady)
            theBall.GetComponent<BallScript>().SetOwnerIfPossible(transform);

        isMoving = false;
        if (transform == ControllablePlayer())
        {
            Vector3 position = transform.position;
            position.y = 0.01f;
            highlight.position = position;
            float multiplier = 1f;

            if (GameManager.SharedObject().IsFirstHalf)
                multiplier = 1f;
            else
                multiplier = -1f;

            if (GameManager.SharedObject().IsGameReady == false)
                multiplier = 0f;

#if !UNITY_EDITOR
            float x = multiplier * GameObject.Find("Single Joystick").GetComponent<Joystick>().position.x;
            float y = multiplier * GameObject.Find("Single Joystick").GetComponent<Joystick>().position.y;
            if (((Mathf.Abs(x) > 0.1f) || (Mathf.Abs(y) > 0.1f)) && (!waitForPass || HasTheBall()))
            {
                if (GetComponent<Animation>()["tiro"].enabled == false)
                    transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * 0.65f);

                isMoving = true;

                if (GetComponent<Animation>()["corriendo"].enabled == false && GetComponent<Animation>()["tiro"].enabled == false)
                    GetComponent<Animation>().Play("corriendo", PlayMode.StopAll);

                transform.eulerAngles = new Vector3(0, 90 + Mathf.Atan2(-y, x) * 180 / Mathf.PI, 0);
            }

            if (transform == ControllablePlayer())
                foreach (Touch touch in Input.touches)
                {
                    Vector2 inputGuiPosition = touch.position;
                    inputGuiPosition.y = Screen.height - inputGuiPosition.y;

                    if (touch.phase != TouchPhase.Canceled && HasTheBall())
                    {
                        if (sprintButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
                        {
                            sprintButtonPressed = true;
                            break;
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            if (sprintButtonPressed == true)
                            {
                                sprintButtonPressed = false;
                                break;
                            }
                        }

                        if (passButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
                            passButtonPressed = true;
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            if (passButtonPressed == true)
                            {
                                passButtonPressed = false;
                                //PASS CODE HERE..
                                StartCoroutine(PassTheBall());
                                Player.ballGetableDistance = 2.5f;
                                waitForPass = true;
                                Invoke("waitForPassMethod", 3.5f);
                                break;
                            }
                        }

                        if (shootButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
                            shootButtonPressed = true;
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            if (shootButtonPressed == true)
                            {
                                shootButtonPressed = false;
                                //SHOOT CODE HERE..
                                progress = 1;
                                StartCoroutine(KickTheBall());
                                break;
                            }
                        }
                    }
                    if (touch.phase != TouchPhase.Canceled && !HasTheBall())
                    {
                        if (sprintButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
                        {
                            sprintButtonPressed = true;
                            break;
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            if (sprintButtonPressed == true)
                            {
                                sprintButtonPressed = false;
                                break;
                            }
                        }

                        if (tackleButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
                            tackleButtonPressed = true;
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            if (tackleButtonPressed == true)
                            {
                                tackleButtonPressed = false;
                                //TACKLE CODE HERE..
                                if (Time.time - lastTackleTime > 3 && theBallScript.ownerPlayer != null && Vector3.Distance(theBallScript.ownerPlayer.position, transform.position) < 1 && GameManager.SharedObject().IsGameReady)
                                {
                                    theBallScript.ownerPlayer.gameObject.GetComponent<Animation>().Play("entrada", PlayMode.StopAll);
                                    theBallScript.SetOwner(transform);
                                    lastTackleTime = Time.time;
                                }
                                break;
                            }
                        }
                    }
                    else if (touch.phase == TouchPhase.Canceled)
                    {
                        passButtonPressed = false;
                        sprintButtonPressed = false;
                        shootButtonPressed = false;
                    }
                    else if (progress > 0)
                    {
                        StartCoroutine(KickTheBall());
                    }
                }

            if (sprintButtonPressed && sprintStamina > 0)
            {
                sprintStamina -= Time.deltaTime;
                moveSpeed = 8;
                GetComponent<Animation>()["corriendo"].speed = 1.5f;
            }
            else
            {
                moveSpeed = 5;
                GetComponent<Animation>()["corriendo"].speed = 1f;
            }
#endif
#if UNITY_EDITOR
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            if (((Mathf.Abs(x) > 0.1f) || (Mathf.Abs(y) > 0.1f)) && (!waitForPass || HasTheBall()))
            {
                if (GetComponent<Animation>()["tiro"].enabled == false)
                    transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * 0.65f);

                isMoving = true;

                if (GetComponent<Animation>()["corriendo"].enabled == false && GetComponent<Animation>()["tiro"].enabled == false)
                    GetComponent<Animation>().Play("corriendo", PlayMode.StopAll);

                transform.eulerAngles = new Vector3(0, 90 + Mathf.Atan2(-y, x) * 180 / Mathf.PI, 0);
            }
            if (Input.GetKeyDown(KeyCode.Space) && !HasTheBall())
            {
                if (Time.time - lastTackleTime > 3 && theBallScript.ownerPlayer != null && Vector3.Distance(theBallScript.ownerPlayer.position, transform.position) < 1 && GameManager.SharedObject().IsGameReady)
                {
                    theBallScript.ownerPlayer.gameObject.GetComponent<Animation>().Play("entrada", PlayMode.StopAll);
                    theBallScript.SetOwner(transform);
                    lastTackleTime = Time.time;
                }
            }
            if (Input.GetKeyDown(KeyCode.M) && HasTheBall())
            {
                progress = 1;
                StartCoroutine(KickTheBall());
            }
            if (Input.GetKeyDown(KeyCode.E) && HasTheBall())
            {
                StartCoroutine(PassTheBall());
            }
            if (Input.GetKey(KeyCode.LeftShift) && sprintStamina > 0)
            {
                sprintStamina -= Time.deltaTime;
                moveSpeed = 8;
                GetComponent<Animation>()["corriendo"].speed = 1.5f;
            }
            else
            {
                moveSpeed = 5;
                GetComponent<Animation>()["corriendo"].speed = 1f;
            }
            // Check for the new shoot and pass key presses
            if (Input.GetKeyDown(KeyCode.M) && HasTheBall())
            {
                progress = 1;
                StartCoroutine(KickTheBall());
            }
            if (Input.GetKeyDown(KeyCode.N) && HasTheBall())
            {
                StartCoroutine(PassTheBall());
            }
#endif
        }

        if (HasTheBall())
        {
            if (GetComponent<Animation>()["reposo"].enabled == false && GetComponent<Animation>()["corriendo"].enabled == false && GetComponent<Animation>()["tiro"].enabled == false)
                GetComponent<Animation>().Play("reposo", PlayMode.StopAll);
        }

        sprintStamina = Mathf.Clamp(sprintStamina + 0.5f * Time.deltaTime, 0f, 10f);
    }

    IEnumerator KickTheBall()
    {
        isMoving = false;
        GetComponent<Animation>().Play("tiro", PlayMode.StopAll);

        // Apply force to the ball
        if (ballRigidbody != null)
        {
            // Get the direction the player is facing
            Vector3 shootDirection = transform.forward; // Use the player's forward direction

            // Define the force magnitude (adjust as needed)
            float forceMagnitude = 100f;

            // Apply force to the ball
            ballRigidbody.AddForce(shootDirection * forceMagnitude, ForceMode.Impulse);
        }

        // Reset progress and animation
        while (progress > 0)
        {
            progress -= Time.deltaTime;
            yield return null;
        }
        GetComponent<Animation>().Play("reposo", PlayMode.StopAll);
    }



    IEnumerator PassTheBall()
    {
        Transform passReceiver = FindNearestPlayer();
        if (passReceiver != null)
        {
            theBallScript.PassBallTo(passReceiver);
            theBallScript.SetFree();
        }
        yield return null;
    }

    private Transform FindNearestPlayer()
    {
        Transform nearestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            if (player != this.transform)
            {
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPlayer = player;
                }
            }
        }

        return nearestPlayer;
    }

    void OnGUI()
    {
        if (noControls) return;

        GUI.DrawTexture(new Rect(10, Screen.height - GetValue(45) - 10, GetValue(150), GetValue(45)), barTexture);
        GUI.DrawTexture(passButtonRect, passButtonPressed ? passButtonSel : passButton);
        if (HasTheBall())
            GUI.DrawTexture(shootButtonRect, shootButtonPressed ? shootButtonSel : shootButton);
        else
            GUI.DrawTexture(tackleButtonRect, tackleButtonPressed ? tackleButtonSel : tackleButton);
        GUI.DrawTexture(sprintButtonRect, sprintButtonPressed ? sprintButtonSel : sprintButton);

        GUI.BeginGroup(new Rect(15, Screen.height - GetValue(45) - 5, (GetValue(150) - 10) * sprintStamina / 10, GetValue(35)));
        GUI.DrawTexture(new Rect(0, 0, GetValue(150), GetValue(45)), barTexture);
        GUI.EndGroup();
    }

    private float GetValue(float value)
    {
        return (Screen.height / 768f) * value;
    }

    private bool waitForPass = false;
    private void waitForPassMethod()
    {
        Player.ballGetableDistance = 0.5f;
        waitForPass = false;
    }
}
