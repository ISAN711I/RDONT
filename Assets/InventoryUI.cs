using UnityEngine;
using TMPro;

public class PlayerInventoryUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject inventoryPanel;
    public TMP_Text coinText;

    [Header("Input Settings")]
    public KeyCode toggleKey = KeyCode.I;

    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false); // Start hidden
    }

    void Update()
    {
        // Toggle inventory panel visibility
        if (Input.GetKeyDown(toggleKey) && inventoryPanel != null)
        {
            bool isActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isActive);
        }

        // Update coin count while panel is visible
        if (inventoryPanel != null && inventoryPanel.activeSelf && playerInventory != null && coinText != null)
        {
            coinText.text = "COINS:" + playerInventory.coins;
        }
    }
}
