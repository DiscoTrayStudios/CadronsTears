using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAnimalMove : MonoBehaviour
{
    private float maxDistance = 2.0f;
    private float speed = 1;
    private bool moving;
    private Vector2 directionMoving;
    private Vector2 moveTo;
    private Rigidbody2D rb;
    private Animator anim;

    private SpriteRenderer rend;
    

    void Start(){
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        StartMoving();
    }

    void Update(){
        if(moving){
            anim.SetBool("inSpot", false);
        }
        else{
            anim.SetBool("inSpot", true);
        }
    }
    IEnumerator GetRandomDirection(){
        //Vector2 point = Random.insideUnitCircle * 5.0;
        Vector2 direction = Random.insideUnitCircle;
        //Debug.DrawRay(transform.position, direction, Color.white);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1);
        //if there is an obstacle in the direction, gets a new one
        while(hit){
            //Debug.DrawRay(transform.position, direction, Color.white);
            direction = Random.insideUnitCircle;
            hit = Physics2D.Raycast(transform.position, direction, 1);
            yield return null;
        }
        
            directionMoving = direction;
            
            StartCoroutine(MoveInDirection());
            //moveTo = point;
        
    }
    private void StartMoving(){
    
        StartCoroutine(GetRandomDirection());
        //Debug.DrawRay(transform.position, directionMoving, Color.white);
        //StartCoroutine(MoveInDirection());

    }
   /* IEnumerator Wander(){
        WaitForSeconds waitTime = new WaitForSeconds(5);
        while(true){
            GetRandomDirection();
            MoveInDirection();
            yield return waitTime;
        }
    }*/
    IEnumerator MoveInDirection(){
        RaycastHit2D hit;
        moving = true;
        while(moving){
            hit = Physics2D.Raycast(transform.position, directionMoving, 1);
            if(hit){
                //Debug.DrawRay(transform.position, directionMoving, Color.white);
                moving = false;
                
                int wait = Random.Range(0, 15);
                Debug.Log(wait);
                if(wait > 5){
                    //if wait is longer than five seconds, the animal will stop for a third of the time, then take a nap.
                    yield return new WaitForSeconds(wait/3);
                    anim.SetBool("nap", true);
                    yield return new WaitForSeconds((wait/3) * 2);
                    anim.SetBool("nap", false);
                    yield return new WaitForSeconds(1);
                }
                else{
                    yield return new WaitForSeconds(wait);
                }
                
            }

            Vector3 newPos = new Vector3(transform.position.x + directionMoving.x, transform.position.y + directionMoving.y, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, newPos, speed * Time.deltaTime);

            if(newPos.x < transform.position.x){
            rend.flipX = true;
            }
            else if(newPos.x > transform.position.x){
            rend.flipX = false;
            }
            
            //Debug.Log(directionMoving);
            
            yield return null;
        }
        StartMoving();
    }

    void OnCollisionEnter2D(Collision2D coll){
        //StopAllCoroutines();
        moving = false;
        GetRandomDirection();
    }
  





}