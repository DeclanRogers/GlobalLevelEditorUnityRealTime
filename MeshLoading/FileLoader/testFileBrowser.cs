using UnityEngine;
using System.Collections;
using MeshLoading;
using Dummiesman;
using System.IO;
using System.Text;

public class testFileBrowser : MonoBehaviour
{
    //skins and textures
    public GUISkin[] skins;
    public Texture2D file, folder, back, drive;
    public Vector3 offset = new Vector3();
    //string[] layoutTypes = { "Type 0", "Type 1" };
    //initialize file browser
    FileBrowser fb = new FileBrowser();
    public string output = "";
    // PlayerController player;
    public string levelName;
    Meshes ml;

    bool spawningOnPlayer;
    GameObject player;
    // Use this for initialization
    void Start()
    {
        print(1);
        MTLLoader mtl = new MTLLoader();
        print(2);
        //setup file browser style
        // fb.guiSkin = skins[0]; //set the starting skin
        //set the various textures
        fb.fileTexture = mtl.TextureLoadFunction(@"Images\file.png", false);
        print(3);
        //Debug.Log(fb.fileTexture.width);
        print(4);
        fb.directoryTexture = mtl.TextureLoadFunction(@"Images\folder.png", false); ;
        print(5);
        fb.backTexture = mtl.TextureLoadFunction(@"Images\back.png", false); ;
        print(6);
        fb.driveTexture = mtl.TextureLoadFunction(@"Images\drive.png", false); ;
        print(7);
        //show the search bar
        fb.showSearch = true;
        print(8);
        //search recursively (setting recursive search may cause a long delay)
        fb.searchRecursively = true;
        print(9);
        // player = FindObjectOfType<PlayerController>();
        ml = FindObjectOfType<Meshes>();
        print(10);


        levelName = "CustomLevel";
        print(11);


        string line;
        string[] linesplit;
        string[] linesplitV3;
        print(12);
        // using (outputFile = new StreamReader(@"CustomLevel\test.txt"))


        StreamReader outputFile = new StreamReader(@"CustomLevel\test2.txt");

        line = outputFile.ReadLine();

        print(13);
        while (line != null)
        {
            linesplit = line.Split(':');

            if (linesplit[0] == "PlayerName")
            {

                player = GameObject.Find(linesplit[1]);
            }
            if (linesplit[0] == "OffSetX")
            {
                print(5);
                offset.x = float.Parse(linesplit[1]);
            }
            if (linesplit[0] == "OffSetY")
            {
                print(5);
                offset.y = float.Parse(linesplit[1]);
            }
            if (linesplit[0] == "OffSetZ")
            {
                print(5);
                offset.z = float.Parse(linesplit[1]);
            }
            if (linesplit[0] == "SpawnOnPlayer")
            {
                if (linesplit[1] == "True" || linesplit[1] == "true")
                spawningOnPlayer = bool.Parse(linesplit[1]);
            }
            line = outputFile.ReadLine();
        }


        print(14);

        outputFile.Close();
        print(15);


    }
    bool fileExploerShown;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ToggleUI();
        }

    }



    private void ToggleUI()
    {
        fileExploerShown = !fileExploerShown;
        Time.timeScale = fileExploerShown ? 0.0f : 1;
        Cursor.lockState = fileExploerShown ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = fileExploerShown;
        print(fileExploerShown + " : " + Time.timeScale + " : " + Cursor.lockState);
        // player.enabled = !fileExploerShown;
    }

    void OnGUI()
    {
        if (fileExploerShown)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            // GUILayout.Label("Layout Type");
            if (GUI.Button(new Rect(new Vector2(10, 20), new Vector2(100, 30)), "Save Level"))
            {
                SaveScene();
            }

            if (GUI.Button(new Rect(new Vector2(10, 60), new Vector2(100, 30)), "Load Level"))
            {
                LoadScene();
            }
            GUILayout.Space(10);
            //select from available gui skins
            //GUILayout.Label("GUISkin");
            //fb.guiSkin = skins[1];
            GUILayout.Space(10);
            // fb.showSearch = GUILayout.Toggle(fb.showSearch, "Show Search Bar");
            //fb.searchRecursively = GUILayout.Toggle(fb.searchRecursively, "Search Sub Folders");
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.Label("Selected File: " + output);
            GUILayout.TextField(levelName);
            GUILayout.EndHorizontal();
            //draw and display output
            if (fb.draw())
            {
                print(fb.outputFile.FullName);
                if (fb.outputFile.Extension == ".obj")
                {
                    print(fb.outputFile.Extension);

                    ml.loadedObjs.Add(new OBJLoader().Load(fb.outputFile.FullName));
                    print(levelName + "\\");
                    print(fb.outputFile.Name);
                    ml.loadedObjs[ml.loadedObjs.Count - 1].AddComponent<SaveData>().filepath = levelName + "\\" + fb.outputFile.Name;
                    if (spawningOnPlayer)
                    {
                    ml.loadedObjs[ml.loadedObjs.Count - 1].transform.position = player.transform.position;
                    }
                    else
                    { 
                    ml.loadedObjs[ml.loadedObjs.Count - 1].transform.position += offset;
                    }
                    ml.loadedObjs[ml.loadedObjs.Count - 1].layer = 8;
                    print(ml.loadedObjs[ml.loadedObjs.Count - 1].GetComponent<SaveData>().filepath);
                   // ml.loadedObjs[ml.loadedObjs.Count - 1].AddComponent<MeshCollider>();
                    //GetComponent<MeshCollider>().sharedMesh = ml.loadedObjs[ml.loadedObjs.Count - 1].transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh;

                }


                //true is returned when a file has been selected
                //the output file is a member if the FileInfo class, if cancel was selected the value is null
                output = (fb.outputFile == null) ? "cancel hit" : fb.outputFile.ToString();
            }
        }
    }

    public void SaveScene()
    {
        StreamWriter outputFile;

        using (outputFile = new StreamWriter(@"CustomLevel\test.txt"))
        {
            foreach (var item in ml.loadedObjs)
            {
                outputFile.WriteLine(item.GetComponent<SaveData>().filepath + "`" + item.transform.position + "`" + item.transform.rotation.eulerAngles + "`" + item.transform.localScale);
            }

        }

    }

    public void LoadScene()
    {
        // StreamReader outputFile;
        string line;
        string[] linesplit;
        string[] linesplitV3;

        // using (outputFile = new StreamReader(@"CustomLevel\test.txt"))


        StreamReader outputFile = new StreamReader(@"CustomLevel\test2.txt");

        line = outputFile.ReadLine();
        
        while (line != null)
        {
            linesplit = line.Split(':');
            if (linesplit[0] == "OffSetX")
            {
                print(5);
                offset.x = float.Parse(linesplit[1]);
            }
            if (linesplit[0] == "OffSetY")
            {
                print(5);
                offset.y = float.Parse(linesplit[1]);
            }
            if (linesplit[0] == "OffSetZ")
            {
                print(5);
                offset.z = float.Parse(linesplit[1]);
            }
            line = outputFile.ReadLine();
        }



        outputFile.Close();

        print("P1");


         outputFile = new StreamReader(@"CustomLevel\test.txt");
        ///  (item.GetComponent<SaveData>().filepath + "`" + item.transform.position + "`" + item.transform.rotation.eulerAngles + "`" + item.transform.localScale);
        line = outputFile.ReadLine();
        while (line != null)
        {
            print(line);
            linesplit = line.Split('`');
            ml.loadedObjs.Add(new OBJLoader().Load(linesplit[0]));
            ml.loadedObjs[ml.loadedObjs.Count - 1].AddComponent<SaveData>().filepath = linesplit[0];



            linesplit[1] = linesplit[1].Replace('('.ToString(), string.Empty);
            linesplit[1] = linesplit[1].Replace(')'.ToString(), string.Empty);
            linesplit[1] = linesplit[1].Replace(' '.ToString(), string.Empty);
            linesplitV3 = linesplit[1].Split(',');



            ml.loadedObjs[ml.loadedObjs.Count - 1].transform.position = new Vector3(float.Parse(linesplitV3[0]), float.Parse(linesplitV3[1]), float.Parse(linesplitV3[2]));




            linesplit[2] = linesplit[2].Replace('('.ToString(), string.Empty);
            linesplit[2] = linesplit[2].Replace(')'.ToString(), string.Empty);
            linesplit[2] = linesplit[2].Replace(' '.ToString(), string.Empty);
            linesplitV3 = linesplit[2].Split(',');


            ml.loadedObjs[ml.loadedObjs.Count - 1].transform.rotation = Quaternion.Euler(float.Parse(linesplitV3[0]), float.Parse(linesplitV3[1]), float.Parse(linesplitV3[2]));

            linesplit[3] = linesplit[3].Replace('('.ToString(), string.Empty);
            linesplit[3] = linesplit[3].Replace(')'.ToString(), string.Empty);
            linesplit[3] = linesplit[3].Replace(' '.ToString(), string.Empty);
            linesplitV3 = linesplit[3].Split(',');


            ml.loadedObjs[ml.loadedObjs.Count - 1].transform.localScale = new Vector3(float.Parse(linesplitV3[0]), float.Parse(linesplitV3[1]), float.Parse(linesplitV3[2]));

            ml.loadedObjs[ml.loadedObjs.Count - 1].AddComponent<MeshCollider>();
            ml.loadedObjs[ml.loadedObjs.Count - 1].GetComponent<MeshCollider>().sharedMesh = ml.loadedObjs[ml.loadedObjs.Count - 1].transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh;

            line = outputFile.ReadLine();
        }
    }

}




public class SaveData : MonoBehaviour
{
    public string filepath;
}
