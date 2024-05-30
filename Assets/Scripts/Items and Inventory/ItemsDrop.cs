using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChange)
                dropList.Add(possibleDrop[i]);
        }

        if (dropList.Count == 0)
            return;

        int itemCount = Mathf.Min(possibleItemDrop, dropList.Count);
        for (int i = 0; i < itemCount; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }



    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
