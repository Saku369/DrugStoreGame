using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "DrugStore/Ingredient")]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public Sprite icon;
    public IngredientType type;

    public GameObject worldPrefab; // ★追加
}
