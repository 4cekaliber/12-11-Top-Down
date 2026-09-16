using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    [SerializeField] private GameObject spacePrefab;
    [SerializeField] private int slotCount;
    [SerializeField] private GameObject[] itemPrefabs;

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform spaceTransform in inventoryPanel.transform)
        {
            SpaceScript space = spaceTransform.GetComponent<SpaceScript>();
            if (space != null && space.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, spaceTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                space.currentItem = newItem;
                return true;
            }
        }
        print("Iventory Full!");
        return false;
    }
    void Awake()
    {
        for (int i = 0; i <slotCount; i++)
        {
            SpaceScript space = Instantiate(spacePrefab, inventoryPanel.transform).GetComponent<SpaceScript>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], space.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                space.currentItem = item;
            }
        }
    }

    
}
