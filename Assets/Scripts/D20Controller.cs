using UnityEngine;

public class D20Controller : MonoBehaviour
{
    private Vector3 startPos;

    private DiceController diceController;
    public float downForce = 4;
    public float forwardForce = 30;
    public int maxLeft = -5;
    public int maxRight = 5;
    private float height = -100f;
    public int rolledNumber = 0;

    private GameObject numberRolled;

    private Rigidbody rb;

    public AudioSource hitSound;

    public bool settled;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        diceController = GameObject.FindGameObjectWithTag("diceController").GetComponent<DiceController>();
        rb.maxAngularVelocity = 1000f;
        startPos = transform.position;
        settled = false;
    }
    void Update()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (diceController.isRolled)
        {
            if (rb.velocity == Vector3.zero)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).transform.position.y > height)
                    {
                        height = transform.GetChild(i).transform.position.y;
                        numberRolled = transform.GetChild(i).gameObject;
                    }
                }
                rolledNumber = int.Parse(numberRolled.name);
                settled = true;
            }
        }
    }

    public void throwDice()
    {
        if (diceController.isRolled == false)
        {
            settled = false;
            height = -100f;
            int rand = Random.Range(maxLeft, maxRight);
            int randRot = 10 * (0 - rand);
            rb.isKinematic = false;
            rb.velocity = new Vector3(rand, downForce, forwardForce);
            rb.AddTorque(Random.Range(-500f, 500f), Random.Range(-500f, 500f), Random.Range(-500f, 500f), ForceMode.Force);
        }
        else
        {
            rb.isKinematic = true;
            transform.position = startPos;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 12)
        {
            Instantiate(hitSound);
        }
    }
}
