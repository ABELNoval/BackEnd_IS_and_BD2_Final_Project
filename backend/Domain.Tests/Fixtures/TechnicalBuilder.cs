using System;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Fixtures
{
    public class TechnicalBuilder
    {
        private string _name = "Técnico de prueba";
        private Email _email = Email.Create("tecnico@geshteck.com");
        private PasswordHash _passwordHash = PasswordHash.Create("hash123");
        private int _experience = 5;
        private string _specialty = "Electrónica";

        public TechnicalBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        public TechnicalBuilder WithEmail(Email email)
        {
            _email = email;
            return this;
        }
        public TechnicalBuilder WithPasswordHash(PasswordHash hash)
        {
            _passwordHash = hash;
            return this;
        }
        public TechnicalBuilder WithExperience(int experience)
        {
            _experience = experience;
            return this;
        }
        public TechnicalBuilder WithSpecialty(string specialty)
        {
            _specialty = specialty;
            return this;
        }
        public Technical Build()
        {
            return Technical.Create(_name, _email, _passwordHash, _experience, _specialty);
        }
    }
}
