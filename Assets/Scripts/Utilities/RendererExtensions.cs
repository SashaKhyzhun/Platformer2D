
using UnityEngine;

public static class RendererExtensions
{
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        camera.orthographicSize += 3f;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        bool test = GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        camera.orthographicSize -= 3f;
        return test;
    }
}