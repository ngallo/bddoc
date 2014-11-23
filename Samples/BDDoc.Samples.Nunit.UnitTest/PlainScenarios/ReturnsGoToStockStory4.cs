using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.Samples.Nunit.UnitTest.PlainScenarios
{
    [ExcludeFromCodeCoverage]
    [StoryInfo("NUnit-ReturnsGoToStockStory4", GroupName = "Component2")]
    [Story("Returns go to stock 4")]
    [InOrderTo("keep track of stock")]
    [AsA("store owner")]
    [IWantTo("add items back to stock when they're returned")]
    public class ReturnsGoToStockStory4 : IStory
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
    }
}
