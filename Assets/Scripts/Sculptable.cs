using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculptable : MonoBehaviour
{


    GameObject brush = null;
    MeshFilter filter;
    Mesh mesh;

    Vector3[] verts;
    int[] tris;
    Vector2[] uvs;

    public int range = 2;


    GameObject[] trackers;

    GameObject tracker;

    Transform tracked;

    Vector3 currtracked;
    Vector3 prevtracked;




    // Start is called before the first frame update
    void Start()
    {
        brush = GameObject.Find("Brush");
        filter = GetComponent<MeshFilter>();
        mesh = filter.mesh;

        verts = mesh.vertices;
        tris = mesh.triangles;
        uvs = mesh.uv;

        tracker = GameObject.Find("v");

        trackers = new GameObject[mesh.vertices.Length];

        for (int i = 0; i < trackers.Length; i++)
        {
            GameObject newt = Instantiate(tracker, transform.TransformPoint(verts[i]), Quaternion.identity);
            newt.SetActive(true);

            newt.transform.SetParent(this.transform);

            if (i == 35)
            {
                newt.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                tracked = newt.transform;
                prevtracked = tracked.position;

            }

            trackers[i] = newt;
        }
    }



    // Update is called once per frame
    void Update()
    {

        currtracked = tracked.position;

        verts = mesh.vertices;
        tris = mesh.triangles;
        uvs = mesh.uv;


        Vector3 cur, cur_wrld ,diff;
        float coef;


        for (int i = 0; i < verts.Length; i++)
        {

            cur = mesh.vertices[i];
            cur_wrld = transform.TransformPoint(cur);

            diff = currtracked - prevtracked;


            coef = 1 - Mathf.Clamp( Vector3.Magnitude(cur_wrld - currtracked)/2, 0,range);
            



            if (coef < 1 && coef > 0)
            {

                verts[i] = transform.InverseTransformPoint(cur_wrld + coef * diff);
                //mesh.vertices[i] = transform.InverseTransformPoint(cur_wrld + coef * diff);
                trackers[i].transform.position = transform.TransformPoint( verts[i] );
            }
            else
            {   
                verts[i] = mesh.vertices[i];
                //mesh.vertices[i] = transform.InverseTransformPoint(cur_wrld + coef * diff);
            }
        }

        mesh.vertices = verts;

        prevtracked = currtracked;
    }
}
