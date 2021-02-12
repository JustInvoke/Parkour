using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] List<Item> items;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 3) == 0) {  //25% chance to spawn item
            int i = Random.Range(0, items.Count);
            GameObject item = Instantiate(items[i].gameObject);
            item.transform.position = transform.position;
        }
        Destroy(gameObject);
    }
}
