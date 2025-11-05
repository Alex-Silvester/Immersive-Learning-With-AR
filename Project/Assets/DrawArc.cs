using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    /// <summary>
    /// Add this attribute to a float property to make it a logarithmic range slider
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LogRangeAttribute : PropertyAttribute
    {
        public float min;
        public float center;
        public float max;

        /// <summary>
        /// Creates a float property slider with a logarithmic 
        /// </summary>
        /// <param name="min">Minimum range value</param>
        /// <param name="center">Value at the center of the range slider</param>
        /// <param name="max">Maximum range value</param>
        public LogRangeAttribute(float min, float center, float max)
        {
            this.min = min;
            this.center = center;
            this.max = max;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LogRangeAttribute))]
    public class LogRangePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LogRangeAttribute logRangeAttribute = (LogRangeAttribute)attribute;
            LogRangeConverter rangeConverter = new LogRangeConverter(logRangeAttribute.min, logRangeAttribute.center, logRangeAttribute.max);

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float value = rangeConverter.ToNormalized(property.floatValue);
            value = GUI.HorizontalSlider(position, value, 0, 1);

            property.floatValue = rangeConverter.ToRange(value);
            EditorGUI.EndProperty();
        }
    }
#endif

    /// <summary>
    /// Tool to convert a range from 0-1 into a logarithmic range with a user defined center
    /// </summary>
    public struct LogRangeConverter
    {
        public readonly float minValue;
        public readonly float maxValue;

        private readonly float a;
        private readonly float b;
        private readonly float c;

        /// <summary>
        /// Set up a scaler
        /// </summary>
        /// <param name="minValue">Value for t = 0</param>
        /// <param name="centerValue">Value for t = 0.5</param>
        /// <param name="maxValue">Value for t = 1.0</param>
        public LogRangeConverter(float minValue, float centerValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;

            a = (minValue * maxValue - (centerValue * centerValue)) / (minValue - 2 * centerValue + maxValue);
            b = ((centerValue - minValue) * (centerValue - minValue)) / (minValue - 2 * centerValue + maxValue);
            c = 2 * Mathf.Log((maxValue - centerValue) / (centerValue - minValue));
        }

        /// <summary>
        /// Convers the value in range 0 - 1 to the value in range of minValue - maxValue
        /// </summary>
        public float ToRange(float value01)
        {
            return a + b * Mathf.Exp(c * value01);
        }

        /// <summary>
        /// Converts the value in range min-max to a value between 0 and 1 that can be used for a slider
        /// </summary>
        public float ToNormalized(float rangeValue)
        {
            return Mathf.Log((rangeValue - a) / b) / c;
        }
    }
}

public class DrawArc : MonoBehaviour
{
    [SerializeField]
    float s = 10;
    [SerializeField]
    Vector2 u = new Vector2(1,1);
    [SerializeField]
    Vector2 v = new Vector2(1,-1);
    [SerializeField]
    float a = 1;
    [SerializeField]
    float t = 0;

    [Space(20)]
    [SerializeField][Tools.LogRange(0.01f, 0.2f, 1)]
    float time_step = 0.2f;

    [Space(20)]
    [SerializeField]
    GameObject cylinder_prefab;

    List<GameObject> line_segments = new List<GameObject>();
    Vector3 gravity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gravity = new Vector3(0, -a, 0);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject line_segment in line_segments)
        {
            Destroy(line_segment);
        }

        line_segments.Clear();
        PlotTrajectory(gameObject.transform.position, u, time_step, t);
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
    {
        return start + (startVelocity * time) + (0.5f * gravity * time * time);
    }

    public void PlotTrajectory(Vector3 start, Vector3 startVelocity, float timestep, float maxTime)
    {
        Vector3 prev = start;
        for (int i = 1; ; i++)
        {
            float current_time = timestep * i;

            if (current_time > maxTime) 
            {
                break; 
            }

            Vector3 pos = PlotTrajectoryAtTime(start, startVelocity, current_time);

            if(pos.y < 0)
            {
                break;
            }

            line_segments.Add(Instantiate(cylinder_prefab));
            line_segments[^1].transform.position = prev;
            line_segments[^1].transform.localScale = new Vector3(0.01f, 0.03f, 0.01f);
            line_segments[^1].transform.LookAt(pos);
            line_segments[^1].transform.Rotate(90, 0, 0);

            prev = pos;
        }
    }

    public void setAcceleration(Slider acc)
    {
        gravity = new Vector3(0, -acc.value, 0);
    }

    public void setTime(Slider time)
    {
        t = time.value;
    }
}
