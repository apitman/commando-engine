using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Commando;
using Commando.controls;

namespace CommandoTest
{
    /// <summary>
    /// Summary description for EngineStatePauseTest
    /// </summary>
    [TestClass]
    public class EngineStatePauseTest
    {
        public EngineStatePauseTest()
        {
            //
            // TODO: Add constructor logic here
            //
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
        public void TestEngineStatePause()
        {
            // This test is now defunct
            //Engine engine = new Engine();
            //EngineStateGameplay gameplayState = new EngineStateGameplay(engine);
            //EngineStatePause pauseState = new EngineStatePause(engine, gameplayState);
            //InputSet.getInstance().setButton(InputsEnum.CONFIRM_BUTTON, true);
            //EngineStateGameplay returnedState = (EngineStateGameplay)pauseState.update(new GameTime());

            //Assert.AreSame(gameplayState, returnedState);
            //Assert.AreEqual<EngineStateGameplay>(gameplayState, returnedState);
            //Assert.AreEqual(gameplayState, returnedState);
        }
    }
}
