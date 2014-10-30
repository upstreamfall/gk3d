using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using ObjLoader.Loader.Loaders;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Primitives;
using ObjLoader.Loader.Data.VertexData;
using SharpGL.SceneGraph.Quadrics;

namespace SharpGL_project_1
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        private bool _sceneIsRotating;

        private float _angleHorizontal;
        private float _angleVertical;
        private float _angleClockwise;
        private bool _onUpperEdge;
        private bool _onLowerEdge;
        private bool _onLeftEdge;
        private bool _onRightEdge;

        private bool _mouseLeftButtonDown;
        private Point _mousePosition;

        private const float ZNear = 0.01f;
        private const float ZFar = 100.0f;
        private const float Fov = 45.0f;

        private const float StepSize = 0.5f;
        private const float drawEps = 0.01f;

        private Vector3 _cameraEye;
        private Vector3 _cameraTarget;
        private Vector3 _cameraUp;

        private List<Mesh> Models;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
            Models = new List<Mesh>();

            InitializeComponent();
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            float[] position = new float[] { 0.0f, 0.0f, 1.0f };
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, position);
            float[] direction = new float[] { 0.0f, 0.0f, -1.0f };
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_DIRECTION, direction);
            float[] color = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };

            //gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);

            //  Load the identity matrix.
            gl.LoadIdentity();

            DrawModels(gl);
        }

        #region Draw Models

        private void DrawModels(OpenGL gl)
        {
            DrawSportsHall(gl);
            DrawCourt(gl);
            DrawNet(gl);

            foreach (var model in Models)
            {
                model.Draw(gl);
            }
        }

        private void DrawNet(OpenGL gl)
        {
            //gray
            gl.Color(0.5, 0.5, 0.5);
            
            //far column
            gl.LoadIdentity();
            gl.Translate(-0.5, -10, -6);
            //gl.Rotate(90.0, 1.0, 0.0, 0.0);
            //gl.Cylinder(glQuadric, 1.0f, 1.0f, 10, 16, 20);
            DrawNetColumn(gl);

            //near column
            gl.LoadIdentity();
            gl.Translate(-0.5, -10, 7);
            DrawNetColumn(gl);

            //net
            gl.LoadIdentity();
            gl.Translate(-0.5, -10, 5.5f);
            DrawNetPolygon(gl);
        }

        private void DrawNetPolygon(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);

            //top
            gl.Normal(0, 1, 0);
            gl.Vertex(0.0f, 5.0f, 0.0f);
            gl.Vertex(1.0f, 5.0f, 0.0f);
            gl.Vertex(1.0f, 5.0f, -11.0f);
            gl.Vertex(0.0f, 5.0f, -11.0f);

            //bottom
            //float eps = 2 * drawEps;
            gl.Normal(0, -1, 0);
            gl.Vertex(0.0f, 4.0f, 0.0f);
            gl.Vertex(1.0f, 4.0f, 0.0f);
            gl.Vertex(1.0f, 4.0f, -11.0f);
            gl.Vertex(0.0f, 4.0f, -11.0f);

            ////back
            gl.Normal(0, 0, -1);
            gl.Vertex(0.0f, 4.0f, -11.0f);
            gl.Vertex(1.0f, 4.0f, -11.0f);
            gl.Vertex(1.0f, 5.0f, -11.0f);
            gl.Vertex(0.0f, 5.0f, -11.0f);

            ////front
            //gl.Normal(0, 0, 1);
            gl.Vertex(0.0f, 4.0f, 0.0f);
            gl.Vertex(1.0f, 4.0f, 0.0f);
            gl.Vertex(1.0f, 5.0f, 0.0f);
            gl.Vertex(0.0f, 5.0f, 0.0f);

            ////left
            gl.Normal(-1, 0, 0);
            gl.Vertex(0.0f, 4.0f, 0.0f);
            gl.Vertex(0.0f, 4.0f, -11.0f);
            gl.Vertex(0.0f, 5.0f, -11.0f);
            gl.Vertex(0.0f, 5.0f, 0.0f);

            ////right
            gl.Normal(1, 0, 0);
            gl.Vertex(1.0f, 4.0f, 0.0f);
            gl.Vertex(1.0f, 4.0f, -11.0f);
            gl.Vertex(1.0f, 5.0f, -11.0f);
            gl.Vertex(1.0f, 5.0f, 0.0f);

            gl.End();
        }

        private void DrawNetColumn(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);

            //top
            gl.Normal(0, 1, 0);
            gl.Vertex(0.0f, 5.0f, 0.0f);
            gl.Vertex(1.0f, 5.0f, 0.0f);
            gl.Vertex(1.0f, 5.0f, -1.0f);
            gl.Vertex(0.0f, 5.0f, -1.0f);

            //bottom
            //float eps = 2 * drawEps;
            gl.Normal(0, -1, 0);
            gl.Vertex(0.0f, 0.0f + drawEps, 0.0f);
            gl.Vertex(1.0f, 0.0f + drawEps, 0.0f);
            gl.Vertex(1.0f, 0.0f + drawEps, -1.0f);
            gl.Vertex(0.0f, 0.0f + drawEps, -1.0f);

            ////back
            gl.Normal(0, 0, -1);
            gl.Vertex(0.0f, 0.0f, -1.0f);
            gl.Vertex(1.0f, 0.0f, -1.0f);
            gl.Vertex(1.0f, 5.0f, -1.0f);
            gl.Vertex(0.0f, 5.0f, -1.0f);

            //front
            //gl.Normal(0, 0, 1);
            gl.Vertex(0.0f, 0.0f, 0.0f);
            gl.Vertex(1.0f, 0.0f, 0.0f);
            gl.Vertex(1.0f, 5.0f, 0.0f);
            gl.Vertex(0.0f, 5.0f, 0.0f);

            //left
            gl.Normal(-1, 0, 0);
            gl.Vertex(0.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 0.0f, -1.0f);
            gl.Vertex(0.0f, 5.0f, -1.0f);
            gl.Vertex(0.0f, 5.0f, 0.0f);

            //right
            gl.Normal(1, 0, 0);
            gl.Vertex(1.0f, 0.0f, 0.0f);
            gl.Vertex(1.0f, 0.0f, -1.0f);
            gl.Vertex(1.0f, 5.0f, -1.0f);
            gl.Vertex(1.0f, 5.0f, 0.0f);

            gl.End();
        }

        private void DrawCourt(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);

            gl.Color(1.0f, 0.5f, 0.0f, 1.0f);
            gl.Normal(0, 1, 0);
            gl.Vertex(10.0f, -10.0f + drawEps, 5.0f);
            gl.Vertex(-10.0f, -10.0f + drawEps, 5.0f);
            gl.Vertex(-10.0f, -10.0f + drawEps, -5.0f);
            gl.Vertex(10.0f, -10.0f + drawEps, -5.0f);

            gl.End();
        }

        private void DrawSportsHall(OpenGL gl)
        {
            //green
            //top
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.0f, 1.0f, 0.0f, 1.0f);
            gl.Normal(0, -1, 0);
            gl.Vertex(20.0f, 10.0f, 10.0f);
            gl.Vertex(-20.0f, 10.0f, 10.0f);
            gl.Vertex(-20.0f, 10.0f, -10.0f);
            gl.Vertex(20.0f, 10.0f, -10.0f);
            gl.End();

            //gray
            //bottom
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.0f, 0.7f, 0.5f, 1.0f);
            gl.Normal(0, 1, 0);
            gl.Vertex(20.0f, -10.0f, 10.0f);
            gl.Vertex(-20.0f, -10.0f, 10.0f);
            gl.Vertex(-20.0f, -10.0f, -10.0f);
            gl.Vertex(20.0f, -10.0f, -10.0f);
            gl.End();

            //red
            //back
            //gl.Begin(OpenGL.GL_QUADS);
            //gl.Color(1.0f, 0.0f, 0.0f, 1.0f);
            //gl.Normal(0, 0, -1);
            //gl.Vertex(20.0f, 10.0f, 10.0f);
            //gl.Vertex(-20.0f, 10.0f, 10.0f);
            //gl.Vertex(-20.0f, -10.0f, 10.0f);
            //gl.Vertex(20.0f, -10.0f, 10.0f);
            //gl.End();

            //yellow
            //front
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 1.0f, 0.0f, 1.0f);
            gl.Normal(0, 0, 1);
            gl.Vertex(20.0f, 10.0f, -10.0f);
            gl.Vertex(-20.0f, 10.0f, -10.0f);
            gl.Vertex(-20.0f, -10.0f, -10.0f);
            gl.Vertex(20.0f, -10.0f, -10.0f);
            gl.End();

            //blue
            //left
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.0f, 0.0f, 1.0f, 1.0f);
            gl.Normal(1, 0, 0);
            gl.Vertex(-20.0f, 10.0f, 10.0f);
            gl.Vertex(-20.0f, 10.0f, -10.0f);
            gl.Vertex(-20.0f, -10.0f, -10.0f);
            gl.Vertex(-20.0f, -10.0f, 10.0f);
            gl.End();

            //pink
            //right
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 0.0f, 1.0f, 1.0f);
            gl.Normal(-1, 0, 0);
            gl.Vertex(20.0f, 10.0f, -10.0f);
            gl.Vertex(20.0f, 10.0f, 10.0f);
            gl.Vertex(20.0f, -10.0f, 10.0f);
            gl.Vertex(20.0f, -10.0f, -10.0f);
            gl.End();
        }

        #endregion

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //Initialise screen size
            Width = 1280;
            Height = 720;

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE);

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);

            //Initialise
            _sceneIsRotating = true;

            CameraInitialization();

            //LightInitialization(gl);

            ModelsLoading();

        }

        private void ModelsLoading()
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            var fileStream = new FileStream("humanoid_tri.obj", FileMode.Open);
            var result = objLoader.Load(fileStream);
            fileStream.Close();

            //man1
            Vector3 position = new Vector3(10, -11.5f, -5);
            Vector3 rotation = new Vector3(-1, 0, 0);
            float rotationAngle = 90.0f;
            Vector3 scale = new Vector3(1.0f, 0.5f, 0.5f);
            Models.Add(new Mesh(result, position, rotation, rotationAngle, scale));

            //man2
            position = new Vector3(10, -11.5f, 7);
            rotation = new Vector3(-1, 0, 0);
            rotationAngle = 90.0f;
            scale = new Vector3(1.0f, 0.5f, 0.5f);
            Models.Add(new Mesh(result, position, rotation, rotationAngle, scale));

            objLoader = objLoaderFactory.Create();
            fileStream = new FileStream("teapot.obj", FileMode.Open);
            result = objLoader.Load(fileStream);
            fileStream.Close();

            //man1
            position = new Vector3(-2, 1, 1);
            rotation = new Vector3(0, 1, 0);
            rotationAngle = 360.0f;
            scale = new Vector3(0.05f, 0.05f, 0.05f);
            Models.Add(new Mesh(result, position, rotation, rotationAngle, scale));

            objLoader = objLoaderFactory.Create();
            fileStream = new FileStream("cube.obj", FileMode.Open);
            result = objLoader.Load(fileStream);
            fileStream.Close();

            //man1
            position = new Vector3(-10, -10.0f, 1);
            rotation = new Vector3(0, 1, 0);
            rotationAngle = 360.0f;
            scale = new Vector3(2.0f, 2.0f, 2.0f);
            Models.Add(new Mesh(result, position, rotation, rotationAngle, scale));
        }

        private void LightInitialization(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            
            //reflektor1
            float[] color = new float[] { 0.0f, 1.0f, 0.5f, 1.0f };
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, color);
            float[] position = new float[] {-2.0f, 5.0f, 0.0f, 1.0f};
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, position);
            float[] direction = new float[] {-10.0f, 5.0f, 5.0f};
            //gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_DIRECTION, direction);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 5.0f);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_EXPONENT, 10.0f);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_LINEAR_ATTENUATION, 0.01f);

            //gl.Enable(OpenGL.GL_LIGHTING);
            //gl.Enable(OpenGL.GL_LIGHT0);
        }

        private void CameraInitialization()
        {
            _cameraEye = new Vector3(0, 0, 40);
            _cameraTarget = new Vector3(0, 0, -100);
            _cameraUp = new Vector3(0, 1, 0);

            Vector3 HorizontalTarget = new Vector3(_cameraTarget.X, 0.0f, _cameraTarget.Z);
            HorizontalTarget.Normalize();

            if (HorizontalTarget.Z >= 0.0f)
            {
                if (HorizontalTarget.X >= 0.0f)
                {
                    _angleHorizontal = 360.0f - ToDegree(Math.Asin(HorizontalTarget.Z));
                }
                else
                {
                    _angleHorizontal = 180.0f + ToDegree(Math.Asin(HorizontalTarget.Z));
                }
            }
            else
            {
                if (HorizontalTarget.X >= 0.0f)
                {
                    _angleHorizontal = ToDegree(Math.Asin(-HorizontalTarget.Z));
                }
                else
                {
                    _angleHorizontal = 90.0f + ToDegree(Math.Asin(-HorizontalTarget.Z));
                }
            }

            _angleVertical = -ToDegree(Math.Asin(_cameraTarget.Y));
            _angleClockwise = 0;

            _onUpperEdge = _onLowerEdge = _onLeftEdge = _onRightEdge = false;

            _mouseLeftButtonDown = false;
            _mousePosition = new Point(Width / 2, Height / 2);
        }

        private float ToDegree(double arg)
        {
            return (float)(arg * 180 / Math.PI);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  TODO: Set the projection matrix here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(Fov, (double)Width / (double)Height, ZNear, ZFar);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(_cameraEye.X, _cameraEye.Y, _cameraEye.Z,
                //_cameraTarget.X, _cameraTarget.Y, _cameraTarget.Z, 
                _cameraTarget.X + _cameraEye.X, 
                _cameraTarget.Y + _cameraEye.Y, 
                _cameraTarget.Z + _cameraEye.Z,
                _cameraUp.X, _cameraUp.Y, _cameraUp.Z);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void openGLControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool keyPressed = false;

            switch (e.KeyChar)
            {
                #region options

                case 'p':
                case 'P': //pause animation
                    _sceneIsRotating = !_sceneIsRotating;
                    break;
                case (char)27:    //ESC
                case 'q':
                case 'Q': //exit
                    Close();
                    break;

                #endregion

                #region camera movement

                case 's':
                case 'S': //near
                    _cameraEye -= _cameraTarget * StepSize;
                    keyPressed = true;
                    break;
                case 'w':
                case 'W': //far
                    _cameraEye += _cameraTarget * StepSize;
                    keyPressed = true;
                    break;
                case 'z':
                case 'Z': //up
                    _cameraEye.Y += StepSize;
                    _cameraTarget.Y += StepSize;
                    keyPressed = true;
                    break;
                case 'x':
                case 'X': //down
                    _cameraEye.Y -= StepSize;
                    _cameraTarget.Y -= StepSize;
                    keyPressed = true;
                    break;
                case 'd':
                case 'D': //right
                    Vector3 left = _cameraTarget.CrossProduct(_cameraUp);
                    left.Normalize();
                    left *= StepSize;
                    _cameraEye += left;
                    keyPressed = true;
                    break;
                case 'a':
                case 'A': //left
                    Vector3 right = _cameraUp.CrossProduct(_cameraTarget);
                    right.Normalize();
                    right *= StepSize;
                    _cameraEye += right;
                    keyPressed = true;
                    break;

                #endregion
                #region camera rotation
                case 'c':
                case 'C': //clockwise
                    //_cameraUp.Roll(-StepSize);
                    _angleClockwise -= StepSize;
                    keyPressed = true;
                    break;
                case 'v':
                case 'V': //counterclockwise
                    //_cameraUp.Roll(StepSize);
                    _angleClockwise += StepSize;
                    keyPressed = true;
                    break;
                #endregion

            }

            if (keyPressed)
            {
                UpdateCameraVectors();
                //openGLControl_Resized(null, null);
            }
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                _mouseLeftButtonDown = true;
                _mousePosition = new Point(e.X, e.Y);
            }
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                _mouseLeftButtonDown = false;
                _mousePosition = new Point(e.X, e.Y);
            }
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            int margin = 10;

            if (_mouseLeftButtonDown)
            {
                int DeltaX = e.X - _mousePosition.X;
                int DeltaY = e.Y - _mousePosition.Y;

                _mousePosition = new Point(e.X, e.Y);

                _angleHorizontal += DeltaX / 20.0f;
                _angleVertical += DeltaY / 20.0f;

                //if (DeltaX == 0)
                //{
                //    if (e.X <= margin)
                //    {
                //        _onLeftEdge = true;
                //    }
                //    else if (e.X >= (Width - margin))
                //    {
                //        _onRightEdge = true;
                //    }
                //}
                //else
                //{
                //    _onLeftEdge = false;
                //    _onRightEdge = false;
                //}

                //if (DeltaY == 0)
                //{
                //    if (e.Y <= margin)
                //    {
                //        _onUpperEdge = true;
                //    }
                //    else if (e.Y >= (Height - margin))
                //    {
                //        _onLowerEdge = true;
                //    }
                //}
                //else
                //{
                //    _onUpperEdge = false;
                //    _onLowerEdge = false;
                //}

                UpdateCameraVectors();
            }
        }

        private void UpdateCameraVectors()
        {
            Vector3 Vaxis = new Vector3(0.0f, 1.0f, 0.0f);

            // Rotate the view vector by the horizontal angle around the vertical axis
            Vector3 View = new Vector3(1.0f, 0.0f, 0.0f);
            //Vector3 View = _cameraTarget.CrossProduct(_cameraUp);
            View.Rotate(_angleHorizontal, Vaxis);
            View.Normalize();

            // Rotate the view vector by the vertical angle around the horizontal axis
            Vector3 Haxis = Vaxis.CrossProduct(View);
            Haxis.Normalize();
            View.Rotate(_angleVertical, Haxis);
            View.Normalize();

            _cameraTarget = View;
            _cameraTarget.Normalize();

            _cameraUp = _cameraTarget.CrossProduct(Haxis);
            _cameraUp.Normalize();

            _cameraUp.Roll(_angleClockwise);

            openGLControl_Resized(null, null);
        }
    }
}
