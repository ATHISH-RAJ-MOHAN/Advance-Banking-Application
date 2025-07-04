using FinalProject.DataAccess;
using FinalProject.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Metadata;
using FinalProject.Model;
using Document = FinalProject.Model.Document;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private EnquiryLoginDataAccess _elda;

        [TestInitialize]
        public void Setup()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


            _elda = new EnquiryLoginDataAccess(config);
        }


        [TestMethod]
        public void SignUpSuccess()
        {
            // Arrange
            int rowsBeforeInserting = _elda.FetchNoOfEnquiriesFromSignUp();
            string email = "athish3@gmail.com";
            string password = "Athish@1234";

            // Act
            var result = _elda.CreateEnquirer(email, password);
            int rowsAfterInserting = _elda.FetchNoOfEnquiriesFromSignUp();

            // Assert
            Assert.AreEqual(rowsBeforeInserting + 1, rowsAfterInserting);
        }

        [TestMethod]
        public void GetEnquirerOnFirstLogin()
        {
            CreateEnquiry createEnquiry = new CreateEnquiry();
            createEnquiry.Email = "athish3@gmail.com";
            // Using DateTime constructor
            DateTime dateTime1 = new DateTime(1, 1, 1, 0, 0, 0);
            DateTime dateTime2 = DateTime.MinValue;
            createEnquiry.DOB = dateTime1;


            string email = "athish3@gmail.com";
            string password = "Athish@1234";
            CreateEnquiry createEnquiry1 = _elda.GetEnquirer(email, password);
            Assert.AreNotEqual(createEnquiry, createEnquiry1);
        }

        [TestMethod]
        public void GetDocumentsForANewEnquiry()
        {
            // Arrange
            Document expectedDocument = new Document
            {
                Email = "athish3@gmail.com",
                Photo = null,
                Aadhar = null,
                PanCard = null,
                basePanCard = "111",
                baseAadhar = "111",
                basePhoto = "111"
            };

            // Act
            Document actualDocument = _elda.GetDocuments("athish3@gmail.com");

            // Assert
            Assert.AreEqual(expectedDocument.Email, actualDocument.Email);
            Assert.AreEqual(expectedDocument.Photo, actualDocument.Photo);
            Assert.AreEqual(expectedDocument.Aadhar, actualDocument.Aadhar);
            Assert.AreEqual(expectedDocument.PanCard, actualDocument.PanCard);
            Assert.AreEqual(expectedDocument.basePanCard, actualDocument.basePanCard);
            Assert.AreEqual(expectedDocument.baseAadhar, actualDocument.baseAadhar);
            Assert.AreEqual(expectedDocument.basePhoto, actualDocument.basePhoto);
        }

        [TestMethod]
        public void SaveEnquiryTest()
        {
            CreateEnquiry createEnquiry = new CreateEnquiry
            {
                FirstName = "Athish",
                PhoneNumber = "9875326163",
                Email = "athish3@gmail.com",
                DOB = DateTime.Now,
                Status = 0,
                Pincode = 12345,
                WantsCheque = true,
                Feedback = "Test feedback",
                IsActive = true,
                AccountType = 1,
                Balance = 5000.00m
            };
            _elda.SaveEnquiry("Athish", "", "", "", "", "9875326163", "athish3@gmail.com", DateTime.Now, "", "", 0, 12345, true, "Test feedback", true, 1, 5000.00m);
            var actualEnquiry = _elda.GetEnquirer("athish3@gmail.com", "Athish@1234");
            Assert.IsNotNull(actualEnquiry); // Make sure an object is retrieved
            Assert.AreEqual(createEnquiry.FirstName, actualEnquiry.FirstName);
            Assert.AreEqual(createEnquiry.LastName, actualEnquiry.LastName);
            Assert.AreEqual(createEnquiry.Email, actualEnquiry.Email);
        }

        [TestMethod]
        public void SubmitEnquiryTestAndIsActiveIsFalse()
        {
            CreateEnquiry createEnquiry = new CreateEnquiry
            {
                FirstName = "Athish",
                LastName = "Doe",
                Address1 = "124 Main St",
                Address2 = "Apt 101",
                Address3 = "Building B",
                PhoneNumber = "9875326163",
                Email = "athish3@gmail.com",
                DOB = DateTime.Now, // You need to set the DateTime value here
                City = "New York",
                Country = "USA",
                Status = 1, // Update with appropriate value
                Pincode = 12345, // Update with appropriate value
                WantsCheque = true,
                Feedback = "Test feedback",
                IsActive = true,
                AccountType = 1, // Update with appropriate value
                Balance = 5000.00m // Update with appropriate value
            };
            _elda.CreateEnquiry("Athish", "Doe", "124 Main St", "Apt 101", "Building B", "9875326163", "athish3@gmail.com", DateTime.Now, "New York", "USA", 1, 12345, true, "Test feedback", true, 1, 5000.00m);
            var actualEnquiry = _elda.GetEnquirer("athish3@gmail.com", "Athish@1234");
            Assert.IsNull(actualEnquiry);
        }

        [TestMethod]
        public void SubmitEnquiryTestAndIsActiveIsMadeTrue()
        {
            _elda.makeIsActiveTrueInEqnuiryLoginAfterSubmit();
            CreateEnquiry createEnquiry = new CreateEnquiry
            {
                FirstName = "Athish",
                LastName = "Doe",
                Address1 = "124 Main St",
                Address2 = "Apt 101",
                Address3 = "Building B",
                PhoneNumber = "9875326163",
                Email = "athish3@gmail.com",
                DOB = DateTime.Now,
                City = "New York",
                Country = "USA",
                Status = 1,
                Pincode = 12345,
                WantsCheque = true,
                Feedback = "Test feedback",
                IsActive = true,
                AccountType = 1,
                Balance = 5000.00m
            };
            var actualEnquiry = _elda.GetEnquirer("athish3@gmail.com", "Athish@1234");
            Assert.IsNotNull(actualEnquiry); // Make sure an object is retrieved
            Assert.AreEqual(createEnquiry.FirstName, actualEnquiry.FirstName);
            Assert.AreEqual(createEnquiry.LastName, actualEnquiry.LastName);
            Assert.AreEqual(createEnquiry.Email, actualEnquiry.Email);
            Assert.AreEqual(createEnquiry.Balance, actualEnquiry.Balance);
            Assert.AreEqual(createEnquiry.AccountType, actualEnquiry.AccountType);
            Assert.AreEqual(createEnquiry.Address1, actualEnquiry.Address1);
            Assert.AreEqual(createEnquiry.Status, actualEnquiry.Status);
        }

    }
}
