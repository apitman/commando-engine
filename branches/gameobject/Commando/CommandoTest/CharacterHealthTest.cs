using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commando;
using Commando.objects;

namespace CommandoTest
{
    /// <summary>
    /// Summary description for CharacterHealthTest
    /// </summary>
    [TestClass]
    public class CharacterHealthTest
    {
        public CharacterHealthTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private class CharacterStatusObserverConcreteTester : CharacterStatusObserverInterface
        {
            private int value_;

            public CharacterStatusObserverConcreteTester()
            {
                value_ = 0;
            }

            public void notifyOfChange(int value)
            {
                value_ = value;
            }

            public int getValue()
            {
                return value_;
            }
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
        public void TestNotify()
        {
            CharacterHealth instance = new CharacterHealth();
            CharacterStatusObserverConcreteTester observer = new CharacterStatusObserverConcreteTester();

            instance.addObserver(observer);
            instance.update(50);

            Assert.AreEqual(50, observer.getValue());
            instance.update(60);

            Assert.AreEqual(60, observer.getValue());

            CharacterStatusObserverConcreteTester observer2 = new CharacterStatusObserverConcreteTester();
            CharacterStatusObserverConcreteTester observer3 = new CharacterStatusObserverConcreteTester();
            CharacterStatusObserverConcreteTester observer4 = new CharacterStatusObserverConcreteTester();

            instance.addObserver(observer2);
            instance.addObserver(observer3);
            instance.addObserver(observer4);

            instance.update(71);

            Assert.AreEqual(71, observer.getValue());
            Assert.AreEqual(71, observer2.getValue());
            Assert.AreEqual(71, observer3.getValue());
            Assert.AreEqual(71, observer4.getValue());

            instance.update(72);
            instance.update(73);

            Assert.AreEqual(73, observer.getValue());
            Assert.AreEqual(73, observer2.getValue());
            Assert.AreEqual(73, observer3.getValue());
            Assert.AreEqual(73, observer4.getValue());
        }
    }
}
