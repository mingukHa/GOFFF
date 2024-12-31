using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SonarEffect : ScriptableRendererFeature
{
    [System.Serializable]
    public class SonarSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material sonarMaterial;
        public int ringCount = 100;
        public float ringSpeed = 1f;
        public float ringWidth = 0.1f;
        public float ringIntensityScale = 1f;
        public float ringFadeDuration = 2f;
        public Color ringColor;
        public float outlineWidth = 0.02f;
        public Color outlineColor = Color.black;
        public float outlineAlpha = 1.0f;
    }

    public SonarSettings settings = new SonarSettings();

    private SonarPass sonarPass;

    public override void Create()
    {
        sonarPass = new SonarPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.sonarMaterial == null) return;

        sonarPass.renderPassEvent = settings.renderPassEvent;
        renderer.EnqueuePass(sonarPass);
    }

    public class SonarPass : ScriptableRenderPass
    {
        private SonarSettings settings;
        private Material sonarMaterial;
        private RTHandle tempRT;
        private Vector4[] hitPoints;
        private float[] intensities;
        private float startTime;
        private Mesh mesh;
        private MaterialPropertyBlock propertyBlock;

        public SonarPass(SonarSettings settings)
        {
            this.settings = settings;
            this.sonarMaterial = settings.sonarMaterial;
            this.propertyBlock = new MaterialPropertyBlock();

            // 메시 생성 (쿼드)
            mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(-1, -1, 0);
            vertices[1] = new Vector3(1, -1, 0);
            vertices[2] = new Vector3(-1, 1, 0);
            vertices[3] = new Vector3(1, 1, 0);
            mesh.vertices = vertices;
            mesh.triangles = new int[] { 0, 2, 1, 2, 3, 1 };
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            if (tempRT != null) tempRT.Release();
            tempRT = RTHandles.Alloc(descriptor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (settings.sonarMaterial == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Sonar Effect");
            RenderTargetIdentifier source = renderingData.cameraData.targetTexture == null ? BuiltinRenderTextureType.CameraTarget : renderingData.cameraData.targetTexture;

            if (hitPoints == null)
            {
                hitPoints = new Vector4[settings.ringCount];
                intensities = new float[settings.ringCount];
                startTime = Time.time;
                for (int i = 0; i < settings.ringCount; i++)
                {
                    hitPoints[i] = new Vector4(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f), startTime + i * 0.2f);
                    intensities[i] = Random.Range(1f, 3f);
                }
            }

            for (int i = 0; i < settings.ringCount; i++)
            {
                propertyBlock.SetVectorArray("_hitPts", hitPoints);
                propertyBlock.SetFloatArray("_Intensity", intensities);
                propertyBlock.SetColor("_RingColor", settings.ringColor);
            }

            propertyBlock.SetFloat("_RingSpeed", settings.ringSpeed);
            propertyBlock.SetFloat("_RingWidth", settings.ringWidth);
            propertyBlock.SetFloat("_RingIntensityScale", settings.ringIntensityScale);
            propertyBlock.SetFloat("_RingFadeDuration", settings.ringFadeDuration);
            propertyBlock.SetFloat("_OutlineWidth", settings.outlineWidth);
            propertyBlock.SetColor("_OutlineColor", settings.outlineColor);
            propertyBlock.SetFloat("_OutlineAlpha", settings.outlineAlpha);

            cmd.SetRenderTarget(source);
            cmd.ClearRenderTarget(false, true, Color.clear);
            cmd.DrawMesh(mesh, Matrix4x4.identity, sonarMaterial, 0, 0, propertyBlock);

            cmd.SetRenderTarget(source);
            cmd.SetGlobalTexture("_SonarTexture", source);
            cmd.DrawMesh(mesh, Matrix4x4.identity, sonarMaterial, 0, 1);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (tempRT != null) tempRT.Release();
        }
    }
}