using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkyboxBlender))]
public class SkyboxBlenderInspector : Editor
{
    SerializedProperty skyboxMaterials,
    makeFirstMaterialSkybox,
    blendSpeed,
    timeToWait,
    loop,
    rotateTo,
    rotationSpeed;

    void OnEnable(){
        skyboxMaterials = serializedObject.FindProperty("skyboxMaterials");
        makeFirstMaterialSkybox = serializedObject.FindProperty("makeFirstMaterialSkybox");

        blendSpeed = serializedObject.FindProperty("blendSpeed");
        timeToWait = serializedObject.FindProperty("timeToWait");
        loop = serializedObject.FindProperty("loop");

        rotateTo = serializedObject.FindProperty("rotateTo");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
    }

    public override void OnInspectorGUI(){
        var button = GUILayout.Button("Click for more tools");
        if (button) Application.OpenURL("https://assetstore.unity.com/publishers/39163");
        EditorGUILayout.Space(5);

        SkyboxBlender script = (SkyboxBlender) target;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Material Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(skyboxMaterials, new GUIContent("Skybox Materials", "The materials you want to blend to linearly"));
        EditorGUILayout.PropertyField(makeFirstMaterialSkybox, new GUIContent("Make First Material Skybox", "Checking this will instantly make the first material your current skybox"));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Blend Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(blendSpeed, new GUIContent("Blend Speed", "The speed of the blending between the skyboxes"));
        EditorGUILayout.PropertyField(timeToWait, new GUIContent("Time To Wait", "The time to wait before blending the next skybox material"));
        EditorGUILayout.PropertyField(loop, new GUIContent("Loop", "If enabled will loop the materials list"));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Rotations Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(rotateTo, new GUIContent("Rotate To", "Value of rotation - 360 is a full turn"));
        EditorGUILayout.PropertyField(rotationSpeed, new GUIContent("Rotation Speed", "The speed of the skyboxes rotating"));

        serializedObject.ApplyModifiedProperties();
    }
}
