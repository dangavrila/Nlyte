using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhoneBookData;
using PhoneBookData.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PhoneBookTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void QueryContactTypesMock()
        {
            var contactTypes = new List<ContactType> {
                new ContactType() { Name = "Work" },
                new ContactType() { Name = "Cell phone" },
                new ContactType() { Name = "Home" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ContactType>>();
            mockSet.As<IQueryable<ContactType>>().Setup(m => m.Provider).Returns(contactTypes.Provider);
            mockSet.As<IQueryable<ContactType>>().Setup(m => m.Expression).Returns(contactTypes.Expression);
            mockSet.As<IQueryable<ContactType>>().Setup(m => m.ElementType).Returns(contactTypes.ElementType);
            mockSet.As<IQueryable<ContactType>>().Setup(m => m.GetEnumerator()).Returns(contactTypes.GetEnumerator());

            var mockContext = new Mock<PhoneBookContext>();
            mockContext.Setup(c => c.ContactTypes).Returns(mockSet.Object);

            var service = new PhoneBookRepository<ContactType>(mockContext.Object);
            var returnedContactTypes = service.GetEntities().ToList();

            Assert.AreEqual(contactTypes.Count(), returnedContactTypes.Count());
            Assert.AreEqual("Work", returnedContactTypes[0].Name);
            Assert.AreEqual("Cell phone", returnedContactTypes[1].Name);
            Assert.AreEqual("Home", returnedContactTypes[2].Name);
        }

        [TestMethod]
        public void QueryContactTypesEndToEnd()
        {
            var unitOfWorkPhoneBookData = new PhoneBookUnitOfWork();

            var contactTypes = new List<ContactType> {
                new ContactType() { Name = "Work" },
                new ContactType() { Name = "Cell phone" },
                new ContactType() { Name = "Home" }
            };

            var results = unitOfWorkPhoneBookData.ContactTypesRepository.GetEntities().OrderBy(e => e.Id).ToList();

            Assert.AreEqual(3, results.Count);
            Assert.AreEqual("Work", results[0].Name);
            Assert.AreEqual("Cell phone", results[1].Name);
            Assert.AreEqual("Home", results[2].Name);

            unitOfWorkPhoneBookData.Dispose();
        }

        [TestMethod]
        public void AddContactTypesEndToEnd()
        {
            using (var unitOfWorkPhoneBookData = new PhoneBookUnitOfWork())
            {
                unitOfWorkPhoneBookData.ContactTypesRepository.Insert(new ContactType() { Name = "Work" });
                unitOfWorkPhoneBookData.ContactTypesRepository.Insert(new ContactType() { Name = "Cell phone" });
                unitOfWorkPhoneBookData.ContactTypesRepository.Insert(new ContactType() { Name = "Home" });
                unitOfWorkPhoneBookData.Save();
            }
        }

        [TestMethod]
        public void DeleteContactTypesEndToEnd()
        {
            using (var unitOfWorkPhoneBookData = new PhoneBookUnitOfWork())
            {
                var contactTypes = unitOfWorkPhoneBookData.ContactTypesRepository.GetEntities().ToList();

                if (contactTypes != null && contactTypes.Count > 0)
                {
                    foreach (var ct in contactTypes)
                    {
                        unitOfWorkPhoneBookData.ContactTypesRepository.Delete(ct.Id);
                    }
                    unitOfWorkPhoneBookData.Save();
                }
            }
        }

        [TestMethod]
        public void AddContactEndToEnd()
        {
            using (var unitOfWorkPhoneBookData = new PhoneBookUnitOfWork())
            {
                var newContact = new Contact()
                {
                    FirstName = "Dan",
                    LastName = "Gavrila",
                    Number = "+40752278789"
                };

                var contactTypeId = unitOfWorkPhoneBookData.ContactTypesRepository.GetEntities()
                    .Where(c => c.Name == "Home")
                    .Select(c => c.Id)
                    .DefaultIfEmpty(0).FirstOrDefault();

                if (contactTypeId > 0)
                    newContact.ContactTypeId = contactTypeId;

                var addedContact = unitOfWorkPhoneBookData.ContactsRepository.Insert(newContact);

                unitOfWorkPhoneBookData.Save();

                Assert.AreEqual(newContact, addedContact);
            }
        }

        [TestMethod]
        public void UpdateContactEndToEnd()
        {
            using (var unitOfWorkPhoneBookData = new PhoneBookUnitOfWork())
            {
                var toBeModifiedContact = unitOfWorkPhoneBookData.ContactsRepository.GetEntities().Where(c => c.Id == 1).FirstOrDefault();
                if (toBeModifiedContact != null)
                {
                    toBeModifiedContact.FirstName = "Nad";
                    unitOfWorkPhoneBookData.ContactsRepository.Update(toBeModifiedContact);
                }
                else throw new AssertFailedException("Contact with Id 1 was not found in DB.");

                unitOfWorkPhoneBookData.Save();

                var contact = unitOfWorkPhoneBookData.ContactsRepository.GetEntities().Where(c => c.Id == 1 && c.FirstName == "Nad").FirstOrDefault();
                if (contact != null)
                    Assert.AreEqual(toBeModifiedContact.FirstName, contact.FirstName);
                else throw new AssertFailedException();
            }
        }
    }
}
