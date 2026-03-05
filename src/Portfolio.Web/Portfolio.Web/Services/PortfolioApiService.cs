using Portfolio.Shared.Models;
using System.Net.Http.Json;

namespace Portfolio.Web.Services;

public class PortfolioApiService
{
    private readonly HttpClient _httpClient;

    public PortfolioApiService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<ProjectDto>> GetProjectsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects") ?? new List<ProjectDto>();
        }
        catch
        {
            return GetFallbackProjects();
        }
    }

    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skills") ?? new List<SkillDto>();
        }
        catch
        {
            return GetFallbackSkills();
        }
    }

    public async Task<bool> SendContactMessageAsync(ContactMessageDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/contact", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static List<ProjectDto> GetFallbackProjects() => new()
    {
        new ProjectDto { Id = 1, Title = "BookIt", ShortDescription = "A Blazor booking management application", Description = "BookIt is a full-featured booking management system built with ASP.NET Core Blazor. It allows businesses to manage appointments, resources, and customer bookings with an intuitive and modern interface featuring light and dark mode support.", TechStack = "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor", Category = "Web Application", IsFeatured = true, SortOrder = 1 },
        new ProjectDto { Id = 2, Title = "MAUI Cross-Platform App", ShortDescription = "A .NET MAUI mobile and desktop application", Description = "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms with shared business logic.", TechStack = ".NET MAUI, C#, XAML, REST APIs", Category = "Mobile Application", IsFeatured = true, SortOrder = 2 },
        new ProjectDto { Id = 3, Title = "Curo", ShortDescription = "Healthcare care management platform", Description = "Curo is a healthcare care management platform designed to streamline patient care coordination. It provides tools for care plan management, patient tracking, and clinical workflow automation.", TechStack = "ASP.NET Core, Blazor, SQL Server, Entity Framework Core, Azure", Category = "Healthcare", IsFeatured = true, SortOrder = 3 },
        new ProjectDto { Id = 4, Title = "Patient CRM", ShortDescription = "Patient relationship management system (In Development)", Description = "A modern Patient CRM system currently in development, designed to help healthcare providers manage patient relationships, appointments, communications, and care history in one centralised platform.", TechStack = "ASP.NET Core .NET 10, Blazor, SQL Server, EF Core, REST API, MudBlazor", Category = "Healthcare", IsFeatured = true, SortOrder = 4 }
    };

    private static List<SkillDto> GetFallbackSkills() => new()
    {
        new SkillDto { Id = 1, Name = "C#", Category = "Languages", Proficiency = 98 },
        new SkillDto { Id = 2, Name = "ASP.NET Core", Category = "Frameworks", Proficiency = 95 },
        new SkillDto { Id = 3, Name = "Blazor", Category = "Frameworks", Proficiency = 92 },
        new SkillDto { Id = 4, Name = ".NET MAUI", Category = "Frameworks", Proficiency = 85 },
        new SkillDto { Id = 5, Name = "SQL Server", Category = "Databases", Proficiency = 90 },
        new SkillDto { Id = 6, Name = "Entity Framework Core", Category = "Databases", Proficiency = 90 },
        new SkillDto { Id = 7, Name = "REST API Design", Category = "Architecture", Proficiency = 92 },
        new SkillDto { Id = 8, Name = "MudBlazor", Category = "UI Frameworks", Proficiency = 88 },
        new SkillDto { Id = 9, Name = "Azure", Category = "Cloud", Proficiency = 80 },
        new SkillDto { Id = 10, Name = "JavaScript", Category = "Languages", Proficiency = 75 }
    };
}
