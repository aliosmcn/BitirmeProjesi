using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Crafting/Recipe", fileName = "NewRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<ItemSO> materials;
    public ItemSO result;

}