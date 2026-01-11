using UnityEngine;

[CreateAssetMenu(fileName = "CurrentOrder", menuName = "DrugStore/CurrentOrder")]
public class CurrentOrder : ScriptableObject
{
    public Recipe currentRecipe;
}
