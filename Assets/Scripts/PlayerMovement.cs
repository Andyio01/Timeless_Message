using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //You can adjust the speed in the Unity editor
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool faceRight = true;
    public Sprite forward;
    public Sprite back;
    public Sprite right;

    public static bool moveable = true;
    public AudioClip walkSound;//sound effect
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();//get sound
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveable)
        {
            return;
        }
        
        Vector3 hvMove = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        // transform.position = transform.position + hvMove * moveSpeed * Time.deltaTime;
        
        // Stop
        if (hvMove.x == 0 && hvMove.y == 0){
            GetComponent<Animator>().SetInteger("direction", 3);
        }
        
        // Change walk status
        if (hvMove.y < 0){
            GetComponent<Animator>().SetInteger("direction", 0);
            gameObject.GetComponent<SpriteRenderer>().sprite = forward;
        } 
        if (hvMove.y > 0){
            GetComponent<Animator>().SetInteger("direction", 1);
            gameObject.GetComponent<SpriteRenderer>().sprite = back;
        } 
        if (hvMove.x < 0 || hvMove.x > 0){
            GetComponent<Animator>().SetInteger("direction", 2);
            gameObject.GetComponent<SpriteRenderer>().sprite = right;
            // Turn left or right
            if ((hvMove.x <0 && faceRight) || (hvMove.x >0 && !faceRight)){
            // GetComponent<Animator>().SetInteger("direction", 2);
            playerTurn();
            
            }
        }
        // play sound effect
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkSound;
                audioSource.Play();
            }
        }
        else
        {
            
            audioSource.Stop(); // Stop the sound if the player is not moving
        }
    }

    void FixedUpdate()
    {
        //movement
        // rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if (!moveable)
        {
            return;
        }
        Vector2 position = rb.position;
        position.x += Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        position.y += Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }

    private void playerTurn()
    {
        // NOTE: Switch player facing label
        faceRight = !faceRight;
        // NOTE: Multiply player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
