using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SkillsPlusPlus.Modifiers;

namespace Tests {
    [TestClass]
    public class ScalingTests {

        [TestInitialize]
        public void TestInitialize() {
            SkillsPlusPlus.Logger.LOG_LEVEL = SkillsPlusPlus.Logger.LogLevel.None;        
        }

        [TestMethod]
        public void AdditiveScalingTests() {

            // increments by one
            Assert.AreEqual(0, BaseSkillModifier.AdditiveScaling(0, 1, 0));
            Assert.AreEqual(1, BaseSkillModifier.AdditiveScaling(0, 1, 1));
            Assert.AreEqual(2, BaseSkillModifier.AdditiveScaling(0, 1, 2));
            Assert.AreEqual(50, BaseSkillModifier.AdditiveScaling(0, 1, 50));

            Assert.AreEqual(0, BaseSkillModifier.AdditiveScaling(0, 5, 0));
            Assert.AreEqual(5, BaseSkillModifier.AdditiveScaling(0, 5, 1));
            Assert.AreEqual(10, BaseSkillModifier.AdditiveScaling(0, 5, 2));
            Assert.AreEqual(250, BaseSkillModifier.AdditiveScaling(0, 5, 50));

            Assert.AreEqual(10, BaseSkillModifier.AdditiveScaling(10, 5, 0));
            Assert.AreEqual(15, BaseSkillModifier.AdditiveScaling(10, 5, 1));
            Assert.AreEqual(20, BaseSkillModifier.AdditiveScaling(10, 5, 2));
            Assert.AreEqual(260, BaseSkillModifier.AdditiveScaling(10, 5, 50));

            // negative values
            Assert.AreEqual(10, BaseSkillModifier.AdditiveScaling(10, -2, 0));
            Assert.AreEqual(8, BaseSkillModifier.AdditiveScaling(10, -2, 1));
            Assert.AreEqual(6, BaseSkillModifier.AdditiveScaling(10, -2, 2));
            Assert.AreEqual(-90, BaseSkillModifier.AdditiveScaling(10, -2, 50));

        }

        [TestMethod]
        public void MultScalingTests() {

            // mult scaling on zero is stil zero
            Assert.AreEqual(0, BaseSkillModifier.MultScaling(0, 1, 0));
            Assert.AreEqual(0, BaseSkillModifier.MultScaling(0, 1, 1));
            Assert.AreEqual(0, BaseSkillModifier.MultScaling(0, 1, 2));

            // +100% compound scaling (x2 per level)
            Assert.AreEqual(1, BaseSkillModifier.MultScaling(1, 1, 0));
            Assert.AreEqual(2, BaseSkillModifier.MultScaling(1, 1, 1));
            Assert.AreEqual(4, BaseSkillModifier.MultScaling(1, 1, 2));

            // +50% cpompund scaling (x1.5 per level)
            Assert.AreEqual(1, BaseSkillModifier.MultScaling(1, 0.5f, 0));
            Assert.AreEqual(1.5, BaseSkillModifier.MultScaling(1, 0.5f, 1));
            Assert.AreEqual(2.25, BaseSkillModifier.MultScaling(1, 0.5f, 2));

        }

        [TestMethod]
        public void NegativeMultScalingTests() {

            // -50% scaling
            Assert.AreEqual(1, BaseSkillModifier.MultScaling(1, 0.5f, 0));
            Assert.AreEqual(1.5, BaseSkillModifier.MultScaling(1, 0.5f, 1));
            Assert.AreEqual(2.25, BaseSkillModifier.MultScaling(1, 0.5f, 2));

        }

        [TestMethod]
        public void PreconditionFailureMultScalingTests() {

            Assert.AreEqual(1, BaseSkillModifier.MultScaling(1, -2, 0));
            Assert.AreEqual(1, BaseSkillModifier.MultScaling(1, -2, 1));
            Assert.AreEqual(1, BaseSkillModifier.MultScaling(1, -2, 2));

        }
    }
}
