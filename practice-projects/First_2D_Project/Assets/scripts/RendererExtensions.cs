using UnityEngine;

//to check whether an object's renderer is visible by the camera or not. This is a C# extension, not a class or a script.
//to be called on the leftmost object of the infinite layer
public static class RendererExtensions {

	public static bool IsVisibleFrom(this Renderer renderer, Camera camera) {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes (camera);
		return GeometryUtility.TestPlanesAABB (planes, renderer.bounds);
	}
}