using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GfanMath 
{
	//-----------------------------------------------------------------------
	public enum PlaneSide
    {
        NO_SIDE,
        POSITIVE_SIDE,
        NEGATIVE_SIDE,
        BOTH_SIDE
    };
	//-----------------------------------------------------------------------
	
	//----------------------------Plane Section-------------------------------------------
	public static PlaneSide GetSide(Plane plane, Vector3 rkPoint)
	{
		float distance = plane.GetDistanceToPoint(rkPoint);
		
		if ( distance < 0.0f )
			return PlaneSide.NEGATIVE_SIDE;

		if ( distance > 0.0f )
			return PlaneSide.POSITIVE_SIDE;

		return PlaneSide.NO_SIDE;
	}
	
	//-----------------------Frustum Section--------------------------------
	
	
	//-----------------------------------------------------------------------
   	public static bool Intersects(Ray ray, Plane plane, out float distance)
    {
		distance = float.NegativeInfinity;
		
		float denom = Vector3.Dot(plane.normal, ray.direction);
		if (Mathf.Abs(denom) < Mathf.Epsilon)
		{
			return false;
		}
		else
		{
			float nom = Vector3.Dot(plane.normal, ray.origin) + plane.distance;
            float t = -(nom / denom);
			
			distance = t;
            return t >= 0;
		}
		
		return false;
    }
	
	//-----------------------------------------------------------------------
    public static bool Intersects(Ray ray, List<Plane> planeList, bool normalIsOutside, out float distance)
    {
		distance = float.NegativeInfinity;
		
		bool allInside = true;
		
		bool ret = false;
		float retDist = 0.0f;
		bool end = false;
		float endDist = 0.0f;
		
		// derive side
		// NB we don't pass directly since that would require Plane::Side in 
		// interface, which results in recursive includes since Math is so fundamental
		PlaneSide outside = normalIsOutside ? PlaneSide.POSITIVE_SIDE : PlaneSide.NEGATIVE_SIDE;
		
		foreach (Plane plane in planeList)
		{
			// is origin outside?
			if (GetSide(plane, ray.origin) == outside)
			{
				allInside = false;
				// Test single plane
				if (Intersects(ray, plane, out distance))
				{
					// Ok, we intersected
					ret = true;
					// Use the most distant result since convex volume
					retDist = Mathf.Max(retDist, distance);
				}
				else
				{
					distance = 0.0f;
					return false;
				}
			}
			else
			{
				if (Intersects(ray, plane, out distance))
				{
					if (!end)
					{
						end = true;
						endDist = distance;
					}
					else
					{
						endDist = Mathf.Min(distance, endDist);
					}
				}
			}
		}
		
		if (allInside)
		{
			// Intersecting at 0 distance since inside the volume!
			ret = true;
			retDist = 0.0f;
			
			distance = retDist;
			return ret;
		}
		
		if (end)
		{
			if (endDist < retDist)
			{
				ret = false;
				distance = float.NegativeInfinity;
				return ret;
			}
		}
		
		return ret;
    }
	
	//-----------------------------------------------------------------------
    public static void ToRotationMatrix (Quaternion quat, out Matrix4x4 kRot)
    {
		kRot = Matrix4x4.identity;
		
		float x = quat.x;
		float y = quat.y;
		float z = quat.z;
		float w = quat.w;
		
        float fTx  = x+x;
        float fTy  = y+y;
        float fTz  = z+z;
		
        float fTwx = fTx*w;
        float fTwy = fTy*w;
        float fTwz = fTz*w;
		
        float fTxx = fTx*x;
        float fTxy = fTy*x;
        float fTxz = fTz*x;
		
        float fTyy = fTy*y;
        float fTyz = fTz*y;
        float fTzz = fTz*z;
		
        kRot.m00 = 1.0f-(fTyy+fTzz);
        kRot.m01 = fTxy-fTwz;
        kRot.m02 = fTxz+fTwy;
		
        kRot.m10 = fTxy+fTwz;
        kRot.m11 = 1.0f-(fTxx+fTzz);
        kRot.m12 = fTyz-fTwx;
		
        kRot.m20 = fTxz-fTwy;
        kRot.m21 = fTyz+fTwx;
        kRot.m22 = 1.0f-(fTxx+fTyy);
    }
	
	public static void CalcProjectionParameters(Camera cam, out float left, out float right, out float bottom, out float top)
	{
		left = 0.0f;
		right = 0.0f;
		bottom = 0.0f;
		top = 0.0f;
		// Calculate general projection parameters
		// cam.projectionMatrix;
		// cam.fov;
		
		// if (cam == PT_PERSPECTIVE)
			// {
			// 	Radian thetaY (mFOVy * 0.5f);
			// 	Real tanThetaY = Math::Tan(thetaY);
			// 	Real tanThetaX = tanThetaY * mAspect;
			// 
			// 	Real nearFocal = mNearDist / mFocalLength;
			// 	Real nearOffsetX = mFrustumOffset.x * nearFocal;
			// 	Real nearOffsetY = mFrustumOffset.y * nearFocal;
			// 	Real half_w = tanThetaX * mNearDist;
			// 	Real half_h = tanThetaY * mNearDist;
			// 
			// 	left   = - half_w + nearOffsetX;
			// 	right  = + half_w + nearOffsetX;
			// 	bottom = - half_h + nearOffsetY;
			// 	top    = + half_h + nearOffsetY;
			// 
			// 	mLeft = left;
			// 	mRight = right;
			// 	mTop = top;
			// 	mBottom = bottom;
			// }
			// else
			// {
			// 	// Unknown how to apply frustum offset to orthographic camera, just ignore here
			// 	Real half_w = getOrthoWindowWidth() * 0.5f;
			// 	Real half_h = getOrthoWindowHeight() * 0.5f;
			// 
			// 	left   = - half_w;
			// 	right  = + half_w;
			// 	bottom = - half_h;
			// 	top    = + half_h;
			// 
			// 	mLeft = left;
			// 	mRight = right;
			// 	mTop = top;
			// 	mBottom = bottom;
			// }
	}
	
	public enum FrustumPlane
    {
        FRUSTUM_PLANE_NEAR   = 0,
        FRUSTUM_PLANE_FAR    = 1,
        FRUSTUM_PLANE_LEFT   = 2,
        FRUSTUM_PLANE_RIGHT  = 3,
        FRUSTUM_PLANE_TOP    = 4,
        FRUSTUM_PLANE_BOTTOM = 5
    };
	public static void CalcFrustumPlanes(Camera cam, out List<Plane> planes)
	{
		planes = new List<Plane>();
		planes.Clear();
		
		// -------------------------
		// Update the frustum planes
		// -------------------------
		Matrix4x4 viewMatrix = MakeViewMatrix(cam.transform.position, cam.transform.rotation);
		Matrix4x4 combo = cam.projectionMatrix * viewMatrix;
		
		Vector3 normal = Vector3.zero;
		float distance = float.NegativeInfinity;
		
		normal.x = combo.m30 + combo.m00;
		normal.y = combo.m31 + combo.m01;
		normal.z = combo.m32 + combo.m02;
		distance = combo.m33 + combo.m03;
		
		Plane plane = new Plane();
		plane.normal = normal;
		plane.distance = distance;
		planes[(int)FrustumPlane.FRUSTUM_PLANE_LEFT] = plane;
		
		normal.x = combo.m30 - combo.m00;
		normal.y = combo.m31 - combo.m01;
		normal.z = combo.m32 - combo.m02;
		distance = combo.m33 - combo.m03;
		plane = new Plane();
		plane.normal = normal;
		plane.distance = distance;
		planes[(int)FrustumPlane.FRUSTUM_PLANE_RIGHT] = plane;
		
		normal.x = combo.m30 - combo.m10;
		normal.y = combo.m31 - combo.m11;
		normal.z = combo.m32 - combo.m12;
		distance = combo.m33 - combo.m13;
		plane = new Plane();
		plane.normal = normal;
		plane.distance = distance;
		planes[(int)FrustumPlane.FRUSTUM_PLANE_TOP] = plane;
		
		normal.x = combo.m30 + combo.m10;
		normal.y = combo.m31 + combo.m11;
		normal.z = combo.m32 + combo.m12;
		distance = combo.m33 + combo.m13;
		plane = new Plane();
		plane.normal = normal;
		plane.distance = distance;
		planes[(int)FrustumPlane.FRUSTUM_PLANE_BOTTOM] = plane;
		
		normal.x = combo.m30 + combo.m20;
		normal.y = combo.m31 + combo.m21;
		normal.z = combo.m32 + combo.m22;
		distance = combo.m33 + combo.m23;
		plane = new Plane();
		plane.normal = normal;
		plane.distance = distance;
		planes[(int)FrustumPlane.FRUSTUM_PLANE_NEAR] = plane;
		
		normal.x = combo.m30 - combo.m20;
		normal.y = combo.m31 - combo.m21;
		normal.z = combo.m32 - combo.m22;
		distance = combo.m33 - combo.m23;
		plane = new Plane();
		plane.normal = normal;
		plane.distance = distance;
		planes[(int)FrustumPlane.FRUSTUM_PLANE_FAR] = plane;
		
		// Renormalise any normals which were not unit length
		for(int i = 0; i < 6; i++ ) 
		{
			plane = planes[i];
			float length = plane.normal.magnitude;
			plane.normal.Normalize();
			plane.distance /= length;
			
			planes[i] = plane;
		}
	}
	
	public static Matrix4x4 MakeProjMatrix(Vector3 position, Quaternion orientation)
	{
		Matrix4x4 projMatrix;
		
		projMatrix = Matrix4x4.identity;
		return projMatrix;
	}
	
	public static Matrix4x4 MakeViewMatrix(Vector3 position, Quaternion orientation)
	{
		Matrix4x4 viewMatrix;
		
		// View matrix is:
		//
		//  [ Lx  Uy  Dz  Tx  ]
		//  [ Lx  Uy  Dz  Ty  ]
		//  [ Lx  Uy  Dz  Tz  ]
		//  [ 0   0   0   1   ]
		//
		// Where T = -(Transposed(Rot) * Pos)
		
		// This is most efficiently done using 3x3 Matrices
		Matrix4x4 rotMatrix;
		ToRotationMatrix(orientation, out rotMatrix);
		
		// Make the translation relative to new axes
		Matrix4x4 rotMatrixT = rotMatrix.transpose;
		Vector3 trans = -rotMatrixT.MultiplyPoint(position);
		
		// Make final matrix
		viewMatrix = Matrix4x4.identity;
		viewMatrix = rotMatrixT; // fills upper 3x3
		viewMatrix.m03 = trans.x;
		viewMatrix.m13 = trans.y;
		viewMatrix.m23 = trans.z;
		
		return viewMatrix;
	}
}