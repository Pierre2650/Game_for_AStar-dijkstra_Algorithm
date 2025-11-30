using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //private PlayerHealth myHealth;
    private SpriteRenderer mySpr;
    //private Animator myAni;
    private Rigidbody2D myRB;
    

    [Header("Projectile Prefab")]
    //public GameObject atkPrefab;

    [SerializeField] private float attackColdown = 0.3f;
    private Coroutine canATK = null;
    private bool attacking = false;

    [Header("Mouvement")]
    [SerializeField] private float speed = 0f;

    //private  Vector2 horizontal = new Vector2(0.5f, -0.25f) , vertical = new Vector2(0.5f, 0.25f);
    private Vector2 horizontal = new Vector2(1f, 0f), vertical = new Vector2(0, 0.5f);
    private Vector2 velocity;
    private Vector2 input = Vector2.zero;
    private Vector2 startPos;

    [Header("Animations")]
    private bool playDeadAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        //myHealth = GetComponent<PlayerHealth>();
      
        mySpr = GetComponent<SpriteRenderer>();
        myRB = GetComponent<Rigidbody2D>();
        //myAni = GetComponent<Animator>();

        startPos = transform.position;
 


    }

    private void FixedUpdate()
    {

        myRB.AddForce(velocity.normalized * speed);
        
    }

    // Update is called once per frame
    void Update()
    {

        setSprite();

        if (attacking && canATK == null)
        {
            spawnProjectile();
            canATK = StartCoroutine(attackOnColdown());

        }


        velocity = horizontal * input.x + vertical * input.y;
        //transform.position += (Vector3)(velocity.normalized * speed * Time.deltaTime);

    }


    private void setSprite()
    {
        /*if (!myHealth.isDead) 
        { 

            playDeadAnimation = false; 
            if (!boostFX.activeSelf) {
                myAni.SetBool("IsDead", myHealth.isDead);
                mySpr.enabled = true;
                boostFX.SetActive(true); 
            } 
        }

        myAni.SetFloat("Direction", velocity.x);


        if (myHealth.isDead && !playDeadAnimation) {
            StartCoroutine(deadControl());
        }*/

    }

    private IEnumerator deadControl()
    {
        //boostFX.SetActive(false);
        //myAni.SetBool("IsDead", myHealth.isDead);
        //myAni.SetTrigger("Explosion");
        playDeadAnimation = true;

        yield return new WaitForSeconds(1f);

        mySpr.enabled = false;
        transform.position = startPos;
        
    }
    private void spawnProjectile()
    {
       // Instantiate(atkPrefab, transform.position, Quaternion.identity, transform.parent);
    }

    private void Mouvement(Vector2 dir)
    {
        //if (myHealth.isDead) { velocity = Vector2.zero; return; }
        //if (velocity == Vector2.zero ) { myRB.linearVelocity = Vector2.zero; return; 

       transform.Translate(dir * speed);

    }

 
   
    public void movementInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            input = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            input = Vector2.zero;
        }
    }



    private IEnumerator attackOnColdown()
    {
        yield return new WaitForSeconds(attackColdown);
        canATK = null;
    }

  
}
