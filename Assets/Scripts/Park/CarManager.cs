using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public Car myCar;
    private Car[] cars;
    public GameObject goal;

    public GameObject linePrefab;
    public GameObject line;

    public LineRenderer lineRenderer;

    private Vector3 mousePos;
    private RaycastHit hit;
    private Ray clickRay;

    private BoxCollider collider;

    public Queue<Vector3> path;
    private Queue<Vector3> prevPath;

    private Vector3 prevPos;

    public Transform tilePos;

    private bool canRecord = true;

    // Start is called before the first frame update
    void Start()
    {
        cars = FindObjectsOfType<Car>();
        collider = GetComponent<BoxCollider>();
        
        path = new Queue<Vector3>();
        prevPath = new Queue<Vector3>();
        
        if (goal != null)
        {
            goal.GetComponent<Goal>().myCar = myCar.gameObject;
        }

        CreateLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOffCollider()
    {
        collider.enabled = false;
    }

    private void OnMouseDown()
    {
        myCar.ResetPaths();
        canRecord = true;

        path.Clear();
        prevPath.Clear();

        lineRenderer.positionCount = 1;
        
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].CarReset();
        }
    }

    private void OnMouseDrag()
    {
        clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layMask = 1 << LayerMask.NameToLayer("Ground");

        if (Physics.Raycast(clickRay, out hit, layMask))
        {
            if(hit.transform.tag == "Ground")
            {
                mousePos = new Vector3(hit.point.x, tilePos.position.y, hit.point.z);

                if (isNearTheGoal(mousePos))
                {
                    RecordPath(new Vector3(goal.transform.position.x, tilePos.position.y, goal.transform.position.z));
                    canRecord = false;
                }
                else
                {
                    RecordPath(mousePos);
                }
            }
        }
    }

    private void OnMouseUp()
    {
        myCar.SetCarPath(path, prevPath);
        for(int i = 0; i < cars.Length; i++)
        {
            cars[i].CarMove();
        }
    }

    private bool isNearTheGoal(Vector3 pos)
    {
        float dist = Vector3.Distance(pos, goal.transform.position);

        if(dist < 0.4f)
        {
            return true;
        }
        return false;
    }

    private void CreateLine()
    {
        if(line == null)
        {
            line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        }
        lineRenderer = line.GetComponent<LineRenderer>();
        if(lineRenderer == null)
        {
            line.AddComponent<LineRenderer>();
        }
    }

    public void RecordPath(Vector3 pos)
    {
        if (canRecord)
        {
            Vector3 v = new Vector3(pos.x, pos.y + 0.05f, pos.z);

            if (path.Count == 0)
            {
                path.Enqueue(pos);
                lineRenderer.SetPosition(0, v);
                prevPos = pos;
            }
            else
            {
                float dist = Vector3.Distance(prevPos, pos);

                if (dist > 0.05f)
                {
                    path.Enqueue(pos);
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, v);
                    prevPos = pos;
                }
            }

            prevPath = new Queue<Vector3>(path.ToArray());
        }
    }
}
