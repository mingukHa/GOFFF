using UnityEngine;

public class OutlineParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public MeshFilter targetMesh;

    void Start()
    {
        if (targetMesh == null || particleSystem == null)
        {
            Debug.LogError("MeshFilter 또는 ParticleSystem이 설정되지 않았습니다.");
            return;
        }

        // Mesh 경계 추출
        Mesh mesh = targetMesh.sharedMesh;
        Vector3[] vertices = mesh.vertices;

        // 월드 좌표로 변환
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = targetMesh.transform.TransformPoint(vertices[i]);
        }

        // 파티클 생성
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        foreach (var vertex in vertices)
        {
            emitParams.position = vertex;
            particleSystem.Emit(emitParams, 1);
        }
    }
}
