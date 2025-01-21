using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    //[SerializeField] Transform ball;
    public Transform AimTarget;
    private bool hitting;
    //private float force = 13f;
    private Animator animator;
    private Animator animatorOFthebot;
    private Vector3 aimTargetInitialPosition;
    private ShotManager shotManager;
    private Shot currenshot;
    public Transform[] targets;
    [SerializeField] private Bot bot;
    private Vector3 targetPosition;
    [SerializeField] private Transform ball;
    [SerializeField] private Transform center;
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private GameObject line;
    [SerializeField] private HealthSlider slider;
    [SerializeField] private Transform limitoftheplayer;
    [SerializeField] private GameObject winner_canvas;
    public bool CanMovePlayer= false;
    [SerializeField] private GameObject gameHitbox;
    private Vector3 PositionOfTHErandomTarget;
    public bool hasFinished = false;
    private bool isMoving = false;
    public Vector3 PositionOfThErandomTarget => PositionOfTHErandomTarget;
    private float minZ;
    private float maxZ;
    private string blendParameter = "Blend";
    [SerializeField] private CinemachineDollyCart cinemachine; // this is to access the dolly cart speed of kratos
    [SerializeField] private CinemachineDollyCart cinemachine_sherk; // // this is to access the dolly cart speed of Sherk
    public CinemachineVirtualCamera MainCamera; // Reference to your main camera
    public CinemachineVirtualCamera KratosCamera; 
    public CinemachineVirtualCamera SherkCamera;// Reference to your virtual camera
    public AudioSource source;
    public AudioClip sfxplayer, sfx_finish, kratos_die_sfx;


    void Start()
    {
        source.clip = sfxplayer;
        
        SherkCamera.gameObject.SetActive(false);
        MainCamera.enabled = true;
        KratosCamera.gameObject.SetActive(false);
        gameHitbox.SetActive(false);
        animator = GetComponentInChildren<Animator>();
        animatorOFthebot = bot.GetComponentInChildren<Animator>();
        aimTargetInitialPosition = AimTarget.position;
        shotManager = GetComponent<ShotManager>();
        currenshot = shotManager.Topspin;
        minZ = limitoftheplayer.position.z;
        maxZ = transform.position.z;
        animator.SetFloat(blendParameter,0.5f);
        winner_canvas.SetActive(false);
    }

    void Update()
    {
        
        if (CanMovePlayer)
        {
            MovePlayer();
        }
        else
        {
            MoveToCenter();
        }
        if(slider.sliderValue >= 1f && !hasFinished)
        {
            Debug.Log("finish");
            Finish();
            
        }
        
    }
    
    void MovePlayer()
    {
        targetPosition = new Vector3(ball.position.x, transform.position.y, transform.position.z);
        float clampedZ = Mathf.Clamp(targetPosition.z, minZ, maxZ);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y, clampedZ), speed * Time.deltaTime);
        animator.Play("running");
    }

    void Finish()
    {
        
        CanMovePlayer = false;
        animator.Play("win_dance");
        ball.gameObject.SetActive(false);
        animatorOFthebot.Play("die");
        hasFinished = true;
        cinemachine.m_Speed = 4f;
        MainCamera.enabled = false; // Disable the main camera
        KratosCamera.gameObject.SetActive(true); // Activate the virtual camera
        StartCoroutine(kratos_die_sfx_activation(0.8f));
        StartCoroutine(sherk_wins_sfx_activation(5.2f));
        StartCoroutine(DeactivateWithDelay(4f));
        StartCoroutine(Change_to_MainMenu(11f));

    }
    
    IEnumerator Change_to_MainMenu(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(0);

    }
   
    
    void MoveToCenter(){
        


        // Calculate the direction vector from current position to the center
        Vector3 direction = center.position - transform.position;

        // Check if the distance is greater than the stopping distance
        if (direction.magnitude > stoppingDistance)
        {
            // Normalize the direction vector to get a unit vector
            direction.Normalize();

            // Use Translate to move towards the center
            transform.Translate(direction * speed * Time.deltaTime);
            
            
        }
        else
        {
            // Check if the character was previously moving
            if (isMoving)
            {
                // If yes, play the "standing" animation and reset the flag
                animator.Play("standing");
                isMoving = false;
            }
        }

        
    }
    Vector3 PickTarget()
    {
        int randomValue = Random.Range(0, targets.Length);
        return targets[randomValue].position;
    }
    IEnumerator kratos_die_sfx_activation(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        
        source.clip = kratos_die_sfx;
        source.Play();

    }
    
    IEnumerator sherk_wins_sfx_activation(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        
        source.clip = sfx_finish;
        source.Play();

    }
    
    
    IEnumerator DeactivateWithDelay(float delayInSeconds)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        cinemachine_sherk.m_Speed = 4f;

        // Deactivate the GameObject after the delay
        
        KratosCamera.gameObject.SetActive(false);
        SherkCamera.gameObject.SetActive(true);
        winner_canvas.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            source.Play();
            gameHitbox.SetActive(false);
            line.SetActive(false);
            CanMovePlayer = false;
            bot.CanMoveBot = true;
            PositionOfTHErandomTarget = PickTarget();
            Vector3 dir = PositionOfTHErandomTarget - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized*currenshot.hitforce + new Vector3(0, currenshot.upforce, 0);
            animator.Play("hit");
            AimTarget.position = aimTargetInitialPosition;
            isMoving = true;
        }
    }
}


/*float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitting = true;
            currenshot = shotManager.Topspin;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true;
            currenshot = shotManager.flat;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }

        if (hitting)
        {
            AimTarget.Translate(new Vector3(h, 0, 0)*speed*Time.deltaTime);
        }


        if ((h != 0 || v != 0) && !hitting)
        {
            transform.Translate(new Vector3(h, 0, v)*speed*Time.deltaTime);
        }*/
