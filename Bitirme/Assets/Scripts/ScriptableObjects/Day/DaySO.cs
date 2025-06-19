using UnityEngine;

[CreateAssetMenu]
public class DaySO : ScriptableObject
{
    [SerializeField] private int day;

    public int Day
    {
        get { return day; }
    }
    
    [SerializeField] private int customerCount;
    
    public int CustomerCount
    {
        get { return customerCount; }
    }


}