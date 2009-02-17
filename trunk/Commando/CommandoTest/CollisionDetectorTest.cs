using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commando.collisiondetection;
using Microsoft.Xna.Framework;

namespace CommandoTest
{
    /// <summary>
    /// Summary description for CollisionDetectorTest
    /// </summary>
    [TestClass]
    public class CollisionDetectorTest
    {
        public CollisionDetectorTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void checkCollisionsTest()
        {
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(-5.0f, 0f));
            points.Add(new Vector2(-1.0f, -15.0f));
            points.Add(new Vector2(7.0f, -15.0f));
            points.Add(new Vector2(10.0f, 0f));
            points.Add(new Vector2(7.0f, 15.0f));
            points.Add(new Vector2(-1.0f, 15.0f));
            ConvexPolygon boundsPolygon = new ConvexPolygon(points, Vector2.Zero);
            boundsPolygon.rotate(new Vector2(0.0f, 1.0f), Vector2.Zero);
            float minA = 0, maxA = 0, minB = 0, maxB = 0;
            boundsPolygon.projectPolygonOnAxis(new Vector2(1.0f, 0.0f), ref minA, ref maxA);
            Assert.AreEqual(-15.0f, minA);
            Assert.AreEqual(15.0f, maxA);
            boundsPolygon.rotate(new Vector2(1.0f, 0.0f), Vector2.Zero);
            boundsPolygon.projectPolygonOnAxis(new Vector2(1.0f, 0.0f), ref minB, ref maxB);
            Assert.AreNotEqual(minA, minB);
            Assert.AreNotEqual(minA, minB);
            Assert.AreEqual(-5.0f, minB);
            Assert.AreEqual(10.0f, maxB);
        }
    }
}
