
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    public event EventHandler<OnRandomResultItemEventArgs> OnRandomResultItem;

    public class OnRandomResultItemEventArgs : EventArgs
    {
        public Transform itemSelected;

        public int index;
    }

    [SerializeField] private Transform collection;

    private Transform itemSelected;
    public void RandomResultItemBeforeSpin()
    {
        List<Transform> items = collection.GetComponent<Collection>().items;

        int index = UnityEngine.Random.Range(0,items.Count);

        //Debug.Log(collection.GetComponent<Collection>().items.Count);

        itemSelected = items[index];

        items[index].GetComponent<TextMeshProUGUI>().color = Color.red;

        OnRandomResultItem?.Invoke(this, new OnRandomResultItemEventArgs { index = index,itemSelected = itemSelected});
    }
}
