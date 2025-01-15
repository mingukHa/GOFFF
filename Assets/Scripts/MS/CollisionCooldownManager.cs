//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;

//public class CollisionCooldownManager : MonoBehaviourPun
//{
//    public static CollisionCooldownManager Instance { get; private set; }

//    private Dictionary<int, float> collisionCooldowns = new Dictionary<int, float>();
//    private float cooldownDuration = 1f; // 기본 쿨타임
//    private float cleanupInterval = 2f; // 기본 정리 주기

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

//    // 특정 충돌 ID의 쿨타임 체크 및 갱신
//    public bool IsCooldownActive(int id)
//    {
//        float currentTime = Time.time;

//        if (collisionCooldowns.TryGetValue(id, out float lastCollisionTime))
//        {
//            if (currentTime - lastCollisionTime < cooldownDuration)
//            {
//                return true; // 쿨타임 중이면 반환
//            }
//        }

//        // 쿨다운 갱신
//        collisionCooldowns[id] = currentTime;
//        return false;
//    }

//    // 쿨타임 설정
//    public void SetCooldownDuration(float duration)
//    {
//        cooldownDuration = Mathf.Max(0, duration); // 음수 방지
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
