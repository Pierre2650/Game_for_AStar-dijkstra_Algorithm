using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //private PlayerHealth myHealth;
    private SpriteRenderer mySpr;
    private Animator myAni;
    private Rigidbody2D myRB;


    [Header("Bomb")]
    public int nbBombs;
    private int currentNbBombs;
    public GameObject bombprefab;
    private int firePower = 1;
    public int fireLayer = 0;

    [SerializeField] private float attackColdown = 0.3f;
    private Coroutine canATK = null;
    private bool attacking = false;

    [Header("Life")]
    public int LifePoints = 3;
    private bool dead = false;
    private float hurtColdown = 0.3f;

    [Header("Mouvement")]
    public bool restrained = false;
    [SerializeField] private float speed = 0f;

    private Vector2 horizontal = new Vector2(1f, 0f), vertical = new Vector2(0, 0.5f);
    private Vector2 velocity;
    private Vector2 input = Vector2.zero;
    private Vector2 startPos;


    [Header("Animations")]
    private bool playDeadAnimation;

    [Header("UI")]
    public GameObject UIManager;
    private Coroutine hurtColdownC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        //myHealth = GetComponent<PlayerHealth>();
      
        mySpr = GetComponent<SpriteRenderer>();
        myRB = GetComponent<Rigidbody2D>();
        myAni = GetComponent<Animator>();

        startPos = transform.position;
        currentNbBombs = nbBombs;


    }

    private void FixedUpdate()
    {
        if (restrained) {  return; }
        myRB.AddForce(velocity.normalized * speed);
        
    }

    // Update is called once per frame
    void Update()
    {

        setSprite();

        if(!dead )
        { velocity = horizontal * input.x + vertical * input.y; }

        if (LifePoints <= 0)
        {
            dead = true;
            myRB.linearVelocity = Vector3.zero;
            velocity = Vector3.zero;
        }

    }


    private void setSprite()
    {
        if (restrained) { velocity = Vector2.right; }

        if(velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        if(velocity.y < 0 && velocity.x == 0f)
        {
            myAni.SetFloat("VelocityX", Mathf.Abs(velocity.y));
        }
        else
        {

            myAni.SetFloat("VelocityY", velocity.y);
            myAni.SetFloat("VelocityX", Mathf.Abs(velocity.x));

        }

        


        if (dead && !playDeadAnimation ) 
        {
            myAni.SetTrigger("Dead");
            UIManager.GetComponent<GameOveUIController>().showUI();
            playDeadAnimation = true; 
           
        }

        

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

    public void spawnBomb(InputAction.CallbackContext context)
    {
        if (context.performed && currentNbBombs > 0)
        {
            GameObject temp = Instantiate(bombprefab,transform.position, Quaternion.identity);
            temp.GetComponent<Bomb_Controller>().flamesLength = firePower;

            currentNbBombs--;
            StartCoroutine(attackOnColdown());
        }
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "fireBonus":
                if (firePower < 11)
                {
                    firePower++;
                }

                Destroy(collision.gameObject);

                break;
            case "nbBombBonus":
                if (nbBombs < 5)
                {
                    nbBombs++;
                }

                currentNbBombs = nbBombs;
                UIManager.GetComponent<BombsUiController>().updateNbBombs(currentNbBombs.ToString());
                Destroy(collision.gameObject);

                break;
            case "speedBonus":
                if (speed < 140)
                {
                    speed+=10;
                }

                Destroy(collision.gameObject);
                break;

            case "extraLife":
                if (LifePoints < 3)
                {
                    LifePoints++;
                    UIManager.GetComponent<LifesUIController>().lifeGained();
                }

                Destroy(collision.gameObject);
                break;
        }
        

        if (collision.gameObject.layer == fireLayer && hurtColdownC == null)
        {
            Destroy(collision.gameObject);
            LifePoints--;
            hurtColdownC = StartCoroutine(hurtOnColdown());
            UIManager.GetComponent<LifesUIController>().lifeLost();
           
        }


    }


    private IEnumerator attackOnColdown()
    {
        UIManager.GetComponent<BombsUiController>().updateNbBombs(currentNbBombs.ToString());
        yield return new WaitForSeconds(attackColdown);
        currentNbBombs++;
        UIManager.GetComponent<BombsUiController>().updateNbBombs(currentNbBombs.ToString());
    }



    private IEnumerator hurtOnColdown()
    {
        
        yield return new WaitForSeconds(hurtColdown);

        hurtColdownC = null;
    }


}
