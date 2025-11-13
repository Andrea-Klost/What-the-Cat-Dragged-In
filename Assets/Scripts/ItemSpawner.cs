using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    [Header("Inscribed")]
    [SerializeField]
    [Tooltip("Prefab of the item that the ItemSpawner spawns")]
    public GameObject itemPrefab;
    [Tooltip("Location at which items are spawned")]
    public Transform spawnPoint;
    [Tooltip("Max number of items that can be concurrently spawned. " +
             "If the spawner attempts to spawn an item at this limit, the oldest spawned item will be destroyed.")]
    public int maxItemInstances = 3;
    [Tooltip("Number of seconds after an item is taken to spawn a new item")]
    public float itemSpawnDelay = 3f;
    
    // Keeps track of currently spawned in items
    private Queue<GameObject> _itemInstances;
    // Keeps track of item currently occupying the spawner
    private GameObject _occupyingItem;
    
    void Awake() {
        // If spawnPoint is not set, default it to the gameObject's transform
        if (spawnPoint == null) {
            spawnPoint = gameObject.transform;
        }

        _itemInstances = new Queue<GameObject>();
        _occupyingItem = null;
    }

    void Start() {
        SpawnItem();
    }

    void SpawnItem() {
        GameObject newItem = Instantiate(itemPrefab);
        Grabbable itemGrabScript = newItem.GetComponent<Grabbable>();
        if (itemGrabScript != null) {
            itemGrabScript.Manager = this;
        }
        newItem.transform.position = spawnPoint.position;
        _itemInstances.Enqueue(newItem);
        _occupyingItem = newItem;
        // If spawning a newItem causes there to be more instances than allowed, destroy the oldest instance
        if (_itemInstances.Count > maxItemInstances) {
            // Note: since Cat holds references to grabbable items, this may cause a missing reference
            //       if the cat is holding the item or the item is a potential grab target
            Destroy(_itemInstances.Dequeue());
        }
    }

    public void ItemGrabbed(GameObject item) {
        // If item occupying the spawner leaves, spawn a new item after a delay
        if (item == _occupyingItem) {
            _occupyingItem = null;
            Invoke(nameof(SpawnItem), itemSpawnDelay);
        }
    }
}
