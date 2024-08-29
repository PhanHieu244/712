using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;


public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public SplineFollower splineFollower;
    public Transform brickParent;
    public GameObject ladderPieace,Bag,crown;
    public float startingspeed;
    public int bricksCount;
    public int jumpSpeed;
     Transform ladPos;

    [Header("Bool Statements")]
    public bool run;
    public bool once=true;
    public bool onGround;
    public bool bagIsEmpty=true, bagisnotEmpty;
    public bool levelwin;
    public bool pause;
    public Rigidbody rb;

    public GameObject ladderClone, LadderParent;
    public int numberofladders;

    public ParticleSystem winparticle;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        splineFollower = this.GetComponent<SplineFollower>();
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        ladPos = null;
        startingspeed = splineFollower.followSpeed;
    }
    bool isUP;
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !splineFollower.autoFollow)
        {
            splineFollower.autoFollow = true;
            run = true;
            anim.SetBool("Running", true);
            if (!isUP)
            {
                ladPos = null;
                isUP = true;
            }
        }
        else if(Input.GetMouseButton(0) && !bagIsEmpty )
        {
            anim.SetBool("Running", false);
            
           if(ladPos!=null)
            {
                if (ladPos.position.y > 0.1f)
                    transform.position = new Vector3(ladPos.position.x, ladPos.position.y + 1, ladPos.position.z);
            }
            ladderSpawn();
            Time.timeScale = 0.8f;
        }
        else
        {
            ladPos = null;
            Time.timeScale = 1f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isUP = false;
            ladPos = null;
            
        }

        else if(run && onGround)
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
        }
        if (splineFollower.result.percent > AI.instance.splineFollower.result.percent)
        {
            crown.SetActive(true);
        }
        else
            crown.SetActive(false);
    } 

    public void ladderSpawn()
    {
        if (ladderClone != null)
        {
            bricksCount=bricksCount-1;
            brickParent.transform.GetChild(bricksCount).gameObject.SetActive(false);
        }

        if (!bagIsEmpty)
        {
           
            if (ladPos != null ) 
                ladPos = ladderClone.transform.GetChild(0);
            else
            {
                ladPos = transform;
            }

            ladderClone = Instantiate(ladderPieace, ladPos.position, ladderPieace.transform.rotation);
            if (AudioManager.instance)
                AudioManager.instance.PlayRotationSound();
            Destroy(ladderClone.AddComponent<Rigidbody>(),2);
            LadderParent = GameObject.Find("LadderParent");
            ladderClone.transform.parent = LadderParent.transform;
            ladderClone.transform.rotation =Quaternion.Euler(ladderClone.transform.eulerAngles.x, transform.eulerAngles.y, ladderClone.transform.eulerAngles.z);
            numberofladders = LadderParent.transform.childCount;
        }
        else
        {
            ladPos = null;
        }

        if (levelwin)
        {
            AI.instance.bagIsEmpty = true;
            AI.instance.splineFollower.enabled = false;
        }
    }


    public IEnumerator LevelComplete()
    {
        if (AudioManager.instance)
            AudioManager.instance.PlayLevelComplete();
        yield return new WaitForSeconds(6f);
        UiManager.instance.winPanel.SetActive(true);
        
    }
    public IEnumerator LevelFail()
    {
        yield return new WaitForSeconds(3f);
        UiManager.instance.failpanel.SetActive(true);
        if (AudioManager.instance)
            AudioManager.instance.PlayLevelFail();
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
            winparticle.gameObject.SetActive(true);
            winparticle.Play();
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ladder")
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bricks")
        {
            bricksCount++;
            bagisnotEmpty = true;
            other.gameObject.SetActive(false);
            brickParent.transform.GetChild(bricksCount - 1).gameObject.SetActive(true);
            if (AudioManager.instance)
                AudioManager.instance.PlayRotationSound();
        }

        if (other.gameObject.tag == "Finish" && !AI.instance.levelwin)
        {
            Debug.Log("player Finish first");
            levelwin = true;
            StartCoroutine(LevelComplete());
            AI.instance.splineFollower.autoFollow = false;
            AI.instance.Bag.SetActive(false);
            AI.instance.anim.SetTrigger("Die");
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            for (int i = 0; i < AI.instance.brickParent.transform.childCount; i++)
            {
                AI.instance.brickParent.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                AI.instance.brickParent.transform.GetChild(i).GetComponent<Collider>().isTrigger = false;
            }
        }

        if (other.gameObject.tag == "obstacle")
        {
            splineFollower.autoFollow = false;
            Bag.SetActive(false);
            anim.SetTrigger("Die");
            StartCoroutine(LevelFail());
            for (int i = 0; i <brickParent.transform.childCount; i++)
            {
                brickParent.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                brickParent.transform.GetChild(i).GetComponent<Collider>().isTrigger = false;
            }
        }
        if (other.gameObject.tag == "throwback")
        {
            splineFollower.result.percent = splineFollower.result.percent - 0.025f;
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
}