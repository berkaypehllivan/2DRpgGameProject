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

        // Space tuþu býrakýldýðýnda ve hala yükseliyorsa zýplama kuvvetini azalt
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }

        // Eðer xInput sýfýr deðilse, yatay hareketi güncelle
        if (xInput != 0)
        {
            // Yatay hareket
            rb.velocity = new Vector2(xInput * player.moveSpeed, rb.velocity.y);

            // Karakterin yönünü güncellemek için kontrol
            if (xInput > 0 && player.facingDir != 1)
            {
                player.Flip(); // Saða bakýyorsa ve sol tarafa gitmek istiyorsa
            }
            else if (xInput < 0 && player.facingDir == 1)
            {
                player.Flip(); // Sol tarafa bakýyorsa ve saða gitmek istiyorsa
            }
        }

        // Düþüþe geçtiðinde, hava durumuna geçiþ yap
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }

}

