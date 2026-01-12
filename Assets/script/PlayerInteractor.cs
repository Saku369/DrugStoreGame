using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    private Shelf currentShelf;
    private Workbench currentWorkbench;
    private PlayerInventory inventory;
    public OrderQueue orderQueue; // ★assetを入れる（UIと同じ）

    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Shelf shelf = other.GetComponent<Shelf>();
        if (shelf != null)
        {
            currentShelf = shelf;
            return;
        }

        Workbench bench = other.GetComponent<Workbench>();
        if (bench != null)
        {
            currentWorkbench = bench;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Shelf>() == currentShelf)
            currentShelf = null;

        if (other.GetComponent<Workbench>() == currentWorkbench)
            currentWorkbench = null;
    }

    private void OnInteract()
    {
        if (currentShelf != null)
        {
            var ing = currentShelf.Take();
            inventory.AddIngredient(ing);
            return;
        }

        if (currentWorkbench != null)
        {
            if (orderQueue.activeOrders.Count == 0) return;

            var recipe = orderQueue.activeOrders[0];
            inventory.currentRecipe = recipe; // 同期
            currentWorkbench.Use(inventory, recipe);
        }
    }
}
