/*
(C) 2015 AARO4130
PARTS OF TGA LOADING CODE (C) 2013 mikezila
DO NOT USE PARTS OF, OR THE ENTIRE SCRIPT, AND CLAIM AS YOUR OWN WORK
*//*

using System;
using UnityEngine;
using System.Collections;
using System.IO;

public class TextureLoader : MonoBehaviour
{
    public static Texture2D LoadTGA(string fileName)
    {
        using (var imageFile = File.OpenRead(fileName))
        {
            print("1");
            return LoadTGA(imageFile);
        }
    }

    public static Texture2D LoadDDSManual(string ddsPath)
    {
        try
        {

            byte[] ddsBytes = File.ReadAllBytes(ddsPath);

            byte ddsSizeCheck = ddsBytes[4];
            if (ddsSizeCheck != 124)
                throw new System.Exception("Invalid DDS DXTn texture. Unable to read"); //this header byte should be 124 for DDS image files

            int height = ddsBytes[13] * 256 + ddsBytes[12];
            int width = ddsBytes[17] * 256 + ddsBytes[16];

            byte DXTType = ddsBytes[87];
            TextureFormat textureFormat = TextureFormat.DXT5;
            if (DXTType == 49)
            {
                textureFormat = TextureFormat.DXT1;
                //	Debug.Log ("DXT1");
            }

            if (DXTType == 53)
            {
                textureFormat = TextureFormat.DXT5;
                //	Debug.Log ("DXT5");
            }
            int DDS_HEADER_SIZE = 128;
            byte[] dxtBytes = new byte[ddsBytes.Length - DDS_HEADER_SIZE];
            Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, dxtBytes, 0, ddsBytes.Length - DDS_HEADER_SIZE);

            System.IO.FileInfo finf = new System.IO.FileInfo(ddsPath);
            Texture2D texture = new Texture2D(width, height, textureFormat, false);
            texture.LoadRawTextureData(dxtBytes);
            texture.Apply();
            texture.name = finf.Name;

            return (texture);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error: Could not load DDS");
            return new Texture2D(8, 8);
        }
    }

    public static Texture2D LoadTexture(string fn, bool normalMap = false)
    {
        if (!File.Exists(fn))
            return null;
        string ext = Path.GetExtension(fn).ToLower();
        if (ext == ".png" || ext == ".jpg")
        {
            Texture2D t2d = new Texture2D(1, 1);
            t2d.LoadImage(File.ReadAllBytes(fn));
            if (normalMap)
                SetNormalMap(ref t2d);
            return t2d;
        }
        else if (ext == ".dds")
        {
            Texture2D returnTex = LoadDDSManual(fn);
            if (normalMap)
                SetNormalMap(ref returnTex);
            return returnTex;
        }
        else if (ext == ".tga")
        {
            Texture2D returnTex = LoadTGA(fn);
            if (normalMap)
                SetNormalMap(ref returnTex);
            return returnTex;
        }
        else
        {
            Debug.Log("texture not supported : " + fn);
        }
        return null;
    }

    public static Texture2D LoadTGA(Stream TGAStream)
    {
        print(2);
        using (BinaryReader r = new BinaryReader(TGAStream))
        {
            // Skip some header info we don't care about.
            // Even if we did care, we have to move the stream seek point to the beginning,
            // as the previous method in the workflow left it at the end.
            r.BaseStream.Seek(12, SeekOrigin.Begin);

            short width = r.ReadInt16();
            short height = r.ReadInt16();
            int bitDepth = r.ReadByte();

        print(3);
            // Skip a byte of header information we don't care about.
            r.BaseStream.Seek(1, SeekOrigin.Current);

            Texture2D tex = new Texture2D(width, height);
            Color32[] pulledColors = new Color32[width * height];

        print(4);
            if (bitDepth == 32)
            {
        print(5.1);
                for (int i = 0; i < width * height; i++)
                {
                    byte red = r.ReadByte();
                    byte green = r.ReadByte();
                    byte blue = r.ReadByte();
                    byte alpha = r.ReadByte();

                    pulledColors[i] = new Color32(blue, green, red, alpha);
                }
            }
            else if (bitDepth == 24)
            {
        print(5.2);
                for (int i = 0; i < width * height; i++)
                {
                    byte red = r.ReadByte();
                    byte green = r.ReadByte();
                    byte blue = r.ReadByte();

                    pulledColors[i] = new Color32(blue, green, red, 1);
                }
            }
            else
            {
        print(5.3);
                throw new Exception("TGA texture had non 32/24 bit depth.");
            }

        print(6);
            tex.SetPixels32(pulledColors);
        print(5);
            tex.Apply();
        print(8);
            return tex;

        }
    }
}
*/