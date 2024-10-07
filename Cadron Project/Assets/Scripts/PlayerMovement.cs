using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    public static float horizontal;
    public static float vertical;
    public static float h_path;
    public static float v_path;
    
    public bool idle;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private AudioSource walking;
    
    private float moveLimiter = 0.7f;

    public float runSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        idle = false;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // walking = GetComponent<AudioSource>();
    }
    
    /**void Awake(){
        if(GameManager.Instance.player == null){
            GameManager.Instance.player = gameObject;
        }
    }**/


    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsPaused() == false && !GameManager.Instance.isBusy()) {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            if (h_path != 0) { horizontal = h_path; }
            if (v_path != 0) { vertical   = v_path; }
            animator.SetFloat("horizontal", horizontal);
            animator.SetFloat("vertical", vertical);
            if (horizontal < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if(horizontal > 0)
            {
                spriteRenderer.flipX = false;
            }
            // if(horizontal != 0 || vertical != 0){
            //     if(!walking.isPlaying){
            //         walking.Play();
            //     } 
            // }
            // else {
                
            //     walking.Stop();
            // }
        horizontal -= h_path;
        vertical -= v_path;
        }
        else { horizontal = 0f; vertical = 0f; 
        animator.SetFloat("horizontal", 0.0f);
        animator.SetFloat("vertical", 0.0f);
        body.constraints = RigidbodyConstraints2D.FreezeAll;
         }
    }
    public void SetIdle(bool b) { idle = b;
    animator.SetFloat("horizontal", 0.0f);
    SetHorizontal(0.0f);}


    public static void SetHorizontal(float h) { h_path = h; }
    public static void SetVertical(float v) { v_path = v; }

    void FixedUpdate() {
        if(!GameManager.Instance.isBusy()){
            if ( (Mathf.Abs(horizontal) > 0.05f || Mathf.Abs(vertical) > 0.05f)) {
            animator.SetFloat("horizontal", Mathf.Round(5*horizontal)/5);
            animator.SetFloat("vertical", Mathf.Round(5*vertical)/5);
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
            }
        }
        
        
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
    
}
