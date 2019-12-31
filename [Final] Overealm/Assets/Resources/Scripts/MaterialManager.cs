using UnityEngine.Audio;
using System;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{

    public static MaterialManager instance;

    public CustomMat[] materials;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Material GetMaterial(string _matID)
    {
        CustomMat s = Array.Find(materials, item => item.ID == _matID);
        if (s == null)
        {
            Debug.LogWarning("Material: " + _matID + " not found!");
            return null;
        }
        return s.mat;
    }
}

[System.Serializable]
public class CustomMat
{
    public string ID;
    public Material mat;
}