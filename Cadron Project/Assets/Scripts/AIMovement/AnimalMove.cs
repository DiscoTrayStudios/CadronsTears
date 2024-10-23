using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMove : MonoBehaviour
{

    // a class of animal that will wander between a list of 'moveSpots' and stop its movement once it collides with an object in the List<Transform> moveSpots 
    // when it does collid with on of these, it will send a speed value to the animator of 0, and then it will wait for 10 seconds before the animal will activate a "inSpot" boolean and send it to the animator
    // when the animal moves between spots it will also send its "speed" to the animator
    // THE ANIMAL WILL NOT ROTATE IN THE Z DIRECTION - IT IS ON A 2D PLANE, SO ROTATION OF Z MAKES THE ANIMAL APPEAR UPSIDE DOWN
    // the animal moves away when colliding with publlic GameObject "target", and deactivates the "inSpot" boolean, sending the animator a "speed" value agian.

    public GameObject target;
    public float speed;
    public List<Transform> moveSpots;          //list of spots to move to

    public bool inSpot = false;                //boolean to check if the animal is in a spot
    private bool player = false;
    public int spot = 0;       
    private bool moving = false;              // spot to move to
    private Rigidbody2D rb;
    private Animator anim;

    private SpriteRenderer rend;
    private int randomSpot;                     //number of patrol spots 


    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        /*moveSpots = GameManager.Instance.getSpots();
        if(moveSpots.Count == 0){
            moveSpots = GameManager.Instance.getSpots();
        }*/
        //Debug.Log("should have spots");
        randomSpot = Random.Range(0, moveSpots.Count);
        spot = randomSpot;
        StartCoroutine(MoveToSpot());
    }


    private void Update()
    {   
        
        //if the animal just collided with the player it will select another spot from moveSpots
        
        


        // animal selects a spot, and then increases its speed in that direction, without rotating Z
        
        /*else
        {
            // if the animal is in a spot, it will wait 10 seconds before it will activate the "inSpot" boolean
            StartCoroutine(Wait10());
            anim.SetFloat("speed", 0);
        }*/

    }

    IEnumerator MoveToSpot(){
        speed = 1;
        while(!inSpot){
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[spot].position, speed * Time.deltaTime);
            anim.SetFloat("speed", speed);
            if(moveSpots[spot].position.x < transform.position.x){
            rend.flipX = true;
            }
            else if(moveSpots[spot].position.x > transform.position.x){
            rend.flipX = false;
            }    
            yield return null;
        }
        
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("col" + gameObject);
        // if the animal collides with the player it will move away from the player
        Debug.Log(coll.gameObject);
        Debug.Log(moveSpots[spot].gameObject);
        // if the animal collides with a spot that is in the List<Transform> of spots
        // it will activate the "inSpot" boolean and send the animator a speed of 0
        if (coll.gameObject == moveSpots[spot].gameObject)
        {
            Debug.Log("isspot");
            speed = 0;
            anim.SetFloat("speed", speed);
            inSpot = true;
            anim.SetBool("inSpot", inSpot);
            // wait ten seconds before activating the "inSpot" boolean
            

            StartCoroutine(Wait10());
            
        }
        else if (coll.gameObject == target)
        //else
        {
            player = true;
            anim.SetBool("player", player);
            speed = 1;
            anim.SetFloat("speed", speed);
            inSpot = false;
            anim.SetBool("inSpot", false);
            Vector2 direction = transform.position - coll.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            transform.position = Vector2.MoveTowards(transform.position, coll.transform.position, speed * Time.deltaTime);
            spot += 1;
            if (spot >= moveSpots.Count)
            {
                spot = 0;
            }
            player = false;
            StopCoroutine(MoveToSpot());
            StartCoroutine(MoveToSpot());


        }
    }

    IEnumerator Wait10()
    {
        //yield on a new YieldInstruction that waits for 10 seconds.
        //after the waiting is done, starts moving to a new random spot.
        yield return new WaitForSeconds(5);

        
        while(randomSpot == spot){
            randomSpot = Random.Range(0, moveSpots.Count);
        }
        //Checks to make sure the new spot is not the same.
        spot = randomSpot;

        inSpot = false;
        anim.SetBool("inSpot", inSpot);
        StartCoroutine(MoveToSpot());


    }

}
