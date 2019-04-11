using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeshProperties : MonoBehaviour
{
    public string objectType;
    public Material[] materials = new Material[2];
    MeshRenderer mesh;

    // Use this for initialization
    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    public void changeType(int type)
    {
        if (mesh == null)
        {
            Start();
        }

        if (type == 0)
        {
            mesh.material = materials[1];
            objectType = "wall";

        }
        else
        {
            mesh.material = materials[0];
            objectType = "floor";

        }

    }
}
