using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

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
        PlotTrajectory(gameObject.transform.position, u, 0.4f, t);
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

            line_segments.Add(GameObject.CreatePrimitive(PrimitiveType.Cylinder));
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
