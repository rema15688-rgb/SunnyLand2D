using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Menu-driven setup utility that creates scenes, prefabs, and basic UI so the project is ready to open and play.
public static class SunnyLandSetup
{
    [MenuItem("SunnyLand/Setup Project")]
    public static void SetupProject()
    {
        // Create folders
        CreateFolderIfNotExists("Assets/Scenes");
        CreateFolderIfNotExists("Assets/Prefabs");
        CreateFolderIfNotExists("Assets/Art");
        CreateFolderIfNotExists("Assets/Audio");

        // Create simple sprites
        Sprite playerSprite = CreateSpriteAsset("player", Color.yellow);
        Sprite groundSprite = CreateSpriteAsset("ground", new Color(0.3f, 0.7f, 0.3f));
        Sprite coinSprite = CreateSpriteAsset("coin", Color.cyan);

        // Create prefabs
        GameObject playerPrefab = CreatePlayerPrefab(playerSprite);
        PrefabUtility.SaveAsPrefabAsset(playerPrefab, "Assets/Prefabs/Player.prefab");
        Object.DestroyImmediate(playerPrefab);

        GameObject coinPrefab = CreateCoinPrefab(coinSprite);
        PrefabUtility.SaveAsPrefabAsset(coinPrefab, "Assets/Prefabs/Coin.prefab");
        Object.DestroyImmediate(coinPrefab);

        // Create scenes
        CreateMainMenuScene();
        CreateLevelScene("Level1", groundSprite);
        CreateLevelScene("Level2", groundSprite);
        CreateLevelScene("Level3", groundSprite);
        CreateSimpleScene("GameOver");
        CreateSimpleScene("Win");

        // Add scenes to Build Settings
        AddScenesToBuildSettings(new string[] {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Level1.unity",
            "Assets/Scenes/Level2.unity",
            "Assets/Scenes/Level3.unity",
            "Assets/Scenes/GameOver.unity",
            "Assets/Scenes/Win.unity"
        });

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("SunnyLand Setup", "Setup complete. Open the MainMenu scene and play! Use SunnyLand -> Setup Project again to recreate assets if needed.", "OK");
    }

