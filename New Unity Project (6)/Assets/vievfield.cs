using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class vievfield : MonoBehaviour
{
    public float meshResolution;
    public MeshFilter viewMeshFilter;

    [HideInInspector]
    public List<Transform> visibleTarget = new List<Transform>();
   
    Mesh viewMesh;
    public float viewRadios;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", .2f);
    }
    private void LateUpdate()
    {
        DrawFieldView();
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }
    void FindVisibleTarget()
    {
        visibleTarget.Clear();
        Collider[] targetVisibleRadios = Physics.OverlapSphere(transform.position, viewRadios, targetMask);
        for(int i = 0; i < targetVisibleRadios.Length; i++)
        {
            Transform target = targetVisibleRadios[i].transform;
            Vector3 dirTotarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirTotarget) < viewAngle / 2)
            {
                float dstTotarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirTotarget, dstTotarget, obstacleMask))
                {
                    visibleTarget.Add(target);
                }
            }
        }
    }
    void DrawFieldView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAnglesize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for(int i=0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAnglesize * i;
            ViewCastInfo NewViewcat = ViewCast(angle);
            viewPoints.Add(NewViewcat.point);
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] verticies = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        verticies[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            verticies[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 8;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
             }
        }

        viewMesh.Clear();
        viewMesh.vertices = verticies;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();


     }
    ViewCastInfo ViewCast(float globalAngel)
    {
        Vector3 dir = FromAngle(globalAngel, true);
        RaycastHit hit;
        if(Physics.Raycast(transform.position,dir,out hit, viewRadios, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngel);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadios, viewRadios, globalAngel);
        }
    }

    public Vector3 FromAngle(float angleIndegress,bool angleIsglobal)
    {
        if (!angleIsglobal)
        {
            angleIndegress += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleIndegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleIndegress * Mathf.Deg2Rad));
    }
    public class ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point,float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }


    }
}
