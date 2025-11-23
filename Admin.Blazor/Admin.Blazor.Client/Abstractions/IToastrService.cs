namespace Admin.Blazor.Client.Abstractions
{
    public interface IToastrService
    {
        Task ShowToastrSuccess(string message);
        Task ShowToastrInfo(string message);
        Task ShowToastrError(string message);
    }
}
