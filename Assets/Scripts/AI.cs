using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;


public class AI : MonoBehaviour
{
    public static AI instance;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public SplineFollower splineFollower;
    public Transform brickParent;
    public GameObject ladderPieace, Bag, crown;
    public float startingspeed;
    public int bricksCount;
    public int jumpSpeed;
    Transform ladPos;

    [Header("Bool Statements")]
    public bool run;
    public bool onGround;
    public bool jumpUp;
    public bool levelwin;
    public bool bagIsEmpty = true,bagisnotEmpty;
    public Rigidbody rb;
    public GameObject ladderClone, LadderParent;
    public int numberofladders;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        splineFollower = this.GetComponent<SplineFollower>();
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        ladPos = transform;
        startingspeed = splineFollower.followSpeed;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !splineFollower.autoFollow)
        {
            splineFollower.autoFollow = true;
            run = true;
            anim.SetBool("Running", true);
        }
        if (!bagIsEmpty && jumpUp)
        {
            print("bagnotempty");
            
            //anim.SetBool("Running", false);
            //transform.position = new Vector3(ladPos.position.x, ladPos.position.y + 1, ladPos.position.z);
            AI_ladderSpawn();
        }

        else if (run && onGround)
        {
            anim.SetBool("Running", true);
        }
        if (bricksCount == 0)
        {
            bagIsEmpty = true;
        }
        else
        {
            bagIsEmpty = false;
            //ladPos = transform;
        }
        if (!jumpUp)
        {
            ladPos = transform;
        }
        if (splineFollower.result.percent > PlayerScript.instance.splineFollower.result.percent)
        {
            crown.SetActive(true);
        }
        else
            crown.SetActive(false);
        if (levelwin)
        {
            PlayerScript.instance.bagIsEmpty = true;
            PlayerScript.instance.splineFollower.enabled = false;
        }
    }

    public void AI_ladderSpawn()
    {
        if (ladderClone != null)
        {
            bricksCount = bricksCount - 1;
            brickParent.transform.GetChild(bricksCount).gameObject.SetActive(false);
            ladderClone = null;
        }

        if (!bagIsEmpty && jumpUp)
        {
            ladderClone = Instantiate(ladderPieace, ladPos.position, ladderPieace.transform.rotation);
            ladPos = ladderClone.transform.GetChild(0);
            Destroy(ladderClone.AddComponent<Rigidbody>(), 2);

            LadderParent = GameObject.Find("AILadderParent");
            ladderClone.transform.parent = LadderParent.transform;
            ladderClone.transform.rotation = Quaternion.Euler(ladderClone.transform.eulerAngles.x, transform.eulerAngles.y, ladderClone.transform.eulerAngles.z);
            numberofladders = LadderParent.transform.childCount;
        }
    }
    /*    
            Collisions         
    */
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }

        if (collision.gameObject.tag == "win")
        {
            anim.SetTrigger("D1");
            splineFollower.autoFollow = false;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bricks")
        {
            bricksCount++;
            other.gameObject.SetActive(false);
            bagisnotEmpty = true;
            brickParent.transform.GetChild(bricksCount - 1).gameObject.SetActive(true);
        }

        if (other.gameObject.tag == "Finish" && !PlayerScript.instance.levelwin)
        {
            Debug.Log("AI Finish first");
            levelwin = true;
            PlayerScript.instance.splineFollower.autoFollow = false;
            PlayerScript.instance.Bag.SetActive(false);

            StartCoroutine(PlayerScript.instance.LevelFail());
            PlayerScript.instance.anim.SetTrigger("Die");

            for (int i = 0; i < PlayerScript.instance.brickParent.transform.childCount; i++)
            {
                PlayerScript.instance.brickParent.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                PlayerScript.instance.brickParent.transform.GetChild(i).GetComponent<Collider>().isTrigger = false;
            }

        }

        if (other.gameObject.tag == "obstacle")
        {
            splineFollower.enabled = false;
            anim.SetBool("Running", false);
            Bag.SetActive(false);
            anim.SetTrigger("Die");
            for (int i = 0; i < brickParent.transform.childCount; i++)
            {
                brickParent.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                brickParent.transform.GetChild(i).GetComponent<Collider>().isTrigger = false;
            }
        }
        
        if(other.gameObject.tag== "throwback")
        {
            splineFollower.result.percent = splineFollower.result.percent-0.025f;
        }
        if (other.gameObject.tag == "powerup")
        {
            splineFollower.followSpeed += 5f;
            StartCoroutine(normalspeed());
        }
    }
    IEnumerator normalspeed()
    {
        yield return new WaitForSeconds(1);
        splineFollower.followSpeed = startingspeed;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "jumpai")
        {
            jumpUp = true;
            anim.SetBool("Running", false);
            transform.position = new Vector3(ladPos.position.x, ladPos.position.y + 1, ladPos.position.z);
            // ladderSpawn();
        }  
        

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "jumpai")
        {
             jumpUp = false;
            //anim.SetBool("Running", true);
            // transform.position = new Vector3(ladPos.position.x, ladPos.position.y + 1, ladPos.position.z);
            //ladderSpawn();
        }
    }
}
