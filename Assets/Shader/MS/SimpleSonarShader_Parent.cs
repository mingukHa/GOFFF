using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;
using System;

public class SimpleSonarShader_Parent : MonoBehaviourPun
{
    // 파동을 적용할 렌더러들을 모두 가져옴
    private Renderer[] ObjectRenderers;

    // 쓰레기값을 미리 넣어놔서 초기화된 0,0,0,0에서 파동이 발생하지 않도록 방지
    private static readonly Vector4 GarbagePosition = new Vector4(-5000, -5000, -5000, -5000);

    // 큐 사이즈, 셰이더의 배열 사이즈와 같아야 함
    private static int QueueSize = 100;

    // Queue에 정보를 저장해서 셰이더에 배열 형태로 보냄
    private Queue<Vector4> positionsQueue = new Queue<Vector4>(QueueSize);

    private Queue<float> intensityQueue = new Queue<float>(QueueSize);

    private Queue<Vector4> colorQueue = new Queue<Vector4>(QueueSize);
    private Color ringColor = Color.white;

    // 재직렬화 할때 저장할 장소를 미리 선언
    private Vector4[] hitPtsVec = new Vector4[QueueSize];
    private Vector4[] ringColorsVec = new Vector4[QueueSize];

    // photon 서버가 시작된 시간을 Start할때 받아옴
    private double sceneStartTimePhoton;
    // 게임의 시작하고부터의 시간
    private float timeSinceSceneLoadPhoton;
    private MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        // 시작할 때 포톤 서버의 시간 가져오기
        sceneStartTimePhoton = PhotonNetwork.Time;
        // 자식에서 렌더러 가져오기
        ObjectRenderers = GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        // 큐를 미리 쓰레기 값으로 넣어둠
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
        
        // 포톤 서버 시간을 셰이더에 보내줌으로 시간을 동기화
        foreach (Renderer r in ObjectRenderers)
        {
            if (r)
            {
                r.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat("_RingTime", (float)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                r.SetPropertyBlock(propertyBlock);
            }
        }
    }

    public void StartSonarRing(Vector4 position, float intensity, int type)
    {
        Debug.Log("링 발생 요청");

        float[] positionArray = new float[] { position.x, position.y, position.z, position.w };

        photonView.RPC("RequestStartSonarRing", RpcTarget.MasterClient, positionArray, intensity, type);
    }

    [PunRPC]
    private void RequestStartSonarRing(float[] position, float intensity, int type)
    {
        if (!PhotonNetwork.IsMasterClient)
            return; // 마스터 클라이언트가 아니면 무시

        Debug.Log("마스터 클라이언트가 링 생성 처리");

        //float timeSinceSceneLoadPhoton = (float)(PhotonNetwork.Time - sceneStartTimePhoton);
        position[3] = (float)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

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
        for (int i = 0; i < hitPtsVec.Length; i++)
        {
            hitPtsVec[i] = new Vector4(positions[i * 4], positions[i * 4 + 1], positions[i * 4 + 2], positions[i * 4 + 3]);
        }
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

}
