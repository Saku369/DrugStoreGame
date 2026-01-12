using UnityEngine;

public class Shelf : MonoBehaviour
{
    public Ingredient ingredient;

    public Ingredient Take()
    {
        Debug.Log($"棚から取得：{ingredient.ingredientName}");
        return ingredient;
    }
}
