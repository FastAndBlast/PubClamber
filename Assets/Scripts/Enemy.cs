using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    public float detectionRadius = 5;

    public float speed = 5;

    bool active = false;
    bool setActive
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
            anim.SetBool("active", value);
        }
    }

    Rigidbody rb;

    Animator anim;

    Vector3 originalPosition;
    Vector3 originalRotation;

    public static List<Enemy> enemyList;

    public GestureType gestureRequired;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;

        rb = GetComponent<Rigidbody>();

        anim = transform.GetChild(0).GetComponent<Animator>();

        enemyList.Add(this);

        //GetComponent<Animator>().speed;
    }



    private void Update()
    {
        if (GameManager.instance && GameManager.instance.player && GameManager.instance.player.GetComponent<WalkingManager>().on && !GameManager.paused)
        {
            GameObject target = GameManager.instance.player;

            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                BodyFunctions.instance.Die("Got smacked", "Tip: Wave to hobos, flip off nutjobs");
                return;
            }

            if (!active && Vector3.Distance(transform.position, target.transform.position) < detectionRadius)
            {
                setActive = true;
            }

            if (active && !GameManager.paused)
            {
                Vector3 newPos = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z), speed * Time.deltaTime);

                newPos.z = newPos.y;
                newPos.y = 0;

                rb.MovePosition(newPos);

                transform.forward = (target.transform.position - transform.position).normalized;

                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            }

            anim.speed = 1;

            GestureCheck();
        }
        else if (GameManager.paused)
        {
            anim.speed = 0;
        }
        else if (GameManager.instance && GameManager.instance.player && !GameManager.instance.player.GetComponent<WalkingManager>().on)
        {
            setActive = false;
        }
    }

    public void Restart()
    {
        transform.position = originalPosition;
        transform.eulerAngles = originalRotation;
        setActive = false;
        gameObject.SetActive(true);
    }

    public void GestureCheck()
    {
        if (BodyFunctions.instance)
        {
            if (BodyFunctions.instance.gestureTime > 0)
            {
                if (BodyFunctions.instance.gesture == gestureRequired)
                {
                    Die();
                }
            }
        }
    }

    public void Die()
    {
        //enemyList.Remove(this);
        //Destroy(gameObject);

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
