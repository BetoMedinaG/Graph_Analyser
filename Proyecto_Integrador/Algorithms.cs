﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Proyecto_Integrador
{
    class Algorithms
    {
        public List<Vertex> VertexL { get; set; }
        public List<Edge> EdgesL { get; set; }
        public Graph MyGraph { get; set; }
        //public NTree GraphTree { get; set; }
        private List<Point> lPR;
        private Vertex v_act, des;

        public Algorithms( List<Vertex> lV )
        {
            VertexL = lV;
            EdgesL = new List<Edge>();
            MyGraph = new Graph();

            foreach(Vertex cir in lV)
                foreach(Edge ari in cir.GetLA())
                    if(!EdgesL.Contains(ari))
                        EdgesL.Add(ari);
        }

        public Graph BFS( Vertex v_o, List<Point> lP )
        {
            var queue = new Queue<Vertex>();
            var visitados = new HashSet<Vertex>();

            MyGraph = new Graph();

            queue.Enqueue(v_o);
            visitados.Add(v_o);
            MyGraph.VerL.Add(v_o);

            BFS(queue, visitados, MyGraph, lP);
            return MyGraph;
        }
        private void BFS( Queue<Vertex> queue, HashSet<Vertex> visited, Graph myGraph, List<Point> lP )
        {
            v_act = queue.Dequeue();
            foreach(Edge e in v_act.GetLA())
            {
                des = e.GetDestino();
                if(!visited.Contains(des))
                {
                    visited.Add(des);
                    queue.Enqueue(des);

                    myGraph.VerL.Add(des);
                    myGraph.EdgL.AddLast(e);

                    lPR = e.GetLP();
                    lP.AddRange(e.GetLP());

                    BFS(queue, visited, myGraph, lP);

                    lPR = new List<Point>(e.GetLP());
                    lPR.Reverse();
                    lP.AddRange(lPR);
                    myGraph.EdgL.AddLast(new Edge(e.GetDestino(), e.GetOrigen(), e.GetPonderacion(), e.GetId(), lPR));
                }
            }
        }

        public Graph DFS( Vertex v_o, List<Point> lP )
        {
            var pila = new Stack<Vertex>();
            var visitados = new HashSet<Vertex>();

            MyGraph = new Graph();

            pila.Push(v_o);
            visitados.Add(v_o);
            MyGraph.VerL.Add(v_o);

            DFS(pila, visitados, MyGraph, lP);
            return MyGraph;
        }
        private void DFS( Stack<Vertex> pila, HashSet<Vertex> visited, Graph G, List<Point> lP )
        {
            v_act = pila.Pop();
            foreach(Edge e in v_act.GetLA())
            {
                des = e.GetDestino();
                if(!visited.Contains(des))
                {
                    visited.Add(des);
                    pila.Push(des);

                    G.VerL.Add(des);
                    G.EdgL.AddLast(e);

                    lPR = e.GetLP();
                    lP.AddRange(e.GetLP());

                    DFS(pila, visited, G, lP);

                    lPR = new List<Point>(e.GetLP());
                    lPR.Reverse();
                    lP.AddRange(lPR);
                    G.EdgL.AddLast(new Edge(e.GetDestino(), e.GetOrigen(), e.GetPonderacion(), e.GetId(), lPR));
                }
            }
        }

        public Graph Dijsktra( Vertex org, Vertex des, List<Point> lP )
        {
            var g = new Graph();
            var S = new List<DijElemnt>();
            var Q = new Queue<DijElemnt>();
            var DijEleL = new List<DijElemnt>();
            DijElemnt u, v;

            for(int i = 0; i < VertexL.Count; i++)
            {
                DijEleL.Add(new DijElemnt(VertexL[i]));
            }
            u = DijEleL.Find(x => x.Ver.GetId() == org.GetId());

            u.DistAcu = 0;
            u.Definitivo = true;

            while(!SolucionDij(DijEleL))//Todos no este definitivos
            {
                foreach(Edge e in u.Ver.GetLA())
                {
                    v = DijEleL.Find(x => x.Ver.GetId() == e.GetDestino().GetId());

                    if(v.DistAcu > u.DistAcu + e.GetPonderacion())
                    {
                        v.DistAcu = u.DistAcu + e.GetPonderacion();
                        v.Proveniente = u.Ver;
                    }

                }
                u = MinDist(DijEleL);
                u.Definitivo = true;

                if(u.Ver.GetId() == des.GetId())
                {
                    break;
                }
            }

            //Provenientes construir camino
            DijElemnt auxDijEle = u;//Des
            List<Point> lPR;
            g.VerL.Add(auxDijEle.Ver);

            while(auxDijEle.Ver.GetId() != org.GetId())
            {
                foreach(Edge edg in auxDijEle.Proveniente.GetLA())
                {
                    if(auxDijEle.Ver.GetId() == edg.GetDestino().GetId())
                    {
                        lPR = new List<Point>(edg.GetLP());
                        lPR.Reverse();
                        lP.AddRange(lPR);
                        //Add Edge to Tree
                        g.EdgL.AddFirst(edg);//Añade al PRINCIPIO
                        break;
                    }
                }
                auxDijEle = DijEleL.Find(x => x.Ver.GetId() == auxDijEle.Proveniente.GetId());
                //Add Vertex to Tree
                g.VerL.Add(auxDijEle.Ver);
            }

            lP.Reverse();//Begint to end
            return g;
        }
        private bool SolucionDij( List<DijElemnt> dEL )
        {
            for(int i = 0; i < dEL.Count; i++)
            {
                if(dEL[i].Definitivo == false)
                    return false;
            }
            return true;
        }

        public Graph Kruskal()
        {
            Edge edge;
            int a_1, a_2, n;
            var lLP = new List<List<Point>>();
            var link = new List<int>();
            var size = new List<double>();

            n = VertexL.Count;
            EdgesL.Sort(( x, y ) => x.GetPonderacion().CompareTo(y.GetPonderacion()));//O(nlog(n))

            for(int i = 0; i < n; i++) link.Add(i);//O(n)
            for(int i = 0; i < n; i++) size.Add(1);//O(n)

            for(int i = 0; i < EdgesL.Count; i++)
            {//O(n^2)
                edge = EdgesL[i];
                a_1 = edge.GetOrigen().GetId() - 1;
                a_2 = edge.GetDestino().GetId() - 1;

                if(!Same(a_1, a_2, link))
                {
                    Unite(a_1, a_2, link, size);
                    MyGraph.EdgL.AddLast(edge);
                }
            }
            return MyGraph;
        }
        public Graph Prim( Vertex v_i )
        {
            Edge ariMin;
            Vertex vertex = v_i;
            List<Point> lP = new List<Point>();
            int numOfVer = DFS(v_i, lP).VerL.Count;
            var aCandidatas = new List<Edge>();
            var vVistitados = new List<Vertex>();

            MyGraph = new Graph();

            while(MyGraph.VerL.Count < numOfVer - 1)
            {
                vVistitados.Add(vertex);
                MyGraph.VerL.Add(vertex);

                foreach(Edge a in vertex.GetLA())
                    aCandidatas.Add(a);

                ariMin = SelecAriMin(aCandidatas, vVistitados);

                vertex = ariMin.GetDestino();
                aCandidatas.Remove(ariMin);
                MyGraph.EdgL.AddLast(ariMin);
            }
            return MyGraph;
        }

        //Kruskal
        private int Find( int x, List<int> link )
        {
            while(x != link[x])
                x = link[x];
            return x;
        }
        private bool Same( int a, int b, List<int> link )
        {
            return Find(a, link) == Find(b, link);
        }
        private void Unite( int a, int b, List<int> link, List<double> size )
        {
            int aux;
            a = Find(a, link);
            b = Find(b, link);
            if(size[a] < size[b])
            {
                aux = a;
                a = b;
                b = aux;
            }
            size[a] += size[b];
            link[b] = a;
        }
        //Prim
        private bool Solucion( List<Edge> ARM_T )
        {
            return (ARM_T.Count == VertexL.Count - 1);
        }
        private Edge SelecAriMin( List<Edge> aCandidatas, List<Vertex> vVistitados )
        {
            Edge ariMin = new Edge(null, null, double.MaxValue, -1, null);

            foreach(Edge e in aCandidatas)
            {
                if(!vVistitados.Contains(e.GetDestino()))
                    if(e.GetPonderacion() < ariMin.GetPonderacion())
                        ariMin = e;
            }
            return ariMin;
        }
        //Dijkstra
        DijElemnt MinDist( List<DijElemnt> Q )
        {
            DijElemnt u = new DijElemnt(null);
            for(int i = 0; i < Q.Count; i++)
            {
                if(Q[i].DistAcu < u.DistAcu && Q[i].Definitivo == false)
                    u = Q[i];
            }
            return u;
        }
        //BFS
        public bool SameHeightTree( Graph Tree )
        {
            var node = Tree.EdgL.First;
            var visited = new List<Vertex>();
            int currDepth = 0, biggestDepth = int.MinValue;
            Vertex currVer;
            bool firstIter = true;

            visited.Add(node.Value.GetOrigen());

            foreach(Edge edg in Tree.EdgL)
            {
                currVer = edg.GetDestino();
                if(visited.Contains(currVer))
                {
                    if(currDepth > biggestDepth)
                    {
                        if(firstIter)
                        {
                            biggestDepth = currDepth;
                            firstIter = false;
                        } else
                        {
                            return false;
                        }
                    }
                    currDepth--;
                } else
                {
                    visited.Add(currVer);
                    currDepth++;
                }
                //System.Windows.Forms.MessageBox.Show("CurrVer: " + currVer +
                //                                   "\nBiggest Depth: " + biggestDepth +
                //                                   "\nCurrDepth: " + currDepth);
            }
            return true;
        }
    }
}
