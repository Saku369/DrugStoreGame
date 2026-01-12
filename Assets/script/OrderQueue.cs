using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DrugStore/OrderQueue")]
public class OrderQueue : ScriptableObject
{
    public List<Recipe> activeOrders = new List<Recipe>();

    public void Clear()
    {
        activeOrders.Clear();
    }

    public void CompleteOrder(Recipe recipe)
    {
        if (activeOrders.Contains(recipe))
        {
            activeOrders.Remove(recipe);
        }
    }

    void OnEnable()
    {
        Clear();
    }
}