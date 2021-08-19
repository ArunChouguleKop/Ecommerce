using BusinessPersister.TransactionObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecommerce.Model;
using Moq;
using System;

namespace UnitTest
{
    [TestClass]
    public class ItemStockTransactionTest
    {
        private readonly Mock<ItemStockTransaction> _bal = new Mock<ItemStockTransaction>();
        Item _mockItemObj = new Item();
        ItemDetails _mockItemCost = new ItemDetails();
        ItemTrans trans = new ItemTrans();
        int StockIn = 10;
        int StockOut = 0;
        Guid first = Guid.Parse("08b6e0d5-1a58-48f3-b90d-fd915850bf0a");
       
        public ItemStockTransactionTest()
        {
            TestDataSetup();
        }
        private void TestDataSetup()
        {
            trans.item = _mockItemObj;
            trans.itemDetails = _mockItemCost;
            _mockItemObj.Id = Guid.Empty;
            _mockItemObj.Name = "BMW";
            _mockItemObj.ItemCategoryId = Guid.NewGuid();
            _mockItemObj.StockIn = StockIn;
            _mockItemObj.StockOut = StockOut;
            _mockItemObj.IsUsed = true;


            _mockItemCost.Id = Guid.Empty;
            _mockItemCost.Cost = 32500;
            _mockItemCost.ItemId = Guid.NewGuid();
        }

        [TestMethod]
        public void Insert_ItemStockTransaction_Test()
        {
            //Arrange
            trans.item.Id =  first;
            trans.itemDetails.ItemId = trans.item.Id;
            trans.itemDetails.Id = _mockItemCost.Id;
            _bal.Setup(x => x.Insert(trans)).Returns(trans);

            //Act
            var respose = _bal.Object.Insert(trans);

            //Asert
            Assert.AreEqual(respose.item.Id, first);
            Assert.IsTrue(respose.item.StockIn>0);
            Assert.IsTrue(respose.itemDetails.Cost > 0);
            Assert.IsNotNull(respose);
        }

        [TestMethod]
        public void Update_ItemStockTransaction_Test()
        {
            //Arrange
            trans.item.Id =  first;
            StockOut = 2;
            trans.item.StockIn = StockIn - StockOut;
            trans.item.StockOut = StockOut;
            trans.itemDetails.ItemId = trans.item.Id;
            trans.itemDetails.Id = _mockItemCost.Id;
            _bal.Setup(x => x.Update(trans)).Returns(trans);

            //Act
            var respose = _bal.Object.Update(trans);

            //Asert
            Assert.AreEqual(respose.item.Id, first);
            Assert.IsTrue(respose.itemDetails.Cost > 0);
            Assert.IsTrue(respose.item.StockIn>-1);
            Assert.IsTrue(respose.item.StockOut>-1);
            Assert.IsNotNull(respose);
        }
    }
}
