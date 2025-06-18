using UnityEngine;

[CreateAssetMenu(menuName = "OrderSystem/Order", fileName = "NewOrder")]
public class OrderSO : ScriptableObject
{
    [SerializeField] private GameObject hasta;
    
    public GameObject Hasta { get => hasta; set => hasta = value; }
    
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