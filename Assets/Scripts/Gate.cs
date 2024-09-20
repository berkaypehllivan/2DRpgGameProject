using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private UI ui;
    [SerializeField] private LionGate lionGate;
    private bool canLeaveLevel;

    private void Update()
    {
        if (canLeaveLevel)
            if (Input.GetKeyDown(KeyCode.E))
                StartCoroutine(ui.LoadSceneWithFadeEffect(1.5f, scene));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (lionGate != null && lionGate.doorOpen)
                canLeaveLevel = true;
        }
    }
}
