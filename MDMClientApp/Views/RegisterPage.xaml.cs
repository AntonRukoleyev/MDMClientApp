using MDMClientApp.Models;
using MDMClientApp.Repositories;
using MDMClientApp.Services;

namespace MDMClientApp.Views;

public partial class RegisterPage : ContentPage
{
    private UserRepository _userRepository;
    private CompanyRepository _companyRepository;

    public RegisterPage()
    {
        InitializeComponent();
        _userRepository = new UserRepository(DatabaseService.GetConnection().DatabasePath);
        _companyRepository = new CompanyRepository(DatabaseService.GetConnection().DatabasePath);
    }

    [Obsolete]
    private void OnRegisterClicked(object sender, EventArgs e)
    {
        var companyName = CompanyNameEntry.Text;
        if (string.IsNullOrEmpty(companyName))
        {
            DisplayAlert("Ошибка", "Название компании обязательно", "OK");
            return;
        }

        var company = _companyRepository.GetCompanies().FirstOrDefault(c => c.Name == companyName);

        // Если компании нет, создаем её
        if (company == null)
        {
            company = new Company { Name = companyName };
            _companyRepository.SaveCompany(company);
        }

        var user = new User
        {
            FirstName = FirstNameEntry.Text,
            LastName = LastNameEntry.Text,
            MiddleName = MiddleNameEntry.Text,
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text,
            Department = DepartmentEntry.Text,
            Position = PositionEntry.Text,
            CompanyId = company.Id
        };

        _userRepository.SaveUser(user);

        // Отправляем сообщение о новом пользователе
        MessagingCenter.Send(this, "UserAdded", user.CompanyId);

        DisplayAlert("Успех", "Пользователь успешно зарегистрирован", "OK");
        Navigation.PopAsync();
    }
}