using Microsoft.Extensions.Logging;
using Portfolio.Shared.Models;
using System.Net.Http.Json;

namespace Portfolio.Web.Services;

public class PortfolioApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PortfolioApiService> _logger;

    public PortfolioApiService(HttpClient httpClient, ILogger<PortfolioApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<ProjectDto>> GetProjectsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects") ?? new List<ProjectDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "API unavailable when fetching projects. Using fallback data.");
            return GetFallbackProjects();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching projects. Using fallback data.");
            return GetFallbackProjects();
        }
    }

    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skills") ?? new List<SkillDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "API unavailable when fetching skills. Using fallback data.");
            return GetFallbackSkills();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching skills. Using fallback data.");
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
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "API unavailable when sending contact message.");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error sending contact message.");
            return false;
        }
    }

    private static List<ProjectDto> GetFallbackProjects() => new()
    {
        new ProjectDto { Id = 1, Title = "BookIt", ShortDescription = "A Blazor booking management application", Description = "BookIt is a full-featured booking management system built with ASP.NET Core Blazor. It allows businesses to manage appointments, resources, and customer bookings with an intuitive and modern interface featuring light and dark mode support.", TechStack = "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor", Category = "Web Application", IsFeatured = true, SortOrder = 1 },
        new ProjectDto { Id = 2, Title = "MAUI Cross-Platform App", ShortDescription = "A .NET MAUI mobile and desktop application", Description = "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms with shared business logic.", TechStack = ".NET MAUI, C#, XAML, REST APIs", Category = "Mobile Application", IsFeatured = true, SortOrder = 2 },
        new ProjectDto { Id = 3, Title = "Curo", ShortDescription = "Healthcare care management platform", Description = "Curo is a healthcare care management platform designed to streamline patient care coordination. It provides tools for care plan management, patient tracking, and clinical workflow automation.", TechStack = "ASP.NET Core, Blazor, SQL Server, Entity Framework Core, Azure", Category = "Healthcare", IsFeatured = true, SortOrder = 3 },
        new ProjectDto { Id = 4, Title = "Patient CRM", ShortDescription = "Patient relationship management system (In Development)", Description = "A modern Patient CRM system currently in development, designed to help healthcare providers manage patient relationships, appointments, communications, and care history in one centralised platform.", TechStack = "ASP.NET Core .NET 10, Blazor, SQL Server, EF Core, REST API, MudBlazor", Category = "Healthcare", IsFeatured = true, SortOrder = 4 },
        new ProjectDto { Id = 5, Title = "AI Diagnostic Assistant", ShortDescription = "AI powered clinical decision support tool", Description = "An AI assistant integrated into a healthcare platform that helps clinical staff surface relevant patient history, flag anomalies in test results, and draft care plan notes. Built on Semantic Kernel and Azure OpenAI with a strict evaluation layer to prevent hallucinations reaching clinical staff.", TechStack = "Semantic Kernel, Azure OpenAI, ASP.NET Core, Blazor, SQL Server, Vector Search, .NET 10", Category = "AI", IsFeatured = true, SortOrder = 5 },
        new ProjectDto { Id = 6, Title = "SecureAPI Framework", ShortDescription = "Hardened API security baseline for .NET", Description = "A reusable security baseline for ASP.NET Core APIs covering JWT authentication with algorithm pinning, OWASP Top Ten mitigations, rate limiting, structured security logging, and automated dependency vulnerability scanning in the CI pipeline.", TechStack = "ASP.NET Core, JWT, OAuth2, OWASP, Rate Limiting, Polly, GitHub Actions", Category = "Security", IsFeatured = true, SortOrder = 6 }
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
        new SkillDto { Id = 9, Name = "Azure", Category = "Cloud", Proficiency = 82 },
        new SkillDto { Id = 10, Name = "JavaScript", Category = "Languages", Proficiency = 75 },
        new SkillDto { Id = 11, Name = "Semantic Kernel", Category = "AI", Proficiency = 88 },
        new SkillDto { Id = 12, Name = "Azure OpenAI", Category = "AI", Proficiency = 85 },
        new SkillDto { Id = 13, Name = "ML.NET", Category = "AI", Proficiency = 75 },
        new SkillDto { Id = 14, Name = "RAG Pipelines", Category = "AI", Proficiency = 82 },
        new SkillDto { Id = 15, Name = "Vector Search", Category = "AI", Proficiency = 78 },
        new SkillDto { Id = 16, Name = "OWASP Top Ten", Category = "Security", Proficiency = 90 },
        new SkillDto { Id = 17, Name = "OAuth2 / OIDC", Category = "Security", Proficiency = 88 },
        new SkillDto { Id = 18, Name = "JWT Authentication", Category = "Security", Proficiency = 92 },
        new SkillDto { Id = 19, Name = "Threat Modelling", Category = "Security", Proficiency = 80 },
        new SkillDto { Id = 20, Name = "Penetration Testing", Category = "Security", Proficiency = 72 }
    };
}
