﻿using BDDoc.Core;
using BDDoc.UnitTest.Fakes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BDDoc.UnitTest
{
    [ExcludeFromCodeCoverage]
    public class ScenarioFactoryTests
    {
        //Tests

        [Test]
        public void CreateScenarioFactory_WithoutPassAnInstanceOfReflectionHelper_ThorowAnException()
        {
            Assert.Throws<ArgumentNullException>(() => new ScenarioFactory(null));
        }

        [Test]
        public void UsingTheScenarioFactory_ToCreateAPlainScenario_UsesTheReflectionHelper()
        {
            const string storyId = "STORYID";
            const string storyTxt = "STORY";
            const int storyOrder = 11;
            const string inOrderToTxt = "INORDERTO";
            const int inOrderToOrder = 12;
            const string asAtxt = "ASA";
            const int asAtxtOrder = 13;
            const string iWantTxt = "IWANTTO";
            const int iWantTxtOrder = 14;
            const string scenarioTxt = "SCENARIO";
            const int scenarioOrder = 15;

            var storyInfoAttrib = new StoryInfoAttribute(storyId);

            IList<IStoryAttrib> storyAttribs = new List<IStoryAttrib>();
            storyAttribs.Add(new StoryAttribute(storyTxt) { Order = storyOrder});
            storyAttribs.Add(new InOrderToAttribute(inOrderToTxt) { Order = inOrderToOrder });
            storyAttribs.Add(new AsAAttribute(asAtxt) { Order = asAtxtOrder });
            storyAttribs.Add(new IWantToAttribute(iWantTxt) { Order = iWantTxtOrder });
            
            IList<IScenarioAttrib> scenarioAttribs = new List<IScenarioAttrib>();
            scenarioAttribs.Add(new ScenarioAttribute(scenarioTxt) { Order = scenarioOrder });

            var reflectionFake = new ReflectionHelperFake(storyInfoAttrib, storyAttribs, scenarioAttribs);
            var scenarioFactory = new ScenarioFactory(reflectionFake);
            var plainScenario = scenarioFactory.CreateScenario(1);

            Assert.NotNull(plainScenario);
            Assert.AreEqual(storyInfoAttrib, plainScenario.StoryInfoAttribute);
            Assert.AreEqual(4, plainScenario.StoryAttributes.Count);
            Assert.AreEqual(storyTxt, plainScenario.StoryAttributes.ElementAt(0).Text);
            Assert.AreEqual(storyOrder, plainScenario.StoryAttributes.ElementAt(0).Order);
            Assert.AreEqual(inOrderToTxt, plainScenario.StoryAttributes.ElementAt(1).Text);
            Assert.AreEqual(inOrderToOrder, plainScenario.StoryAttributes.ElementAt(1).Order);
            Assert.AreEqual(asAtxt, plainScenario.StoryAttributes.ElementAt(2).Text);
            Assert.AreEqual(asAtxtOrder, plainScenario.StoryAttributes.ElementAt(2).Order);
            Assert.AreEqual(iWantTxt, plainScenario.StoryAttributes.ElementAt(3).Text);
            Assert.AreEqual(iWantTxtOrder, plainScenario.StoryAttributes.ElementAt(3).Order);

            Assert.AreEqual(1, plainScenario.ScenarioAttributes.Count);
            Assert.AreEqual(scenarioTxt, plainScenario.ScenarioAttributes.ElementAt(0).Text);
            Assert.AreEqual(scenarioOrder, plainScenario.ScenarioAttributes.ElementAt(0).Order);
        }
    }
}
