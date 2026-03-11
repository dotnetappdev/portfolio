using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Portfolio.Shared.Models;

namespace Portfolio.Web.Services;

public class PortfolioApiService(
    IHttpClientFactory httpClientFactory,
    ILogger<PortfolioApiService> logger)
{
    private HttpClient GetClient() => httpClientFactory.CreateClient("PortfolioApi");

    public async Task<List<HeroStatDto>> GetHeroStatsAsync()
    {
        try
        {
            return await GetClient().GetFromJsonAsync<List<HeroStatDto>>("api/herostats") ?? new List<HeroStatDto>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "API unavailable when fetching hero stats. Using fallback data.");
            return GetFallbackHeroStats();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error fetching hero stats. Using fallback data.");
            return GetFallbackHeroStats();
        }
    }

    public async Task<(HeroStatDto? Stat, string? Error)> CreateHeroStatAsync(HeroStatDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Post, "api/herostats", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<HeroStatDto>(), null);
            return (null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create hero stat via API");
            return (null, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateHeroStatAsync(int id, HeroStatDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Put, $"api/herostats/{id}", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update hero stat via API");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteHeroStatAsync(int id, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Delete, $"api/herostats/{id}", adminToken);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete hero stat via API");
            return (false, ex.Message);
        }
    }

    public async Task<List<ProjectDto>> GetProjectsAsync()
    {
        try
        {
            return await GetClient().GetFromJsonAsync<List<ProjectDto>>("api/projects") ?? new List<ProjectDto>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "API unavailable when fetching projects. Using fallback data.");
            return GetFallbackProjects();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error fetching projects. Using fallback data.");
            return GetFallbackProjects();
        }
    }

    public async Task<ProjectDto?> GetProjectBySlugAsync(string slug)
    {
        try
        {
            var response = await GetClient().GetAsync($"api/projects/by-slug/{Uri.EscapeDataString(slug)}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ProjectDto>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "API unavailable when fetching project by slug '{Slug}'. Using fallback data.", slug);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error fetching project by slug '{Slug}'. Using fallback data.", slug);
        }

        return GetFallbackProjects().FirstOrDefault(p =>
            string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        try
        {
            return await GetClient().GetFromJsonAsync<List<SkillDto>>("api/skills") ?? new List<SkillDto>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "API unavailable when fetching skills. Using fallback data.");
            return GetFallbackSkills();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error fetching skills. Using fallback data.");
            return GetFallbackSkills();
        }
    }

    public async Task<bool> SendContactMessageAsync(ContactMessageDto dto)
    {
        try
        {
            var response = await GetClient().PostAsJsonAsync("api/contact", dto);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "API unavailable when sending contact message.");
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error sending contact message.");
            return false;
        }
    }

    public async Task<(ProjectDto? Project, string? Error)> CreateProjectAsync(ProjectDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Post, "api/projects", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<ProjectDto>(), null);
            return (null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create project via API");
            return (null, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateProjectAsync(int id, ProjectDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Put, $"api/projects/{id}", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update project via API");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteProjectAsync(int id, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Delete, $"api/projects/{id}", adminToken);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete project via API");
            return (false, ex.Message);
        }
    }

    // ── Blog ─────────────────────────────────────────────────────────────────

    public async Task<List<BlogPostDto>> GetBlogPostsAsync()
    {
        try
        {
            return await GetClient().GetFromJsonAsync<List<BlogPostDto>>("api/blog") ?? new List<BlogPostDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching blog posts");
            return new List<BlogPostDto>();
        }
    }

    public async Task<BlogPostDto?> GetBlogPostBySlugAsync(string slug)
    {
        try
        {
            var response = await GetClient().GetAsync($"api/blog/{Uri.EscapeDataString(slug)}");
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<BlogPostDto>()
                : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching blog post by slug '{Slug}'", slug);
            return null;
        }
    }

    public async Task<List<BlogPostDto>> GetBlogPostsForAdminAsync(string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Get, "api/blog/admin/all", adminToken);
            using var response = await GetClient().SendAsync(request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<List<BlogPostDto>>() ?? new List<BlogPostDto>()
                : new List<BlogPostDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all blog posts for admin");
            return new List<BlogPostDto>();
        }
    }

    public async Task<(BlogPostDto? Post, string? Error)> CreateBlogPostAsync(BlogPostDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Post, "api/blog", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<BlogPostDto>(), null);
            return (null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create blog post via API");
            return (null, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateBlogPostAsync(int id, BlogPostDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Put, $"api/blog/{id}", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update blog post via API");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteBlogPostAsync(int id, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Delete, $"api/blog/{id}", adminToken);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete blog post via API");
            return (false, ex.Message);
        }
    }

    // ── CMS Pages ─────────────────────────────────────────────────────────────

    public async Task<List<CmsPageDto>> GetPublishedCmsPagesAsync()
    {
        try
        {
            return await GetClient().GetFromJsonAsync<List<CmsPageDto>>("api/cmspages") ?? new List<CmsPageDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching published CMS pages");
            return new List<CmsPageDto>();
        }
    }

    public async Task<CmsPageDto?> GetCmsPageBySlugAsync(string slug)
    {
        try
        {
            var response = await GetClient().GetAsync($"api/cmspages/{Uri.EscapeDataString(slug)}");
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<CmsPageDto>()
                : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching CMS page by slug '{Slug}'", slug);
            return null;
        }
    }

    public async Task<List<CmsPageDto>> GetCmsPagesForAdminAsync(string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Get, "api/cmspages/admin/all", adminToken);
            using var response = await GetClient().SendAsync(request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<List<CmsPageDto>>() ?? new List<CmsPageDto>()
                : new List<CmsPageDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all CMS pages for admin");
            return new List<CmsPageDto>();
        }
    }

    public async Task<(CmsPageDto? Page, string? Error)> CreateCmsPageAsync(CmsPageDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Post, "api/cmspages", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<CmsPageDto>(), null);
            return (null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create CMS page via API");
            return (null, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateCmsPageAsync(int id, CmsPageDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Put, $"api/cmspages/{id}", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update CMS page via API");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteCmsPageAsync(int id, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Delete, $"api/cmspages/{id}", adminToken);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete CMS page via API");
            return (false, ex.Message);
        }
    }

    // ── Menu Items ────────────────────────────────────────────────────────────

    public async Task<List<MenuItemDto>> GetMenuItemsAsync()
    {
        try
        {
            return await GetClient().GetFromJsonAsync<List<MenuItemDto>>("api/menuitems") ?? new List<MenuItemDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching menu items");
            return new List<MenuItemDto>();
        }
    }

    public async Task<List<MenuItemDto>> GetMenuItemsForAdminAsync(string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Get, "api/menuitems/admin/all", adminToken);
            using var response = await GetClient().SendAsync(request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<List<MenuItemDto>>() ?? new List<MenuItemDto>()
                : new List<MenuItemDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all menu items for admin");
            return new List<MenuItemDto>();
        }
    }

    public async Task<(MenuItemDto? Item, string? Error)> CreateMenuItemAsync(MenuItemDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Post, "api/menuitems", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<MenuItemDto>(), null);
            return (null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create menu item via API");
            return (null, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateMenuItemAsync(int id, MenuItemDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Put, $"api/menuitems/{id}", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update menu item via API");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteMenuItemAsync(int id, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Delete, $"api/menuitems/{id}", adminToken);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete menu item via API");
            return (false, ex.Message);
        }
    }

    // ── App Settings ──────────────────────────────────────────────────────────

    public async Task<AppSettingsDto?> GetAppSettingsAsync(string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Get, "api/appsettings", adminToken);
            using var response = await GetClient().SendAsync(request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AppSettingsDto>()
                : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching app settings");
            return null;
        }
    }

    public async Task<PublicAppSettingsDto?> GetPublicAppSettingsAsync()
    {
        try
        {
            return await GetClient().GetFromJsonAsync<PublicAppSettingsDto>("api/appsettings/public");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error fetching public app settings");
            return null;
        }
    }


    public async Task<(bool Success, string? Error)> SaveAppSettingsAsync(AppSettingsDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Put, "api/appsettings", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to save app settings via API");
            return (false, ex.Message);
        }
    }

    // ── Mail Settings ─────────────────────────────────────────────────────────

    public async Task<MailSettingsDto?> GetMailSettingsAsync(string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Get, "api/mailsettings", adminToken);
            using var response = await GetClient().SendAsync(request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<MailSettingsDto>()
                : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching mail settings");
            return null;
        }
    }

    public async Task<(bool Success, string? Error)> SaveMailSettingsAsync(MailSettingsDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Put, "api/mailsettings", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to save mail settings via API");
            return (false, ex.Message);
        }
    }

    // ── SMS Settings ──────────────────────────────────────────────────────────

    public async Task<SmsSettingsDto?> GetSmsSettingsAsync(string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Get, "api/smssettings", adminToken);
            using var response = await GetClient().SendAsync(request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<SmsSettingsDto>()
                : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching SMS settings");
            return null;
        }
    }

    public async Task<(bool Success, string? Error)> SaveSmsSettingsAsync(SmsSettingsDto dto, string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Put, "api/smssettings", adminToken, dto);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to save SMS settings via API");
            return (false, ex.Message);
        }
    }

    // ── Notifications ─────────────────────────────────────────────────────────

    public async Task NotifyVisitorArrivedAsync(string ip, string page, string time, string serviceApiKey)
    {
        try
        {
            var payload = new { ip, page, time };
            using var request = new HttpRequestMessage(HttpMethod.Post, "api/notifications/visitor-arrived")
            {
                Content = JsonContent.Create(payload)
            };
            request.Headers.Add("X-Service-Api-Key", serviceApiKey);
            await GetClient().SendAsync(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending visitor notification to API");
        }
    }

    public async Task<bool> SendTestEmailAsync(string to, string adminToken)
    {
        try
        {
            var payload = new { to };
            using var request = CreateAuthorizedRequest(HttpMethod.Post, "api/notifications/test-email", adminToken, payload);
            using var response = await GetClient().SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to send test email via API");
            return false;
        }
    }

    public async Task<bool> SendTestSmsAsync(string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Post, "api/notifications/test-sms", adminToken);
            using var response = await GetClient().SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to send test SMS via API");
            return false;
        }
    }

    // ── Media Library ─────────────────────────────────────────────────────────

    public async Task<List<MediaFileDto>> GetMediaFilesAsync(string adminToken)
    {
        try
        {
            using var request = CreateAuthorizedRequest(HttpMethod.Get, "api/media", adminToken);
            using var response = await GetClient().SendAsync(request);
            if (!response.IsSuccessStatusCode) return [];
            var files = await response.Content.ReadFromJsonAsync<List<MediaFileDto>>() ?? [];
            var baseUrl = GetClient().BaseAddress?.ToString().TrimEnd('/') ?? string.Empty;
            foreach (var f in files)
                if (f.Url.StartsWith('/'))
                    f.Url = baseUrl + f.Url;
            return files;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching media files");
            return [];
        }
    }

    public async Task<(MediaFileDto? File, string? Error)> UploadMediaFileAsync(
        System.IO.Stream fileStream, string fileName, string contentType, string adminToken)
    {
        try
        {
            using var content   = new MultipartFormDataContent();
            using var sc        = new StreamContent(fileStream);
            sc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            content.Add(sc, "file", fileName);

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/media/upload")
            {
                Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken) },
                Content = content,
            };
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var mf = await response.Content.ReadFromJsonAsync<MediaFileDto>();
                if (mf is not null && mf.Url.StartsWith('/'))
                {
                    var baseUrl = GetClient().BaseAddress?.ToString().TrimEnd('/') ?? string.Empty;
                    mf.Url = baseUrl + mf.Url;
                }
                return (mf, null);
            }
            return (null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to upload media file");
            return (null, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteMediaFileAsync(int id, string adminToken)
    {
        try
        {
            using var request  = CreateAuthorizedRequest(HttpMethod.Delete, $"api/media/{id}", adminToken);
            using var response = await GetClient().SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete media file via API");
            return (false, ex.Message);
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static HttpRequestMessage CreateAuthorizedRequest(
        HttpMethod method, string url, string token, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        if (body != null)
            request.Content = JsonContent.Create(body);
        return request;
    }

    private static List<ProjectDto> GetFallbackProjects() => new()
    {
        new ProjectDto { Id = 1, Title = "BookIt", Slug = "bookit", ShortDescription = "A real-time Blazor booking management system", Description = "BookIt is a full-featured booking management system built with ASP.NET Core Blazor. Businesses use it to manage appointments, resources, and customer bookings through a modern interface with light and dark mode support. Built on a clean architecture with real-time availability tracking, SMS notifications for customers, and a responsive MudBlazor UI.", TechStack = "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor, C# .NET 10", Category = "Work Project", IsFeatured = true, SortOrder = 1, ImageUrl = "/images/bookit.svg", GitHubUrl = "https://github.com/dotnetappdev/bookit" },
        new ProjectDto { Id = 2, Title = "MAUI Cross-Platform App", Slug = "maui-cross-platform-app", ShortDescription = "A .NET MAUI mobile and desktop application", Description = "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms from a single shared codebase, with REST API integration and offline-friendly data handling.", TechStack = ".NET MAUI, C#, XAML, REST APIs", Category = "Mobile Application", IsFeatured = true, SortOrder = 2, GitHubUrl = "https://github.com/dotnetappdev/maui-app" },
        new ProjectDto { Id = 3, Title = "Curo", Slug = "curo", ShortDescription = "Healthcare care management platform", Description = "Curo is a healthcare care management platform that replaced a paper-based system used by community carers. Built with Blazor and ASP.NET Core, it gives carers a task-driven workflow on any device and provides care managers with a live dashboard showing real-time visit progress. Hosted on Azure with full audit logging and role-based access control.", TechStack = "ASP.NET Core, Blazor, SQL Server, Entity Framework Core, Azure, MudBlazor, C# .NET 10", Category = "Work Project", IsFeatured = true, SortOrder = 3, ImageUrl = "/images/curo.svg", GitHubUrl = "https://github.com/dotnetappdev/curo" },
        new ProjectDto { Id = 4, Title = "Patient CRM", Slug = "patient-crm", ShortDescription = "Patient relationship management system (in development)", Description = "A Patient CRM currently in development, designed to help healthcare providers manage patient relationships, appointments, communications, and care history in one centralised platform. Built with .NET 10, Blazor, and a REST API backend.", TechStack = "ASP.NET Core .NET 10, Blazor, SQL Server, EF Core, REST API, MudBlazor", Category = "Healthcare", IsFeatured = true, SortOrder = 4, GitHubUrl = "https://github.com/dotnetappdev/patient-crm" },
        new ProjectDto { Id = 5, Title = "AI Diagnostic Assistant", Slug = "ai-diagnostic-assistant", ShortDescription = "AI-powered clinical decision support tool", Description = "An AI assistant integrated into a healthcare platform that helps clinical staff surface relevant patient history, flag anomalies in test results, and draft care plan notes. Built on Semantic Kernel and Azure OpenAI with a strict evaluation layer that confidence-scores every response before it reaches clinical staff. All AI output is fully audited.", TechStack = "Semantic Kernel, Azure OpenAI, ASP.NET Core, Blazor, SQL Server, Vector Search, .NET 10", Category = "AI", IsFeatured = true, SortOrder = 5, GitHubUrl = "https://github.com/dotnetappdev/ai-diagnostic-assistant" },
        new ProjectDto { Id = 6, Title = "SecureAPI Framework", Slug = "secure-api-framework", ShortDescription = "Hardened API security baseline for .NET", Description = "A reusable security baseline for ASP.NET Core APIs covering JWT authentication with algorithm pinning, OWASP Top Ten mitigations, rate limiting, structured security logging, and automated dependency vulnerability scanning in the CI pipeline. Used as the starting point for all new API projects so that security is built in from the first commit rather than retrofitted.", TechStack = "ASP.NET Core, JWT, OAuth2, OWASP, Rate Limiting, Polly, GitHub Actions", Category = "Security", IsFeatured = true, SortOrder = 6, GitHubUrl = "https://github.com/dotnetappdev/secure-api-framework" },
        new ProjectDto { Id = 7, Title = "TalentConnect", Slug = "talentconnect", ShortDescription = "A Blazor recruitment management platform", Description = "TalentConnect is a full-featured recruitment management platform built with Blazor and ASP.NET Core. It streamlines the end-to-end hiring process with job posting management, a configurable multi-stage candidate pipeline, interview scheduling, automated notifications, and detailed recruitment analytics. Built for teams who want a data-driven hiring workflow without the spreadsheets.", TechStack = "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor, REST API, C# .NET 10", Category = "Work Project", IsFeatured = true, SortOrder = 7, ImageUrl = "/images/talentconnect.svg", GitHubUrl = "https://github.com/dotnetappdev/talentconnect" }
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

    private static List<HeroStatDto> GetFallbackHeroStats() => new()
    {
        new HeroStatDto { Id = 1, Value = "30+",    Label = "Years in .NET",          Color = "Primary",   SortOrder = 1 },
        new HeroStatDto { Id = 2, Value = "AI",     Label = "First Approach",         Color = "Secondary", SortOrder = 2 },
        new HeroStatDto { Id = 3, Value = "SecOps", Label = "Security Built In",      Color = "Error",     SortOrder = 3 },
        new HeroStatDto { Id = 4, Value = "TDD/BDD",Label = "Test-Focused Developer", Color = "Success",   SortOrder = 4 }
    };
}
