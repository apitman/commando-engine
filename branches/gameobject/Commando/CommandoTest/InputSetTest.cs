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

using Commando.controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace CommandoTest
{
    /// <summary>
    /// Tests the InputSet class
    /// </summary>
    [TestClass]
    public class InputSetTest
    {
        public InputSetTest()
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
        public void TestSetters()
        {
            InputSet instance = InputSet.getInstance();
            instance.reset(); // Reset the singleton

            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            Assert.AreEqual(true, instance.getButton1());
            Assert.AreEqual(true, instance.getButton2());
            Assert.AreEqual(true, instance.getButton3());
            Assert.AreEqual(true, instance.getButton4());
            Assert.AreEqual(true, instance.getConfirmButton());
            Assert.AreEqual(true, instance.getCancelButton());
            Assert.AreEqual(true, instance.getLeftTrigger());
            Assert.AreEqual(true, instance.getRightTrigger());
            Assert.AreEqual(true, instance.getLeftBumper());
            Assert.AreEqual(true, instance.getRightBumper());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(1.0f, instance.getRightDirectionalX());
            Assert.AreEqual(1.0f, instance.getRightDirectionalY());

            instance.setButton1(false);
            instance.setButton2(false);
            instance.setButton3(false);
            instance.setButton4(false);
            instance.setConfirmButton(false);
            instance.setCancelButton(false);
            instance.setLeftBumper(false);
            instance.setRightBumper(false);
            instance.setLeftTrigger(false);
            instance.setRightTrigger(false);
            instance.setLeftDirectional(-1.0f, -1.0f);
            instance.setRightDirectional(-1.0f, -1.0f);

            Assert.AreEqual(false, instance.getButton1());
            Assert.AreEqual(false, instance.getButton2());
            Assert.AreEqual(false, instance.getButton3());
            Assert.AreEqual(false, instance.getButton4());
            Assert.AreEqual(false, instance.getConfirmButton());
            Assert.AreEqual(false, instance.getCancelButton());
            Assert.AreEqual(false, instance.getLeftTrigger());
            Assert.AreEqual(false, instance.getRightTrigger());
            Assert.AreEqual(false, instance.getLeftBumper());
            Assert.AreEqual(false, instance.getRightBumper());
            Assert.AreEqual(-1.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(-1.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(-1.0f, instance.getRightDirectionalX());
            Assert.AreEqual(-1.0f, instance.getRightDirectionalY());

        }

        [TestMethod]
        public void TestToggles()
        {
            InputSet instance = InputSet.getInstance();
            instance.reset(); // Reset the singleton

            // Set all items to being pushed
            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            // Now, set Toggles on, so that if they are pressed
            // again they will act released until actually being released
            instance.setAllToggles();

            // White box testing - using setAllToggles() and checking each
            // individual device will work fine unless two devices were
            // perfectly crossed (by the pigeonhole principle), which is
            // not very likely (and would be readily apparent during playtesting).

            // Set them again, as if the buttons were still held down
            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            // Now assert that they act released because of the toggle
            Assert.AreEqual(false, instance.getButton1());
            Assert.AreEqual(false, instance.getButton2());
            Assert.AreEqual(false, instance.getButton3());
            Assert.AreEqual(false, instance.getButton4());
            Assert.AreEqual(false, instance.getConfirmButton());
            Assert.AreEqual(false, instance.getCancelButton());
            Assert.AreEqual(false, instance.getLeftTrigger());
            Assert.AreEqual(false, instance.getRightTrigger());
            Assert.AreEqual(false, instance.getLeftBumper());
            Assert.AreEqual(false, instance.getRightBumper());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(0.0f, instance.getRightDirectionalX());
            Assert.AreEqual(0.0f, instance.getRightDirectionalY());

            // Now, act like the user released all buttons, which
            // should clear the toggles
            instance.setButton1(false);
            instance.setButton2(false);
            instance.setButton3(false);
            instance.setButton4(false);
            instance.setConfirmButton(false);
            instance.setCancelButton(false);
            instance.setLeftBumper(false);
            instance.setRightBumper(false);
            instance.setLeftTrigger(false);
            instance.setRightTrigger(false);
            instance.setLeftDirectional(0.0f, 0.0f);
            instance.setRightDirectional(0.0f, 0.0f);

            // Verify the release worked correctly
            Assert.AreEqual(false, instance.getButton1());
            Assert.AreEqual(false, instance.getButton2());
            Assert.AreEqual(false, instance.getButton3());
            Assert.AreEqual(false, instance.getButton4());
            Assert.AreEqual(false, instance.getConfirmButton());
            Assert.AreEqual(false, instance.getCancelButton());
            Assert.AreEqual(false, instance.getLeftTrigger());
            Assert.AreEqual(false, instance.getRightTrigger());
            Assert.AreEqual(false, instance.getLeftBumper());
            Assert.AreEqual(false, instance.getRightBumper());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(0.0f, instance.getRightDirectionalX());
            Assert.AreEqual(0.0f, instance.getRightDirectionalY());

            // Now the toggles should be cleared, so set all buttons
            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            // And make sure everything works normally
            Assert.AreEqual(true, instance.getButton1());
            Assert.AreEqual(true, instance.getButton2());
            Assert.AreEqual(true, instance.getButton3());
            Assert.AreEqual(true, instance.getButton4());
            Assert.AreEqual(true, instance.getConfirmButton());
            Assert.AreEqual(true, instance.getCancelButton());
            Assert.AreEqual(true, instance.getLeftTrigger());
            Assert.AreEqual(true, instance.getRightTrigger());
            Assert.AreEqual(true, instance.getLeftBumper());
            Assert.AreEqual(true, instance.getRightBumper());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(1.0f, instance.getRightDirectionalX());
            Assert.AreEqual(1.0f, instance.getRightDirectionalY());

            // Now, set Toggles on, so that if they are set
            // again they will act released until actually being released
            instance.setAllToggles();

            // And now clear all the Toggles
            instance.clearToggles();

            // Now the toggles should be cleared, so set all buttons
            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            // And make sure everything works correctly
            Assert.AreEqual(true, instance.getButton1());
            Assert.AreEqual(true, instance.getButton2());
            Assert.AreEqual(true, instance.getButton3());
            Assert.AreEqual(true, instance.getButton4());
            Assert.AreEqual(true, instance.getConfirmButton());
            Assert.AreEqual(true, instance.getCancelButton());
            Assert.AreEqual(true, instance.getLeftTrigger());
            Assert.AreEqual(true, instance.getRightTrigger());
            Assert.AreEqual(true, instance.getLeftBumper());
            Assert.AreEqual(true, instance.getRightBumper());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(1.0f, instance.getRightDirectionalX());
            Assert.AreEqual(1.0f, instance.getRightDirectionalY());
        }

        [TestMethod]
        public void TestSticks()
        {
            InputSet instance = InputSet.getInstance();
            instance.reset(); // Reset the singleton

            // Force everything to stick released for two updates
            instance.setAllSticks(2);

            // White box testing - using setAllSticks() and checking each
            // individual device will work fine unless two devices were
            // perfectly crossed (by the pigeonhole principle), which is
            // not very likely (and would be readily apparent during playtesting).

            // Now try to tell it that everything is still held down
            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            // Make sure the Stick is working and devices act released
            Assert.AreEqual(false, instance.getButton1());
            Assert.AreEqual(false, instance.getButton2());
            Assert.AreEqual(false, instance.getButton3());
            Assert.AreEqual(false, instance.getButton4());
            Assert.AreEqual(false, instance.getConfirmButton());
            Assert.AreEqual(false, instance.getCancelButton());
            Assert.AreEqual(false, instance.getLeftTrigger());
            Assert.AreEqual(false, instance.getRightTrigger());
            Assert.AreEqual(false, instance.getLeftBumper());
            Assert.AreEqual(false, instance.getRightBumper());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(0.0f, instance.getRightDirectionalX());
            Assert.AreEqual(0.0f, instance.getRightDirectionalY());

            // Try again for update #2
            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            // Make sure the Stick is working and devices act released
            Assert.AreEqual(false, instance.getButton1());
            Assert.AreEqual(false, instance.getButton2());
            Assert.AreEqual(false, instance.getButton3());
            Assert.AreEqual(false, instance.getButton4());
            Assert.AreEqual(false, instance.getConfirmButton());
            Assert.AreEqual(false, instance.getCancelButton());
            Assert.AreEqual(false, instance.getLeftTrigger());
            Assert.AreEqual(false, instance.getRightTrigger());
            Assert.AreEqual(false, instance.getLeftBumper());
            Assert.AreEqual(false, instance.getRightBumper());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(0.0f, instance.getRightDirectionalX());
            Assert.AreEqual(0.0f, instance.getRightDirectionalY());

            // Now, the stick should have worn off
            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            // Verify it wore off correctly
            Assert.AreEqual(true, instance.getButton1());
            Assert.AreEqual(true, instance.getButton2());
            Assert.AreEqual(true, instance.getButton3());
            Assert.AreEqual(true, instance.getButton4());
            Assert.AreEqual(true, instance.getConfirmButton());
            Assert.AreEqual(true, instance.getCancelButton());
            Assert.AreEqual(true, instance.getLeftTrigger());
            Assert.AreEqual(true, instance.getRightTrigger());
            Assert.AreEqual(true, instance.getLeftBumper());
            Assert.AreEqual(true, instance.getRightBumper());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(1.0f, instance.getRightDirectionalX());
            Assert.AreEqual(1.0f, instance.getRightDirectionalY());

            // Force everything to stick released for two updates
            instance.setAllSticks(2);

            // And then we can test that clearing them works
            instance.clearSticks();

            // The stick should be cleared, so we can set everything
            instance.setButton1(true);
            instance.setButton2(true);
            instance.setButton3(true);
            instance.setButton4(true);
            instance.setConfirmButton(true);
            instance.setCancelButton(true);
            instance.setLeftBumper(true);
            instance.setRightBumper(true);
            instance.setLeftTrigger(true);
            instance.setRightTrigger(true);
            instance.setLeftDirectional(1.0f, 1.0f);
            instance.setRightDirectional(1.0f, 1.0f);

            // And now verify that the sticks cleared correctly
            Assert.AreEqual(true, instance.getButton1());
            Assert.AreEqual(true, instance.getButton2());
            Assert.AreEqual(true, instance.getButton3());
            Assert.AreEqual(true, instance.getButton4());
            Assert.AreEqual(true, instance.getConfirmButton());
            Assert.AreEqual(true, instance.getCancelButton());
            Assert.AreEqual(true, instance.getLeftTrigger());
            Assert.AreEqual(true, instance.getRightTrigger());
            Assert.AreEqual(true, instance.getLeftBumper());
            Assert.AreEqual(true, instance.getRightBumper());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(1.0f, instance.getRightDirectionalX());
            Assert.AreEqual(1.0f, instance.getRightDirectionalY());
        }

        [TestMethod]
        public void TestInstances()
        {
            InputSet inst = InputSet.getInstance();
            InputSet p1 = InputSet.getInstance(PlayerIndex.One);
            InputSet p2 = InputSet.getInstance(PlayerIndex.Two);
            InputSet p3 = InputSet.getInstance(PlayerIndex.Three);
            InputSet p4 = InputSet.getInstance(PlayerIndex.Four);

            // Verify getInstance() returns player 1
            Assert.AreSame(inst, p1);

            // And now set each player to different values to make
            // sure they all work independently
            p1.setLeftDirectional(-1.0f, -1.0f);
            p2.setLeftDirectional(0.0f, 0.0f);
            p3.setLeftDirectional(1.0f, 1.0f);
            p4.setLeftDirectional(0.5f, 0.5f);

            Assert.AreEqual(p1.getLeftDirectionalX(), -1.0f);
            Assert.AreEqual(p2.getLeftDirectionalX(), 0.0f);
            Assert.AreEqual(p3.getLeftDirectionalX(), 1.0f);
            Assert.AreEqual(p4.getLeftDirectionalX(), 0.5f);
        }

    }
}
