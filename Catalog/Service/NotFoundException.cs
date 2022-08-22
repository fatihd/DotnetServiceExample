namespace Catalog.Service
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base(message: "Resource not found.") { }
    }
}
