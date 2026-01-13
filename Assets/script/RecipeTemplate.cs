using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "DrugStore/RecipeTemplate")]
public class RecipeTemplate : ScriptableObject
{
    public string templateName;
    public List<ProcessStep> steps = new();
}
