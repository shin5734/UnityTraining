﻿using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(EnemyParams))]
[CanEditMultipleObjects]
public class EnemyParamsEditor : CharacterParameterEditor {

    ReorderableList reorderableList;
    SerializedProperty characterSprite;

    EnemyParams setting = null;

    public override void OnEnable () {
        base.OnEnable();
        characterSprite = serializedObject.FindProperty("characterSprite");
        var prop = serializedObject.FindProperty ("routine");
        reorderableList = new ReorderableList (serializedObject, prop);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 9;
        reorderableList.drawElementCallback =
        (rect, index, isActive, isFocused) => {
            var element = prop.GetArrayElementAtIndex (index);
            rect.height -= 4;
            rect.y += 2;
            EditorGUI.PropertyField (rect, element);
        };

        var defaultColor = GUI.backgroundColor;

        reorderableList.drawHeaderCallback = (rect) =>
        EditorGUI.LabelField (rect, prop.displayName);
    }

    // ラベル設定
    public static readonly GUIContent spriteLabel = EditorGUIUtility.TrTextContent("Sprite", "The Sprite to render", (Texture) null);

    public override void OnInspectorGUI() {
        serializedObject.Update ();

        setting = (EnemyParams) target;

        EditorGUILayout.PropertyField(this.actorName);
        EditorGUILayout.PropertyField(this.characterSprite, spriteLabel);
        EditorGUILayout.IntSlider(this.hp, MinHp, MaxHp);
        EditorGUILayout.IntSlider(this.atk, MinParam, MaxParam);
        EditorGUILayout.IntSlider(this.dfc, MinParam, MaxParam);
        int totalparam = setting.hp + setting.atk + setting.dfc;
        EditorGUILayout.LabelField("総合戦闘力", totalparam.ToString());
        
        if (setting.needRefresh) {
            reorderableList.index = setting.routineIndex;
            setting.needRefresh = false;
        }
        reorderableList.DoLayoutList ();

        serializedObject.ApplyModifiedProperties ();
    }
}
