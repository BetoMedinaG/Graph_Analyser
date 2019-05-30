﻿using System;
using System.Drawing;
using System.Collections.Generic;

namespace Proyecto_Integrador
{
	public class Vertex
	{
		int r;
		int id;
		Point p;
		List<Edge> lA;
		
		public Vertex(Point p, int r, int id)
		{
			this.p = p;
			this.r = r;
			this.id = id;
			lA = new List<Edge>();
		}
		
		public void AddArista(Vertex origen, Vertex destino, double pond, int id, List<Point> lP)
		{
			lA.Add(new Edge(origen, destino, pond, id, lP));
		}
		
		public List<Edge> getLA()
		{
			return lA;
		}
		
		public void SetId(int id)
		{
			this.id = id;
		}
		
		public void SetP(Point p){
			this.p = p;
		}
		
		public void SetR(int r){
			this.r = r;
		}
		
		public void SetLA(List<Edge> lA){
			this.lA = lA;
		}
		
		public int GetX()
		{
			return p.X;
		}
		
		public int GetY()
		{
			return p.Y;
		}
		
		public int GetR()
		{
			return r;
		}
		
		public int GetId()
		{
			return id;
		}
		
		public Point GetPoint()
		{
			return p;
		}
		
		public override string ToString()
		{
			return string.Format("Vertice {0}", id);
		}
	}
}
