using External.MyInventoryApi.CrossCutting.Crypto;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.Tests.CrossCutting
{
    public class CryptoTests
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenKeyIsMissing()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Crypto:IV"] = "1234567890123456"
                })
                .Build();

            Action act = () => new Crypto(configuration);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenIvIsMissing()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Crypto:KEY"] = "1234567890123456"
                })
                .Build();

            Action act = () => new Crypto(configuration);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Encrypt_ShouldReturnEncryptedText_WhenInputIsValid()
        {
            var crypto = CreateCrypto();

            var result = crypto.Encrypt("Hello World");

            result.Should().NotBeNullOrWhiteSpace();
            result.Should().NotBe("Hello World");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Encrypt_ShouldReturnInput_WhenInputIsNullOrEmpty(string? input)
        {
            var crypto = CreateCrypto();

            var result = crypto.Encrypt(input!);

            result.Should().Be(input);
        }

        [Fact]
        public void Decrypt_ShouldReturnOriginalText_WhenCipherTextIsValid()
        {
            var crypto = CreateCrypto();

            const string plainText = "Hello World";

            var encrypted = crypto.Encrypt(plainText);

            var decrypted = crypto.Decrypt(encrypted);

            decrypted.Should().Be(plainText);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Decrypt_ShouldReturnInput_WhenInputIsNullOrEmpty(string? input)
        {
            var crypto = CreateCrypto();

            var result = crypto.Decrypt(input!);

            result.Should().Be(input);
        }

        private static Crypto CreateCrypto()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Crypto:KEY"] = "1234567890123456",
                    ["Crypto:IV"] = "1234567890123456"
                })
                .Build();

            return new Crypto(configuration);
        }
    }
}
