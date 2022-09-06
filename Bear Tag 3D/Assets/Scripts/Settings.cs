using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings")]
public class Settings : ScriptableObject
{
    public static float MoveSpeed => Instance.moveSpeed;
    public static float WalkSpeed => Instance.walkSpeed;
    public static float RotateVelocity => Instance.rotateVelocity;
    public static Sensivity VerticalCamera => Instance.verticalSensivity;
    public static Sensivity HorizontalCamera => Instance.horizontalSensivity;
    public static Ability Dash => Instance.dash;
    public static float FlickeringTime => Instance.duration;
    public static Material FlickeringMaterial => Instance.material;

    [Header("Controls")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float rotateVelocity;
    [SerializeField]
    private Sensivity verticalSensivity;
    [SerializeField]
    private Sensivity horizontalSensivity;
    [SerializeField]
    private Ability dash;

    [Header("Tagging")]
    [SerializeField]
    private float duration;
    [SerializeField]
    private Material material;

    private static Settings Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<Settings>("Settings");

            return _instance;
        }
    }

    private static Settings _instance;

    [System.Serializable]
    public struct Sensivity
    {
        [Range(0, 100)]
        public float speed;
        [Range(0, 360)]
        public float extremum;

        public float Min => -extremum;
        public float Max => extremum;
    }

    [System.Serializable]
    public struct Ability
    {
        [Range(0, 100)]
        public float duration;
        [Range(0, 100)]
        public float cooldown;
        [Range(0, 100)]
        public float distance;
    }
}
