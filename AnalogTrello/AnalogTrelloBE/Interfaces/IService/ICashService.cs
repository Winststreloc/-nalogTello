namespace AnalogTrelloBE.Interfaces.IService;

public interface ICashService
{
    Task CashingData(long id, object data);
}