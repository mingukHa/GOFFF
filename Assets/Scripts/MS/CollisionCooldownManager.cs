//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;

//public class CollisionCooldownManager : MonoBehaviourPun
//{
//    public static CollisionCooldownManager Instance { get; private set; }

//    private Dictionary<int, float> collisionCooldowns = new Dictionary<int, float>();
//    private float cooldownDuration = 1f; // �⺻ ��Ÿ��
//    private float cleanupInterval = 2f; // �⺻ ���� �ֱ�

//    private double sceneStartTimePhoton;
//    private float timeSinceSceneLoadPhoton;


//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//            StartCoroutine(CleanupCooldowns());
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        sceneStartTimePhoton = PhotonNetwork.Time;
//    }

//    // Ư�� �浹 ID�� ��Ÿ�� üũ �� ����
//    public bool IsCooldownActive(int id)
//    {
//        float currentTime = Time.time;

//        if (collisionCooldowns.TryGetValue(id, out float lastCollisionTime))
//        {
//            if (currentTime - lastCollisionTime < cooldownDuration)
//            {
//                return true; // ��Ÿ�� ���̸� ��ȯ
//            }
//        }

//        // ��ٿ� ����
//        collisionCooldowns[id] = currentTime;
//        return false;
//    }

//    // ��Ÿ�� ����
//    public void SetCooldownDuration(float duration)
//    {
//        cooldownDuration = Mathf.Max(0, duration); // ���� ����
//    }

//    private IEnumerator CleanupCooldowns()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(cleanupInterval);

//            float currentTime = Time.time;
//            List<int> keysToRemove = new List<int>();

//            foreach (var cooldown in collisionCooldowns)
//            {
//                if (currentTime - cooldown.Value >= cleanupInterval)
//                {
//                    keysToRemove.Add(cooldown.Key);
//                }
//            }

//            foreach (var key in keysToRemove)
//            {
//                collisionCooldowns.Remove(key);
//            }
//        }
//    }
//}
