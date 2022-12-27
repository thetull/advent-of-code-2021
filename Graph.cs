using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Graph
    {

        private Dictionary<string, Vertex> Vertices;
        private Dictionary<Vertex, HashSet<Edge>> Connections;
        private HashSet<Edge> Edges;
        public Graph()
        {
            Vertices = new Dictionary<string, Vertex>();
            Connections = new Dictionary<Vertex, HashSet<Edge>>();
            Edges = new HashSet<Edge>();
        }

        public void AddVertex(Vertex v)
        {
            if (Vertices.ContainsKey(v.Identifier))
                return;
            Vertices[v.Identifier] = v;
            Connections[v] = new HashSet<Edge>();
        }

        private void CreateEdgeBetween(Vertex origin, Vertex destination, int distance)
        {
            if (origin.Equals(destination))
                return;
            Edge e = new Edge(origin, destination, distance);
            AddEdge(e);
        }

        public void CreateEdgeBetween(string origin, string destination, int distance)
        {
            CreateEdgeBetween(new Vertex(origin), new Vertex(destination), distance);
        }

        public HashSet<Vertex> GetConnectedVertices(Vertex v)
        {
            HashSet<Vertex> connectedVertices = new HashSet<Vertex>();
            foreach (Edge e in Connections[v])
            {
                connectedVertices.Add(e.Destination);
                connectedVertices.Add(e.Origin);
            }

            connectedVertices.Remove(v);
            return connectedVertices;
        }

        public HashSet<string> GetConnectedIdentifiers(Vertex v)
        {
            return GetConnectedVertices(v).Select(x => x.Identifier).ToHashSet();
        }

        public HashSet<string> GetConnectedIdentifiers(string identifier)
        {
            return GetConnectedIdentifiers(new Vertex(identifier));
        }

        public HashSet<Edge> getVertexEdges(Vertex v)
        {
            HashSet<Edge> temp = Connections[v];
            if (temp == null)
                return null;
            HashSet<Edge> copy = new HashSet<Edge>(temp);
            return copy;
        }

        private void AddEdge(Edge e)
        {
            if (e == null)
            {
                return;
            }

            Vertex v1 = e.Origin;
            Vertex v2 = e.Destination;
            

            // Add the vertices to the graph
            AddVertex(v1);
            AddVertex(v2);

            Connections[v1].Add(e);
            Connections[v2].Add(e);

            Edges.Add(e);
        }

        public int DistanceBetween(Vertex origin, Vertex destination)
        {
            Dictionary<Vertex, int> costs = new Dictionary<Vertex, int>();
            HashSet<Vertex> completedVertices = new HashSet<Vertex>();
            costs.Add(origin, 0);
            Vertex current = origin;
            int currentDistance = 0;
            while (!current.Equals(destination))
            {
                //System.out.println("Pre Distance: "+currentDistance);
                //System.out.println(current.getIdentifier());
                foreach (Edge e in this.getVertexEdges(current))
                {
                    
                    Vertex newVertex = e.getVertices().Item2;
                    if (completedVertices.Contains(newVertex))
                        continue;
                    if (!costs.ContainsKey(newVertex))
                    {
                        costs[newVertex] = currentDistance + e.Distance;
                    }
                    else
                    {
                        int currentCost = costs[newVertex];
                        if (currentDistance + e.Distance < currentCost)
                        {
                            costs[newVertex] = currentDistance + e.Distance;
                        }
                    }
                }
                completedVertices.Add(current);
                Tuple<Vertex, int> best = null;
                foreach (Vertex candidate in costs.Keys)
                {
                    if (completedVertices.Contains(candidate))
                    {
                        continue;
                    }

                    int vertexCost = costs[candidate];
                    if (best == null)
                    {
                        best = new Tuple<Vertex, int>(candidate, vertexCost);
                        continue;
                    }

                    if (vertexCost < best.Item2)
                    {
                        best = new Tuple<Vertex, int>(candidate, vertexCost);
                        continue;
                    }
                }
                current = best.Item1;
                currentDistance = best.Item2;
                //System.out.println("Current Distance: "+currentDistance);
            }

            return currentDistance;
        }
    }

    internal class Vertex : IEquatable<Vertex>
    {
        public bool Equals(Vertex other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Identifier == other.Identifier;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Vertex)obj);
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        public readonly string Identifier;


        public Vertex(string identifier)
        {
            Identifier = identifier;
        }

    }

    internal class Edge : IEquatable<Edge>
    {

        public readonly int Distance;
        public readonly Vertex Origin;
        public readonly Vertex Destination;
        public Edge(Vertex origin, Vertex destination, int distance)
        {
            Origin = origin;
            Destination = destination;
            Distance = distance;
        }

        public Tuple<Vertex, Vertex> getVertices()
        {
            return new Tuple<Vertex, Vertex>(this.Origin, this.Destination);
        }

        public bool Equals(Edge other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Distance == other.Distance && Equals(Origin, other.Origin) && Equals(Destination, other.Destination);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Edge)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Distance, Origin, Destination);
        }
    }
}
