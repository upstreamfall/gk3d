using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ObjLoader.Loader.Data.VertexData;
using SharpGL;
using SharpGL.Enumerations;

namespace SharpGL_project_1
{
    public class Mesh
    {
        private ObjLoader.Loader.Loaders.LoadResult result;
        private Vector3 position;
        private Vector3 rotation;
        private float rotationAngle;
        private Vector3 scale;
        private float[] color;

        public Mesh(ObjLoader.Loader.Loaders.LoadResult result, Vector3 position, Vector3 rotation, float rotationAngle, Vector3 scale, float[] color)
        {
            // TODO: Complete member initialization
            this.result = result;
            this.position = position;
            this.rotation = rotation;
            this.rotationAngle = rotationAngle;
            this.scale = scale;
            this.color = color;
        }

        public void Draw(OpenGL gl)
        {
            gl.LoadIdentity();
            gl.Translate(position.X, position.Y, position.Z);
            gl.Rotate(rotationAngle, rotation.X, rotation.Y, rotation.Z);
            gl.Scale(scale.X, scale.Y, scale.Z);

            gl.Color(color[0], color[1], color[2], color[3]);
            gl.Begin(OpenGL.GL_TRIANGLES);
            
            foreach (var group in result.Groups)
            {
                foreach (var face in group.Faces)
                {
                    for (int i = 0; i < face.Count; ++i)
                    {
                        int normalIndex = face[i].NormalIndex;
                        int vertexIndex = face[i].VertexIndex;

                        if (normalIndex > 0)
                        {
                            Normal normal = result.Normals[normalIndex - 1];
                            gl.Normal(normal.X, normal.Y, normal.Z);
                        }
                        ObjLoader.Loader.Data.VertexData.Vertex vertex = result.Vertices[vertexIndex - 1];
                        gl.Vertex(vertex.X, vertex.Y, vertex.Z);
                    }
                }
            }

            gl.End();
        }
    }
}
