using UnityEngine;

[CreateAssetMenu(fileName = "CurrentOrder", menuName = "DrugStore/CurrentOrder")]
public class CurrentOrder : ScriptableObject
{
    // 旧: Recipe currentRecipe
    // 新: Order を保持（必要なら使用）
    public Order currentOrder;
}
