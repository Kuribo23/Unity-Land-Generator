using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    public string terrainsFolderPath = "Assets/Resources/terrains/";
    public string mapFolderPathString = "Assets/Resources/terrains/{0}/map";

    public string mapOverlayString = "tm_env_prop_{0}_layout_{1}_slots_x1";
    public string mapString = "tm_env_prop_{0}_layout_{1}_x1";

    // Start is called before the first frame update
    void Start()
    {
        GetRandomMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public (string, Sprite, Sprite) GetRandomMap()
    {
        //return (string.Empty, null, null);

        DirectoryInfo terrainFolderInfo = new DirectoryInfo(terrainsFolderPath);

        var terrainFolders = terrainFolderInfo.GetDirectories();
        int terrainCount = terrainFolderInfo.GetDirectories().Length;

        //1. Select a random folder
        int terrainChoice = Random.Range(0, terrainCount - 1);
        DirectoryInfo selectedTerrainFolder = terrainFolders[terrainChoice];
        Debug.Log(selectedTerrainFolder.Name);

        //2. Select a random map folder inside the selected folder
        string mapFolderPath = string.Format(mapFolderPathString, selectedTerrainFolder.Name);
        DirectoryInfo mapFolder = new DirectoryInfo(mapFolderPath);
        //Need to divide by 4.
        //half of them are metafiles
        //remaining half are overlay
        int mapCount = mapFolder.GetFiles().Length / 4;
        int mapChoice = Random.Range(1, mapCount);
        string mapSpriteName = string.Format(mapString, selectedTerrainFolder.Name, mapChoice);
        string mapOverlaySpriteName = string.Format(mapOverlayString, selectedTerrainFolder.Name, mapChoice);

        string mapSpriteFullPath = string.Format("terrains/{0}/map/{1}", selectedTerrainFolder.Name, mapSpriteName);
        Sprite mapSprite = Resources.Load<Sprite>(mapSpriteFullPath);

        string mapSpriteOverlayFullPath = string.Format("terrains/{0}/map/{1}", selectedTerrainFolder.Name, mapOverlaySpriteName);
        Sprite mapOverlaySprite = Resources.Load<Sprite>(mapSpriteOverlayFullPath);

        return (selectedTerrainFolder.Name, mapSprite, mapOverlaySprite);
    }
}
