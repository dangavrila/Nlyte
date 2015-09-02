using PhoneBookData.Models;
using System;

namespace PhoneBookData
{
    public class PhoneBookUnitOfWork : IDisposable
    {
        private PhoneBookContext _context;
        private IPhoneBookRepository<Contact> _contactsRepository;
        private IPhoneBookRepository<ContactType> _contactTypesRepository;

        public PhoneBookUnitOfWork()
        {
            _context = new PhoneBookContext();
            _contactsRepository = new PhoneBookRepository<Contact>(_context);
            _contactTypesRepository = new PhoneBookRepository<ContactType>(_context);
        }

        public IPhoneBookRepository<Contact> ContactsRepository
        {
            get
            {
                if (_contactsRepository != null)
                    return _contactsRepository;
                else
                    _contactsRepository = new PhoneBookRepository<Contact>(_context);
                return _contactsRepository;
            }
        }

        public IPhoneBookRepository<ContactType> ContactTypesRepository
        {
            get
            {
                if (_contactsRepository != null)
                    return _contactTypesRepository;
                else
                    _contactTypesRepository = new PhoneBookRepository<ContactType>(_context);
                return _contactTypesRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
