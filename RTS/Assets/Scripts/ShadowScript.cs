using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    public GameObject shadowPlane;
    public Transform player;
    public LayerMask shadowLayer;
    public float shadowRadius = 10f;
    private float radiuscircle { get { return shadowRadius * shadowRadius; } }

    private Mesh mesh;
    private Vector3[] vertecies;
    private Color[] colors;
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Ray r = new Ray(transform.position, player.position - transform.position);
        RaycastHit hit;
        if(Physics.Raycast(r, out hit,100,shadowLayer, QueryTriggerInteraction.Collide))
        {
            for (int i = 0; i < vertecies.Length; i++) 
            {
                Vector3 v = shadowPlane.transform.TransformPoint(vertecies[i]);
                float distance = Vector3.SqrMagnitude(v - hit.point);
                if(distance < radiuscircle)
                {
                    float alpha = Mathf.Min(colors[i].a, distance / radiuscircle);
                    colors[i].a = alpha;
                }
            }

            UpdateColors();
        }
    }
    public void UpdateColors()
    {
        mesh.colors= colors;
    }

    void Initialize()
    {
        mesh = shadowPlane.GetComponent<MeshFilter>().mesh;
        vertecies = mesh.vertices;
        colors = new Color[vertecies.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }
        UpdateColors();
    }
}
