using UnityEngine;
using System.Collections;

public class testFileBrowser : MonoBehaviour
{
    //skins and textures
    public GUISkin[] skins;
    public Texture2D file, folder, back, drive;

    //string[] layoutTypes = { "Type 0", "Type 1" };
    //initialize file browser
    FileBrowser fb = new FileBrowser();
    public string output = "";
    // Use this for initialization
    void Start()
    {
        //setup file browser style
        fb.guiSkin = skins[0]; //set the starting skin
                               //set the various textures
        fb.fileTexture = file;
        fb.directoryTexture = folder;
        fb.backTexture = back;
        fb.driveTexture = drive;
        //show the search bar
        fb.showSearch = true;
        //search recursively (setting recursive search may cause a long delay)
        fb.searchRecursively = true;
    }
    bool fileExploerShown;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            print("asd");
            ToggleUI();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            print(output);
        }
    }

    private void ToggleUI()
    {
        fileExploerShown = !fileExploerShown;
    }

    void OnGUI()
    {
        if (fileExploerShown)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
           // GUILayout.Label("Layout Type");
           // fb.setLayout(GUILayout.SelectionGrid(fb.layoutType, layoutTypes, 1));
            GUILayout.Space(10);
            //select from available gui skins
            //GUILayout.Label("GUISkin");
            fb.guiSkin = skins[1];
            GUILayout.Space(10);
           // fb.showSearch = GUILayout.Toggle(fb.showSearch, "Show Search Bar");
            //fb.searchRecursively = GUILayout.Toggle(fb.searchRecursively, "Search Sub Folders");
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.Label("Selected File: " + output);
            GUILayout.EndHorizontal();
            //draw and display output
            if (fb.draw())
            { //true is returned when a file has been selected
              //the output file is a member if the FileInfo class, if cancel was selected the value is null
                output = (fb.outputFile == null) ? "cancel hit" : fb.outputFile.ToString();
            }
        }
    }
}
