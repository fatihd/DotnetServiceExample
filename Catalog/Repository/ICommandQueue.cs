namespace Catalog.Repository
{
    public interface ICommandQueue<TCommand>
    {
        void Send(TCommand command);
    }
}
