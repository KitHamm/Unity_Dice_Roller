using System.Collections;
using UnityEngine;

public class audioDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillAudio());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator KillAudio()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
