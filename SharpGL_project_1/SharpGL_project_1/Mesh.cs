using System;
using System.Collections.Generic;
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

        public Mesh(ObjLoader.Loader.Loaders.LoadResult result1, Vector3 position)
        {
            // TODO: Complete member initialization
            this.result = result1;
            this.position = position;
        }

        public void Draw(OpenGL gl)
        {
            gl.Color(1.0f, 0.0f, 0.0f, 1.0f);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var group in result.Groups)
            {
                foreach (var face in group.Faces)
                {
                    for (int i = 0; i < face.Count; ++i)
                    {
                        int normalIndex = face[i].NormalIndex;
                        int vertexIndex = face[i].VertexIndex;

                        Normal normal = result.Normals[normalIndex - 1];
                        gl.Normal(normal.X, normal.Y, normal.Z);
                        ObjLoader.Loader.Data.VertexData.Vertex vertex = result.Vertices[vertexIndex - 1];
                        gl.Vertex(vertex.X, vertex.Y, vertex.Z);
                    }
                }
            }

            gl.End();
        }
    }
}
