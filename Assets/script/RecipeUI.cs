using System.Text;
using System.Linq;
using UnityEngine;
using TMPro;

public class RecipeUI : MonoBehaviour
{
    public OrderQueue orderQueue;
    [SerializeField] private TMP_Text recipeText;

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (orderQueue == null || recipeText == null)
            return;

        if (orderQueue.activeOrders.Count == 0)
        {
            recipeText.text = "注文なし";
            return;
        }

        var sb = new StringBuilder();

        // === 1人目の注文 ===
        Recipe first = orderQueue.activeOrders[0];
        sb.AppendLine("一人目の注文");
        AppendRecipe(sb, first);

        // === 次の注文（あれば） ===
        if (orderQueue.activeOrders.Count >= 2)
        {
            Recipe next = orderQueue.activeOrders[1];
            sb.AppendLine();
            sb.AppendLine("次の注文");
            AppendRecipe(sb, next);
        }

        // 残り注文数
        if (orderQueue.activeOrders.Count > 2)
        {
            sb.AppendLine();
            sb.AppendLine($"（残り {orderQueue.activeOrders.Count - 2} 件）");
        }

        recipeText.text = sb.ToString();
    }

    private void AppendRecipe(StringBuilder sb, Recipe recipe)
    {
        // 工程名（最初の工程＝完成物っぽく見せる）
        sb.AppendLine($"・{recipe.recipeName} 1");

        // 工程から「材料セット」を抽出して表示
        // Mix工程があればそれを優先表示
        var mixStep = recipe.processSteps
            .FirstOrDefault(s => s.workbenchType == WorkbenchType.Mix);

        if (mixStep != null && mixStep.inputs != null)
        {
            string materials =
                string.Join(" + ", mixStep.inputs.Select(i => i.ingredientName));
            sb.AppendLine($"　{materials}");
        }
        else
        {
            // Mixが無い場合は全工程の材料をざっくり表示
            var allInputs = recipe.processSteps
                .SelectMany(s => s.inputs)
                .Distinct()
                .Select(i => i.ingredientName);

            sb.AppendLine($"　{string.Join(" + ", allInputs)}");
        }
    }
}
