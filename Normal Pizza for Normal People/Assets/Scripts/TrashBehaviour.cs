using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBehaviour : MonoBehaviour
{
    private ParticleSystem ps;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips; //trash //yeet

    private void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("pizza") || col.gameObject.CompareTag("topping"))
        {
            StartCoroutine("WaitKill", col.gameObject.transform.parent.gameObject);
        }
    }

    IEnumerator WaitKill( GameObject g)
    {
        yield return new WaitForSeconds(.25f);
        if (ps != null)
        {
            ps.Play();
            audioSource.clip = audioClips[0];
            audioSource.loop = false;
            audioSource.Play();
        }

        else
        {
            audioSource.clip = audioClips[1];
            audioSource.loop = false;
            audioSource.Play();
        }
        GameObject ticket = g.GetComponent<PizzaScript>().ticket;
        if (ticket != null)
        {
            ticket.GetComponent<LockToPoint1>().ChangeSnap(ticket.GetComponent<OrderTicketBehaviour>().startLockPos.transform);
        }
        Destroy(g);
    }
}
