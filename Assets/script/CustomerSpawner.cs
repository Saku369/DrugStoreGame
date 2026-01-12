using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Customer customerPrefab;
    public Recipe[] recipePool;
    public OrderQueue orderQueue;

    public float spawnInterval = 30f;
    public Transform spawnPoint;

    private void Start()
    {
        SpawnOne();                 // 最初の1人（不要なら消してOK）
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnOne();
        }
    }

    private void SpawnOne()
    {
        if (customerPrefab == null)
        {
            Debug.LogError("customerPrefab が未設定");
            return;
        }
        if (orderQueue == null)
        {
            Debug.LogError("orderQueue が未設定（OrderQueue.asset を入れて）");
            return;
        }
        if (recipePool == null || recipePool.Length == 0)
        {
            Debug.LogError("recipePool が空（Recipe.asset を配列に入れて）");
            return;
        }

        Recipe recipe = recipePool[Random.Range(0, recipePool.Length)];

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        Customer c = Instantiate(customerPrefab, pos, rot);
        c.Init(recipe, orderQueue);

        Debug.Log($"客生成：{recipe.recipeName} / 注文数={orderQueue.activeOrders.Count}");
    }
}
