using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexRoadSet
{
    public bool[] Roads = new bool[6];

    public void SetRandom(int roadLimit)
    {
        for (int i = 0; i < 6; i++)
            Roads[i] = false;

        int _actualLimit = Mathf.Clamp(roadLimit, 0, 6);
        if (_actualLimit == 0)
            return;

        List<int> _availableIndices = new List<int> { 0, 1, 2, 3, 4, 5 };

        for (int i = 0; i < _actualLimit; i++)
        {
            int _randomIndex = Random.Range(0, _availableIndices.Count);
            int _chosenHexIndex = _availableIndices[_randomIndex];

            Roads[_chosenHexIndex] = true;
            _availableIndices.RemoveAt(_randomIndex);
        }
    }
}

[CreateAssetMenu(fileName = "HexTile", menuName = "Scriptable Objects/HexTile")]
public class HexTile : ScriptableObject
{
    public GameObject TilePrefab;
    public HexRoadSet Roads;
    public bool HasRoad(HexDirection direction) => Roads.Roads[(int)direction];
}
