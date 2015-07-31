using UnityEngine;
using System;
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
    public List<Texture> primaryBrickTextures;
    public List<Texture> primaryGrassTextures;
    public List<Texture> primarySandTextures;
    
    public List<Texture> pitTextures;
    public List<Texture> taintTextures;    

    public Tiled2Unity.TiledMap preferredMap;
    
    public List<Tiled2Unity.TiledMap> availableRandomMaps = new List<Tiled2Unity.TiledMap>();

    private Tiled2Unity.TiledMap currentMap;

    public LevelSpawner smallSpawnerPrefab;
    public LevelSpawner bigSpawnerPrefab;

    public GameObject bigCrate;
    public GameObject smallCrate;
    public GameObject medCrate;

    public GameObject bigBoomCrate;
    public GameObject smallBoomCrate;

    private List<Transform> sceneObjects = new List<Transform>();
    private List<GameObject> allCrates = new List<GameObject>();

    

    public enum PrimaryTileset
    {
        Rock,
        Metal,
        Brick,
        Grass,
        Sand
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

        if (this == null) { return; }

        allCrates.Add(bigCrate);
        allCrates.Add(medCrate);
        allCrates.Add(smallCrate);
        allCrates.Add(bigBoomCrate);
        allCrates.Add(smallBoomCrate);

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
            selectedMap = availableRandomMaps[UnityEngine.Random.Range(0, availableRandomMaps.Count)];
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
        else if (sceneObject.tag.Equals("BigCrate"))
        {
            PlaceCrate(sceneObject, bigCrate);
        }
        else if (sceneObject.tag.Equals("MedCrate"))
        {
            PlaceCrate(sceneObject, medCrate);
        }
        else if (sceneObject.tag.Equals("SmallCrate"))
        {
            PlaceCrate(sceneObject, smallCrate);
        }
        else if (sceneObject.tag.Equals("BigBoomCrate"))
        {
            PlaceCrate(sceneObject, bigBoomCrate);
        }
        else if (sceneObject.tag.Equals("SmallBoomCrate"))
        {
            PlaceCrate(sceneObject, smallBoomCrate);
        }
        else if (sceneObject.tag.Equals("Crate"))
        {
            PlaceRandomCrate(sceneObject);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (DebugLogger.DEBUG_MODE.Equals("DEBUG"))
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                GenerateRandomTileset();
            }
        }

	}

    [ContextMenu("Apply Random Tileset")]
    public void GenerateRandomTileset()
    {
        PrimaryTileset random = preferredTileset;

        try
        {
            random = (PrimaryTileset)UnityEngine.Random.Range(0, Enum.GetNames(typeof(PrimaryTileset)).Length);
        }
        catch { }

        currentTileset = random;

        int textureVariant = 0;

        switch (currentTileset)
        {
            case PrimaryTileset.Rock:
                textureVariant = UnityEngine.Random.Range(0, primaryRockTextures.Count);
                currentPrimaryTexture = primaryRockTextures[textureVariant];
                break;
            case PrimaryTileset.Metal:
                textureVariant = UnityEngine.Random.Range(0, primaryMetalTextures.Count);
                currentPrimaryTexture = primaryMetalTextures[textureVariant];
                break;
            case PrimaryTileset.Brick:
                textureVariant = UnityEngine.Random.Range(0, primaryBrickTextures.Count);
                currentPrimaryTexture = primaryBrickTextures[textureVariant];
                break;
            case PrimaryTileset.Grass:
                textureVariant = UnityEngine.Random.Range(0, primaryGrassTextures.Count);
                currentPrimaryTexture = primaryGrassTextures[textureVariant];
                break;
            case PrimaryTileset.Sand:
                textureVariant = UnityEngine.Random.Range(0, primarySandTextures.Count);
                currentPrimaryTexture = primarySandTextures[textureVariant];
                break;
            default:
                //broke?? should never get here??
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
            case PrimaryTileset.Brick:
                currentPrimaryTexture = primaryBrickTextures[Mathf.Min((int)currentVariant, primaryBrickTextures.Count - 1)];
                break;
            case PrimaryTileset.Grass:
                currentPrimaryTexture = primaryGrassTextures[Mathf.Min((int)currentVariant, primaryGrassTextures.Count - 1)];
                break;
            case PrimaryTileset.Sand:
                currentPrimaryTexture = primarySandTextures[Mathf.Min((int)currentVariant, primarySandTextures.Count - 1)];
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
            currentlyTainted = UnityEngine.Random.Range(0.0f, 100.0f) < taintChance;
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
            currentTaintTexture = taintTextures[UnityEngine.Random.Range(0, taintTextures.Count)];
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
        LevelSpawner spawner;

        switch (enemyType)
        {
            case SpawnManager.EnemyType.Small:
                spawner = smallSpawnerPrefab;
                break;
            case SpawnManager.EnemyType.Big:
                spawner = bigSpawnerPrefab;
                break;
            default:
                return; //unknown type
        }

        if (spawner != null)
        {
            LevelSpawner enemySpawner = (LevelSpawner)GameObject.Instantiate(spawner, placeAt.position, placeAt.rotation);

            //enemySpawner.enemyType = enemyType;

            enemySpawner.transform.parent = SpawnManager.Instance.SpawnerObjects.transform;
        }
    }

    public void PlaceCrate(Transform placeAt, GameObject prefab)
    {
        if (prefab != null)
        {
            GameObject crate = (GameObject)GameObject.Instantiate(prefab, placeAt.position, placeAt.rotation);

            //enemySpawner.enemyType = enemyType;

            crate.transform.parent = ObjectManager.Instance.dynamicObjects.transform;
        }
    }

    public void PlaceRandomCrate(Transform placeAt)
    {
        int crateToPlace = UnityEngine.Random.Range(0, allCrates.Count);

        PlaceCrate(placeAt, allCrates[crateToPlace]);

    }
}
