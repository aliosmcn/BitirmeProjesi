using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    

    public GameObject createdItem;
    private ItemSO resultItem;

    private void OnEnable()
    {
        EventManager.OnMixed += CreateRecipe;
        EventManager.OnMixing += Mix;
    }

    private void OnDisable()
    {
        EventManager.OnMixed -= CreateRecipe;
        EventManager.OnMixing -= Mix;
    }

    public void PlaceObject(Interactable obj)
    {
        items.Add(obj);
        if (items.Count == 3) alabiliyorMu = false;
        
    }

    public ItemSO SearchRecipes()
    {
        foreach (var recipe in recipes)
        {
            int matchedCount = 0;

            foreach (var material in recipe.materials)
            {
                foreach (var item in items)
                {
                    if (item.ItemData.ItemID == material.ItemID)
                    {
                        matchedCount++;
                        break;
                    }
                }
            }

            if (matchedCount == recipe.materials.Count)
                return recipe.result;
        }

        return null;
    }

    public void Mix()
    {
        if (items.Count == 0) return;
        if (!SearchRecipes()) return;
    
        Kepce.Instance.gameObject.TryGetComponent(out Animator animator);
        animator.SetBool("Mixing", true); 
    }
    
    public void CreateRecipe()
    {
        resultItem = null;
        resultItem = SearchRecipes();
        
        if (resultItem == null || resultItem.prefab == null) 
            return;
        
        createdItem = Instantiate(resultItem.prefab, spawnPoint.position, Quaternion.identity);
        createdItem.TryGetComponent(out Rigidbody rb);
        ClearItems();
        
        if (rb)
        {
            rb.isKinematic = true;
            
            StartCoroutine(AnimateCreatedItem(createdItem));
        }
        else
        {
            Debug.LogWarning("Oluşturulan objede Rigidbody yok!");
        }
    }
    IEnumerator AnimateCreatedItem(GameObject obj)
    {
        Vector3 startPos = obj.transform.position;
        Vector3 targetPos = startPos + Vector3.up * 1.2f;
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPos;

        // Döndürme animasyonu
        float rotateDuration = 0.5f;
        float rotateElapsed = 0f;
        Quaternion startRot = obj.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 10f, 0);

        while (rotateElapsed < rotateDuration)
        {
            obj.transform.rotation = Quaternion.Lerp(startRot, endRot, Mathf.PingPong(rotateElapsed * 2, 1));
            rotateElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void ClearItems()
    {
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();
        alabiliyorMu = true;
    }

    public Transform GetTransform()
    {
        return placePoint;
    }
}