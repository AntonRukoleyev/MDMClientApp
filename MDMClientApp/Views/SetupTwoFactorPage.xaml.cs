using MDMClientApp.Models;
using MDMClientApp.Repositories;
using MDMClientApp.Services;
using OtpNet;
using QRCoder;
using System.IO;
using System.Text;

namespace MDMClientApp.Views;

public partial class SetupTwoFactorPage : ContentPage
{
    private string _secretKey;
    private User _user;

    public SetupTwoFactorPage(User user)
    {
        InitializeComponent();
        _user = user;

        // Если 2FA уже настроена, показываем сообщение
        if (!string.IsNullOrEmpty(_user.TwoFactorSecretKey))
        {
            DisplayAlert("Информация", "2FA уже настроена.", "OK");
            return;
        }

        // Генерация секретного ключа
        _secretKey = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
        _user.TwoFactorSecretKey = _secretKey;

        // Генерация QR-кода
        var qrCodeData = GenerateQrCodeData(_secretKey, _user.Email);
        QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeData));

        // Отображение ключа для ручного ввода
        ManualKeyLabel.Text = _secretKey;
    }

    private byte[] GenerateQrCodeData(string secretKey, string email)
    {
        var issuer = "ВашеПриложение"; // Название вашего приложения
        var label = Uri.EscapeDataString(email); // Email пользователя
        var uri = $"otpauth://totp/{issuer}:{label}?secret={secretKey}&issuer={issuer}";

        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        // Сохраняем секретный ключ в базу данных
        var userRepository = new UserRepository(DatabaseService.GetConnection().DatabasePath);
        userRepository.SaveUser(_user);

        await DisplayAlert("Успех", "Настройка 2FA завершена. Пожалуйста, подтвердите код.", "OK");
        await Navigation.PopAsync(); // Возвращаемся на страницу профиля
    }
}