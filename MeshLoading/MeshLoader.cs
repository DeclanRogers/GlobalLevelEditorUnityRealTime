using BepInEx;
using HarmonyLib;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.Rendering;
using UnityEditor;
using Dummiesman;




namespace MeshLoading
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class MeshLoader : BaseUnityPlugin
    {
        const string NAME = "MeshLoading";
        const string VERSION = "0.1.5";
        const string GUID = "org.bepinex.plugins.MeshLoading";
        GameObject loadedObj;
        GameObject player;
        private void Start()
        {
            print("==========================================================INIT");

            string pc = "PlayerController";
            GameObject fb = new GameObject("fb", typeof(testFileBrowser), typeof(Meshes));
            GameObject asd = new GameObject("fb", typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider), typeof(BoxCollider), typeof(SphereCollider));


            // StreamReader outputFile = new StreamReader(@"CustomLevel\test2.txt");
            //
            // string line = "";
            // string[] linesplit;
            // line = outputFile.ReadLine();
            // float offset = 0;
            //
            // while (line != null)
            // {
            //     print(line);
            //     linesplit = line.Split(':');
            //     if (linesplit[0] == "PlayerName")
            //     {
            //         player = GameObject.Find(linesplit[1]);
            //     }
            //
            //     if (linesplit[0] == "OffSet")
            //     {
            //         offset = float.Parse(linesplit[1]);
            //     }
            //     line = outputFile.ReadLine();
            // }
            // outputFile.Close();
            // GameObject qwerty = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // qwerty.transform.position = new Vector3(offset, offset, offset);
            // player.transform.position = qwerty.transform.position + Vector3.up * 2;

            //foreach (var item in asd.GetComponent(pc) as MonoBehaviour))
            //{
            //    print(item);
            //}

            testFileBrowser tfb = fb.GetComponent<testFileBrowser>();

        }

        bool value = false;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {

                GameObject fb = new GameObject("fb", typeof(testFileBrowser), typeof(Meshes));

                testFileBrowser tfb = fb.GetComponent<testFileBrowser>();
            }





        }

        private void Teleport(float offset)
        {
            player.transform.position = new Vector3(offset, offset, offset) + Vector3.up * 2;
            print(player.transform.position);
        }
    }

    class Meshes : MonoBehaviour
    {
        public List<GameObject> loadedObjs;
        GameObject player;
        private void Start()
        {
            loadedObjs = new List<GameObject>();
        }
        GameObject PodHead;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                print(1);
                StreamReader outputFile = new StreamReader(@"CustomLevel\test2.txt");

                print(2);
                string line = "";
                string[] linesplit;
                line = outputFile.ReadLine();
                Vector3 offset = new Vector3();

                print(3);
                while (line != null)
                {
                    linesplit = line.Split(':');
                    if (linesplit[0] == "PlayerName")
                    {
                        print(4);

                        //   print(line);
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
                    line = outputFile.ReadLine();
                }
                print(6);
                outputFile.Close();
                if (player.GetComponent<CharacterController>() != null)
                {
                    player.GetComponent<CharacterController>().enabled = false;
                }
                print(7);


                GameObject qwerty = GameObject.CreatePrimitive(PrimitiveType.Plane);
                qwerty.transform.localScale = Vector3.one * 5;
                qwerty.transform.position = Vector3.up * 0.5f;
                qwerty.layer = 8;
                qwerty.GetComponent<MeshRenderer>().material.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");
                qwerty.GetComponent<MeshRenderer>().material.EnableKeyword("_GLOSSYREFLECTIONS_OFF");
                qwerty.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1);
                Time.timeScale = 0;
                qwerty.transform.position = offset- Vector3.up * 0.5f;
                //Teleport(offset);

                player.transform.localPosition = offset + Vector3.up * 2;
                print(player.transform.localPosition);
                Time.timeScale = 1;

                if (player.GetComponent<CharacterController>() != null)
                {
                    player.GetComponent<CharacterController>().enabled = true;
                }
                print(8);

            }
        }
    }
}

