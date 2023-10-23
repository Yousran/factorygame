using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortGen : MonoBehaviour
{
    public GameObject port;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnPort(int BesarX, int BesarZ)
    {
        Vector3 posisi = new Vector3(transform.position.x + BesarX /2, transform.position.y + 0.9f, transform.position.z + BesarZ + 10);
        GameObject SpawnedPort = Instantiate(port, posisi, Quaternion.identity);
        Ray ray = new Ray(SpawnedPort.transform.position, Vector3.back);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            SpawnedPort.transform.position = new Vector3(hit.point.x, SpawnedPort.transform.position.y, hit.point.z);

            // Mengatur rotasi GameObject sesuai dengan normal permukaan
            SpawnedPort.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(hit.normal.z, 0f, -hit.normal.x));

            SpawnPlayer(SpawnedPort.transform.position);
        }
    }

    public void SpawnPlayer(Vector3 SpawnPoint)
    {
        GameObject SpawnedPlayer = GameObject.Find("Character");
        if (SpawnedPlayer != null)
        {
            SpawnedPlayer.transform.position = new Vector3(SpawnPoint.x, SpawnPoint.y + 2, SpawnPoint.z);
        }
        else
        {
            SpawnedPlayer = Instantiate(Player, new Vector3(SpawnPoint.x, SpawnPoint.y + 2, SpawnPoint.z), Quaternion.identity);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(port.transform.position, Vector3.back);
    }
}
