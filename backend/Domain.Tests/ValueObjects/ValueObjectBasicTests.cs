using System;
using Xunit;
using Domain.ValueObjects;
using Domain.Exceptions;

namespace Domain.Tests.ValueObjects
{
    public class PerformanceScoreTests
    {
        [Fact]
        public void Create_ValidValue_ReturnsPerformanceScore()
        {
            var score = PerformanceScore.Create(85.5m);
            Assert.Equal(85.5m, score.Value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void Create_InvalidValue_ThrowsDomainException(decimal value)
        {
            Assert.Throws<DomainException>(() => PerformanceScore.Create(value));
        }
    }

    public class EmailTests
    {
        [Fact]
        public void Create_ValidEmail_ReturnsEmail()
        {
            var email = Email.Create("test@domain.com");
            Assert.Equal("test@domain.com", email.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalidemail.com")]
        public void Create_InvalidEmail_ThrowsInvalidValueObjectException(string value)
        {
            Assert.Throws<InvalidValueObjectException>(() => Email.Create(value));
        }

                [Fact]
                public void Create_TooLongEmail_ThrowsInvalidValueObjectException()
                {
                    var longEmail = new string('a', 145) + "@x.com"; // 150+ chars
                    Assert.Throws<InvalidValueObjectException>(() => Email.Create(longEmail));
                }
    }

    public class PasswordHashTests
    {
        [Fact]
        public void Create_ValidPasswordHash_ReturnsPasswordHash()
        {
            var hash = PasswordHash.Create("hashed_password");
            Assert.Equal("hashed_password", hash.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_InvalidPasswordHash_ThrowsInvalidValueObjectException(string value)
        {
            Assert.Throws<InvalidValueObjectException>(() => PasswordHash.Create(value));
        }

                [Fact]
                public void Create_TooLongPasswordHash_ThrowsInvalidValueObjectException()
                {
                    var longHash = new string('x', 256); // 256 chars, exceeds max
                    Assert.Throws<InvalidValueObjectException>(() => PasswordHash.Create(longHash));
                }
    }
}
