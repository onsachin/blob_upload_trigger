namespace Sample.FunctionApp.Services;

public class CRUDOperation:IOperation
{
    public void Insert(UserModel user)
    {
        throw new NotImplementedException();
    }

    public void Update(UserModel user)
    {
        throw new NotImplementedException();
    }

    public IList<UserModel> GetAll()
    {
        return new List<UserModel>
        {
            new UserModel
            {
                id = "12345678",
                Name = "John Doe",
                Age = 34,
                Gender = "Male",
            },
            new UserModel
            {
                id = "3333333123",
                Name = "John x",
                Age = 54,
                Gender = "Female",
            }
        };
    }

    public UserModel GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}