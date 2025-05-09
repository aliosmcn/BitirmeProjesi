using System.Collections.Generic;
using UnityEngine;

public class Kazan : MonoBehaviour
{
    #region Singleton
    private static Kazan instance;

    public static Kazan Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion Singleton

    [SerializeField] private List<CraftingRecipe> recipes;
    [SerializeField] private List<Interactable> items;

    [SerializeField] private Transform placePoint;

    public bool alabiliyorMu = true;

    public void PlaceObject(Interactable obj)
    {
        
        items.Add(obj);
        if (items.Count == 4) alabiliyorMu = false;
        Debug.Log(items.Count);
    }
    
    public ItemSO SearchRecipes(string item1id, string item2id, string item3id, string item4id)
    {
        foreach(var recipe in recipes)
        {
            foreach (var material in recipe.materials)
            {

                if(item1id.Equals(material.ItemID))
                {
                    
                }

                if (item2id.Equals(material.ItemID))
                {
                    
                }
            }
        }
        return null;
    }

    public void CreateRecipe(ItemSO item)
    {
        
    }

    public Transform GetTransform()
    {
        return placePoint;
    }
}
