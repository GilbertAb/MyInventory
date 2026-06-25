using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.DTOs.Response;
using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Application.Services;
using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.Repositories;
using External.MyInventoryApi.DataAccess.Contracts.Results;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.Tests.Application
{
    public class ProductServiceTests
    {
        //
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _service = new ProductService(_repositoryMock.Object);
        }

        /*
         *---------------------------------------------------------
         *---------------| GetAllProducts use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetAllProducts_ShouldReturnMappedResult_WhenRepositoryReturnsData()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    ProductName = "Test",
                    Stock = 10,
                    Category = "Test"
                },
                new Product
                {
                    Id = 1,
                    ProductName = "Test",
                    Stock = 10,
                    Category = "Test"
                }
            };
            
            var operationResult = new OperationResult<IEnumerable<Product>?>
            {
                Data = products,
                ErrorCode = 0,
                ErrorMessage = "Succeed"
            };

            _repositoryMock
                .Setup(r => r.GetAllProducts())
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetAllProducts();

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);
            result.Data.Should().HaveCount(2);
            result.Data.Should().BeEquivalentTo(products, options =>
                options
                    .Excluding(x => x.CreatedAt)
                    .Excluding(x => x.UpdatedAt)
            );

            _repositoryMock.Verify(r => r.GetAllProducts(), Times.Once());
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnEmptyCollection_WhenNoProductsExist()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<Product>?>
            {
                Data = [],
                ErrorCode = 0
            };

            _repositoryMock
                .Setup(r => r.GetAllProducts())
                .ReturnsAsync(operationResult);

            // Act
            var result = await _service.GetAllProducts();

            // Assert
            result.ErrorCode.Should().Be(0);
            result.Data.Should().BeEmpty();

            _repositoryMock.Verify(r => r.GetAllProducts(), Times.Once());
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnError_WhenRepositoryReturnsError()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<Product>?>
            {
                Data = null,
                ErrorCode = 500,
                ErrorMessage = "Database error"
            };

            _repositoryMock
                .Setup(r => r.GetAllProducts())
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetAllProducts();

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");

            _repositoryMock.Verify(r => r.GetAllProducts(), Times.Once());

        }

        /*
         *---------------------------------------------------------
         *---------------| GetProductById use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetProductById_ShouldReturnMappedResult_WhenRepositoryReturnsData()
        {
            // Arrange
            var product = new Product
            {
                Id = 27,
                ProductName = "Test",
                Stock = 10,
                Category = "Test"
            };
            
            var operationResult = new OperationResult<Product?>
            {
                Data = product,
                ErrorCode = 0,
                ErrorMessage = "Succeed"
            };

            _repositoryMock
                .Setup(r => r.GetProductById(27))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetProductById(27);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);
            result.Data.Should().BeEquivalentTo(product, options =>
                options
                    .Excluding(x => x.CreatedAt)
                    .Excluding(x => x.UpdatedAt)
            );

            _repositoryMock.Verify(r => r.GetProductById(27), Times.Once());
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var operationResult = new OperationResult<Product?>
            {
                Data = null,
                ErrorCode = 0
            };

            _repositoryMock
                .Setup(r => r.GetProductById(0))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _service.GetProductById(0);

            // Assert
            result.ErrorCode.Should().Be(0);
            result.Data.Should().BeNull();

            _repositoryMock.Verify(r => r.GetProductById(0), Times.Once());
        }

        [Fact]
        public async Task GetProductById_ShouldReturnError_WhenRepositoryReturnsError()
        {
            // Arrange
            var operationResult = new OperationResult<Product?>
            {
                Data = null,
                ErrorCode = 500,
                ErrorMessage = "Database error"
            };

            _repositoryMock
                .Setup(r => r.GetProductById(0))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetProductById(0);

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");

            _repositoryMock.Verify(r => r.GetProductById(0), Times.Once());

        }

        /*
         *---------------------------------------------------------
         *---------------| AddProduct use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task AddProduct_ShouldReturnMappedResult_WhenRepositoryReturnsData()
        {
            // Arrange
            var productDto = new ProductDto
            {
                ProductName = "Test",
                Stock = 10,
                Category = "Test"
            };

            var operationResult = new OperationResult<int?>
            {
                Data = 27,
                ErrorCode = 0,
                ErrorMessage = "Succeed"
            };

            _repositoryMock
                .Setup(r => r.AddProduct(It.IsAny<Product>()))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.AddProduct(productDto);

            // Assert
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(
                new AddProductResponseDto
                {
                    Id = 27
                }    
            );
            result.ErrorCode.Should().Be(0);

            _repositoryMock.Verify(r => r.AddProduct(It.Is<Product>(p => 
                p.ProductName == productDto.ProductName &&
                p.Stock == productDto.Stock &&
                p.Category == productDto.Category
                )), Times.Once());
        }
        
        [Fact]
        public async Task AddProduct_ShouldReturnError_WhenProductIsNull()
        {
            // Arrange
            ProductDto? productDto = null;

            var operationResult = new OperationResult<int?>
            {
                Data = null,
                ErrorCode = -1,
                ErrorMessage = "Product can't be null"
            };

            _repositoryMock
                .Setup(r => r.AddProduct(null))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.AddProduct(null);

            // Assert
            result.Data.Should().BeNull();
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Product can't be null");

            _repositoryMock.Verify(r => r.AddProduct(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public async Task AddProduct_ShouldReturnError_WhenProductNameIsEmpty()
        {
            // Arrange
            var productDto = new ProductDto
            {
                ProductName = "",
                Stock = 10,
                Category = "Test"
            };

            var operationResult = new OperationResult<int?>
            {
                Data = null,
                ErrorCode = -1,
                ErrorMessage = "Product name can't be null or empty"
            };

            _repositoryMock
                .Setup(r => r.AddProduct(It.IsAny<Product>()))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.AddProduct(productDto);

            // Assert
            result.Data.Should().BeNull();
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Product name can't be null or empty");

            _repositoryMock.Verify(r => r.AddProduct(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public async Task AddProduct_ShouldReturnError_WhenProductNameIsNull()
        {
            // Arrange
            var productDto = new ProductDto
            {
                ProductName = null,
                Stock = 10,
                Category = "Test"
            };

            var operationResult = new OperationResult<int?>
            {
                Data = null,
                ErrorCode = -1,
                ErrorMessage = "Product name can't be null or empty"
            };

            _repositoryMock
                .Setup(r => r.AddProduct(It.IsAny<Product>()))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.AddProduct(productDto);

            // Assert
            result.Data.Should().BeNull();
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Product name can't be null or empty");

            _repositoryMock.Verify(r => r.AddProduct(It.IsAny<Product>()), Times.Never());
        }
        
        [Fact]
        public async Task AddProduct_ShouldReturnError_WhenRepositoryReturnsError()
        {
            // Arrange
            var productDto = new ProductDto
            {
                ProductName = "Test",
                Stock = 10,
                Category = "Test"
            };

            var operationResult = new OperationResult<int?>
            {
                Data = null,
                ErrorCode = 500,
                ErrorMessage = "Database error"
            };

            _repositoryMock
                .Setup(r => r.AddProduct(It.IsAny<Product>()))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.AddProduct(productDto);

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");

            _repositoryMock.Verify(r => r.AddProduct(It.IsAny<Product>()), Times.Once());

        }



        /*
         *---------------------------------------------------------
         *---------------| UpdateProduct use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task UpdateProduct_ShouldReturnMappedResult_WhenRepositoryReturnsData()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 27,
                ProductName = "Test",
                Stock = 10,
                Category = "Test"
            };

            var operationResult = new OperationResult<int?>
            {
                Data = 27,
                ErrorCode = 0,
                ErrorMessage = "Succeed"
            };

            _repositoryMock
                .Setup(r => r.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.UpdateProduct(productDto);

            // Assert
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(
                new UpdateProductResponseDto
                {
                    Id = 27
                }    
            );
            result.ErrorCode.Should().Be(0);

            _repositoryMock.Verify(r => r.UpdateProduct(It.Is<Product>(p => 
                p.Id == productDto.Id &&
                p.ProductName == productDto.ProductName &&
                p.Stock == productDto.Stock &&
                p.Category == productDto.Category
                )), Times.Once());
        }
        
        [Fact]
        public async Task UpdateProduct_ShouldReturnError_WhenProductIsNull()
        {
            // Arrange
            ProductDto? productDto = null;
            // Serice returns before calling the repository
            /*var operationResult = new OperationResult<int?>
            {
                Data = null,
                ErrorCode = -1,
                ErrorMessage = "Product can't be null"
            };

            _repositoryMock
                .Setup(r => r.UpdateProduct(null!))
                .ReturnsAsync(operationResult);*/


            // Act
            var result = await _service.UpdateProduct(null!);

            // Assert
            result.Data.Should().BeNull();
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Product can't be null");

            _repositoryMock.Verify(r => r.UpdateProduct(It.IsAny<Product>()), Times.Never());
        }
        
        [Fact]
        public async Task UpdateProduct_ShouldReturnError_WhenProductNameIsEmpty()
        {
            // Arrange
            var productDto = new ProductDto
            {
                ProductName = "",
                Stock = 10,
                Category = "Test"
            };

            var operationResult = new OperationResult<int?>
            {
                Data = null,
                ErrorCode = -1,
                ErrorMessage = "Product name can't be null or empty"
            };

            _repositoryMock
                .Setup(r => r.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.UpdateProduct(productDto);

            // Assert
            result.Data.Should().BeNull();
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Product name can't be null or empty");

            _repositoryMock.Verify(r => r.UpdateProduct(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnError_WhenProductNameIsNull()
        {
            // Arrange
            var productDto = new ProductDto
            {
                ProductName = null!,
                Stock = 10,
                Category = "Test"
            };

            var operationResult = new OperationResult<int?>
            {
                Data = null,
                ErrorCode = -1,
                ErrorMessage = "Product name can't be null or empty"
            };

            _repositoryMock
                .Setup(r => r.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.UpdateProduct(productDto);

            // Assert
            result.Data.Should().BeNull();
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Product name can't be null or empty");

            _repositoryMock.Verify(r => r.UpdateProduct(It.IsAny<Product>()), Times.Never());
        }
        
        [Fact]
        public async Task UpdateProduct_ShouldReturnError_WhenRepositoryReturnsError()
        {
            // Arrange
            var productDto = new ProductDto
            {
                ProductName = "Test",
                Stock = 10,
                Category = "Test"
            };

            var operationResult = new OperationResult<int?>
            {
                Data = null,
                ErrorCode = 500,
                ErrorMessage = "Database error"
            };

            _repositoryMock
                .Setup(r => r.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.UpdateProduct(productDto);

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");

            _repositoryMock.Verify(r => r.UpdateProduct(It.IsAny<Product>()), Times.Once());

        }
    }
}
