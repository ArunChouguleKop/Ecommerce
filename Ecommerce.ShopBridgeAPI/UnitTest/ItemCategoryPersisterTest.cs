using BusinessPersister.BuisnessObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecommerce.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class ItemCategoryPersisterTest
    {

        List<ItemCategory> _mockList = new List<ItemCategory>();
        List<Item> _mockitemList = new List<Item>();
        ItemCategory _mockObj = new ItemCategory();
        ItemCategory _mockInvalidObj = new ItemCategory();
        private readonly Mock<ItemCategoryPersister> _bal = new Mock<ItemCategoryPersister>();
        private Guid first;
        private Guid second;
        private Guid third;


        public ItemCategoryPersisterTest()
        {

            TestDataSetup();
        }
        private void TestDataSetup()
        {
            first = Guid.Parse("38670b75-a33a-4b8e-a730-49d98568f8db");
            second = Guid.Parse("1f8210a2-9e1b-45a3-8b7c-85834130899e");
            third = Guid.Parse("b89c45fb-0ae5-4367-9d0e-96b5f63fa874");
            //----------------------------------------------------------//
            _mockList.Add(new ItemCategory
            {
                Id = first,
                Name = "Car",
                IsUsed = true
            });
            _mockList.Add(new ItemCategory
            {
                Id = second,
                Name = "Sedan",
                IsUsed = true
            });
            _mockList.Add(new ItemCategory
            {
                Id = third,
                Name = "SUV",
                IsUsed = true
            });
            //----------------------------------------------------------//
            _mockObj.Id = Guid.Parse("b89c45fb-0ae5-4367-9d0e-96b5f63fa873");
            _mockObj.Name = "Valid Name";
            _mockObj.IsUsed = true;
            //----------------------------------------------------------//
            _mockInvalidObj.Id = Guid.Empty;
            _mockInvalidObj.Name = "InValid Name.................................";
            _mockInvalidObj.IsUsed = false;
            _mockInvalidObj.Errors.Add("InValid Name");
            //----------------------------------------------------------//

            _mockitemList.Add(new Item
            {
                Id = Guid.Parse("b89c45fb-0ae5-4367-9d0e-96b5f63fa863"),
                ItemCategoryId = first,
                Name = "Car",
                StockIn = 5,
                StockOut = 1,
                IsUsed = true

            });
            _mockitemList.Add(new Item
            {
                Id = Guid.Parse("c89c45fb-0ae5-4367-9d0e-96b5f63fa863"),
                ItemCategoryId = Guid.NewGuid(),
                Name = "SUV",
                StockIn = 5,
                StockOut = 1,
                IsUsed = true

            });
        }



        [TestMethod]
        public void Get_ItemCategoryList_Test()
        {
            //Arrange
            _bal.Setup(x => x.Get(true)).Returns(_mockList);

            //Act
            var respose = _bal.Object.Get(true);

            //Asert
            Assert.AreEqual(_mockList, respose);
            Assert.IsNotNull(respose);
        }

        [TestMethod]
        public void GetById_ItemCategory_Test()
        {
            //Arrange
            _bal.Setup(x => x.GetById(first, true)).Returns(_mockList.Where(x => x.Id == first).FirstOrDefault());

            //Act
            var respose = _bal.Object.GetById(first, true);

            //Asert
            Assert.AreEqual(_mockList.Where(x => x.Id == first).FirstOrDefault(), respose);
            Assert.AreNotEqual(_mockList.Where(x => x.Id == second).FirstOrDefault(), respose);
            Assert.IsNotNull(respose);
        }

        [TestMethod]
        public void InsertValid_ItemCategory_Test()
        {
            //Arrange
            _bal.Setup(x => x.Insert(_mockObj)).Returns(_mockObj);

            //Act
            var respose = _bal.Object.Insert(_mockObj);

            //Asert
            Assert.IsTrue(respose.Name.Length <= 20);
            Assert.IsTrue(respose.Id != Guid.Empty);
            Assert.IsTrue(respose.Errors.Count() < 1);
        }

        [TestMethod]
        public void InsertInValid_ItemCategory_Test()
        {
            //Arrange
            _bal.Setup(x => x.Insert(_mockInvalidObj)).Returns(_mockInvalidObj);

            //Act
            var respose = _bal.Object.Insert(_mockInvalidObj);

            //Asert
            if (_mockInvalidObj.Name.Length > 20)
            {
                Assert.IsTrue(respose.Id == Guid.Empty);
                Assert.IsTrue(respose.Errors.Count() > 0);

            }


        }


        [TestMethod]
        public void UpdateValid_ItemCategory_Test()
        {
            string UpdatedName = "Updated";
            _mockObj.Name = UpdatedName;
            //Arrange
            _bal.Setup(x => x.Update(_mockObj)).Returns(_mockObj);

            //Act
            var respose = _bal.Object.Update(_mockObj);

            //Asert
            if (_mockInvalidObj.Name.Length <= 20)
            {
                Assert.IsTrue(respose.Id != Guid.Empty);
                Assert.IsTrue(respose.Errors.Count() != 0);
                Assert.Equals(respose.Name, UpdatedName);

            }


        }


        [TestMethod]
        public void UpdateInValid_ItemCategory_Test()
        {

            //Arrange
            string UpdatedName = "Updated.................................asghdvad";
            if (UpdatedName.Length > 20)
            {
                _mockObj.Errors.Add("InValid");
            }
            _mockObj.Name = UpdatedName;
            _bal.Setup(x => x.Update(_mockObj)).Returns(_mockObj);

            //Act
            var respose = _bal.Object.Update(_mockObj);

            //Asert
            if (_mockInvalidObj.Name.Length > 20)
            {
                Assert.IsTrue(respose.Id != Guid.Empty);
                Assert.IsTrue(respose.Errors.Count() != 0);
                Assert.AreEqual(respose.Name, UpdatedName);

            }


        }


        [TestMethod]
        public void CheckRefereceInItem_ItemCategory_Test()
        {
            bool IsPresent = false;
            //Arrange
            foreach (var item in _mockitemList)
            {
                if (item.ItemCategoryId == first)
                {
                    IsPresent = true;
                    break;
                }
            }
            _bal.Setup(x => x.CheckRefereceInItem(first, true)).Returns(IsPresent);

            //Act
            var respose = _bal.Object.CheckRefereceInItem(first, true);

            //Asert
            if (respose)
            {
                Assert.IsTrue(respose);
            }


        }
    }
}
