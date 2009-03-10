using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Commando.levels;

namespace Commando.collisiondetection
{
    public interface ConvexPolygonInterface
    {
        Vector2 getCenter();

        Vector2 getEdge(int edgeNumber);
        
        int getNumberOfPoints();
        
        Vector2 getPoint(int index);
        
        Vector2[] getPoints();
        
        void projectPolygonOnAxis(Vector2 axis, Height height, ref float min, ref float max);
        
        void rotate(Vector2 newAxis, Vector2 position);

        void translate(Vector2 translation);
    }
}
