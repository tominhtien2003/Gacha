using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField] private string[] itemNames;

    [SerializeField] private Transform itemPrefab;

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

            float angle = i * distanceBetweenTwoItems - 15f;

            Vector3 position = new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad) * 7f, // Tính toán tọa độ x
                Mathf.Cos(angle * Mathf.Deg2Rad) * 7f, // Tính toán tọa độ y
                0f); // Vòng tròn nằm trên mặt phẳng x-y, nên z = 0

            itemTransform.localPosition = position;

            itemTransform.localRotation = Quaternion.Euler(0f, 0f, 90f - angle);

            itemTransform.GetComponent<TextMeshProUGUI>().text = itemNames[i];

            items.Add(itemTransform);

            //Debug.Log(items[i].GetComponent<TextMeshProUGUI>().text);
        }
        //Debug.Log(items.Count);
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
    public string CaculatePositionItemSelected(Transform arrow)
    {
        float minDis = Mathf.Infinity;
        string result = string.Empty;
        foreach (Transform item in items)
        {
            float distance = Vector3.Distance(arrow.position, item.position);
            if (distance < minDis)
            {
                minDis = distance;
                result = item.GetComponent<TextMeshProUGUI>().text;
            }
        }
        return result;
    }
}
