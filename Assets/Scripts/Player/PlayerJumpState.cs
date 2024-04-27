using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Space tu�u b�rak�ld���nda ve hala y�kseliyorsa z�plama kuvvetini azalt
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }

        // E�er xInput s�f�r de�ilse, yatay hareketi g�ncelle
        if (xInput != 0)
        {
            // Yatay hareket
            rb.velocity = new Vector2(xInput * player.moveSpeed, rb.velocity.y);

            // Karakterin y�n�n� g�ncellemek i�in kontrol
            if (xInput > 0 && player.facingDir != 1)
            {
                player.Flip(); // Sa�a bak�yorsa ve sol tarafa gitmek istiyorsa
            }
            else if (xInput < 0 && player.facingDir == 1)
            {
                player.Flip(); // Sol tarafa bak�yorsa ve sa�a gitmek istiyorsa
            }
        }

        // D����e ge�ti�inde, hava durumuna ge�i� yap
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }

}

