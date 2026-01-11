using UnityEngine;
using TMPro;

public class RecipeUI : MonoBehaviour
{
    public OrderQueue orderQueue;
    public TMP_Text recipeText;

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        recipeText.text = "";

        foreach (var recipe in orderQueue.activeOrders)
        {
            recipeText.text += recipe.recipeName + "\n";

            foreach (var ing in recipe.requiredIngredients)
            {
                recipeText.text += "・" + ing.ingredientName + "\n";
            }

            recipeText.text += "\n";
        }
    }
}