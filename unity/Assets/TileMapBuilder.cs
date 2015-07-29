using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMapBuilder : Singleton<TileMapBuilder> {

    public PrimaryTileset preferredTileset;
    public PrimaryVariant preferredVariant;
    public bool preferredTainted = false;

    public PrimaryTileset currentTileset;
    public PrimaryVariant currentVariant;
    public bool currentlyTainted = false;

    public bool loadMapOnStart = true;
    public bool randomizedMap = false;
    public bool randomizedTiles = false;
    public bool randomizedTainted = false;

    public float taintChance = 25.0f;
    
    public Texture currentPrimaryTexture;
    public Texture currentPitTexture;
    public Texture currentTaintTexture;

    public Material primaryMaterial;
    public Material pitMaterial;
    public Material taintMaterial;

    public List<MeshRenderer> taintMeshes;
    public List<MeshRenderer> accentMeshes;

    public List<Texture> primaryRockTextures;
    public List<Texture> primaryMetalTextures;
    public List<Texture> pitTextures;
    public List<Texture> taintTextures;    

    public Tiled2Unity.TiledMap preferredMap;
    
    public List<Tiled2Unity.TiledMap> availableRandomMaps = new List<Tiled2Unity.TiledMap>();

    private Tiled2Unity.TiledMap currentMap;

    public LevelSpawner enemySpawnerPrefab;

    private List<Transform> sceneObjects = new List<Transform>();

    

    public enum PrimaryTileset
    {
        Rock,
        Metal
    }

    public enum PrimaryVariant
    {
        Normal,
        Green,
        Blue,
        Yellow,
        Red
    }
    
	// Use this for initialization
	public override void Start () {
        base.Start();

        if (loadMapOnStart)
        {
            LoadMap();
        }

	}

    public void LoadMap()
    {
        PrepareMap();

        if (randomizedTiles)
        {
            GenerateRandomTileset();
        }
        else
        {
            GeneratePreferredTileset();
        }
    }

    [ContextMenu("LoadPreferredMap")]
    public void LoadPreferredMap()
    {
        randomizedMap = false;
        randomizedTiles = false;

        LoadMap();
    }

    [ContextMenu("LoadRandomMap")]
    public void LoadRandomMap()
    {
        randomizedMap = true;
        randomizedTiles = true;

        LoadMap();
    }

    private void PrepareMap()
    {

        Tiled2Unity.TiledMap selectedMap;

        bool canLoadRandom = availableRandomMaps.Count > 0;
        bool canLoadPreferred = preferredMap != null;


        //if we can't load any maps, return
        if (!canLoadRandom && !canLoadPreferred)
        {
            return;
        }
        else if (!canLoadPreferred) //if we can't load preferred map, set to random
        {
            randomizedMap = true;
        }
        else if (!canLoadRandom) //if we can't load random maps, set not random
        {
            randomizedMap = false;
        }


        if (randomizedMap)
        {
            selectedMap = availableRandomMaps[Random.Range(0, availableRandomMaps.Count)];
        }
        else
        {
            selectedMap = preferredMap;
        }
        
        
        //now that we know what map we're loading, load it
        
        //instantiate the map
        currentMap = (Tiled2Unity.TiledMap) GameObject.Instantiate(selectedMap, transform.position, Quaternion.identity);

        Vector2 mapOffset = new Vector2();

        mapOffset.x = (currentMap.NumTilesWide * -0.75f);
        mapOffset.y = (currentMap.NumTilesHigh * 0.75f);

        currentMap.transform.position = mapOffset;

        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        currentMap.GetComponentsInChildren<MeshRenderer>(meshRenderers);

        foreach (MeshRenderer mesh in meshRenderers)
        {
            SetMeshMaterials(mesh); //set up mesh materials in case the prefab wasn't fixed
        }

        //load map data
        Transform sceneObjectsParent = currentMap.transform.Find("SceneObjects");

        if (sceneObjectsParent != null)
        {
            foreach (Transform sceneObject in sceneObjectsParent)
            {
                ParseSceneObject(sceneObject);
            }
        }

    }

    private void SetMeshMaterials(MeshRenderer mesh)
    {
        if (mesh.sortingLayerName.Equals("Pit"))
        {
            mesh.material = pitMaterial;
        }
        else if (mesh.sortingLayerName.Contains("Taint"))
        {
            mesh.material = taintMaterial;
            taintMeshes.Add(mesh);
        }
        else
        {
            mesh.material = primaryMaterial;
        }

        if (mesh.sortingLayerName.Contains("Accent"))
        {
            accentMeshes.Add(mesh);
        }

    }

    private void ParseSceneObject(Transform sceneObject)
    {
        sceneObjects.Add(sceneObject);

        //turn off collisions if there are any
        sceneObject.gameObject.layer = Globals.THE_VOID_LAYER;
        sceneObject.gameObject.SetActive(false); //turn off the object, we just use the positional info

        if (sceneObject.tag.Equals("PlayerSpawner"))
        {
            PlacePlayerSpawner(sceneObject);
        }
        else if (sceneObject.tag.Equals("BigEnemySpawner"))
        {
            PlaceEnemySpawner(sceneObject, SpawnManager.EnemyType.Big);
        }
        else if (sceneObject.tag.Equals("SmallEnemySpawner"))
        {
            PlaceEnemySpawner(sceneObject, SpawnManager.EnemyType.Small);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.B))
        {
            GenerateRandomTileset();
        }

	}

    [ContextMenu("Apply Random Tileset")]
    public void GenerateRandomTileset()
    {
        switch (currentTileset)
        {
            case PrimaryTileset.Rock:
                currentTileset = PrimaryTileset.Metal;
                break;
            case PrimaryTileset.Metal:
                currentTileset = PrimaryTileset.Rock;
                break;
            default:
                break;
        }

        int textureVariant = 0;
        
        switch (currentTileset)
        {
            case PrimaryTileset.Rock:
                textureVariant = Random.Range(0, primaryRockTextures.Count);                
                currentPrimaryTexture = primaryRockTextures[textureVariant];                
                break;
            case PrimaryTileset.Metal:
                textureVariant = Random.Range(0, primaryMetalTextures.Count);
                currentPrimaryTexture = primaryMetalTextures[textureVariant];
                break;
            default:
                break;
        }        

        currentVariant = (PrimaryVariant)textureVariant;

        GenerateTainted();
       
        ApplyTileset();

    }

    [ContextMenu("Apply Preferred Tileset")]
    public void GeneratePreferredTileset()
    {

        currentTileset = preferredTileset;

        currentVariant = preferredVariant;        

        switch (currentTileset)
        {
            case PrimaryTileset.Rock:
                currentPrimaryTexture = primaryRockTextures[Mathf.Min((int)currentVariant, primaryRockTextures.Count - 1)];
                break;
            case PrimaryTileset.Metal:
                currentPrimaryTexture = primaryMetalTextures[Mathf.Min((int)currentVariant, primaryMetalTextures.Count - 1)];
                break;
            default:
                break;
        }

        GenerateTainted();

        ApplyTileset();

    }

    public void GenerateTainted()
    {
        if (randomizedTainted)
        {
            currentlyTainted = Random.Range(0.0f, 100.0f) < taintChance;
        }
        else
        {
            currentlyTainted = preferredTainted;
        }
    }

    private void ApplyTileset()
    {
        primaryMaterial.mainTexture = currentPrimaryTexture;

        //set up colored pit to use correct material
        if (pitMaterial != null && pitTextures.Count > 0)
        {
            int pitVariant = Mathf.Min((int)currentVariant, pitTextures.Count - 1);

            currentPitTexture = pitTextures[pitVariant];

            pitMaterial.mainTexture = currentPitTexture;
        }

        //display accents if we have them
        foreach (MeshRenderer accentMesh in accentMeshes)
        {
            accentMesh.gameObject.SetActive(HasAccentLayer(currentTileset));
        }

        //display taint if map is tainted
        if (taintMeshes.Count == 0)
        {
            currentlyTainted = false;
        }

        //just use random taint material for right now
        if (taintMaterial != null && taintTextures.Count > 0)
        {
            currentTaintTexture = taintTextures[Random.Range(0, taintTextures.Count)];
            taintMaterial.mainTexture = currentTaintTexture;
        }
        foreach (MeshRenderer taintMesh in taintMeshes)
        {
            taintMesh.gameObject.SetActive(currentlyTainted);
        }

    }

    private bool HasAccentLayer(PrimaryTileset tileset)
    {

        return false; //not implemented yet

    }

    public void PlacePlayerSpawner(Transform placeAt)
    {
        PlayerSpawner playerSpawner = ObjectManager.Instance.playerSpawner;
        if (playerSpawner != null)
        {
            //ObjectManager.Instance.playerSpawner.transform.position = sceneObject.transform.position;
            playerSpawner.transform.position = placeAt.transform.position;
            playerSpawner.transform.rotation = placeAt.transform.rotation;
            playerSpawner.spawnOnStart = false;
            playerSpawner.DoSpawnPlayer();
        }
    }

    public void PlaceEnemySpawner(Transform placeAt, SpawnManager.EnemyType enemyType)
    {
        
        if (enemySpawnerPrefab != null)
        {
            LevelSpawner enemySpawner = (LevelSpawner)GameObject.Instantiate(enemySpawnerPrefab, placeAt.position, placeAt.rotation);

            enemySpawner.enemyType = enemyType;

            enemySpawner.transform.parent = SpawnManager.Instance.SpawnerObjects.transform;
        }
    }
}
