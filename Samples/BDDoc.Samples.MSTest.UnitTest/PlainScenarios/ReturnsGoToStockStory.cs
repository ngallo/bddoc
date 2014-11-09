using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.Samples.MSTest.UnitTest.PlainScenarios
{
    [ExcludeFromCodeCoverage]
    [StoryInfo("MSTest-ReturnsGoToStockStory")]
    [Story("Returns go to stock")]
    [InOrderTo("keep track of stock")]
    [AsA("store owner")]
    [IWantTo("add items back to stock when they're returned")]
    [TestClass]
    public class ReturnsGoToStockStory
    {
        [TestMethod]
        [Scenario("Refunded items should be returned to stock")]
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

        [TestMethod]
        [Scenario("Replaced items should be returned to stock")]
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
