using TMPro;
using UnityEngine;

public class NumberIdentifier : MonoBehaviour
{
    private Transform linkedDie;
    private Vector3 position;
    private TMP_Text text;
    private SingleDieController dieController;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        linkedDie = transform.parent.GetChild(0);
        dieController = linkedDie.GetComponent<SingleDieController>();
        rb = linkedDie.GetComponent<Rigidbody>();
        text = GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (dieController.rolledNumber != 0)
        {
            if (dieController.rolledNumber == -1)
            {
                text.text = "!";
            }
            else
            {
                text.text = dieController.rolledNumber.ToString();
            }
        }
        else
        {
            text.text = "";
        }

        position = linkedDie.position;
        position.y = linkedDie.position.y + 3f;
        transform.position = position;
    }
}
