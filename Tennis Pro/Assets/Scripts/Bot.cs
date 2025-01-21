using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private float speed = 13f;
    private Animator animatorOfthebot;
    [SerializeField] private Transform ball;
    [SerializeField] private Transform hitTarget;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform center;
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private PlayerCtrl player1;
    [SerializeField] private Ball linemov;
    [SerializeField] private Transform line; 
    [SerializeField] private Transform limitoftheBot;
    //private float force = 13f;
    public Transform[] targets;
    private ShotManager shotManager;
    private Vector3 targetPosition;
    [SerializeField] private GameObject gameHitbox;
    private Vector3 PositionOfTHErandomBotTarget;
    private bool isMoving = false;
    private float minZ;
    private float maxZ;
    public AudioSource source_bot;
    public AudioClip sfxbot;

    public Vector3 PositionOfThErandomTarget => PositionOfTHErandomBotTarget;
    public bool CanMoveBot = false;
    // Start is called before the first frame update
    void Start()
    {
        source_bot.clip = sfxbot;
        gameHitbox.SetActive(false);
        animatorOfthebot = GetComponentInChildren<Animator>();
        targetPosition = transform.position;
        shotManager = GetComponent<ShotManager>();
        minZ = limitoftheBot.position.z;
        //maxZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMoveBot)
        {
            MoveBot();
        }
        else
        {
            MoveToCenter();
        }
        
    }

    void MoveToCenter(){
        


        // Calculate the direction vector from current position to the center
        Vector3 direction = center.position - transform.position;

        // Set the y component of the direction vector to 0
        direction.y = 0f;

        // Check if the distance is greater than the stopping distance
        if (direction.magnitude > stoppingDistance)
        {
            // Normalize the direction vector to get a unit vector
            direction.Normalize();

            // Use Translate to move towards the center in the x-axis only
            transform.Translate(new Vector3(direction.x, 0f, direction.z) * speed * Time.deltaTime);
        }
        else
        {
            // Check if the character was previously moving
            if (isMoving && player1.hasFinished == false)
            {
                // If yes, play the "standing" animation and reset the flag
                animatorOfthebot.Play("standing");
                isMoving = false;
            }
        }
    }

    void MoveBot()
    {
        float targetZ = Mathf.Clamp(ball.position.z, minZ, transform.position.z);
        targetPosition = new Vector3(ball.position.x, transform.position.y, targetZ);
    
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        animatorOfthebot.Play("running");
    }

    Vector3 PickTarget()
    {
        int randomValue = Random.Range(0, targets.Length);
        return targets[randomValue].position;
    }

    Shot PickShot()
    {
        int randomValue = Random.Range(0, 0);
        if (randomValue == 0)
            return shotManager.Topspin;
        else 
            return shotManager.flat;


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {   
            source_bot.Play();
            //linemov.isLineMoving = true;
            line.gameObject.SetActive(true);
            gameHitbox.SetActive(true);
            player1.CanMovePlayer = true;
            Shot currentShot = PickShot();
            
            PositionOfTHErandomBotTarget = PickTarget();
            Vector3 dir = PositionOfTHErandomBotTarget - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized*currentShot.hitforce + new Vector3(0, currentShot.upforce, 0);
            animatorOfthebot.Play("hit");
            CanMoveBot = false;
            isMoving = true;
        }
        
    }
}
