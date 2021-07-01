using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    [SerializeField]
    GameObject linePrefab;
    [SerializeField]
    Camera cam;
    [SerializeField]
    LayerMask paintable;

    [SerializeField]
    float smoothness = 0.1f;
    GameObject currentLine;
    LineRenderer lineRenderer;
    List<Vector3> fingerPositions;

   

    [SerializeField]
    DrawMesh mesh;
    
    // Start is called before the first frame update
    void Start()
    {
        fingerPositions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            createLine();
        }
        if(Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit,float.MaxValue,paintable))
            {
                Vector3 fingerPos = hit.point;
                if(Vector3.Distance(fingerPos,fingerPositions[fingerPositions.Count-1]) > smoothness)
                {
                    updateLine(fingerPos);
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
           
           if(fingerPositions.Count>2)
           {
                mesh.GenerateMesh(fingerPositions.ToArray());
           }
           
        }
    }

    void createLine()
    {  
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit hit,float.MaxValue,paintable))
        {
            Destroy(currentLine);
            mesh.Mesh.Clear();
            currentLine = Instantiate(linePrefab,Vector3.zero,Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            fingerPositions.Clear();
       
            fingerPositions.Add(hit.point);
            fingerPositions.Add(hit.point);

            lineRenderer.SetPosition(0,fingerPositions[0]);
        }
    }
    void updateLine(Vector3 finPos)
    {
        fingerPositions.Add(finPos);
        fingerPositions.Add(finPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount-1,finPos);
    }
}
