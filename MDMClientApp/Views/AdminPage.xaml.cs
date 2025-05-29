using MDMClientApp.Models;
using MDMClientApp.Repositories;
using MDMClientApp.Services;

namespace MDMClientApp.Views;

public partial class AdminPage : ContentPage
{
    private CompanyRepository _companyRepository;
    private UserRepository _userRepository;

    public AdminPage()
    {
        InitializeComponent();
        _companyRepository = new CompanyRepository(DatabaseService.GetConnection().DatabasePath);
        _userRepository = new UserRepository(DatabaseService.GetConnection().DatabasePath);
        LoadCompanies();
    }

    private void LoadCompanies()
    {
        var companies = _companyRepository.GetCompanies();
        CompaniesCollectionView.ItemsSource = companies;
    }

    private void OnCompanySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Company selectedCompany)
        {
            LoadEmployees(selectedCompany.Id);
        }
    }

    private void LoadEmployees(int companyId)
    {
        var employees = _userRepository.GetUsers().Where(u => u.CompanyId == companyId).ToList();
        EmployeesCollectionView.ItemsSource = employees;
    }

    private async void OnBackToProfileClicked(object sender, EventArgs e)
    {
        // Возвращаемся на страницу профиля
        await Navigation.PopAsync();
    }
}