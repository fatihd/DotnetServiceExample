using Catalog.Models;
using Catalog.Repository;
using Catalog.Service;
using Catalog.Service.Impl;
using Email.Contracts.Commands;
using Moq;
using Xunit;

namespace Catalog.Test.Service
{
    public class ProductServiceTest
    {
        public readonly Mock<IRepository<Guid, Product>> mockProductRepository = new();
        public readonly Mock<ICommandQueue<SendEmail>> mockSendEmailQueue = new();

        [Fact]
        public async void SaveProduct_Success()
        {

            Guid generatedId = Guid.NewGuid();

            var createProduct = new CreateProduct()
            {
                Name = "test name",
                Price = 2,
                Cost = 1,
                Image = "test image"
            };

            Product? savedProduct = null;

            mockProductRepository.Setup(x => x.Save(It.IsAny<Product>()))
                .Callback<Product>(p => { 
                    savedProduct = p; 
                    p.Id = generatedId;
                });

            SendEmail? sendEmail = null;

            mockSendEmailQueue.Setup(x => x.Send(It.IsAny<SendEmail>()))
                .Callback<SendEmail>(se => sendEmail = se);


            ProductService productService = new ProductService(
                mockProductRepository.Object,
                mockSendEmailQueue.Object);

            await productService.PostProduct(createProduct);

            Assert.Equal(createProduct.Name, savedProduct?.Name);
            Assert.Equal(createProduct.Price, savedProduct?.Price);
            Assert.Equal(createProduct.Cost, savedProduct?.Cost);
            Assert.Equal(createProduct.Image, savedProduct?.Image);

            Assert.Contains(generatedId.ToString(), sendEmail?.Subject);
            Assert.Contains(generatedId.ToString(), sendEmail?.Body);
        }
    }
}