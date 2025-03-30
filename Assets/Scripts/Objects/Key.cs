using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private bool isFollowing;
    [SerializeField] private float followSpeed;

    [HideInInspector] public Transform target;
    [HideInInspector] public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
    }

    private void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!isFollowing)
            {
                Player player = PlayerManager.instance.player;
                target = player.keyFollowPoint;
                isFollowing = true;
                player.followingKey = this;
            }
        }
    }
}
