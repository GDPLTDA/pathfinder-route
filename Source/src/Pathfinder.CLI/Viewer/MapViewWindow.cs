using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;
using System.Threading;
namespace Pathfinder.CLI.Viewer
{
    public class MapViewWindow : GameWindow
    {
        int _width, _height;
        int BlockSize;
        readonly int FPS = 60;
        string[] Map;
        public MapViewWindow(string[] map, int width, int height, int blocksize)
             : base(width * blocksize, height * blocksize)
        {
            Title = "Map Viewer";
            BlockSize = blocksize;
            _width = width * BlockSize;
            _height = height * BlockSize;
            Map = map;

        }

        public static void OpenGlWindow(string textMap, int blockSize)
        {
            var map = textMap.Split('\n');
            map = map.Where(e => !e.StartsWith("?")).ToArray();

            var width = map[0].Length;
            var height = map.Count();

            using (var window = new MapViewWindow(map, width, height, blockSize))
            {
                window.Run();
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, _width, 0.0, _height, -2.0, 2.0);
            base.OnLoad(e);
        }
        public void DrawMap()
        {

            for (int i = 0; i < Map.Length; i++)
                for (int j = 0; j < Map[i].Length; j++)
                {
                    var node = Map[j][i];
                    var c = Color.White;
                    if (node == FileTool.Start)
                        c = Color.Green;
                    else if (node == FileTool.End)
                        c = Color.Red;
                    else if (node == FileTool.Path)
                        c = Color.Yellow;
                    else if (node == FileTool.Wall)
                        c = Color.DarkGray;
                    else if (node == FileTool.Closed)
                        c = Color.LightGreen;
                    else if (node == FileTool.Opened)
                        c = Color.LightBlue;
                    DrawBlock(i * BlockSize, j * BlockSize, BlockSize, c);
                }
        }
        void DrawBlock(int tX, int tY, int tS, Color tC)
        {
            tY = _height - tY;
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(tC);
            GL.Vertex2(tX, tY);
            GL.Vertex2(tX + tS, tY);
            GL.Vertex2(tX + tS, tY - tS);
            GL.Vertex2(tX, tY - tS);
            GL.End();
        }
        void DrawLine(float tX1, float tY1, float tX2, float tY2, Color tC)
        {
            tY1 = _height - tY1;
            tY2 = _height - tY2;
            GL.LineWidth(1.0f);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(tC);
            GL.Vertex2(tX1, tY1);
            GL.Vertex2(tX2, tY2);
            GL.End();
        }
        int tx = 0, ty;
        void DrawGrid()
        {
            for (float i = 0; i < _height; i += BlockSize)
                DrawLine(0, i, _width, i, Color.Gray);
            for (float i = 0; i < _width; i += BlockSize)
                DrawLine(i, 0, i, _height, Color.Gray);
            tx++;
            ty++;
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            base.OnUpdateFrame(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            DrawMap();
            DrawGrid();
            base.OnRenderFrame(e);
            SwapBuffers();
            Thread.Sleep(1000 / FPS);
        }
    }
}
