using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ISaveManager
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

    public void LoadData(GameData _data)
    {
        if (!SaveManager.instance.HasSavedData())
        {
            _data.keyPositionX = transform.position.x;
            _data.keyPositionY = transform.position.y;
        }
        else
            transform.position = new Vector3(_data.keyPositionX, _data.keyPositionY, transform.position.z);
    }

    public void SaveData(GameData _data)
    {
        _data.keyPositionX = transform.position.x;
        _data.keyPositionY = transform.position.y;
    }
}
