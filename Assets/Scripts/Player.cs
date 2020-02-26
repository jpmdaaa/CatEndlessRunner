using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody rb;
    private BoxCollider boxCollider;
    public Animator an;
    public float speed;
    public float minSpeed = 8f;
    public float maxSpeed = 30f;
    public float currentSpeed;
    public float invencibleTime;
    public float doubleScoreTime;
    [HideInInspector]
    public int coins;
    private int currentLane = 1;
    private Vector3 verticalTargetPosition;
    public float laneSpeed;
    public float jumpLength;
    public float jumpHeigth;
    private bool jumping = false;   
    private float jumpStart;
    private bool sliding = false;
    private float slideStart;
    public float slideLength;
    private Vector3 boxColliderSize;
    public int maxLife = 3;
    private int currentLife;
    private bool invencible = false;
    //static int blinkValue;
    private bool isSwipping = false;
    private bool doublescore = false;
    private Vector2 startingTouch;
    public GameObject model;
    private UlManager ulManager;
    [HideInInspector]
    public float score;
    public bool canMove;
   
    void Start()
    {
        canMove = false;
        rb = gameObject.GetComponent<Rigidbody>();
        an = gameObject.GetComponentInChildren<Animator>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
      
        currentLife = maxLife;
      
        ulManager = FindObjectOfType<UlManager>();
       
        ulManager.UpdateCoins(coins);
        GameManeger.gm.StartMissions();
        currentSpeed = minSpeed;

        Invoke("StartRun", 3f); 
    }


    void Update()
    {
        if (!canMove)
            return; 
        score += Time.deltaTime * speed;
        
        ulManager.UpdateScore((int)score);
        
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide();
        }

        if(Input.touchCount== 1)
        {
            if(isSwipping)
            {
                Vector2 diff = Input.GetTouch(0).position - startingTouch;
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);

                if(diff.magnitude>0.01f)
                {
                    if(Mathf.Abs(diff.y)> Mathf.Abs(diff.x))
                    {
                        if(diff.y<0)
                        {
                            Slide();
                        }
                        else
                        {
                            Jump();
                        }
                    }
                    else
                    {
                        if(diff.x<0)
                        {
                            ChangeLane(-1);
                        }
                        else
                        {
                            ChangeLane(1);
                        }

                    }
                    isSwipping = false;
                }

            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startingTouch = Input.GetTouch(0).position;
                isSwipping = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isSwipping = false;
            }


        }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if(ratio>=1)
            {
                jumping = false;
                an.SetBool("Jumping", false);
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeigth;
            }

        }
        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }

        if(sliding)
        {
            float ratio = (transform.position.z - slideStart) / slideLength;
            if(ratio>=1)
            {
                sliding = false;
                an.SetBool("Sliding", false);
                boxCollider.size = boxColliderSize;

            }
        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y,gameObject.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

   
    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;


    }
    private void StartRun()
    {
        PlayServices.UnlockAchiviment(CatRunServices.achievement_faa_uma_corrida);
        canMove = true;
        an.Play("runStart");
        speed = minSpeed;
    }

    private void Jump()
    {
        if(!jumping)
        {
            jumpStart = transform.position.z;
            an.SetFloat("JumpSpeed", speed / jumpLength);
            an.SetBool("Jumping", true);
            jumping = true;
        }
    }
    private void Slide()
    {
        if (!jumping && !sliding)
        {
            slideStart = transform.position.z;
            an.SetFloat("JumpSpeed", speed / slideLength);
            an.SetBool("Sliding", true);
            Vector3 newSize = boxCollider.size;
            newSize.y = newSize.y / 2;
            boxCollider.size = newSize;
            sliding = true;
        }
    }
    private void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;

        if (targetLane < 0 || targetLane > 2)
        {
            return;
        }

        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0);


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            PlayServices.IncrementeAchivement(CatRunServices.achievement_colete_100_peixes, 1);
            coins++;
            ulManager.UpdateCoins(coins);
            other.transform.parent.gameObject.SetActive(false);

            if (coins > PlayServices.GetPlayerScore(CatRunServices.leaderboard_ranking_coins))
            {
                PlayServices.PostScore((long)score, CatRunServices.leaderboard_ranking_coins);
            }


        }

        if (other.CompareTag("DoubleScore"))
        {
            StartCoroutine(DoublePoints(doubleScoreTime));
            other.gameObject.SetActive(false);
        }

        if (invencible)
            return;
        if(other.CompareTag("Obstacle"))
        {
            canMove = false;
            currentLife--;
            ulManager.UpdateLives(currentLife);
            an.SetTrigger("Hit");
            speed = 0;
         

            if(currentLife<=0)
            {
                speed = 0;
                an.SetBool("Dead",true);
                ulManager.gameOverPanel.SetActive(true);

                if(score>PlayServices.GetPlayerScore(CatRunServices.leaderboard_ranking))
                {
                    PlayServices.PostScore((long)score, CatRunServices.leaderboard_ranking);
                }

                Invoke("CallMenu", 2f);


             
            }
            else
            {
                Invoke("CanMove", 0.75f);
             
                StartCoroutine(Blinking(invencibleTime));
            }

        }
    }

     void CanMove()
    {
        canMove = true;
        an.Play("runStart");

    }

    IEnumerator Blinking(float time)
    {
        invencible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0;
        float blinkPeriod = 0.1f;
        bool enabled = false;
        yield return new WaitForSeconds(0.5f);
        speed = currentSpeed;

        while (timer<time && invencible)
        {
            model.SetActive(enabled);
            yield return null;
            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;
            if(blinkPeriod<lastBlink)
            {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled; 

            }
        }
        model.SetActive(true);
        invencible = false;
    }



    IEnumerator DoublePoints(float time)
    {
        doublescore = true;
        float timer = 0;
        float currentDouble = 1f;
        float lastDouble = 0;
        float doublePeriod = 0.1f;
        yield return new WaitForSeconds(0.5f);
        //speed = currentSpeed;

        while (timer < time && doublescore)
        {
            score += Time.deltaTime * speed*2;
            yield return null;
            timer += Time.deltaTime;
            lastDouble += Time.deltaTime;
            if (doublePeriod < lastDouble)
            {
                lastDouble = 0;
                currentDouble = 1f - currentDouble;
              
            }
        }
        doublescore = false;
    }


    void CallMenu()
    {
        GameManeger.gm.coins += coins;
        GameManeger.gm.EndRun();

    }

    public void IncreaseSpeed()
    {
        speed *= 1.15f;
        currentSpeed = speed;
        if(speed>= maxSpeed)
        {
            speed = maxSpeed;   
        }
    }
}
