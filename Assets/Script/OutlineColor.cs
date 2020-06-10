using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineColor : MonoBehaviour
{
    [SerializeField]
    int materialID = 1;
    [SerializeField]
    string propertySizeName = "_SilhouetteSize";
    [SerializeField]
    string propertyColorName = "_SilhouetteColor";

    Renderer rend;
    float iniSize;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        iniSize = rend.materials[materialID].GetFloat(propertySizeName);
        Hide();
    }

    public void SetColor (Color color) {
        if(rend.materials[materialID].GetFloat(propertySizeName) == 0) Show();

        rend.materials[materialID].SetColor(propertyColorName, color);
    }

    public void Hide () {
        rend.materials[materialID].SetFloat(propertySizeName, 0);
    }

    public void Show () {
        rend.materials[materialID].SetFloat(propertySizeName, iniSize);
    }
}
