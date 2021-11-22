using UnityEngine;
using UnityEditor;
using Game;

[CustomEditor(typeof(LevelMap))]
public class LevelMapGUI : Editor
{
    Vector2 scrollPosition;
    int _selected;
    string[] _selection = { "Empty", "Wall", "PlayerSpawnPoint", "Zombie" };

    LevelMap _levelMap;

    private void OnEnable()
    {
        _levelMap = (LevelMap)target;
        if(_levelMap.Chunks is null) _levelMap.Chunks = new Chunk[0, 0];
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        #region WorldSize

        GUILayout.BeginHorizontal();

            GUILayout.Label("World size:");
            _levelMap.X = EditorGUILayout.IntField(_levelMap.X);
            GUILayout.Label(" X ");
            _levelMap.Y = EditorGUILayout.IntField(_levelMap.Y);

        GUILayout.EndHorizontal();

        #endregion

        #region MapEditor

        _selected = EditorGUILayout.Popup("Prefabs:", _selected, _selection);

        GUILayout.BeginVertical("box");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(500), GUILayout.Height(500));
        if (_levelMap.Chunks.Length != _levelMap.X * _levelMap.Y) _levelMap.Chunks = new Chunk[_levelMap.X, _levelMap.Y];
        for (int i = 0; i < _levelMap.X; i++)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < _levelMap.Y; j++)
            {
                if (_levelMap.Chunks[i, j] is null) _levelMap.Chunks[i, j] = new Chunk(i, j);

                if (GUILayout.Button(_levelMap.Chunks[i, j].GetTexture(), GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    switch (_selected)
                    {
                        case 0:
                            _levelMap.Chunks[i, j].Type = ChunkType.Empty;
                            break;
                        case 1:
                            _levelMap.Chunks[i, j].Type = ChunkType.Wall;
                            break;
                        case 2:
                            _levelMap.Chunks[i, j].Type = ChunkType.Player;
                            break;
                        case 3:
                            _levelMap.Chunks[i, j].Type = ChunkType.Zombie;
                            break;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        #endregion

        #region Buttons

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Apply"))
        {
            _levelMap.Apply();
            return;
        }

        if (GUILayout.Button("Import"))
        {
            _levelMap.LoadFromFile();
            return;
        }

        if (GUILayout.Button("Export"))
        {
            _levelMap.SaveToFile();
            return;
        }

        GUILayout.EndHorizontal();

        #endregion Buttons

        if (GUI.changed)
        {
            Undo.RecordObject(_levelMap, "Level Map Modify");
            EditorUtility.SetDirty(_levelMap);
        }
    }
}
