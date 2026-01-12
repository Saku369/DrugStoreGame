using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DrugStore/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;

    // 工程順リスト
    public List<ProcessStep> processSteps;

}
