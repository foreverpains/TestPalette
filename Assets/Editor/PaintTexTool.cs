using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ColorPaint
{
    public class PaintTexTool
    {

        [MenuItem("PaintTexTool/CreateImage4Hexagon")]
        public static void CreateImage4Hexagon()
        {

            const int COLOR_H = 6;
            const int COLOR_V = 3;
            int colorStep = 360 / (COLOR_H * COLOR_V);
            Texture2D HexagonTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Hexagon.png");
            Color[] hexagonPixels = HexagonTexture.GetPixels(); 
            int CELL_W = HexagonTexture.width;
            int CELL_H = HexagonTexture.height;
            int width = CELL_W * COLOR_H;
            int height = CELL_H * COLOR_V + CELL_H/2;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i< width; i+= CELL_W)
            {
                for (int j = 0; j < height; j+= CELL_H)
                {                    
                     DrawHexagon(i, j, CELL_W, CELL_H, width, height, hexagonPixels, pixels);  
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            string rangeTexPath = Application.dataPath + "/Textures/HexagonPalette.png";
            File.WriteAllBytes(rangeTexPath, tex.EncodeToPNG());
            Debug.Log("Create Image Success!");
        }

        private static void DrawHexagon(int x, int y, int CELL_W, int CELL_H, int width, int height, Color[] hexagonPixels, Color[] pixels)
        {
            int offsetX = 0;
            int offsetY = 0;
//             if ((x/ CELL_W) % 2 == 1)
//             {
//                 offsetX = -CELL_W / 2;
//                 offsetY = CELL_H / 2;
//             }
            Debug.Log(x);
            for (int i = 0; i < CELL_W; i++)
            {
                for (int j = 0; j < CELL_H; j++)
                {
                    Color c = hexagonPixels[j * CELL_W + i];
                    pixels[(j + y + offsetY) * width + x + i + offsetX] = c;
                }
            }
        }



        [MenuItem("PaintTexTool/CreatePaletteImage")]
        public static void CreatePaletteImage()
        {
            const int COLOR_NUM = 16;
            const int CELL_SIZE = 16;
            int width = CELL_SIZE * COLOR_NUM;
            int height = CELL_SIZE * 7;
            int colorStep = 360 / (COLOR_NUM - 1);

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = y * width + x;
                    float h, s, v;
                    int i = x / CELL_SIZE;
                    int j = y / CELL_SIZE;
                    if (i == COLOR_NUM-1)
                    {
                        h = 0.0f;
                        s = 0.0f;
                        v = 1.0f - j / 6.0f;
                    }
                    else
                    {                        
                        h = (i * colorStep) / 360.0f;
                        s = (j < 3) ? ((j + 1) * 0.25f) : 1.0f;
                        v = (j <= 3) ? 1.0f : (7 - j) * 0.25f;
                    }

                    pixels[index] = Color.HSVToRGB(h, s, v);
                }
            }
            tex.SetPixels(pixels);
            tex.Apply();
            string rangeTexPath = Application.dataPath + "/Textures/palette.png";
            File.WriteAllBytes(rangeTexPath, tex.EncodeToPNG());
            Debug.Log("Create Image Success!");
        }
/*
        [MenuItem("PaintTexTool/CreatePaintTex")]
        public static void CreatePaintTex()
        {
            string filepath = EditorUtility.OpenFilePanel("选择纹理图片", "Assets/", "tga");
            if (string.IsNullOrEmpty(filepath) == false)
            {
                Texture2D SplitTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Model/gril_cs/tex_shenti02.tga");
                if (SplitTexture != null)
                {
                    Texture2D RangeTexture = new Texture2D(SplitTexture.width, SplitTexture.height, TextureFormat.Alpha8, false);

                    bool[,] flag = new bool[SplitTexture.width, SplitTexture.height];
                    for (int x = 0; x < SplitTexture.width; x++)
                    {
                        for (int y = 0; y < SplitTexture.height; y++)
                        {
                            flag[x, y] = false;
                        }
                    }

                    Dictionary<Color, int> colorRangeMap = new Dictionary<Color, int>();
                    colorRangeMap.Add(Color.green, 0);

                    float progress = 0.0f;
                    float step = 1.0f / (SplitTexture.width * SplitTexture.height);
                    EditorUtility.DisplayProgressBar("Create Paint Texture", "Create Paint Texture", progress);
                    int rangeId = 1;
                    for (int x = 0; x < SplitTexture.width; x++)
                    {
                        for (int y = 0; y < SplitTexture.height; y++)
                        {
                            if (flag[x, y] == false)
                            {
                                Color c = SplitTexture.GetPixel(x, y);
                                int range;
                                if (colorRangeMap.ContainsKey(c))
                                {
                                    range = colorRangeMap[c];
                                }
                                else
                                {
                                    range = rangeId;
                                    colorRangeMap.Add(c, range);
                                    rangeId++;
                                }
                                RangeTexture.SetPixel(x, y, new Color32((byte)range, (byte)range, (byte)range, (byte)range));
                                //if (c == Color.green)
                                //{
                                //    FloodFill(x, y, 0, SplitTexture, RangeTexture, ref flag);
                                //}
                                //else
                                //{
                                //    FloodFill(x, y, (byte)rangeId, SplitTexture, RangeTexture, ref flag);
                                //    rangeId++;
                                //}

                                progress += step;
                                EditorUtility.DisplayProgressBar("Create Paint Texture", "Create Paint Texture", progress);
                            }
                        }
                    }
                    RangeTexture.Apply();

                    progress = 1.0f;
                    EditorUtility.DisplayProgressBar("Create Paint Texture", "Create Paint Texture", progress);

                    string rangeTexPath = filepath.Substring(0, filepath.Length - 4) + "_range.png";
                    File.WriteAllBytes(rangeTexPath, RangeTexture.EncodeToPNG());

                    EditorUtility.ClearProgressBar();
                }
            }
        }
        static bool IsSameColor(ref Color32 l, ref Color32 r)
        {
            return l.r == r.r && l.g == r.g && l.b == r.b && l.a == r.a;
        }

        static void FloodFill(int x, int y, byte rangeId, Texture2D SplitTexture, Texture2D RangeTexture, ref bool[,] flag)
        {
            Point2D start = new Point2D(x, y);
            if (start.x >= 0 && start.x < SplitTexture.width && start.y >= 0 && start.y < SplitTexture.height)
            {
                Color32 oldColor = SplitTexture.GetPixel(start.x, start.y);
                Stack<Point2D> stack = new Stack<Point2D>(SplitTexture.width * SplitTexture.height);
                stack.Push(start);
                while (stack.Count > 0)
                {
                    Point2D p = stack.Pop();
                    if (p.x >= 0 && p.x < SplitTexture.width && p.y >= 0 && p.y < SplitTexture.height)
                    {
                        int index = p.y * SplitTexture.width + p.x;
                        Color32 refc = SplitTexture.GetPixel(p.x, p.y);
                        if (IsSameColor(ref refc, ref oldColor) && flag[p.x, p.y] == false)
                        {
                            RangeTexture.SetPixel(p.x, p.y, new Color32(rangeId, rangeId, rangeId, rangeId));
                            stack.Push(new Point2D(p.x + 1, p.y));
                            stack.Push(new Point2D(p.x - 1, p.y));
                            stack.Push(new Point2D(p.x, p.y + 1));
                            stack.Push(new Point2D(p.x, p.y - 1));
                            stack.Push(new Point2D(p.x - 1, p.y + 1));
                            stack.Push(new Point2D(p.x - 1, p.y - 1));
                            stack.Push(new Point2D(p.x + 1, p.y + 1));
                            stack.Push(new Point2D(p.x + 1, p.y - 1));
                        }
                    }
                }
            }
        } 
        */      
    }
}
