namespace IncidentResponseAPI.Interfaces
{
    public interface IConfigurationValidator
    {
        void Validate(string configurationJson);
    }
}