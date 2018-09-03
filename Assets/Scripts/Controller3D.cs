using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Controller3D : MonoBehaviour {

    public int xAxisRayCount = 4;
    public int yAxisRayCount = 8;
    public int zAxisRayCount = 4;

    BoxCollider collider;
    RaycastOrigin raycastOrigin;
    float xAxisRaySpacing, yAxisRayspacing, zAxisRayspacing;


    const float SKIN_WIDTH = 0.015f;

    // Use this for initialization
    void Start () {
        collider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigin();
        yCollision(ref velocity);
        
        transform.Translate(velocity);
    }

    void yCollision(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SKIN_WIDTH;

        for (int i = 0; i < xAxisRayCount; i++)
        {
            Vector3 rayOrigin = (directionY == -1) ? raycastOrigin.innerBottomLeft : raycastOrigin.innerTopLeft;
            rayOrigin += Vector3.right * (yAxisRayspacing * i + velocity.x);
            Debug.DrawRay(raycastOrigin.innerBottomLeft + Vector3.right * yAxisRayspacing * i, Vector2.up * -2, Color.red);
        }
    }


    void UpdateRaycastOrigin()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SKIN_WIDTH * -2);

        raycastOrigin.innerBottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigin.innerBottomRight = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        raycastOrigin.innerTopLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        raycastOrigin.innerTopRight = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        raycastOrigin.outerBottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        raycastOrigin.outerBottomRight = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        raycastOrigin.outerTopLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigin.outerTopRight = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SKIN_WIDTH * -2);
        xAxisRayCount = Mathf.Clamp(xAxisRayCount, 2, int.MaxValue);
        yAxisRayCount = Mathf.Clamp(yAxisRayCount, 2, int.MaxValue);
        zAxisRayCount = Mathf.Clamp(zAxisRayCount, 2, int.MaxValue);

        xAxisRaySpacing = bounds.size.x / (xAxisRayCount - 1);
        yAxisRayspacing = bounds.size.y / (yAxisRayCount - 1);
        zAxisRayspacing = bounds.size.z / (zAxisRayCount - 1);
    }

    struct RaycastOrigin
    {
        public Vector3 innerTopLeft, innerTopRight, innerBottomLeft, innerBottomRight;
        public Vector3 outerTopLeft, outerTopRight, outerBottomLeft, outerBottomRight;
    }
}

