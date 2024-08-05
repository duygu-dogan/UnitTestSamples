namespace JobApplicationLibrary.Services
{
    public interface IIdentityValidator
    {
        bool IsValid(string identityNum);

        //bool CheckConnectionToRemoteServer();
        ICountryDataProvider CountryDataProvider { get; }
    }

    public interface ICountryData
    {
       string Country { get; }
    }

    public interface ICountryDataProvider
    {
        ICountryData CountryData { get; }
    }
}