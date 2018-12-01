using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;

public class KEMAPlayerController : MonoBehaviour
{
    public AudioClip win;
    public AudioClip lose;
    public float speed;             //Floating point variable to store the player's movement speed.
 
    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private int count;              //Integer to store the number of pickups collected so far.

    //This handles an internal timer
    private float timer;
    private int wholetime;
    private AudioSource source1;
    private AudioSource source2;

    public Text endText;
    public Text winText;



    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();

        //Initialize count to zero.
        count = 0;

        //Initialze winText to a blank string since we haven't won yet at beginning.
        winText.text = "";
        endText.text = "";

        //Call our SetCountText function which will update the text with the current value for count.
        SetCountText();



    }

    void Awake()
    {
        source1 = GetComponent<AudioSource>();
        source2 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.AddForce(movement * speed);

        //This does a timer before ending the game after 10 seconds.
        timer = timer + Time.deltaTime;
        if (timer >= 10)
        {
            endText.text = "GAME OVER";
            source1.PlayOneShot(lose, 1f);
            //StartCoroutine(ByeAfterDelay(2));
        }

    }

    //OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("PickUp"))
        {
            //other.gameObject.SetActive(false);

            //Add one to the current value of our count variable.
            count = count + 10;

            // add a point to the game
            GameLoader.AddScore(1);

            //Update the currently displayed count by calling the SetCountText function.
            SetCountText();
        }
    }

    //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
    void SetCountText()
    {
        
        //Check if we've collected all 12 pickups. If we have...
        if (count >= 10)
        {
            //... then set the text property of our winText object to "You win!"
            winText.text = "You win!";
            source2.PlayOneShot(win, 1f);
            StartCoroutine(ByeAfterDelay(2));
            
        }
    }

    IEnumerator ByeAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        GameLoader.gameOn = false;
    }
}
