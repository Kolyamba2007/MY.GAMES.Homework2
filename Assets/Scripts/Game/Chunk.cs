using UnityEngine;

public class Chunk
{
    private ChunkType _type = ChunkType.Empty;

    private Vector3 _position;

    public Chunk(int X, int Y)
    {
        _position = new Vector3(X, 0, Y);
    }

    public ChunkType Type { get => _type; set => _type = value; }
    public Vector3 Position => _position;

    public Texture2D GetTexture()
    {
        Texture2D texture = new Texture2D(8, 8);

        switch (_type)
        {
            case ChunkType.Empty:
                for (int x = 0; x < texture.width; x++)
                    for (int y = 0; y < texture.height; y++)
                        texture.SetPixel(x, y, Color.white);
                texture.Apply();
                break;
            case ChunkType.Wall:
                for (int x = 0; x < texture.width; x++)
                    for (int y = 0; y < texture.height; y++)
                        texture.SetPixel(x, y, Color.yellow);
                texture.Apply();
                break;
            case ChunkType.Player:
                for (int x = 0; x < texture.width; x++)
                    for (int y = 0; y < texture.height; y++)
                        texture.SetPixel(x, y, Color.blue);
                texture.Apply();
                break;
            case ChunkType.Zombie:
                for (int x = 0; x < texture.width; x++)
                    for (int y = 0; y < texture.height; y++)
                        texture.SetPixel(x, y, Color.red);
                texture.Apply();
                break;
        }

        return texture;
    }
}