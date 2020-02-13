using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBehaviour : MonoBehaviour
{
    //private ParticleSystem ps;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips; //trash //yeet

    private void Start()
    {
        //ps = gameObject.GetComponent<ParticleSystem>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out PizzaScript pizza) || col.gameObject.CompareTag("topping"))
        {
            StartCoroutine(nameof(WaitKill), col.gameObject.transform.parent.gameObject);
        }
    }

    IEnumerator WaitKill( GameObject gameObjectToDestroy)
    {
        yield return new WaitForSeconds(.25f);
        //if (ps != null)
        {
            //ps.Play();
            audioSource.clip = audioClips[0];
            audioSource.loop = false;
            audioSource.Play();
        }

        //else
        {
            //audioSource.clip = audioClips[1];
            //audioSource.loop = false;
            //audioSource.Play();
        }
        GameObject ticket = gameObjectToDestroy.GetComponent<PizzaScript>().ticket;
        if (ticket != null)
        {
            ticket.GetComponent<LockToPoint1>().ChangeSnap(ticket.GetComponent<OrderTicketBehaviour>().startLockPos.transform);
        }
        Destroy(gameObjectToDestroy);
    }
}
