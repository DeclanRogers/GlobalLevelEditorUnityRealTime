using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dummiesman
{
    public static class ImageUtils
    {
        public static void ConvertToNormalMap(Texture2D tex)
        {
            Color[] pixels = tex.GetPixels();
/*            for (int i = 0; i < pixels.Length; i++)
            {
                Color temp = pixels[i];
                temp.r = pixels[i].g;
                temp.a = pixels[i].r;
                pixels[i] = temp;
            }*/

            for (int i = 0; i < pixels.Length; i++)
            {
                Color temp = pixels[i];
                temp.a = pixels[i].a;
                temp.r = pixels[i].r;
                temp.g = pixels[i].g;
                temp.b = pixels[i].b;
                pixels[i] = temp;
            }

            tex.SetPixels(pixels);
        }
        
    }
}

