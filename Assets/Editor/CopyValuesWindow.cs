using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations.Rigging;

public class CopyValuesWindow : EditorWindow
{
	//public string importCharacterName = "CharacterEXP2";

	GameObject copyFrom;
	GameObject copyTo;

	[MenuItem("Window/CopyValuesWindow")]
	public static void ShowWindow()
	{
		GetWindow<CopyValuesWindow>("CopyValuesWindow");
	}

	void OnGUI()
	{
		GUILayout.Label("COPY CHARACTER VALUES", EditorStyles.boldLabel);

		//color = EditorGUILayout.ColorField("Color", color);

		//importCharacterName = EditorGUILayout.TextField("Import Character Name", importCharacterName);

		//GUILayoutOption options = new GUILayoutOption();

		//EditorGUILayout.BeginHorizontal();
		copyFrom = (GameObject)EditorGUILayout.ObjectField("Copy From", copyFrom, typeof(GameObject), true);
		copyTo = (GameObject)EditorGUILayout.ObjectField("Copy To", copyTo, typeof(GameObject), true);
		//EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Copy Values"))
		{
			AddValues();
		}
	}

	void AddValues()
	{
		/*
		if (Selection.activeGameObject)
		{
			if (Selection.activeGameObject.name == "LeftLegMove")
			{
				//Selection.activeGameObject.GetComponent<TwoBoneIKConstraint>().tip
				serializedObject.FindProperty("value");
				EditorGUI.PropertyField(textFieldRect, _value);
			}
		}
		*/


		//GameObject firstObj = Selection.gameObjects[0];
		//GameObject secondObj = Selection.gameObjects[1];

		/*
		if (firstObj.name == "CharacterEXP2")
        {
			firstObj = Selection.gameObjects[1];
			secondObj = Selection.gameObjects[0];
		}
		*/

		WalkingManager walkingManager = copyTo.GetComponent<WalkingManager>();

		if (!walkingManager)
        {
			walkingManager = copyTo.AddComponent<WalkingManager>();
        }

		walkingManager.leftFoot = ExtensiveFind(copyTo.transform, "LeftLegTarget").transform;
		walkingManager.rightFoot = ExtensiveFind(copyTo.transform, "RightLegTarget").transform;

		walkingManager.spinePart = ExtensiveFind(copyTo.transform, "spine.001").transform;

		RagDollParts ragDollParts = copyTo.GetComponent<RagDollParts>();

		if (!ragDollParts)
		{
			ragDollParts = copyTo.AddComponent<RagDollParts>();
		}

		ragDollParts.legGameObjects = new List<GameObject>() {
			ExtensiveFind(copyTo.transform, "leftTopLeg"),
			ExtensiveFind(copyTo.transform, "leftKneeLeg"),
			ExtensiveFind(copyTo.transform, "rightTopLeg"),
			ExtensiveFind(copyTo.transform, "rightKneeLeg")
		};

		ragDollParts.armGameObjects = new List<GameObject>() {
			ExtensiveFind(copyTo.transform, "upper_arm.L"),
			ExtensiveFind(copyTo.transform, "forearm.L"),
			ExtensiveFind(copyTo.transform, "upper_arm.R"),
			ExtensiveFind(copyTo.transform, "forearm.R")
		};

		ragDollParts.bodyGameObjects = new List<GameObject>() {
			ExtensiveFind(copyTo.transform, "spine.002"),
			ExtensiveFind(copyTo.transform, "spine")
		};

		ragDollParts.headGameObjects = new List<GameObject>() {
			ExtensiveFind(copyTo.transform, "spine.006")
		};

		ragDollParts.body = ExtensiveFind(copyTo.transform, "spine").transform;

		ragDollParts.legsDisabled = true;


		if (!copyFrom || !copyTo)
        {
			Debug.LogError("Copy objects not set");
			return;
        }

		//Debug.Log("First: " + copyFrom.name);
		//Debug.Log("Second: " + copyTo.name);

		GameObject A = ExtensiveFind(copyFrom.transform, "spine");
		GameObject B = ExtensiveFind(copyTo.transform, "spine");
		

		CopyRigidBodies(A, B);
		CopyBoxColliders(A, B);

		GameObject C = B;
		A = ExtensiveFind(copyFrom.transform, "spine.002");
		B = ExtensiveFind(copyTo.transform, "spine.002");

		CopyRigidBodies(A, B);
		CopyBoxColliders(A, B);
		CopyCharacterJoints(A, B, C);

		C = B;
		A = ExtensiveFind(copyFrom.transform, "spine.006");
		B = ExtensiveFind(copyTo.transform, "spine.006");

		CopyRigidBodies(A, B);
		CopySphereColliders(A, B);
		CopyCharacterJoints(A, B, C);

		A = ExtensiveFind(copyFrom.transform, "upper_arm.R");
		B = ExtensiveFind(copyTo.transform, "upper_arm.R");
		C = ExtensiveFind(copyTo.transform, "spine.002");

		CopyRigidBodies(A, B);
		CopyCapsuleColliders(A, B);
		CopyCharacterJoints(A, B, C);

		C = B;
		A = ExtensiveFind(copyFrom.transform, "forearm.R");
		B = ExtensiveFind(copyTo.transform, "forearm.R");


		CopyRigidBodies(A, B);
		CopyCapsuleColliders(A, B);
		CopyCharacterJoints(A, B, C);

		A = ExtensiveFind(copyFrom.transform, "upper_arm.L");
		B = ExtensiveFind(copyTo.transform, "upper_arm.L");
		C = ExtensiveFind(copyTo.transform, "spine.002");

		CopyRigidBodies(A, B);
		CopyCapsuleColliders(A, B);
		CopyCharacterJoints(A, B, C);

		C = B;
		A = ExtensiveFind(copyFrom.transform, "forearm.L");
		B = ExtensiveFind(copyTo.transform, "forearm.L");

		CopyRigidBodies(A, B);
		CopyCapsuleColliders(A, B);
		CopyCharacterJoints(A, B, C);

		GameObject leftTopLeg = ExtensiveFind(copyTo.transform, "leftTopLeg");
		GameObject rightTopLeg = ExtensiveFind(copyTo.transform, "rightTopLeg");

		leftTopLeg.GetComponent<CharacterJoint>().connectedBody = ExtensiveFind(copyTo.transform, "spine").GetComponent<Rigidbody>();
		rightTopLeg.GetComponent<CharacterJoint>().connectedBody = ExtensiveFind(copyTo.transform, "spine").GetComponent<Rigidbody>();


		

		


	}

