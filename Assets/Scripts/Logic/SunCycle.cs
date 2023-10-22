using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SunCycle : MonoBehaviour
{
    public GameObject SunLight;
    public Material skyboxMaterial; // Store the Skybox material here
    public float rotationSpeed = 5.0f;
    public float SkyboxLerpValue = 0f;
    public float currentRotation = 0.0f;
    public Light light;

    void Start()
    {
        // Duplicate the assigned Skybox material to ensure it's a separate instance
        skyboxMaterial = new Material(skyboxMaterial);
        UnityEngine.RenderSettings.skybox = skyboxMaterial;
        light = SunLight.GetComponent<Light>();
        // Start your rotation coroutine
        StartCoroutine(RotateSun());
    }

    void Update()
    {
        // Rotate the light continuously based on the currentRotation
        

        if (currentRotation <= 5)
        {
            SkyboxLerpValue = currentRotation / 5;
            light.color = Color.Lerp(new Color32(10, 10, 10, 255), Color.white, SkyboxLerpValue);
        }
        else if (currentRotation >= 180)
        {
            SkyboxLerpValue = Mathf.Clamp((5 - (currentRotation - 180)) / 5,0f,1f);
            light.color = Color.Lerp(new Color32(10, 10, 10, 255), Color.white, SkyboxLerpValue);
        }


        // Set the Skybox material's "TimeOfDay" property
        skyboxMaterial.SetFloat("_TimeOfDay", SkyboxLerpValue);

        UnityEngine.RenderSettings.fogColor = Color.Lerp(Color.black, new Color32(181, 255, 249, 255), SkyboxLerpValue);

        //Debug.Log(skyboxMaterial.GetFloat("TimeOfDay"));

        if (currentRotation > 180)
        {
            //light.color = new Color32(10, 10, 10, 255);
            SunLight.transform.rotation = Quaternion.Euler(currentRotation-180, 0, 0);
            //SunLight.SetActive(false);
        }
        else
        {
            //light.color = Color.white;
            SunLight.transform.rotation = Quaternion.Euler(currentRotation, 0, 0);
            //    SunLight.SetActive(true);
        }
    }

    IEnumerator RotateSun()
    {
        while (true)
        {
            // Increment the rotation
            currentRotation += rotationSpeed * Time.deltaTime;

            // If we exceed 360 degrees, reset the rotation
            if (currentRotation >= 360.0f)
            {
                currentRotation = 0.0f;
            }

            yield return null;
        }
    }
}
