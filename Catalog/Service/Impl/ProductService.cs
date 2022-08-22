using Catalog.Models;
using Catalog.Repository;
using Email.Contracts.Commands;
using System.Text.Json;

namespace Catalog.Service.Impl
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Guid, Product> productRepository;
        private readonly ICommandQueue<SendEmail> sendEmailQueue;

        public ProductService(
            IRepository<Guid, Product> productRepository, 
            ICommandQueue<SendEmail> sendEmailQueue)
        {
            this.productRepository = productRepository;
            this.sendEmailQueue = sendEmailQueue;
        }

        public async Task<IEnumerable<Product>> GetProducts() => await productRepository.GetAll();

        public async Task<Product?> GetProduct(Guid id) => await productRepository.Get(id);

        public async Task PutProduct(Guid id, UpdateProduct updateProduct)
        {
            Product product = await productRepository.Get(id) ?? throw new NotFoundException();

            product.Name = updateProduct.Name;
            product.Cost = updateProduct.Cost;
            product.Price = updateProduct.Price;
            product.Image = updateProduct.Image;

            productRepository.Update(product);

            await productRepository.SaveChanges();
        }

        public async Task<Guid> PostProduct(CreateProduct createProduct)
        {
            Product product = new()
            {
                Name = createProduct.Name,
                Cost = createProduct.Cost,
                Price = createProduct.Price,
                Image = createProduct.Image
            };

            productRepository.Save(product);

            await productRepository.SaveChanges();

            Guid newProductId = product.Id;

            OnProductCreated(newProductId);

            return newProductId;
        }

        private void OnProductCreated(Guid newProductId)
        {
            SendProductCreatedEmail(newProductId);
        }

        private void SendProductCreatedEmail(Guid newProductId)
        {
            SendEmail sendEmail = CreateProductAddedEmail(newProductId);
            sendEmailQueue.Send(sendEmail);
        }

        private static SendEmail CreateProductAddedEmail(Guid newProductId) => new()
        {
            To = "frances33@ethereal.email",
            Subject = $"Product Added with Id: {newProductId}.",
            Body = $@"Hello,
This is a test email message sent by an application.
Product Added with Id: {newProductId}.",
        };


        public async Task DeleteProduct(Guid id)
        {
            Product product = await productRepository.Get(id) ?? throw new NotFoundException();

            productRepository.Delete(product);

            await productRepository.SaveChanges();
        }
    }
}
