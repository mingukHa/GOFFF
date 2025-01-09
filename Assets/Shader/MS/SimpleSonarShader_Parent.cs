// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

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
                r.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat("_RingTime", timeSinceSceneLoadPhoton);
                r.SetPropertyBlock(propertyBlock);
            }
        }
    }

    //[PunRPC]
    //public void UpdateSonarMaterial(float[] hitPts, float[] intensities, float[] ringColor)
    //{
    //    // 받은 float[] 배열을 다시 Vector4로 변환
    //    //Vector4[] hitPtsVec = new Vector4[hitPts.Length / 4];
    //    for (int i = 0; i < hitPtsVec.Length; i++)
    //    {
    //        hitPtsVec[i] = new Vector4(hitPts[i * 4], hitPts[i * 4 + 1], hitPts[i * 4 + 2], hitPts[i * 4 + 3]);
    //    }
    //    //Vector4[] ringColorsVec = new Vector4[ringColor.Length / 4];
    //    for (int i = 0; i < ringColorsVec.Length; i++)
    //    {
    //        ringColorsVec[i] = new Vector4(ringColor[i * 4], ringColor[i * 4 + 1], ringColor[i * 4 + 2], ringColor[i * 4 + 3]);
    //    }

    //    MaterialPropertyBlock block = new MaterialPropertyBlock();

    //    foreach (Renderer r in ObjectRenderers)
    //    {
    //        if (r)
    //        {
    //            r.GetPropertyBlock(block);

    //            block.SetVectorArray("_hitPts", hitPtsVec);
    //            block.SetFloatArray("_Intensity", intensities);
    //            block.SetVectorArray("_RingColor", ringColorsVec);

    //            r.SetPropertyBlock(block);
    //        }
    //    }
    //}

    public void StartSonarRing(Vector4 position, float intensity, int type)
    {
        Debug.Log("링 발생 요청");

        //float timeSinceSceneLoadPhoton = (float)(PhotonNetwork.Time - sceneStartTimePhoton);

        //position.w = timeSinceSceneLoadPhoton;
        //positionsQueue.Dequeue();
        //positionsQueue.Enqueue(position);

        //intensityQueue.Dequeue();
        //intensityQueue.Enqueue(intensity);

        //ringColor = type == 0 ? Color.white : Color.red; // 일반: 0, 몬스터: 1

        //colorQueue.Dequeue();
        //colorQueue.Enqueue(ringColor);

        float[] positionArray = new float[] { position.x, position.y, position.z, position.w };
        //photonView.RPC("RPCSonarRing", RpcTarget.All, positionArray, intensity);

        // Vector4를 float[] 배열로 변환
        //float[] hitPts = positionsQueue.SelectMany(v => new float[] { v.x, v.y, v.z, v.w }).ToArray();
        //float[] intensities = intensityQueue.ToArray();
        //float[] ringColors = colorQueue.SelectMany(c => new float[] { c.x, c.y, c.z, c.w }).ToArray();

        photonView.RPC("RequestStartSonarRing", RpcTarget.MasterClient, positionArray, intensity, type);
    }

    [PunRPC]
    private void RequestStartSonarRing(float[] position, float intensity, int type)
    {
        if (!PhotonNetwork.IsMasterClient)
            return; // 마스터 클라이언트가 아니면 무시

        Debug.Log("마스터 클라이언트가 링 생성 처리");

        float timeSinceSceneLoadPhoton = (float)(PhotonNetwork.Time - sceneStartTimePhoton);
        position[3] = timeSinceSceneLoadPhoton;

        // 링 색상 결정
        ringColor = type == 0 ? Color.white : Color.red; // 일반: 0, 몬스터: 1

        Vector4 rePosition = new Vector4(position[0], position[1], position[2], position[3]);

        // 마스터 클라이언트에서 큐 업데이트
        positionsQueue.Dequeue();
        positionsQueue.Enqueue(rePosition);

        intensityQueue.Dequeue();
        intensityQueue.Enqueue(intensity);

        colorQueue.Dequeue();
        colorQueue.Enqueue(ringColor);

        // Vector4를 float[] 배열로 변환
        float[] hitPts = positionsQueue.SelectMany(v => new float[] { v.x, v.y, v.z, v.w }).ToArray();
        float[] intensities = intensityQueue.ToArray();
        float[] ringColors = colorQueue.SelectMany(c => new float[] { c.x, c.y, c.z, c.w }).ToArray();

        // 큐 상태를 모든 클라이언트와 동기화
        photonView.RPC("SyncEntireQueue", RpcTarget.All,
            hitPts,
            intensities,
            ringColors);
    }

    [PunRPC]
    public void SyncEntireQueue(float[] positions, float[] intensities, float[] colors)
    {
        Debug.Log("클라이언트가 큐 동기화 받음");

        // 받은 float[] 배열을 다시 Vector4로 변환
        //Vector4[] hitPtsVec = new Vector4[hitPts.Length / 4];
        for (int i = 0; i < hitPtsVec.Length; i++)
        {
            hitPtsVec[i] = new Vector4(positions[i * 4], positions[i * 4 + 1], positions[i * 4 + 2], positions[i * 4 + 3]);
        }
        //Vector4[] ringColorsVec = new Vector4[ringColor.Length / 4];
        for (int i = 0; i < ringColorsVec.Length; i++)
        {
            ringColorsVec[i] = new Vector4(colors[i * 4], colors[i * 4 + 1], colors[i * 4 + 2], colors[i * 4 + 3]);
        }

        // 큐 동기화
        positionsQueue = new Queue<Vector4>(hitPtsVec);
        intensityQueue = new Queue<float>(intensities);
        colorQueue = new Queue<Vector4>(ringColorsVec);

        // 동기화된 데이터로 쉐이더 업데이트
        UpdateShaderQueues();
    }

    private void UpdateShaderQueues()
    {
        //float[] hitPts = positionsQueue.SelectMany(v => new float[] { v.x, v.y, v.z, v.w }).ToArray();
        //float[] intensities = intensityQueue.ToArray();
        //float[] ringColors = colorQueue.SelectMany(c => new float[] { c.r, c.g, c.b, c.a }).ToArray();

        foreach (Renderer r in ObjectRenderers)
        {
            if (r)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                r.GetPropertyBlock(block);

                block.SetVectorArray("_hitPts", positionsQueue.ToArray());
                block.SetFloatArray("_Intensity", intensityQueue.ToArray());
                block.SetVectorArray("_RingColor", colorQueue.ToArray());

                r.SetPropertyBlock(block);
            }
        }
    }



    //[PunRPC]
    //private void RPCSonarRingQueue(float[] positionArray, float intensity)
    //{
    //    float timeSinceSceneLoadPhoton = (float)(PhotonNetwork.Time - sceneStartTimePhoton);

    //    Vector4 position = new Vector4(positionArray[0], positionArray[1], positionArray[2], timeSinceSceneLoadPhoton);
    //    //position.w = timeSinceSceneLoadPhoton;

    //    positionsQueue.Dequeue();
    //    positionsQueue.Enqueue(position);

    //    intensityQueue.Dequeue();
    //    intensityQueue.Enqueue(intensity);

    //    colorQueue.Dequeue();
    //    colorQueue.Enqueue(ringColor);
    //}
}
