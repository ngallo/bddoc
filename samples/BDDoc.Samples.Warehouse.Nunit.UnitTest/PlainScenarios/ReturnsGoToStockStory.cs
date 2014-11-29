using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.Samples.Warehouse.Nunit.UnitTest.PlainScenarios
{
    [ExcludeFromCodeCoverage]
    [StoryInfo("NUnit-ReturnsGoToStockStory", GroupName = "Warehouse")]
    [Story("Returns go to stock")]
    [InOrderTo("keep track of stock")]
    [AsA("store owner")]
    [IWantTo("add items back to stock when they're returned")]
    public class ReturnsGoToStockStory : IStory
    {
        [Test]
        [Scenario("Refunded items should be returned to stock")]
        public void RefundedItemsReturnedToStockTest()
        {
            var scenario = this.CreateScenario();
            scenario.Given("a customer previously bought a black sweater from me");
            //----------------------------------------------------------------------------------------------------------------//
            var sweaters = new[]
                {
                    new Sweater(Color.Black), new Sweater(Color.Black), new Sweater(Color.Black), new Sweater(Color.Black),
                    new Sweater(Color.Blue), new Sweater(Color.Blue), new Sweater(Color.Yellow), new Sweater(Color.Yellow)
                };
            var stock = new Stock<Sweater>(sweaters);

            var boughtSweater = stock.BuyItem(Color.Black);
            Assert.IsNotNull(boughtSweater);

            scenario.And("I currently have three black sweaters left in stock");
            //----------------------------------------------------------------------------------------------------------------//
            var leftBalckSweatersCount = stock.GetNumberOfLeftItems(Color.Black);
            Assert.AreEqual(3, leftBalckSweatersCount);


            scenario.When("he returns the sweater for a refund");
            //----------------------------------------------------------------------------------------------------------------//
            var returned = stock.ReturnItem(boughtSweater);
            Assert.IsTrue(returned);

            scenario.Then("I should have four black sweaters in stock");
            //----------------------------------------------------------------------------------------------------------------//
            leftBalckSweatersCount = stock.GetNumberOfLeftItems(Color.Black);
            Assert.AreEqual(4, leftBalckSweatersCount);

            scenario.Complete();
        }

        [Test]
        [Scenario("Replaced items should be returned to stock")]
        public void ReplacedItemsReturnedToStockTest()
        {
            var scenario = this.CreateScenario();
            scenario.Given("that a customer buys a blue jacket");
            //----------------------------------------------------------------------------------------------------------------//
            var jackets = new[]
                {
                    new Jacket(Color.Black), new Jacket(Color.Black), new Jacket(Color.Black),
                    new Jacket(Color.Blue), new Jacket(Color.Blue), new Jacket(Color.Blue)
                };
            var stock = new Stock<Jacket>(jackets);

            var boughtJacket = stock.BuyItem(Color.Blue);
            Assert.IsNotNull(boughtJacket);

            scenario.And("I have two blue jackets in stock");
            //----------------------------------------------------------------------------------------------------------------//
            var leftBlueJacketCount = stock.GetNumberOfLeftItems(Color.Blue);
            Assert.AreEqual(2, leftBlueJacketCount);

            scenario.And("three black jackets in stock.");
            //----------------------------------------------------------------------------------------------------------------//
            var leftBalckJacketCount = stock.GetNumberOfLeftItems(Color.Black);
            Assert.AreEqual(3, leftBalckJacketCount);

            scenario.When("he returns the jacket for a replacement in black,");
            //----------------------------------------------------------------------------------------------------------------//
            var replacedJacket = stock.ReplaceItem(boughtJacket, Color.Black);
            Assert.IsNotNull(replacedJacket);

            scenario.Then("I should have three blue jackets in stock");
            //----------------------------------------------------------------------------------------------------------------//
            leftBlueJacketCount = stock.GetNumberOfLeftItems(Color.Blue);
            Assert.AreEqual(3, leftBlueJacketCount);

            scenario.And("two black jackets in stock");
            //----------------------------------------------------------------------------------------------------------------//
            leftBalckJacketCount = stock.GetNumberOfLeftItems(Color.Black);
            Assert.AreEqual(2, leftBalckJacketCount);

            scenario.Complete();
        }
    }
}
