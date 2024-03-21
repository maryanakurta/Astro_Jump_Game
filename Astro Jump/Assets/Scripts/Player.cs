using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    bool isGrounded;
    int curHp;
    int maxHp = 3;
    public Main main;
    public bool key = false;
    bool canTP = true;
    int coins = 0;
    public GameObject blueGem;
    int gemCount = 0;
    Animator anim;
    public InterAd interAd;
    private int tryCount;
    public UltimateJoystick joystick;
    public float minJumpHeight;
    public float maxJumpHeight;
    //public Soundeffector soundeffector;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curHp = maxHp;
        anim = GetComponent<Animator>();
        tryCount = PlayerPrefs.GetInt("tryCount");
    }

    void Update()
    {
        CheckGround();

        if (joystick.GetHorizontalAxis() == 0 && (isGrounded))
        {
            anim.SetInteger("State", 1);
        }
        else{
            Flip();
            if (isGrounded)
            anim.SetInteger("State", 2);
        }
        
    }

    public void Jump()
    {
        if (isGrounded) 
        {
        rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = joystick.GetHorizontalAxis();
    
    rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    
    }

    void Flip()
    {
        if (joystick.GetHorizontalAxis() > 0)
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    else if (joystick.GetHorizontalAxis() < 0)
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded)
        anim.SetInteger("State", 3);
    }

    public void RecountHp(int deltaHp)
    {
        curHp = curHp + deltaHp;
        if (curHp > maxHp)
        {
            curHp = maxHp;
        }
        if(curHp <= 0)
        {
            tryCount++;
            PlayerPrefs.SetInt("tryCount", tryCount);
            if (tryCount % 3 == 0)
            interAd.ShowAd();
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1f);
        }
    }


    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            key = true;
        }

        if(collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {
            collision.gameObject.GetComponent<Door>().Teleport(gameObject);
            canTP = false;
            StartCoroutine(TPwait());
            }
            else if(key)
            collision.gameObject.GetComponent<Door>().Unlock();
        }

        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coins++;
            //soundeffector.PlayCoinSound();
        }

        if (collision.gameObject.tag == "Hearh")
        {
            Destroy(collision.gameObject);
            RecountHp(1);
        }

        if (collision.gameObject.tag == "Mushroom")
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
        }

        if (collision.gameObject.tag == "BlueGem")
        {
            Destroy(collision.gameObject);
            StartCoroutine(SpeedBonus());
        }
    }

    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ladder")
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
        }

        if(collision.gameObject.tag == "Icy")
        {
            if(rb.gravityScale == 1f)
            {
            rb.gravityScale = 7f;
            speed *= 0.25f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if(collision.gameObject.tag == "Icy")
        {
            if(rb.gravityScale == 7f)
            {
            rb.gravityScale = 1f;
            speed *= 4f;
            }
        }
    }

    IEnumerator SpeedBonus()
    {
        gemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);

        speed = speed * 2;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(8f);
        StartCoroutine(Invis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;

        gemCount--;
        blueGem.SetActive(false);
        // CheckGems(blueGem);
    }

    void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
        obj.transform.localPosition = new Vector3(0.2f, 2f, obj.transform.localPosition.z);
        else if (gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-1f, 2f, blueGem.transform.localPosition.z);
            //greenGem.transform.localPosition = new Vector3(1f, 2f, greenGem.transform.localPosition.z);s
        }
    }

    IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
        StartCoroutine(Invis(spr, time));
    }
    
    public int GetCoins()
    {
        return coins;
    }

    public int GetHp()
    {
        return curHp;
    }
}
