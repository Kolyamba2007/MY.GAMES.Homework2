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

    SerializedProperty X;
    SerializedProperty Y;

    private void OnEnable()
    {
        X = serializedObject.FindProperty("X");
        Y = serializedObject.FindProperty("Y");

        _levelMap = (LevelMap)target;
        if (_levelMap.Chunks is null) _levelMap.Chunks = new Chunk[0];
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        #region WorldSize

        GUILayout.BeginHorizontal();

            GUILayout.Label("World size:");
            X.intValue = EditorGUILayout.IntField(X.intValue);
            GUILayout.Label(" X ");
            Y.intValue = EditorGUILayout.IntField(Y.intValue);

        GUILayout.EndHorizontal();

        #endregion

        #region MapEditor

        _selected = EditorGUILayout.Popup("Prefabs:", _selected, _selection);

        GUILayout.BeginVertical("box");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        if (_levelMap.Chunks.Length != X.intValue * Y.intValue) _levelMap.Chunks = new Chunk[X.intValue * Y.intValue];
        for (int x = 0; x < X.intValue; x++)
        {
            GUILayout.BeginHorizontal();
            for (int y = 0; y < Y.intValue; y++)
            {
                if (_levelMap.Chunks[x * Y.intValue + y] is null) _levelMap.Chunks[x * Y.intValue + y] = new Chunk(x, y);

                if (GUILayout.Button(_levelMap.Chunks[x * Y.intValue + y].GetTexture(), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    switch (_selected)
                    {
                        case 0:
                            _levelMap.Chunks[x * Y.intValue + y].Type = ChunkType.Empty;
                            break;
                        case 1:
                            _levelMap.Chunks[x * Y.intValue + y].Type = ChunkType.Wall;
                            break;
                        case 2:
                            _levelMap.Chunks[x * Y.intValue + y].Type = ChunkType.Player;
                            break;
                        case 3:
                            _levelMap.Chunks[x * Y.intValue + y].Type = ChunkType.Zombie;
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

        serializedObject.ApplyModifiedProperties();
    }
}
