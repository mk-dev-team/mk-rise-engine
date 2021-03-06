﻿using Maker.Rise.Framework.Primitives;
using Maker.Rise.Framework.Ressource.RessourceType;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maker.Rise.Framework.Ressource.RessourceImporter
{
    public class ModelImporter : IRessourceImporter
    {
        public string RessourceTypeName => "model";

        public IRessource Import(RessourceManager ressourceManager, string fileName)
        {
            List<Vertex> vertecies = new List<Vertex>();
            List<Vector3> normals = new List<Vector3>();
            List<Point2D> textureCoords = new List<Point2D>();
            List<int> Indecies = new List<int>();

            foreach (var line in File.ReadAllLines(fileName))
            {
                string[] token = line.Split(' ');


                switch (token[0])
                {
                    case "v":
                        vertecies.Add(new Vertex(new Vector3(float.Parse(token[1], CultureInfo.InvariantCulture), float.Parse(token[2], CultureInfo.InvariantCulture), float.Parse(token[3], CultureInfo.InvariantCulture))));
                        break;
                    case "vt":
                        textureCoords.Add(new Point2D(float.Parse(token[1], CultureInfo.InvariantCulture), 1f - float.Parse(token[2], CultureInfo.InvariantCulture)));
                        break;
                    case "vn":
                        normals.Add(new Vector3(float.Parse(token[1], CultureInfo.InvariantCulture), float.Parse(token[2], CultureInfo.InvariantCulture), float.Parse(token[3], CultureInfo.InvariantCulture)));
                        break;
                    case "f":
                        for (int i = 0; i < 3; i++)
                        {
                            string[] faceToken = token[1 + i].Split('/');
                            int vertexIndex = int.Parse(faceToken[0]) - 1;
                            Vertex v = vertecies[vertexIndex];
                            v.TextureCoordinate = textureCoords[int.Parse(faceToken[1]) - 1];
                            v.Normal = normals[int.Parse(faceToken[2]) - 1];
                            Indecies.Add(vertexIndex);
                        }
                        break;
                    default:
                        break;
                }
            }

            return new Model(vertecies.ToArray(), Indecies.ToArray());
        }

        public void Destroy()
        {

        }
    }
}
