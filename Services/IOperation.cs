namespace Sample.FunctionApp.Services;

public interface IOperation
{
    void Insert(UserModel user);
    void Update( UserModel user);
    IList<UserModel> GetAll();
    UserModel GetById(int id);
    void Delete(int id);
}