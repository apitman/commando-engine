/*
 ***************************************************************************
 * Copyright 2009 Eric Barnes, Ken Hartsook, Andrew Pitman, & Jared Segal  *
 *                                                                         *
 * Licensed under the Apache License, Version 2.0 (the "License");         *
 * you may not use this file except in compliance with the License.        *
 * You may obtain a copy of the License at                                 *
 *                                                                         *
 * http://www.apache.org/licenses/LICENSE-2.0                              *
 *                                                                         *
 * Unless required by applicable law or agreed to in writing, software     *
 * distributed under the License is distributed on an "AS IS" BASIS,       *
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.*
 * See the License for the specific language governing permissions and     *
 * limitations under the License.                                          *
 ***************************************************************************
*/




using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commando;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CommandoTest
{
    /// <summary>
    /// Summary description for MenuListTest
    /// </summary>
    [TestClass]
    public class MenuListTest
    {
        public MenuListTest()
        {
            List<string> testStringList = new List<string>();
            testStringList.Add("test");
            testStringList.Add("test");
            testStringList.Add("test");
            testMenuList_ = new MenuList(testStringList, Vector2.Zero);
            
        }
        private MenuList testMenuList_;
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test calculateExactPath.
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
        // Use ClassInitialize to calculateExactPath code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to calculateExactPath code after all tests in a class have calculateExactPath
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to calculateExactPath code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to calculateExactPath code after each test has calculateExactPath
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestIncrementCursor()
        {
            Assert.AreEqual(0, testMenuList_.getCursorPos());
            testMenuList_.incrementCursorPos();
            Assert.AreEqual(1, testMenuList_.getCursorPos());
            testMenuList_.incrementCursorPos();
            Assert.AreEqual(2, testMenuList_.getCursorPos());
            testMenuList_.incrementCursorPos();
            //this is a three item menu, so the cursor should wrap
            //back to the first item
            Assert.AreEqual(0, testMenuList_.getCursorPos());
            for (int i = 0; i < 90; i++)
            {
                testMenuList_.decrementCursorPos();
            }
            Assert.AreEqual(0, testMenuList_.getCursorPos());
        }
        public void TestDecrementCursor()
        {
            Assert.AreEqual(0, testMenuList_.getCursorPos());
            testMenuList_.decrementCursorPos();
            Assert.AreEqual(2, testMenuList_.getCursorPos());
            testMenuList_.decrementCursorPos();
            Assert.AreEqual(1, testMenuList_.getCursorPos());
            testMenuList_.decrementCursorPos();
            //this is a three item menu, so the cursor should wrap
            //back to the first item
            Assert.AreEqual(0, testMenuList_.getCursorPos());
            for (int i = 0; i < 90; i++)
            {
                testMenuList_.decrementCursorPos();
            }
            Assert.AreEqual(0, testMenuList_.getCursorPos());
        }
    }
}
