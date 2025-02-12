using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartGenScript : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 100;  
    float time = 0f;
    Vector3[] finalPos;
    Vector3[] initPos;
    float[] riseSpeed = new float[numSphere];
    float[] randomOffsets = new float[numSphere];
    float[] colorOffsets = new float[numSphere];


    void Start()
    {
        spheres = new GameObject[numSphere]; 
        finalPos = new Vector3[numSphere];
        initPos = new Vector3[numSphere];

        for (int i = 0; i < numSphere; i++)
        {
            float t = Mathf.PI * 2 * i / numSphere; 
            
            float x = 16 * Mathf.Pow(Mathf.Sin(t), 3);
            float y = 13 * Mathf.Cos(t) - 5 * Mathf.Cos(2 * t) - 2 * Mathf.Cos(3 * t) - Mathf.Cos(4 * t);
            
            // Final heart position
            float scale = 0.5f;
            finalPos[i] = new Vector3(x * scale, y * scale, 0f); 

            // Start position at the ground level randomly
            float randX = Random.Range(-10f, 10f); 
            float randZ = Random.Range(-10f, 10f); 
            initPos[i] = new Vector3(randX, -5f, randZ); 

            // Assign a unique rise speed 
            riseSpeed[i] = Random.Range(5.0f, 12.0f); 

            // Assign random movement offsets
            randomOffsets[i] = Random.Range(0.5f, 2.5f); 

            // Assign different pink hues based on position
            colorOffsets[i] = Random.Range(0.8f, 1.0f); 

            // Create a sphere
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].transform.position = initPos[i];
            spheres[i].transform.localScale = Vector3.one * 0.3f; 
            
            
            // Color Assignment
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = 0.9f; 
            float saturation = 0.7f + 0.2f * Mathf.Sin(time); 
            float brightness = 0.8f + 0.2f * Mathf.Cos(time); 

            Color color = Color.HSVToRGB(hue, saturation, brightness);
            sphereRenderer.material.color = color;
            
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        for (int i = 0; i < numSphere; i++)
        {
            // Random floaty movement (applies only in the early stages)
            float noiseX = Mathf.PerlinNoise(time * 0.5f + randomOffsets[i], randomOffsets[i]) - 0.5f;
            float noiseZ = Mathf.PerlinNoise(time * 0.5f - randomOffsets[i], randomOffsets[i]) - 0.5f;
            float noiseY = Mathf.Sin(time * randomOffsets[i]) * 0.3f; 

            Vector3 floatyOffset = new Vector3(noiseX, noiseY, noiseZ) * (1.5f - time / 10f); 

            // Calculate rise animation (each sphere rises at a different speed)
            float t = Mathf.Clamp01(time / riseSpeed[i]); 
            spheres[i].transform.position = Vector3.Lerp(initPos[i], finalPos[i], t * t) + floatyOffset;

            // Make the heart pulsate slightly (breathing effect)
            float scale = 0.5f + Mathf.Sin(time) * 0.05f;
            spheres[i].transform.localScale = Vector3.one * (0.3f * scale);

            // Dynamic pink color transition (Light ↔ Dark Pink)
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = 0.9f; 
            float saturation = 0.7f + 0.2f * Mathf.Sin(time + colorOffsets[i]); 
            float brightness = 0.8f + 0.2f * Mathf.Cos(time + colorOffsets[i]); 

            Color color = Color.HSVToRGB(hue, saturation, brightness);
            sphereRenderer.material.color = color;
        }
    }
}
