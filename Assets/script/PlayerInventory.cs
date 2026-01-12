using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Ingredient> ingredients = new List<Ingredient>();

    // 今作っているレシピ
    public Recipe currentRecipe;
    public int currentProcessIndex = 0;

    public void AddIngredient(Ingredient ingredient)
    {
        ingredients.Add(ingredient);
        Debug.Log($"取得：{ingredient.ingredientName}");

        string list = string.Join(", ", ingredients.ConvertAll(i => i.ingredientName));
        Debug.Log($"現在の材料：{list}");
    }

    public void Clear()
    {
        ingredients.Clear();
    }

    public void ResetProcess()
    {
        currentProcessIndex = 0;
    }
}
