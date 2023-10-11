using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrySelling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // Mendeteksi objek yang masuk ke dalam collider
        if (other) // Anda bisa mengganti "Player" dengan tag objek yang Anda ingin deteksi
        {
            Debug.Log("Nama objek: " + other.gameObject.name);
        }
    }
}
