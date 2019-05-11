namespace NotBook.Service.Hash
{
    public interface IHashService
    {
        void CreateHash(string text, out byte[] hash, out byte[] salt);

        bool VerifyHash(string text, byte[] hash, byte[] salt);
    }
}