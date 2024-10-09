using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWallTrigger : MonoBehaviour
{
    [SerializeField] private Animator movingWallAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            movingWallAnimator.SetTrigger("MoveTrigger");
        }
    }
}
