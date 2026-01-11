using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DrugStore/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;

    [TextArea]
    public string description;

    public List<Ingredient> requiredIngredients;
}