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
        private PCControllerInput controller_;

        public InputSetTest()
        {
            //
            // TODO: Add constructor logic here
            //
            controller_ = new PCControllerInput(null);
        }

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
        public void TestSetters()
        {
            InputSet instance = InputSet.getInstance();
            instance.reset(); // Reset the singleton

            instance.update(controller_);
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);

            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_BUMPER));
            Assert.AreEqual(1.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(1.0f, instance.getRightDirectionalX());
            Assert.AreEqual(1.0f, instance.getRightDirectionalY());

            instance.update(controller_);
            instance.setButton(InputsEnum.BUTTON_1, false);
            instance.setButton(InputsEnum.BUTTON_2, false);
            instance.setButton(InputsEnum.BUTTON_3, false);
            instance.setButton(InputsEnum.BUTTON_4, false);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, false);
            instance.setButton(InputsEnum.CANCEL_BUTTON, false);
            instance.setButton(InputsEnum.LEFT_BUMPER, false);
            instance.setButton(InputsEnum.RIGHT_BUMPER, false);
            instance.setButton(InputsEnum.LEFT_TRIGGER, false);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, false);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, -1.0f, -1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, -1.0f, -1.0f);

            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_BUMPER));
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
            instance.update(controller_);
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);

            // Now, set Toggles on, so that if they are pressed
            // again they will act released until actually being released
            instance.setAllToggles();

            // White box testing - using setAllToggles() and checking each
            // individual device will work fine unless two devices were
            // perfectly crossed (by the pigeonhole principle), which is
            // not very likely (and would be readily apparent during playtesting).

            // Set them again, as if the buttons were still held down
            instance.update(controller_);
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);

            // Now assert that they act released because of the toggle
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_BUMPER));
            Assert.AreEqual(0.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(0.0f, instance.getRightDirectionalX());
            Assert.AreEqual(0.0f, instance.getRightDirectionalY());

            // Now, act like the user released all buttons, which
            // should clear the toggles
            instance.update(controller_);
            instance.setButton(InputsEnum.BUTTON_1, false);
            instance.setButton(InputsEnum.BUTTON_2, false);
            instance.setButton(InputsEnum.BUTTON_3, false);
            instance.setButton(InputsEnum.BUTTON_4, false);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, false);
            instance.setButton(InputsEnum.CANCEL_BUTTON, false);
            instance.setButton(InputsEnum.LEFT_BUMPER, false);
            instance.setButton(InputsEnum.RIGHT_BUMPER, false);
            instance.setButton(InputsEnum.LEFT_TRIGGER, false);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, false);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 0.0f, 0.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 0.0f, 0.0f);

            // Verify the release worked correctly
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_BUMPER));
            Assert.AreEqual(0.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(0.0f, instance.getRightDirectionalX());
            Assert.AreEqual(0.0f, instance.getRightDirectionalY());

            // Now the toggles should be cleared, so set all buttons
            instance.update(controller_);
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);

            // And make sure everything works normally
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_BUMPER));
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
            instance.update(controller_);
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);

            // And make sure everything works correctly
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_BUMPER));
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
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);
            instance.update(controller_);

            // Make sure the Stick is working and devices act released
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_BUMPER));
            Assert.AreEqual(0.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(0.0f, instance.getRightDirectionalX());
            Assert.AreEqual(0.0f, instance.getRightDirectionalY());

            // Try again for update #2
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);
            instance.update(controller_);

            // Make sure the Stick is working and devices act released
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(false, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(false, instance.getButton(InputsEnum.RIGHT_BUMPER));
            Assert.AreEqual(0.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(0.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(0.0f, instance.getRightDirectionalX());
            Assert.AreEqual(0.0f, instance.getRightDirectionalY());

            // Now, the stick should have worn off
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);
            instance.update(controller_);

            // Verify it wore off correctly
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_BUMPER));
            Assert.AreEqual(1.0f, instance.getLeftDirectionalX());
            Assert.AreEqual(1.0f, instance.getLeftDirectionalY());
            Assert.AreEqual(1.0f, instance.getRightDirectionalX());
            Assert.AreEqual(1.0f, instance.getRightDirectionalY());

            // Force everything to stick released for two updates
            instance.setAllSticks(2);

            // And then we can test that clearing them works
            instance.clearSticks();

            // The stick should be cleared, so we can set everything
            instance.setButton(InputsEnum.BUTTON_1, true);
            instance.setButton(InputsEnum.BUTTON_2, true);
            instance.setButton(InputsEnum.BUTTON_3, true);
            instance.setButton(InputsEnum.BUTTON_4, true);
            instance.setButton(InputsEnum.CONFIRM_BUTTON, true);
            instance.setButton(InputsEnum.CANCEL_BUTTON, true);
            instance.setButton(InputsEnum.LEFT_BUMPER, true);
            instance.setButton(InputsEnum.RIGHT_BUMPER, true);
            instance.setButton(InputsEnum.LEFT_TRIGGER, true);
            instance.setButton(InputsEnum.RIGHT_TRIGGER, true);
            instance.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            instance.setDirectional(InputsEnum.RIGHT_DIRECTIONAL, 1.0f, 1.0f);
            instance.update(controller_);

            // And now verify that the sticks cleared correctly
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_1));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_2));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_3));
            Assert.AreEqual(true, instance.getButton(InputsEnum.BUTTON_4));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CONFIRM_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.CANCEL_BUTTON));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_TRIGGER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.LEFT_BUMPER));
            Assert.AreEqual(true, instance.getButton(InputsEnum.RIGHT_BUMPER));
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
            p1.update(controller_);
            p2.update(controller_);
            p3.update(controller_);
            p4.update(controller_);
            p1.setDirectional(InputsEnum.LEFT_DIRECTIONAL, -1.0f, -1.0f);
            p2.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 0.0f, 0.0f);
            p3.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 1.0f, 1.0f);
            p4.setDirectional(InputsEnum.LEFT_DIRECTIONAL, 0.5f, 0.5f);

            Assert.AreEqual(p1.getLeftDirectionalX(), -1.0f);
            Assert.AreEqual(p2.getLeftDirectionalX(), 0.0f);
            Assert.AreEqual(p3.getLeftDirectionalX(), 1.0f);
            Assert.AreEqual(p4.getLeftDirectionalX(), 0.5f);
        }

    }
}
