using System.Data;

namespace NotBook.Data.MicroOrm
{
    public interface IMicroOrmRepository
    {
        IDbConnection Connection { get; }
    }
}