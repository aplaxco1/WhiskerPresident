using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData
{

    public static Resolution Resolution1 = new Resolution(1920, 1080);
    public static Resolution Resolution2 = new Resolution(1600, 900);
    public static Resolution Resolution3 = new Resolution(1440, 900);
    public static Resolution Resolution4 = new Resolution(1366, 768);
    public static Resolution Resolution5 = new Resolution(1280, 1024);
   
    public class Resolution
    {
        public int width;
        public int height;

        public Resolution(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    public class Volume
    {
        public float magnitude;
    }
}

