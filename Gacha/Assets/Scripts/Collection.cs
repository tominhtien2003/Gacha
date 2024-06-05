using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField] private string[] itemNames;

    [SerializeField] private Transform itemPrefab;

    [SerializeField] private Transform itemPrefabSide;

    [SerializeField] private Transform itemHolder;

    public List<Transform> items { get; set; }

    private void Awake()
    {
        items = new List<Transform>();
        ShuffleItemNames();
    }

    private void Start()
    {
        CreateItems();
    }

    private void CreateItems()
    {
        float numberItems = 12;
        float distanceBetweenTwoItems = 360f / numberItems;

        for (int i= 0;i<numberItems; i++)
        {
            Transform itemTransform = Instantiate(itemPrefab, itemHolder);

            Transform itemTransformSide = Instantiate(itemPrefabSide, itemHolder);

            float angle = i * distanceBetweenTwoItems - 15f;

            Vector3 position = new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad) * 7f, // Tính toán tọa độ x
                Mathf.Cos(angle * Mathf.Deg2Rad) * 7f, // Tính toán tọa độ y
                .9f); 
            Vector3 positionSide = new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad) * 12f, // Tính toán tọa độ x
                Mathf.Cos(angle * Mathf.Deg2Rad) * 11f, // Tính toán tọa độ y
                4f);
            if (positionSide.y > 10.6)
            {
                positionSide.y = 10.6f;
            }
            itemTransform.localPosition = position;

            itemTransformSide.localPosition = positionSide;

            itemTransform.localRotation = Quaternion.Euler(0f, 0f, 90f - angle);

            Vector3 angleItemSide = new Vector3(itemPrefabSide.localEulerAngles.x+angle+15f,itemPrefabSide.localEulerAngles.y, itemPrefabSide.localEulerAngles.z);

            itemTransformSide.localEulerAngles = angleItemSide;

            itemTransform.GetComponent<TextMeshProUGUI>().text = itemNames[i];

            itemTransformSide.GetComponent<TextMeshProUGUI>().text = itemNames[i];

            items.Add(itemTransform);
        }
    }

    private void ShuffleItemNames()
    {
        System.Random rng = new System.Random();

        int n = itemNames.Length;

        while (n > 1)
        {
            int k = rng.Next(n--);

            string temp = itemNames[n];

            itemNames[n] = itemNames[k];

            itemNames[k] = temp;
        }
    }
    public TextMeshProUGUI CaculatePositionItemSelected(Vector3 arrow)
    {
        float minDis = Mathf.Infinity;
        TextMeshProUGUI result = null;
        foreach (Transform item in items)
        {
            float distance = Vector3.Distance(arrow, item.position);
            if (distance < minDis)
            {
                minDis = distance;
                result = item.GetComponent<TextMeshProUGUI>();
            }
        }
        return result;
    }
}
