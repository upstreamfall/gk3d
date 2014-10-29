using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Primitives;

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

        private const float StepSize = 0.1f;

        private Vector3 _cameraEye;
        private Vector3 _cameraTarget;
        private Vector3 _cameraUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
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

            //  Load the identity matrix.
            gl.LoadIdentity();

            //  Rotate
            //gl.Rotate(_angleHorizontal, 0.0f, 1.0f, 0.0f);
            //gl.Rotate(-_angleVertical, 1.0f, 0.0f, 0.0f);
            //gl.Translate(-_cameraEye.X, -_cameraEye.Y, -_cameraEye.Z);

            DrawSportsHall(gl);
            
            //  Nudge the rotation.
            if (_sceneIsRotating)
            {
                //_angleHorizontal += 3.0f;
            }
        }

        /// <summary>
        /// Draw sports hall in the shape of a cuboid
        /// </summary>
        /// <param name="gl">openGL object</param>
        private void DrawSportsHall(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);

            //green
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(1.0f, 1.0f, -3.0f);
            gl.Vertex(-1.0f, 1.0f, -3.0f);
            gl.Vertex(-1.0f, 1.0f, 1.0f);
            gl.Vertex(1.0f, 1.0f, 1.0f);

            //orange
            gl.Color(1.0f, 0.5f, 0.0f);
            gl.Vertex(1.0f, -1.0f, 1.0f);
            gl.Vertex(-1.0f, -1.0f, 1.0f);
            gl.Vertex(-1.0f, -1.0f, -3.0f);
            gl.Vertex(1.0f, -1.0f, -3.0f);

            //red
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(1.0f, 1.0f, 1.0f);
            gl.Vertex(-1.0f, 1.0f, 1.0f);
            gl.Vertex(-1.0f, -1.0f, 1.0f);
            gl.Vertex(1.0f, -1.0f, 1.0f);

            //yellow
            gl.Color(1.0f, 1.0f, 0.0f);
            gl.Vertex(1.0f, -1.0f, -3.0f);
            gl.Vertex(-1.0f, -1.0f, -3.0f);
            gl.Vertex(-1.0f, 1.0f, -3.0f);
            gl.Vertex(1.0f, 1.0f, -3.0f);

            //blue
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(-1.0f, 1.0f, 1.0f);
            gl.Vertex(-1.0f, 1.0f, -3.0f);
            gl.Vertex(-1.0f, -1.0f, -3.0f);
            gl.Vertex(-1.0f, -1.0f, 1.0f);

            //pink
            gl.Color(1.0f, 0.0f, 1.0f);
            gl.Vertex(1.0f, 1.0f, -3.0f);
            gl.Vertex(1.0f, 1.0f, 1.0f);
            gl.Vertex(1.0f, -1.0f, 1.0f);
            gl.Vertex(1.0f, -1.0f, -3.0f);

            gl.End();
        }

        private void drawColouredPyramid(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(-1.0f, -1.0f, 1.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(1.0f, -1.0f, 1.0f);

            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(1.0f, -1.0f, 1.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(1.0f, -1.0f, -1.0f);

            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(1.0f, -1.0f, -1.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(-1.0f, -1.0f, -1.0f);

            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(-1.0f, -1.0f, -1.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(-1.0f, -1.0f, 1.0f);
            gl.End();
        }

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

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);

            //Initialise
            _sceneIsRotating = true;

            CameraInitialization();

            LightInitialization();
        }

        private void LightInitialization()
        {
            OpenGL gl = openGLControl.OpenGL;

            //gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            //gl.ShadeModel(ShadeModel.Smooth);
            //float[] matAmbient = new float[]{0.2f, 0.2f, 0.2f, 0.2f};
            //gl.Material(FrontFaceMode.ClockWise, , matAmbient);
            //Material matA = new Material();
            //matA.Ambient = Color.FromArgb(255, Color.Red);
            

            //simple
            Light light = new Light();
            GLColor ambientColor= new GLColor(0.2f, 0.2f, 0.2f, 1f);
            GLColor color = new GLColor(0.5f, 0.5f, 0.5f, 1f);
            light.Ambient = ambientColor;
            light.Specular = color;
            light.Diffuse = color;
            light.Position = new Vertex(0, 0, -5);
            light.GLCode = 10;
            light.On = true;
            light.Bind(gl);
        }

        private void CameraInitialization()
        {
            _cameraEye = new Vector3(0, 0, -10);
            _cameraTarget = new Vector3(0, 0, 100);
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
            _mousePosition = new Point(Width/2, Height/2);
        }

        private float ToDegree(double arg)
        {
            return (float) (arg*180/Math.PI);
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
            gl.Perspective(Fov, (double) Width/(double) Height, ZNear, ZFar);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(_cameraEye.X, _cameraEye.Y, _cameraEye.Z,
                //_cameraTarget.X, _cameraTarget.Y, _cameraTarget.Z, 
                _cameraTarget.X + _cameraEye.X, _cameraTarget.Y + _cameraEye.Y, _cameraTarget.Z + _cameraEye.Z,
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
                case 'q':
                case 'Q': //exit
                    Close();
                    break;

                    #endregion

                    #region camera movement

                case 's':
                case 'S': //near
                    _cameraEye -= _cameraTarget*StepSize;
                    keyPressed = true;
                    break;
                case 'w':
                case 'W': //far
                    _cameraEye += _cameraTarget*StepSize;
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
                openGLControl_Resized(null, null);
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

                _angleHorizontal += DeltaX/20.0f;
                _angleVertical += DeltaY/20.0f;

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
