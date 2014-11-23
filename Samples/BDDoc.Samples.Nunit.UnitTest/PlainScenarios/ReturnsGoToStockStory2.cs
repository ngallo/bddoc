using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.Samples.Nunit.UnitTest.PlainScenarios
{
    [ExcludeFromCodeCoverage]
    [StoryInfo("NUnit-ReturnsGoToStockStory2", GroupName = "Component2")]
    [Story("Returns go to stock 2")]
    [InOrderTo("keep track of stock")]
    [AsA("store owner")]
    [IWantTo("add items back to stock when they're returned")]
    public class ReturnsGoToStockStory2 : IStory
    {
        [Test]
        [Scenario("Refunded items should be returned to stock", Order = 5)]
        public void RefundedItemsReturnedToStockTest()
        {
            var scenario = this.CreateScenario();
            scenario.Given("a customer previously bought a black sweater from me");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.And("I currently have three black sweaters left in stock");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.When("he returns the sweater for a refund");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.Then("I should have four black sweaters in stock");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.Complete();
        }

        [Test]
        [Scenario("Replaced items should be returned to stock")]
        [CustomScenarioAttribute1("Custom1.1d", Order = 4)]
        [CustomScenarioAttribute2("Custom2.1b", Order = 2)]
        [CustomScenarioAttribute1("Custom1.2a", Order = 1)]
        [CustomScenarioAttribute2("Custom2.2c", Order = 3)]
        public void ReplacedItemsReturnedToStockTest()
        {
            var scenario = this.CreateScenario();
            scenario.Given("that a customer buys a blue garment");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.And("I have two blue garments in stock");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.And("three black garments in stock.");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.When("he returns the garment for a replacement in black,");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.Then("I should have three blue garments in stock");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.And("two black garments in stock");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.Complete();
        }
    }
}
