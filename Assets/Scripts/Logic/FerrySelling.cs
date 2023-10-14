using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrySelling : MonoBehaviour
{
    public int total = 0;
    public TreeChopProgress UI;
    // Start is called before the first frame update
    void Start()
    {
        UI = FindObjectOfType<TreeChopProgress>();
    }

    // Update is called once per frame
    void Update()
    {
            UI.BankUI.text = total.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Mendeteksi objek yang masuk ke dalam collider
        if (other) // Anda bisa mengganti "Player" dengan tag objek yang Anda ingin deteksi
        {
            Debug.Log("Nama objek: " + other.gameObject.name);
        }
        if (other.GetComponent<WoodStat>())
        {
            total += other.GetComponent<WoodStat>().HargaWood;
            Destroy(other.gameObject);
        }
    }
}