	void CopyRigidBodies(GameObject A, GameObject B)
    {
		Rigidbody componentA = A.GetComponent<Rigidbody>();

		if (!componentA)
		{
			return;
		}

		Rigidbody componentB = B.GetComponent<Rigidbody>();

		if (!componentB)
        {
			componentB = B.AddComponent<Rigidbody>();
        }

		componentB.mass = componentA.mass;
		componentB.drag = componentA.drag;
		componentB.angularDrag = componentA.angularDrag;
		componentB.useGravity = componentA.useGravity;
		componentB.isKinematic = componentA.isKinematic;
		//componentB.interpolation = componentA.interpolation;
		componentB.constraints = componentA.constraints;
	}

	void CopyCharacterJoints(GameObject A, GameObject B, GameObject C=null)
	{
		CharacterJoint componentA = A.GetComponent<CharacterJoint>();

		if (!componentA)
		{
			return;
		}

		CharacterJoint componentB = B.GetComponent<CharacterJoint>();

		if (!componentB)
		{
			componentB = B.AddComponent<CharacterJoint>();
		}

		if (C)
        {
			componentB.connectedBody = C.GetComponent<Rigidbody>();
		}
		componentB.highTwistLimit = componentA.highTwistLimit;
		componentB.lowTwistLimit = componentA.lowTwistLimit;
		componentB.projectionAngle = componentA.projectionAngle;
		componentB.projectionDistance = componentA.projectionDistance;
		componentB.swing1Limit = componentA.swing1Limit;
		//componentB.interpolation = componentA.interpolation;
		componentB.swing2Limit = componentA.swing2Limit;
		componentB.swingAxis = componentA.swingAxis;
		componentB.swingLimitSpring = componentA.swingLimitSpring;
		componentB.twistLimitSpring = componentA.twistLimitSpring;
	}

	void CopyCapsuleColliders(GameObject A, GameObject B)
    {
		CapsuleCollider componentA = A.GetComponent<CapsuleCollider>();

		if (!componentA)
		{
			return;
		}

		CapsuleCollider componentB = B.GetComponent<CapsuleCollider>();

		if (!componentB)
		{
			componentB = B.AddComponent<CapsuleCollider>();
		}

		componentB.center = componentA.center;
		componentB.radius = componentA.radius;
		componentB.height = componentA.height;
		componentB.direction = componentA.direction;
	}

	void CopySphereColliders(GameObject A, GameObject B)
	{
		SphereCollider componentA = A.GetComponent<SphereCollider>();

		if (!componentA)
		{
			return;
		}

		SphereCollider componentB = B.GetComponent<SphereCollider>();

		if (!componentB)
		{
			componentB = B.AddComponent<SphereCollider>();
		}

		componentB.center = componentA.center;
		componentB.radius = componentA.radius;
	}

	void CopyBoxColliders(GameObject A, GameObject B)
	{
		BoxCollider componentA = A.GetComponent<BoxCollider>();

		if (!componentA)
		{
			return;
		}

		BoxCollider componentB = B.GetComponent<BoxCollider>();

		if (!componentB)
		{
			componentB = B.AddComponent<BoxCollider>();
		}

		componentB.center = componentA.center;
		componentB.size = componentA.size;
		componentB.isTrigger = componentA.isTrigger;
	}

	GameObject ExtensiveFind(Transform obj, string name)
    {
		if (obj.name == name)
        {
			return obj.gameObject;
        }


		foreach (Transform child in obj)
        {
			GameObject result = ExtensiveFind(child, name);
			if (result)
            {
				return result;
            }
        }
		return null;
    }

}
