using UnityEngine;

public class PlayerItemCollection : MonoBehaviour
{
    private InventoryController inventoryController;
    void Awake()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D context)
    {
        if (context.CompareTag("Item"))
        {
            ItemScript itemScript = FindAnyObjectByType<ItemScript>();
            if (itemScript != null)
            {
                bool itemAdded = inventoryController.AddItem(context.gameObject);

                if (itemAdded)
                {
                    Destroy(context.gameObject);
                }
            }

        }
    }
}
