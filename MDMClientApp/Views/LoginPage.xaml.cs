using MDMClientApp.Repositories;
using MDMClientApp.Services;

namespace MDMClientApp.Views;

public partial class LoginPage : ContentPage
{
    private UserRepository _userRepository;

    public LoginPage()
    {
        InitializeComponent();
        _userRepository = new UserRepository(DatabaseService.GetConnection().DatabasePath);
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var user = _userRepository.GetUserByCredentials(FirstNameEntry.Text, PasswordEntry.Text);
        if (user != null)
        {
            if (string.IsNullOrEmpty(user.TwoFactorSecretKey))
            {
                // Если 2FA не настроена, переходим на страницу профиля
                await Navigation.PushAsync(new ProfilePage(user));
            }
            else
            {
                // Если 2FA настроена, переходим на страницу ввода кода
                await Navigation.PushAsync(new TwoFactorAuthPage(user));
            }
        }
        else
        {
            await DisplayAlert("Ошибка", "Неверные учетные данные", "OK");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}