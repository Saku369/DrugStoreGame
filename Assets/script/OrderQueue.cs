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
    void OnEnable()
    {
        Clear();
    }
}
