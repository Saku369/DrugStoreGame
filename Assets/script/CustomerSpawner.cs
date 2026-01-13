using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Customer customerPrefab;
    public Transform spawnPoint;

    public float spawnInterval = 30f;

    public OrderFactory factory;
    public OrderQueueSO orderQueue;

    private void Start()
    {
        SpawnOne(); // 最初に1人（不要なら消してOK）
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnOne();
        }
    }

    private void SpawnOne()
    {
        if (customerPrefab == null || factory == null || orderQueue == null) return;

        // 注文生成
        Order order = factory.CreateRandomOrder();

        // 客生成
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        Customer c = Instantiate(customerPrefab, pos, rot);
        c.Init(order);

        // 注文追加（UIが参照する）
        orderQueue.activeOrders.Add(order);

        Debug.Log($"客生成：{order.orderName} / 注文数={orderQueue.activeOrders.Count}");
    }
}
