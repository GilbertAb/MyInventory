namespace External.MyInventoryApi.CrossCutting.Contracts
{
    public interface ICrypto
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
