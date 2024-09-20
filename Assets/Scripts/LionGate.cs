using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LionGate : MonoBehaviour
{
    private Player player;

    public Animator anim;
    public bool doorOpen, waitingToOpen;
    [SerializeField] private string popUpText = string.Empty;

    private void Start()
    {
        player = PlayerManager.instance.player;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (waitingToOpen)
        {
            if (Vector3.Distance(player.followingKey.transform.position, transform.position) < 0.1f)
                StartCoroutine(OpenGateWithDelay());
        }
    }

    private IEnumerator OpenGateWithDelay()
    {
        waitingToOpen = false;
        doorOpen = true;
        player.followingKey.anim.speed = 1;
        yield return new WaitForSeconds(1);
        anim.SetTrigger("OpenGate");
        player.followingKey.gameObject.SetActive(false);
        player.followingKey = null;
        player.fx.CreatePopUpText(popUpText);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (player.followingKey != null)
            {
                player.followingKey.target = transform;
                waitingToOpen = true;
            }
        }
    }
}
