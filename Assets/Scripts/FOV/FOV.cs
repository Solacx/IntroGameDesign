using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [SerializeField] [Range(0, 360)] public int viewRadius = 5;
    [SerializeField] [Range(0, 360)] public int viewAngle = 45;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    // public float meshResolution;  // Can go bonkers if over 1
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public int edgeResolveIterations;
    public float edgeDistanceThreshold;

    public float maskCutawayDistance = 1.0F;

    // public Controller temp;

    public float meshResolution;        // Some reason values above 1 are bonkers

    void Start() {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    // void Update() {
    //     DrawFOV();
    // }

    // private void DrawFOV() {
    //     int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
    //     float stepAngleSize = viewAngle / stepCount;

    //     List<Vector3> viewPoints = new List<Vector3>();

    //     for (int i = 0; i <= stepCount; i++) {
    //         // There is a small issue here where one more line is drawn
    //         // on left or right side based on mouse position, but not
    //         // really a big problem

    //         float angle = transform.eulerAngles.z - viewAngle/2 + stepAngleSize * i;
    //         Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);

    //         ViewCastData newViewCast = ViewCast(angle);
    //         viewPoints.Add(newViewCast.point);
    //     }

    //     // TEMP
    //     // for (int i = 0; i < viewPoints.Count; i++ ) {
    //     //     Vector3 temp = new Vector3(viewPoints[i].x, -viewPoints[i].y, viewPoints[i].z);
    //     //     viewPoints[i] = temp;
    //     // }

    //     int vertexCount = viewPoints.Count + 1;
    //     Vector3[] vertices = new Vector3[vertexCount];
    //     int[] triangles = new int[(vertexCount - 2) * 3];

    //     vertices[0] = Vector3.zero;
    //     for (int i = 0; i < vertexCount - 1; i++) {
    //         vertices[i + 1] = viewPoints[i];

    //         if (i < vertexCount - 2) {
    //             triangles[i * 3] = 0;
    //             triangles[i * 3 + 1] = i + 1;
    //             triangles[i * 3 + 2] = i + 2;
    //         }
    //     }

    //     viewMesh.Clear();
    //     viewMesh.vertices = vertices;
    //     viewMesh.triangles = triangles;
    //     viewMesh.RecalculateNormals();
    // }

    // ViewCastData ViewCast(float globalAngle) {
    //     Vector3 dir = DirFromAngle(globalAngle, true);
    //     RaycastHit hit;

    //     Debug.DrawRay(transform.position, dir * viewRadius, Color.green);

    //     if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)) {
    //         return new ViewCastData(true, hit.point, hit.distance, globalAngle);
    //     } else {
    //         return new ViewCastData(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    //     }
    // }

    // public struct ViewCastData {
    //     public bool isHit;
    //     public Vector3 point;
    //     public float distance;
    //     public float angle;

    //     public ViewCastData(bool isHit, Vector3 point, float distance, float angle) {
    //         this.isHit = isHit;
    //         this.point = point;
    //         this.distance = distance;
    //         this.angle = angle;
    //     }
    // }

    void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();
        CastData oldViewCast = new CastData();

        for (int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;

            Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);

            CastData newViewCast = ViewCast(angle);

            if (i > 0) {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        // NB: Positions of vertices need to be in local space
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.right * maskCutawayDistance;
            // Debug.Log(vertices[i + 1]);

            if (i < vertexCount - 2) {
                triangles[i*3] = 0;
                triangles[i*3+1] = i + 1;
                triangles[i*3+2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(CastData minCast, CastData maxCast) {
        float minAngle = minCast.angle;
        float maxAngle = maxCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            CastData newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hit == minCast.hit && !edgeDistanceThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    CastData ViewCast(float globalAngle) {
        // Good news is, even though this doesn't work it's just
        // the visualisation. The actual hitboxes still line up
        // proper.

        // Distances are messed up (huge)
        // Direction is fine, seems to be detecting collisions fine

        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        // Viewcast is not aligned with viewcone
        // Casts seem to be centered around world origin
        // Debug.Log(dir);

        // Debug.Log(transform.position);
        // Debug.DrawRay(transform.position, dir * viewRadius, Color.green);
        // Debug.DrawLine(transform.position, dir * viewRadius, Color.green);

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)) {
            GameObject temp = hit.collider.gameObject;
            if (temp.GetComponent<AppearOnSight>() != null) {   // THIS IS FROM THE TEST SCENE
                temp.GetComponent<AppearOnSight>().setActiveColor(true);
            } else if (temp.GetComponent<DisappearOnSight>() != null) {
                temp.GetComponent<DisappearOnSight>().Disappear();
            }
            
            return new CastData(true, hit.point, hit.distance, globalAngle);
        } else {
            return new CastData(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct CastData {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public CastData(bool _hit, Vector3 _point, float _distance, float _angle) {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    void LateUpdate() {
        FindVisibleTargets();
        DrawFieldOfView();
    }

    void FindVisibleTargets() {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            // Debug.Log("Something in radius");
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2) {
                // Debug.Log("In angle");
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    // No obstacles
                    Debug.Log(gameObject.name + " saw a target.");
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool isAngleGlobal) {
        if (!isAngleGlobal) {
            angleInDegrees += transform.eulerAngles.z;

            // Issue is because eulerAngles is returning between
            // 270-360 and 0-90 (bottom, top)
            // Debug.Log(transform.eulerAngles.z);
        }

        if (Input.mousePosition.x < Camera.main.WorldToScreenPoint(transform.position).x) {
            // Debug.Log("LEFT");
            if (Input.mousePosition.y < Camera.main.WorldToScreenPoint(transform.position).y) {
                // Debug.Log("DOWN");
                angleInDegrees = (90 - angleInDegrees) + 90;
            } else {
                // Debug.Log("UP");
                angleInDegrees = (90 - angleInDegrees) + 90;
            }
        }
        // Debug.Log(angleInDegrees);

        // return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public struct EdgeInfo {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 a, Vector3 b) {
            pointA = a;
            pointB = b;
        }
    }
}
