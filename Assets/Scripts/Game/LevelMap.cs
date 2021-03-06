using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using Structs;

namespace Game
{
    public class LevelMap : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _zombie;
        [SerializeField] private GameObject _playerSpawnPoint;

        [Space, Header("Roots")]
        [SerializeField] private Transform _floor;
        [SerializeField] private Transform _rootWall;
        [SerializeField] private Transform _rootZombie;
        [SerializeField] private Transform _rootPlayerSpawnPoint;

        [Space, SerializeField, HideInInspector] private List<Vector3> _wallPoints;

        [SerializeField, HideInInspector] private Chunk[] _chunks;
        [SerializeField, HideInInspector] private int X, Y;

        public Chunk[] Chunks { get => _chunks; set => _chunks = value; }

        public IReadOnlyList<Vector3> WallPoints => _wallPoints;

        public void Apply()
        {
            Clear();
            InstantiateChunks();
        }

        public void SaveToFile(string path)
        {
            MapStruct mapStruct = new MapStruct
            {
                X = this.X,
                Y = this.Y,
                types = new ChunkType[X * Y]
            };

            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                    mapStruct.types[x * Y + y] = _chunks[x * Y + y].Type;

            string json = JsonUtility.ToJson(mapStruct, prettyPrint: true);

            try
            {
                File.WriteAllText(path, contents: json);
            }
            catch(Exception e)
            {
                Debug.Log(message: "{GameLog} => [MapStruct] - (<color=red>Error</color>) - SaveToFile -> " + e.Message);
            }
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log(message: "{GameLog} => [MapStruct] - LoadFromFile -> File Not Found!");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);

                MapStruct mapStruct = JsonUtility.FromJson<MapStruct>(json);
                this.X = mapStruct.X;
                this.Y = mapStruct.Y;

                _chunks = new Chunk[X * Y];
                for (int x = 0; x < X; x++)
                {
                    for (int y = 0; y < Y; y++)
                    {
                        _chunks[x * Y + y] = new Chunk(x, y);
                        _chunks[x * Y + y].Type = mapStruct.types[x * Y + y];
                    }
                }
            }
            catch(Exception e)
            {
                Debug.Log(message: "{GameLog} - [MapStruct] - (<color=red>Error</color>) - LoadFromFile -> " + e.Message);
            }
        }

        private void InstantiateChunks()
        {
            _floor.localScale = new Vector3(X, 1, Y);
            _floor.position = new Vector3(X * .5f - .5f, -.5f, Y * .5f - .5f);
            _wallPoints = new List<Vector3>();

            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    GameObject obj = null;
                    switch (_chunks[x * Y + y].Type)
                    {
                        case ChunkType.Empty:
                            continue;
                        case ChunkType.Wall:
                            obj = PrefabUtility.InstantiatePrefab(_wall, _rootWall) as GameObject;
                            _wallPoints.Add(obj.transform.position = _chunks[x * Y + y].Position + Vector3.up);
                            break;
                        case ChunkType.Zombie:
                            obj = PrefabUtility.InstantiatePrefab(_zombie, _rootZombie) as GameObject;
                            obj.transform.position = _chunks[x * Y + y].Position;
                            break;
                        case ChunkType.Player:
                            obj = PrefabUtility.InstantiatePrefab(_playerSpawnPoint, _rootPlayerSpawnPoint) as GameObject;
                            obj.transform.position = _chunks[x * Y + y].Position;
                            break;
                    }
                }
            }
        }

        private void Clear()
        {
            var count = _rootWall.childCount;
            for (var i = count - 1; i >= 0; i--)
            {
                DestroyImmediate(_rootWall.GetChild(i).gameObject);
            }

            count = _rootZombie.childCount;
            for (var i = count - 1; i >= 0; i--)
            {
                DestroyImmediate(_rootZombie.GetChild(i).gameObject);
            }

            count = _rootPlayerSpawnPoint.childCount;
            for (var i = count - 1; i >= 0; i--)
            {
                DestroyImmediate(_rootPlayerSpawnPoint.GetChild(i).gameObject);
            }
        }
    }
}