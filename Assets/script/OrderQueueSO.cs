using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DrugStore/OrderQueue")]
public class OrderQueueSO : ScriptableObject
{
    public List<Order> activeOrders = new();

    private void OnEnable()
    {
        // 再生停止後に残らないように
        activeOrders.Clear();
    }
}
