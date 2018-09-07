using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Controller3D : MonoBehaviour {

    public LayerMask collisionMask;
    public int xAxisRayCount = 4;
    public int yAxisRayCount = 8;
    public int zAxisRayCount = 4;
    public RaycastDebugger DebugData;

    BoxCollider collider;
    RaycastOrigin raycastOrigin;
    float xAxisRaySpacing, yAxisRaySpacing, zAxisRaySpacing;


    const float SKIN_WIDTH = 0.015f;

    // Use this for initialization
    void Start () {
        collider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
        CalculateRaycastOrigin();
    }

    public void Move(Vector3 velocity)
    {
        YCollision(ref velocity);
        XCollision(ref velocity);
        ZCollision(ref velocity);

        transform.Translate(velocity);
        UpdateRaycastOrigin(velocity);
    }

    void YCollision(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SKIN_WIDTH;

        for (int i = 0; i < xAxisRayCount; i++)
        {
            //Casting Rays in the Y axis
            Vector3 rayOrigin = (directionY == -1) ? raycastOrigin.innerBottomLeft : raycastOrigin.innerTopLeft;
            rayOrigin += Vector3.right * (xAxisRaySpacing * i + velocity.x + velocity.z);
            if (DebugData.yAxisRays) Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);

            //Handling collision happening ...
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask))
            {
                velocity.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;
            }
        }
    }

    void XCollision(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SKIN_WIDTH;
        for (int h = 0; h < yAxisRayCount; h++)
        {
            for (int i = 0; i < zAxisRayCount; i++)
            {
                //Casting Rays in the X axis
                Vector3 rayOrigin = (directionX == -1) ? raycastOrigin.innerBottomLeft : raycastOrigin.innerBottomRight;
                rayOrigin += Vector3.forward * (zAxisRaySpacing * i + velocity.z) + Vector3.up * (h * yAxisRaySpacing + velocity.y);
                if (DebugData.xAxisRays) Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLength, Color.red);

                //Handling collision happening ...
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, rayLength, collisionMask))
                {
                    velocity.x = (hit.distance - SKIN_WIDTH) * directionX;
                    rayLength = hit.distance;
                }
            }
        }
    }

    void ZCollision(ref Vector3 velocity)
    {
        float directionZ = Mathf.Sign(velocity.z);
        float rayLength = Mathf.Abs(velocity.z) + SKIN_WIDTH;
        for (int h = 0; h < yAxisRayCount; h++)
        {
            for (int i = 0; i < xAxisRayCount; i++)
            {
                //Casting Rays in the Z axis
                Vector3 rayOrigin = (directionZ == -1) ? raycastOrigin.innerBottomLeft : raycastOrigin.outerBottomLeft;
                rayOrigin += Vector3.right * (xAxisRaySpacing * i + velocity.x) + Vector3.up * (h * yAxisRaySpacing + velocity.y);
                if (DebugData.zAxisRays) Debug.DrawRay(rayOrigin, Vector3.forward * directionZ * rayLength, Color.red);

                //Handling collision happening ...
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin, Vector3.forward * directionZ, out hit, rayLength, collisionMask))
                {
                    velocity.z = (hit.distance - SKIN_WIDTH) * directionZ;
                    rayLength = hit.distance;
                }
            }
        }
    }

    void CalculateRaycastOrigin()
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

    void UpdateRaycastOrigin(Vector3 velocity)
    {
        raycastOrigin.innerBottomLeft.x += velocity.x; raycastOrigin.innerBottomLeft.y += velocity.y; raycastOrigin.innerBottomLeft.z += velocity.z;
        raycastOrigin.innerBottomRight.x += velocity.x; raycastOrigin.innerBottomRight.y += velocity.y; raycastOrigin.innerBottomRight.z += velocity.z;
        raycastOrigin.innerTopLeft.x += velocity.x; raycastOrigin.innerTopLeft.y += velocity.y; raycastOrigin.innerTopLeft.z += velocity.z;
        raycastOrigin.innerTopRight.x += velocity.x; raycastOrigin.innerTopRight.y += velocity.y; raycastOrigin.innerTopRight.z += velocity.z;
        raycastOrigin.outerBottomLeft.x += velocity.x; raycastOrigin.outerBottomLeft.y += velocity.y; raycastOrigin.outerBottomLeft.z += velocity.z;
        raycastOrigin.outerBottomRight.x += velocity.x; raycastOrigin.outerBottomRight.y += velocity.y; raycastOrigin.outerBottomRight.z += velocity.z;
        raycastOrigin.outerTopLeft.x += velocity.x; raycastOrigin.outerTopLeft.y += velocity.y; raycastOrigin.outerTopLeft.z += velocity.z;
        raycastOrigin.outerTopRight.x += velocity.x; raycastOrigin.outerTopRight.y += velocity.y; raycastOrigin.outerTopRight.z += velocity.z;
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SKIN_WIDTH * -2);
        xAxisRayCount = Mathf.Clamp(xAxisRayCount, 2, int.MaxValue);
        yAxisRayCount = Mathf.Clamp(yAxisRayCount, 2, int.MaxValue);
        zAxisRayCount = Mathf.Clamp(zAxisRayCount, 2, int.MaxValue);

        xAxisRaySpacing = bounds.size.x / (xAxisRayCount - 1);
        yAxisRaySpacing = bounds.size.y / (yAxisRayCount - 1);
        zAxisRaySpacing = bounds.size.z / (zAxisRayCount - 1);
    }

    struct RaycastOrigin
    {
        public Vector3 innerTopLeft, innerTopRight, innerBottomLeft, innerBottomRight;
        public Vector3 outerTopLeft, outerTopRight, outerBottomLeft, outerBottomRight;
    }

    [System.Serializable]
    public struct RaycastDebugger
    {
        public bool xAxisRays, yAxisRays, zAxisRays;
    }
}

