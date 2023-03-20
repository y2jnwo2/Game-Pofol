//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ItemDatabase : MonoBehaviour
//{
//    public static ItemDatabase instance;

//    public GameObject fieldItemPrefab;
//    public List<Item> itemDB = new List<Item>();
//    public Vector3[] pos;

//    void Awake()
//    {
//        instance = this;
//    }

//    void Start()
//    {
//        for (int i = 0; i < 5; i++) {
//            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
//            go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, 3)]);
//        }
//    }
//}