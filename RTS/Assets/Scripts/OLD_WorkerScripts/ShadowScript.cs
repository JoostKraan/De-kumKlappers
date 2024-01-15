using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    public GameObject shadowPlane;
    public Transform[] detectors;
    public LayerMask shadowLayer;
    public float shadowRadius = 10f;
    private float radiusSquared { get { return shadowRadius * shadowRadius; } }

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        for (int j = 0; j < detectors.Length; j++)
        {
            Vector3 direction = detectors[j].position - transform.position;
            Ray r = new Ray(transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(r, out hit, 100, shadowLayer, QueryTriggerInteraction.Collide))
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 v = shadowPlane.transform.TransformPoint(vertices[i]);
                    float distance = Vector3.SqrMagnitude(v - hit.point);

                    if (distance < radiusSquared)
                    {
                        float alpha = Mathf.Min(colors[i].a, distance / radiusSquared);
                        colors[i].a = alpha;
                    }
                }
            }
        }

        UpdateColors();
    }

    public void UpdateColors()
    {
        mesh.colors = colors;
    }

    void Initialize()
    {
        mesh = shadowPlane.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }

        UpdateColors();
    }
}
