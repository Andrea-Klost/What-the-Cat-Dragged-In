using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryLocation : MonoBehaviour {
    void OnTriggerEnter(Collider coll) {
        // If coll is a potion, notify OrderSystem that it has been delivered and destroy coll
        Potion collPotion = coll.GetComponent<Potion>();
        if (collPotion != null) {
            if (OrderSystem.POTION_DELIVERED(collPotion)) {
                Destroy(coll.gameObject);
            }
        }
    }
}
