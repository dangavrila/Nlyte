using System;
using System.Collections.Generic;

namespace PhoneBookData.Models
{
    public class ContactType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}
