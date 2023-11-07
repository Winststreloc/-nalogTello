namespace AnalogTrelloBE.Intefaces.IService;

public interface ICashService
{
    Task CashingData(long id, object data);
}