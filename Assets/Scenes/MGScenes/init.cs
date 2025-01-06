using UnityEngine;

public class OutlineParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public MeshFilter targetMesh;

    void Start()
    {
        if (targetMesh == null || particleSystem == null)
        {
            Debug.LogError("MeshFilter �Ǵ� ParticleSystem�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // Mesh ��� ����
        Mesh mesh = targetMesh.sharedMesh;
        Vector3[] vertices = mesh.vertices;

        // ���� ��ǥ�� ��ȯ
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = targetMesh.transform.TransformPoint(vertices[i]);
        }

        // ��ƼŬ ����
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        foreach (var vertex in vertices)
        {
            emitParams.position = vertex;
            particleSystem.Emit(emitParams, 1);
        }
    }
}
