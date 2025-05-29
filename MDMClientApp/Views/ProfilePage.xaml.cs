using MDMClientApp.Models;
using MDMClientApp.Repositories;
using MDMClientApp.Services;

namespace MDMClientApp.Views;

public partial class ProfilePage : ContentPage
{
    private User _user;
    private CompanyRepository _companyRepository;

    public ProfilePage(User user)
    {
        InitializeComponent();
        _user = user;
        _companyRepository = new CompanyRepository(DatabaseService.GetConnection().DatabasePath);
        LoadUserData();

        // Показываем кнопку админ-панели только для пользователя admin
        AdminPanelButton.IsVisible = _user.FirstName == "admin";
    }

    private void LoadUserData()
    {
        FirstNameLabel.Text = _user.FirstName;
        LastNameLabel.Text = _user.LastName;
        MiddleNameLabel.Text = _user.MiddleName;
        DepartmentLabel.Text = _user.Department;
        PositionLabel.Text = _user.Position;

        var company = _companyRepository.GetCompany(_user.CompanyId);
        CompanyLabel.Text = company?.Name ?? "Неизвестно";
    }

    private async void OnSetupTwoFactorClicked(object sender, EventArgs e)
    {
        // Переход на страницу настройки 2FA
        await Navigation.PushAsync(new SetupTwoFactorPage(_user));
    }

    private async void OnAdminPanelClicked(object sender, EventArgs e)
    {
        // Переход в админ-панель
        await Navigation.PushAsync(new AdminPage());
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Возвращаемся на страницу входа
        await Navigation.PopToRootAsync();
    }
}