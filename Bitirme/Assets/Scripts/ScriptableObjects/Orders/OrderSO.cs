using UnityEngine;

[CreateAssetMenu(menuName = "OrderSystem/Order", fileName = "NewOrder")]
public class OrderSO : ScriptableObject
{
    [SerializeField] private GameObject hasta;
    [SerializeField] private CraftingRecipe orderRecipe;
    public string orderText;

    public CraftingRecipe OrderRecipe
    {
        get
        {
            return orderRecipe;
        }
    }
}