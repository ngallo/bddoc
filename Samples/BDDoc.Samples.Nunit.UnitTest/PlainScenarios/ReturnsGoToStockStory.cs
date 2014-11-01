using NUnit.Framework;

namespace BDDoc.Samples.Nunit.UnitTest.PlainScenarios
{
    [Story("Returns go to stock")]
    [InOrderTo("keep track of stock")]
    [AsA("store owner")]
    [IWantTo("add items back to stock when they're returned")]
    public class ReturnsGoToStockStory
    {
        [Test]
        [Scenario("Refunded items should be returned to stock")]
        public void RefundedItemsReturnedToStockTest()
        {
            var scenario = this.CreatePlainScenario();
            scenario.Given("a customer previously bought a black sweater from me");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.And("I currently have three black sweaters left in stock");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.When("he returns the sweater for a refund");
            //----------------------------------------------------------------------------------------------------------------//

            scenario.Then("I should have four black sweaters in stock");
            //----------------------------------------------------------------------------------------------------------------//
        }

        [Test]
        [Scenario("Replaced items should be returned to stock")]
        public void ReplacedItemsReturnedToStockTest()
        {
            var scenario = this.CreatePlainScenario();
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
        }
    }
}
