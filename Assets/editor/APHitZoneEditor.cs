/* Copyright (c) 2014 Advanced Platformer 2D */

using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(APHitZone))]
[CanEditMultipleObjects]
public class APHitZoneEditor : Editor
{
	public void OnSceneGUI () 
	{
		APHitZone oHitZone = (APHitZone)target;
		
		// simply draw hit zone
		if (oHitZone.m_active && oHitZone.gameObject.activeInHierarchy)
		{
			Vector3 pointPos = oHitZone.transform.position;
			Color color = Color.green;
			color.a = 0.5f;
			Handles.color = color;
			var fmh_22_54_638633061706570611 = Quaternion.identity; Vector3 newPos = Handles.FreeMoveHandle(pointPos, oHitZone.m_radius * 2f, Vector3.zero, Handles.SphereHandleCap);
			if (newPos != pointPos)
			{
				Undo.RecordObject(oHitZone.transform, "Move Hit Zone");
				oHitZone.transform.position = newPos;
				
				// mark object as dirty
				EditorUtility.SetDirty(oHitZone);
			}
		}
	}
}
