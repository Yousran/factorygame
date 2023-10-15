using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        // Duplicate the assigned Skybox material to ensure it's a separate instance
        skyboxMaterial = new Material(skyboxMaterial);
        UnityEngine.RenderSettings.skybox = skyboxMaterial;

        // Start your rotation coroutine
        StartCoroutine(RotateSun());
    }

    void Update()
    {
        // Rotate the light continuously based on the currentRotation
        SunLight.transform.rotation = Quaternion.Euler(currentRotation, 0, 0);

        if (currentRotation <= 20)
        {
            SkyboxLerpValue = currentRotation / 20;
        }
        else if (currentRotation >= 155)
        {
            SkyboxLerpValue = (20 - (currentRotation - 155)) / 20;
        }


        // Set the Skybox material's "TimeOfDay" property
        skyboxMaterial.SetFloat("_TimeOfDay", SkyboxLerpValue);
        //Debug.Log(skyboxMaterial.GetFloat("TimeOfDay"));

        if (currentRotation > 180)
        {
            SunLight.SetActive(false);
        }
        else
        {
            SunLight.SetActive(true);
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