    static void CreateFolderIfNotExists(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string parent = Path.GetDirectoryName(path);
            string newFolder = Path.GetFileName(path);
            if (string.IsNullOrEmpty(parent)) parent = "Assets";
            AssetDatabase.CreateFolder(parent, newFolder);
        }
    }

    static Sprite CreateSpriteAsset(string name, Color color)
    {
        int w = 64, h = 64;
        Texture2D tex = new Texture2D(w, h);
        Color[] cols = new Color[w * h];
        for (int i = 0; i < cols.Length; i++) cols[i] = color;
        tex.SetPixels(cols);
        tex.Apply();

        string path = $"Assets/Art/{name}.png";
        File.WriteAllBytes(Path.Combine(Application.dataPath, "Art", name + ".png"), tex.EncodeToPNG());
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
        if (ti != null)
        {
            ti.textureType = TextureImporterType.Sprite;
            ti.SaveAndReimport();
        }

        Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        return s;
    }

    static GameObject CreatePlayerPrefab(Sprite sprite)
    {
        GameObject go = new GameObject("Player");
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        if (sprite != null) sr.sprite = sprite;
        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
        go.tag = "Player";
        go.AddComponent< PlayerController >();

        // ground check
        GameObject gc = new GameObject("GroundCheck");
        gc.transform.SetParent(go.transform);
        gc.transform.localPosition = new Vector3(0, -0.6f, 0);

        return go;
    }

    static GameObject CreateCoinPrefab(Sprite sprite)
    {
        GameObject go = new GameObject("Coin");
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        if (sprite != null) sr.sprite = sprite;
        CircleCollider2D cc = go.AddComponent<CircleCollider2D>();
        cc.isTrigger = true;
        go.AddComponent<Coin>();
        return go;
    }

    static void CreateMainMenuScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject canvasGO = CreateCanvas("Canvas");

        // Title
        GameObject title = new GameObject("Title");
        title.transform.SetParent(canvasGO.transform, false);
        Text t = title.AddComponent<Text>();
        t.text = "SunnyLand";
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = 48;
        t.color = Color.white;
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        RectTransform rt = title.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, 100);
        rt.sizeDelta = new Vector2(400, 100);

        // Start Button
        GameObject startBtn = CreateButton(canvasGO.transform, "Start", new Vector2(0, 0));
        startBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            // Load Level1 when clicked in play mode
            if (Application.isPlaying) UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        });

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainMenu.unity");
    }

    static GameObject CreateCanvas(string name)
    {
        GameObject canvasGO = new GameObject(name);
        Canvas c = canvasGO.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        return canvasGO;
    }

    static GameObject CreateButton(Transform parent, string label, Vector2 anchoredPos)
    {
        GameObject btn = new GameObject("Button");
        btn.transform.SetParent(parent, false);
        Image img = btn.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 1f);
        Button b = btn.AddComponent<Button>();
        RectTransform rt = btn.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(160, 40);
        rt.anchoredPosition = anchoredPos;

        GameObject txt = new GameObject("Text");
        txt.transform.SetParent(btn.transform, false);
        Text t = txt.AddComponent<Text>();
        t.text = label;
        t.alignment = TextAnchor.MiddleCenter;
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.color = Color.black;
        RectTransform trt = txt.GetComponent<RectTransform>();
        trt.sizeDelta = rt.sizeDelta;
        return btn;
    }

    static void CreateLevelScene(string sceneName, Sprite groundSprite)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Managers
        GameObject managers = new GameObject("Managers");
        managers.AddComponent<GameManager>();
        managers.AddComponent<LevelManager>();
        managers.AddComponent<AudioManager>();
        UIManager uim = managers.AddComponent<UIManager>();

        // Canvas + UI
        GameObject canvas = CreateCanvas("Canvas");
        GameObject coinsTextGO = new GameObject("CoinsText");
        coinsTextGO.transform.SetParent(canvas.transform, false);
        Text coinsText = coinsTextGO.AddComponent<Text>();
        coinsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        coinsText.text = "Coins: 0";
        coinsText.alignment = TextAnchor.UpperLeft;
        RectTransform crt = coinsText.GetComponent<RectTransform>();
        crt.anchorMin = new Vector2(0, 1); crt.anchorMax = new Vector2(0, 1);
        crt.anchoredPosition = new Vector2(80, -40);
        crt.sizeDelta = new Vector2(200, 40);
        uim.coinsText = coinsText;

        // Player (instantiate prefab if exists)
        GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        GameObject player;
        if (playerPrefab != null) player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
        else player = CreatePlayerPrefab(null);
        player.transform.position = new Vector3(0, 1, 0);

        // Ground
        GameObject ground = new GameObject("Ground");
        SpriteRenderer gsr = ground.AddComponent<SpriteRenderer>();
        if (groundSprite != null) gsr.sprite = groundSprite;
        BoxCollider2D gbc = ground.AddComponent<BoxCollider2D>();
        ground.transform.position = new Vector3(0, -1, 0);
        ground.transform.localScale = new Vector3(10, 1, 1);

        // Coin
        GameObject coinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Coin.prefab");
        if (coinPrefab != null)
        {
            GameObject c = (GameObject)PrefabUtility.InstantiatePrefab(coinPrefab);
            c.transform.position = new Vector3(2, 0, 0);
        }

        EditorSceneManager.SaveScene(scene, $"Assets/Scenes/{sceneName}.unity");
    }

    static void CreateSimpleScene(string sceneName)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject canvas = CreateCanvas("Canvas");
        GameObject title = new GameObject("Title");
        title.transform.SetParent(canvas.transform, false);
        Text t = title.AddComponent<Text>();
        t.text = sceneName;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = 48;
        t.color = Color.white;
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        RectTransform rt = title.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(400, 100);
        EditorSceneManager.SaveScene(scene, $"Assets/Scenes/{sceneName}.unity");
    }

    static void AddScenesToBuildSettings(string[] scenePaths)
    {
        var list = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        foreach (var sp in scenePaths)
        {
            if (!System.IO.File.Exists(sp)) continue;
            bool already = false;
            foreach (var s in list) if (s.path == sp) { already = true; break; }
            if (!already) list.Add(new EditorBuildSettingsScene(sp, true));
        }
        EditorBuildSettings.scenes = list.ToArray();
    }
}
