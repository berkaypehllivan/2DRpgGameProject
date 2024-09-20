using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character_Stats>() != null)
            collision.gameObject.GetComponent<Character_Stats>().KillEntity();
        else
            Destroy(collision.gameObject);
    }
}
