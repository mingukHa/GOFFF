using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviourPun
{
    [System.Serializable]
    public struct SavePoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public SavePoint(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    public static SaveManager Instance;

    private Dictionary<int, SavePoint> savePoints = new Dictionary<int, SavePoint>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterSavePoint(int playerId, Vector3 position, Quaternion rotation)
    {
        savePoints[playerId] = new SavePoint(position, rotation);
    }

    public SavePoint GetSavePoint(int playerId)
    {
        return savePoints.ContainsKey(playerId) ? savePoints[playerId] : new SavePoint(Vector3.zero, Quaternion.identity);
    }
}
