// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleSonarShader_Parent : MonoBehaviour
{

    // All the renderers that will have the sonar data sent to their shaders.
    private Renderer[] ObjectRenderers;

    // Throwaway values to set position to at the start.
    private static readonly Vector4 GarbagePosition = new Vector4(-5000, -5000, -5000, -5000);

    // The number of rings that can be rendered at once.
    // Must be the samve value as the array size in the shader.
    private static int QueueSize = 100;

    // Queue of start positions of sonar rings.
    // The xyz values hold the xyz of position.
    // The w value holds the time that position was started.
    private Queue<Vector4> positionsQueue = new Queue<Vector4>(QueueSize);

    // Queue of intensity values for each ring.
    // These are kept in the same order as the positionsQueue.
    private Queue<float> intensityQueue = new Queue<float>(QueueSize);

    private Queue<Vector4> colorQueue = new Queue<Vector4>(QueueSize);
    private Color ringColor = Color.white;


    private void Start()
    {
        // Get renderers that will have effect applied to them
        ObjectRenderers = GetComponentsInChildren<Renderer>();

        // Fill queues with starting values that are garbage values
        for (int i = 0; i < QueueSize; i++)
        {
            positionsQueue.Enqueue(GarbagePosition);
            intensityQueue.Enqueue(-5000f);
            colorQueue.Enqueue(GarbagePosition);
        }
    }

    public void StartSonarRing(Vector4 position, float intensity, int type)
    {
        position.w = Time.timeSinceLevelLoad;
        positionsQueue.Dequeue();
        positionsQueue.Enqueue(position);

        intensityQueue.Dequeue();
        intensityQueue.Enqueue(intensity);

        ringColor = type == 0 ? Color.white : Color.red; // 일반: 0, 몬스터: 1

        colorQueue.Dequeue();
        colorQueue.Enqueue(ringColor);


        foreach (Renderer r in ObjectRenderers)
        {
            if (r)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                r.GetPropertyBlock(block);

                block.SetVectorArray("_hitPts", positionsQueue.ToArray());
                block.SetFloatArray("_Intensity", intensityQueue.ToArray());
                //block.SetVectorArray("_RingColor", colorQueue.ToArray());
                block.SetVectorArray("_RingColor", colorQueue.Select(c => (Vector4)c).ToArray());

                block.SetInt("_Type", type); // 추가적인 구분 정보 전달
                r.SetPropertyBlock(block);
            }
        }
    }

    /// <summary>
    /// Starts a sonar ring from this position with the given intensity.
    /// </summary>
    //public void StartSonarRing(Vector4 position, float intensity, bool monster)
    //{
    //    // Put values into the queue
    //    position.w = Time.timeSinceLevelLoad;
    //    positionsQueue.Dequeue();
    //    positionsQueue.Enqueue(position);

    //    intensityQueue.Dequeue();
    //    intensityQueue.Enqueue(intensity);

    //    //colorQueue.Dequeue();
    //    //colorQueue.Enqueue(color);

    //    if (monster)
    //    {
    //        ringColor = Color.red;

    //        // Send updated queues to the shaders
    //        foreach (Renderer r in ObjectRenderers)
    //        {
    //            if (r)
    //            {
    //                r.material.SetVectorArray("_hitPtsM", positionsQueue.ToArray());
    //                r.material.SetFloatArray("_IntensityM", intensityQueue.ToArray());
    //                r.material.SetColor("_RingColorM", ringColor);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        ringColor = Color.white;
    //        foreach (Renderer r in ObjectRenderers)
    //        {
    //            if (r)
    //            {
    //                r.material.SetVectorArray("_hitPts", positionsQueue.ToArray());
    //                r.material.SetFloatArray("_Intensity", intensityQueue.ToArray());
    //                r.material.SetColor("_RingColor", ringColor);
    //            }
    //        }
    //    }

    //}

}
