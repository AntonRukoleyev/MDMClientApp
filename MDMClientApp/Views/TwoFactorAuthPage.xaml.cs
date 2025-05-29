using MDMClientApp.Models;
using OtpNet;

namespace MDMClientApp.Views;
public partial class TwoFactorAuthPage : ContentPage
{
    private User _user;

    public TwoFactorAuthPage(User user)
    {
        InitializeComponent();
        _user = user;
    }

    private void OnVerifyClicked(object sender, EventArgs e)
    {
        var secretKey = _user.TwoFactorSecretKey;
        var totp = new Totp(Base32Encoding.ToBytes(secretKey));
        var code = CodeEntry.Text;

        if (totp.VerifyTotp(code, out long timeStepMatched, new VerificationWindow(1, 1)))
        {
            DisplayAlert("Успех", "2FA подтверждена!", "OK");
            Navigation.PushAsync(new ProfilePage(_user)); // Переход на страницу профиля
        }
        else
        {
            DisplayAlert("Ошибка", "Неверный код. Попробуйте снова.", "OK");
        }
    }
}