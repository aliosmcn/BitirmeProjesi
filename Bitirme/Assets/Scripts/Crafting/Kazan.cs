using System;
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

    [SerializeField] private Transform placePoint, spawnPoint;

    public bool alabiliyorMu = true;
    
    private int itemCount = 0;

    public void PlaceObject(Interactable obj)
    {
        items.Add(obj);
        if (items.Count == 4) alabiliyorMu = false;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateRecipe();
        }
    }

    public ItemSO SearchRecipes()
    {
        foreach(var recipe in recipes)
        {
            foreach (var material in recipe.materials)
            {
                foreach (var item in items)
                {
                    if (item.ItemData.ItemID == material.ItemID)
                    {
                        itemCount++;
                    }

                    if (itemCount == recipe.materials.Count)
                    {
                        return recipe.result;
                    }
                }
            }

            itemCount = 0;
        }

        return null;
    }

    public void CreateRecipe()
    {
        ItemSO resultItem = SearchRecipes();
        
        // Eğer recipe yoksa hiçbir şey yapma
        if (resultItem == null || resultItem.prefab == null) 
            return;
        
        // Prefabı oluştur ve Rigidbody'yi al
        GameObject createdItem = Instantiate(resultItem.prefab, spawnPoint.position, Quaternion.identity);
        createdItem.TryGetComponent(out Rigidbody rb);
        
        
        
        if (rb)
        {
            rb.isKinematic = true;
            
            LeanTween.moveY(createdItem, spawnPoint.position.y + 1.2f, 1.5f)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() => {
                    
                    LeanTween.rotateAround(createdItem, Vector3.up, 10f, 0.5f)
                        .setEase(LeanTweenType.easeInOutSine)
                        .setLoopPingPong(1);
                });
        }
        else
        {
            Debug.LogWarning("Oluşturulan objede Rigidbody yok!");
        }
        resultItem = null;
    }

    public Transform GetTransform()
    {
        return placePoint;
    }
}
