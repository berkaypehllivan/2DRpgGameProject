using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item object - " + itemData.itemName;
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    private static bool hasShownPopupThisTrigger = false; // T�m itemler i�in ortak olacak

    public void PickupItem()
    {
        if (Inventory.instance.CanAddItem() == false || itemData.itemType == ItemType.Equipment)
        {
            if (!hasShownPopupThisTrigger)
            {
                hasShownPopupThisTrigger = true;
                PlayerManager.instance.player.fx.CreatePopUpText("Envanter Dolu!");

                // Popup'� sadece belli bir s�re sonra tekrar g�stermeye izin veriyoruz
                StartCoroutine(ResetPopupFlag());
            }

            rb.velocity = new Vector2(0, 7);
            return;
        }

        AudioManager.instance.PlaySFX(9, transform);
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }

    private IEnumerator ResetPopupFlag()
    {
        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekleyerek tekrar a��lmas�n� sa�l�yoruz
        hasShownPopupThisTrigger = false;
    }

}
