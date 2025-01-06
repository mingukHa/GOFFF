// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class SimpleSonarShader_Parent : MonoBehaviourPun
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

    private Vector4[] hitPtsVec = new Vector4[QueueSize];
    private Vector4[] ringColorsVec = new Vector4[QueueSize];

    private double sceneStartTimePhoton;
    private float timeSinceSceneLoadPhoton;
    private MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        sceneStartTimePhoton = PhotonNetwork.Time;
        // Get renderers that will have effect applied to them
        ObjectRenderers = GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        // Fill queues with starting values that are garbage values
        for (int i = 0; i < QueueSize; i++)
        {
            positionsQueue.Enqueue(GarbagePosition);
            intensityQueue.Enqueue(-5000f);
            colorQueue.Enqueue(GarbagePosition);
        }
    }

    private void Update()
    {
        timeSinceSceneLoadPhoton = (float)(PhotonNetwork.Time - sceneStartTimePhoton);
        // r로 전달이 안됨 다른방법을 써야하나? 코루틴을 써야할것 같음
        foreach (Renderer r in ObjectRenderers)
        {
            if (r)
            {
                //r.material.SetFloat("_RingTime", (float)timeSinceSceneLoadPhoton);
                r.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat("_RingTime", timeSinceSceneLoadPhoton);
                r.SetPropertyBlock(propertyBlock);
            }
        }
    }

    [PunRPC]
    public void UpdateSonarMaterial(float[] hitPts, float[] intensities, float[] ringColor)
    {
        foreach (Renderer r in ObjectRenderers)
        {
            if (r)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                r.GetPropertyBlock(block);

                // 받은 float[] 배열을 다시 Vector4로 변환
                //Vector4[] hitPtsVec = new Vector4[hitPts.Length / 4];
                for (int i = 0; i < hitPtsVec.Length; i++)
                {
                    hitPtsVec[i] = new Vector4(hitPts[i * 4], hitPts[i * 4 + 1], hitPts[i * 4 + 2], hitPts[i * 4 + 3]);
                }
                //Vector4[] ringColorsVec = new Vector4[ringColor.Length / 4];
                for (int i = 0; i < ringColorsVec.Length; i++)
                {
                    ringColorsVec[i] = new Vector4(ringColor[i * 4], ringColor[i * 4 + 1], ringColor[i * 4 + 2], ringColor[i * 4 + 3]);
                }

                block.SetVectorArray("_hitPts", hitPtsVec);
                block.SetFloatArray("_Intensity", intensities);
                block.SetVectorArray("_RingColor", ringColorsVec);

                r.SetPropertyBlock(block);
                //r.material.SetVectorArray("_hitPts", hitPtsVec);
                //r.material.SetFloatArray("_Intensity", intensities);
                //r.material.SetVectorArray("_RingColor", ringColorsVec);

            }
        }
    }

    public void StartSonarRing(Vector4 position, float intensity, int type)
    {
        Debug.Log("충돌됐음");

        //position.w = Time.timeSinceLevelLoad;
        //position.w = (float)PhotonNetwork.Time;
        Debug.Log(Time.timeSinceLevelLoad);
        Debug.Log(PhotonNetwork.Time);
        double timeSinceSceneLoadPhoton = PhotonNetwork.Time - sceneStartTimePhoton;
        Debug.Log("Photon 서버 기준 씬 로드 시점의 경과 시간: " + timeSinceSceneLoadPhoton);

        position.w = (float)timeSinceSceneLoadPhoton;
        positionsQueue.Dequeue();
        positionsQueue.Enqueue(position);

        intensityQueue.Dequeue();
        intensityQueue.Enqueue(intensity);

        ringColor = type == 0 ? Color.white : Color.red; // 일반: 0, 몬스터: 1

        colorQueue.Dequeue();
        colorQueue.Enqueue(ringColor);

        // Vector4를 float[] 배열로 변환
        float[] hitPts = positionsQueue.SelectMany(v => new float[] { v.x, v.y, v.z, v.w }).ToArray();
        float[] intensities = intensityQueue.ToArray();
        float[] ringColors = colorQueue.SelectMany(c => new float[] { c.x, c.y, c.z, c.w }).ToArray();

        photonView.RPC("UpdateSonarMaterial", RpcTarget.All, hitPts, intensities, ringColors);
    }

    //[PunRPC]
    //public void StartSonarRingRPC(Vector4 position, float intensity, int type)
    //{
    //    position.w = Time.timeSinceLevelLoad;
    //    positionsQueue.Dequeue();
    //    positionsQueue.Enqueue(position);

    //    intensityQueue.Dequeue();
    //    intensityQueue.Enqueue(intensity);

    //    ringColor = type == 0 ? Color.white : Color.red; // 일반: 0, 몬스터: 1

    //    colorQueue.Dequeue();
    //    colorQueue.Enqueue(ringColor);


    //    foreach (Renderer r in ObjectRenderers)
    //    {
    //        if (r)
    //        {
    //            MaterialPropertyBlock block = new MaterialPropertyBlock();
    //            r.GetPropertyBlock(block);

    //            block.SetVectorArray("_hitPts", positionsQueue.ToArray());
    //            block.SetFloatArray("_Intensity", intensityQueue.ToArray());
    //            //block.SetVectorArray("_RingColor", colorQueue.ToArray());
    //            block.SetVectorArray("_RingColor", colorQueue.Select(c => (Vector4)c).ToArray());

    //            block.SetInt("_Type", type); // 추가적인 구분 정보 전달
    //            r.SetPropertyBlock(block);
    //        }
    //    }
    //    Debug.Log("RPC 함수 실행 됨");
    //}

    //// 네트워크를 통해서 해당 RPC를 호출하는 메서드
    //public void StartSonarRing(Vector4 position, float intensity, int type)
    //{
    //    Debug.Log("충돌됐음");
    //    photonView.RPC("StartSonarRingRPC", RpcTarget.OthersBuffered, position, intensity, type);
    //}

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
