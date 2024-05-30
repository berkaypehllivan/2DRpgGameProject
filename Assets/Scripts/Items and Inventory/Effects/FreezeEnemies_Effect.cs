using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/Item Effect/Freeze Enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;


    public override void ExecuteEffect(Transform _transform)
    {
        Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
            return;

        if (!Inventory.instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 4);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
