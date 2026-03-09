using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TwilioFromNumber",
                table: "SmsSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TwilioAuthToken",
                table: "SmsSettings",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TwilioAccountSid",
                table: "SmsSettings",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "SmsSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabled",
                table: "SmsSettings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ClickSendUsername",
                table: "SmsSettings",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClickSendFromName",
                table: "SmsSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClickSendApiKey",
                table: "SmsSettings",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminReceiverNumber",
                table: "SmsSettings",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SmsSettings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Projects",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "TechStack",
                table: "Projects",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Projects",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Projects",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "LiveUrl",
                table: "Projects",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Projects",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GitHubUrl",
                table: "Projects",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Projects",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "MenuItems",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "MenuItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "OpenInNewTab",
                table: "MenuItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "MenuItems",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "MenuItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MenuItems",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MenuItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "HeroStats",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "HeroStats",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "HeroStats",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "HeroStats",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HeroStats",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "CmsPages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CmsPages",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "CmsPages",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "CmsPages",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "OgImage",
                table: "CmsPages",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaTitle",
                table: "CmsPages",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "CmsPages",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "CmsPages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FeaturedImage",
                table: "CmsPages",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CanonicalUrl",
                table: "CmsPages",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "CmsPages",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CmsPages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "BlogPosts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<int>(
                name: "ReadMinutes",
                table: "BlogPosts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "BlogPosts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "OgImage",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaTitle",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "BlogPosts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FeaturedImage",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CanonicalUrl",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "BlogPosts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BlogPosts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "ApiBaseUrl",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Body",
                value: "<p>I spent a good chunk of last year integrating large language model features into a healthcare application. Not a side project, not a conference demo. A real system where a nurse practitioner would read the output and make decisions based on it. That context changes everything about how you approach AI.</p>\n\n<h2>The Happy Path Is Not the Full Story</h2>\n<p>The tutorials all show you the happy path. You call the API, you get a smart-looking response, and you feel great about the future. What they do not show you is what happens when the model confidently returns something that sounds completely reasonable and is completely wrong. In a healthcare context that is not an embarrassing chatbot moment. It is a clinical risk.</p>\n\n<h2>Build an Evaluation Layer First</h2>\n<p>The first thing I learned is that you need an evaluation layer before you expose any AI output to users. For us that meant a confidence-scoring approach where low-confidence responses were flagged for human review rather than displayed directly. <strong>Semantic Kernel</strong> makes this fairly manageable because the plugin model lets you compose your own validation steps alongside the model calls.</p>\n\n<h2>Prompt Injection Is Real</h2>\n<p>The second thing I learned is about prompt injection. I had read about it in theory but seeing it attempted in the wild is something else. Users discover quickly that phrasing their input in certain ways can influence what the model does. When your AI assistant has access to a patient record database, that is a problem you care about very much.</p>\n<p>The fix is treating all user input as untrusted data before it ever reaches the prompt. That sounds obvious when you say it, but it is easy to be sloppy about in practice.</p>\n\n<h2>Semantic Kernel as a Solid Foundation</h2>\n<p><strong>Semantic Kernel</strong> has been a reliable foundation for this work. The ability to define typed functions that the kernel can call, log, and audit makes building responsible AI much more tractable than rolling everything by hand. The memory and vector search support is good enough for most RAG scenarios without needing to reach for separate infrastructure.</p>\n\n<h2>Stream Everything</h2>\n<p>The third lesson, and probably the most important one, is about latency expectations. Users who have never interacted with an LLM before will stare at a spinner for about three seconds before concluding the application is broken. <strong>Streaming responses</strong> change the experience completely.</p>\n<p>In C# the async enumerable pattern works beautifully for this. Tokens arrive at the client almost immediately and the perception of performance is transformed, even when the total time is the same.</p>\n\n<h2>Four Things That Will Save You Pain</h2>\n<p>If I were starting a new AI integration today I would go straight to Semantic Kernel, set up proper structured logging from day one, build the evaluation layer before wiring anything to the UI, and stream everything. Those four things will save you enormous amounts of pain and produce a result that real users can trust.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Body",
                value: "<p>There is a version of security awareness that goes like this: you download the OWASP Top Ten PDF, read through it a few days before launch, tick off the obvious ones, and ship. I did that version for longer than I would like to admit.</p>\n\n<h2>What Changed My Thinking</h2>\n<p>The shift happened when I was working on a system that stored clinical data and we brought in an external security consultant for a review. They found four significant vulnerabilities in about two hours. None of them were exotic. All of them were on the list I had been scanning.</p>\n<p>The problem was not that I did not know about injection or broken authentication. The problem was that I had been reading the list as a set of abstract categories rather than as a description of how real attacks actually happen.</p>\n\n<h2>Injection Is About Trust Boundaries</h2>\n<p>Take injection. Reading it as a checkbox, you think about SQL injection, add parameterised queries everywhere, and consider it done. But injection covers a much broader story. It is about trusting data that comes from outside your control boundary.</p>\n<p>In a modern .NET application that boundary is much larger than it used to be. You have user input, obviously. But you also have data coming back from third-party APIs, from uploaded files, from webhook payloads, from AI model outputs. When I started thinking about injection as a story about trust boundaries rather than a technique, I found three places in our application where we were trusting data we should not have been.</p>\n\n<h2>Broken Access Control Is About What Happens After Login</h2>\n<p>Broken access control is the same thing. The list tells you to check authorisation. Fine. But the story underneath that is about what happens when someone knows the shape of your URLs. In one of our early applications we had resource IDs that were sequential integers. You did not need to be a penetration tester to realise that incrementing the number in the URL gave you access to another user's records.</p>\n<p>We had authentication working perfectly. We had no authorisation on the actual resource. Two completely different things.</p>\n<p>In .NET I now use a combination of resource-based authorisation via <strong>IAuthorizationService</strong> and opaque identifiers rather than sequential database IDs for anything that appears in a URL. Neither is complicated. Both would have caught the issues above.</p>\n\n<h2>Use the List as a Story, Not a Checklist</h2>\n<p>The way I approach security reviews now is to sit with the OWASP list and for each item ask not whether we have done the obvious thing, but where in our specific system the story that category describes could unfold. It is slower. But the things it surfaces are real.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Body",
                value: "<p>I wrote code in the late nineties that I had to maintain in the early two thousands. I wrote code in the early two thousands that I had to go back to in 2015. Each of those encounters with past me was educational in ways that no conference talk or book ever matched.</p>\n\n<h2>Naming Is Not a Minor Detail</h2>\n<p>The first thing past me always got wrong was naming. Not catastrophically wrong, just slightly off in ways that compound. A class called <strong>DataHelper</strong>. A method called <strong>Process</strong>. A variable called <code>temp</code> that turned out to be a tax calculation result that several other things depended on.</p>\n<p>I have spent real hours in old codebases trying to understand what something does before I dare change it. The cost of that time is invisible in the moment when you are rushing to ship, but very visible when you are the one paying it later.</p>\n<p>Modern C# is genuinely excellent for expressing intent clearly. Record types, pattern matching, and the improvements to switch expressions since C# 8 have made it possible to write code that reads almost like a description of the business problem rather than a set of mechanical instructions. I use those features aggressively now because I know future me will be grateful.</p>\n\n<h2>Coupling Is the Quiet Killer</h2>\n<p>The second thing past me got wrong was coupling. Not always the obvious kind where everything calls everything else. More often the subtle kind where a bunch of things share a database table, or where a service class has seventeen methods that are really three different concepts that grew together because nobody stopped to separate them.</p>\n<p>The single responsibility principle sounds like a rule imposed by someone who did not have deadlines. Having maintained code that violated it for years, it sounds like common sense.</p>\n<p>In C# terms this means being quite aggressive about keeping classes small and focused. When a class crosses about 200 lines I start asking whether it is actually two things. When a method needs more than three parameters I ask whether those parameters are describing a concept that deserves its own type. These are not hard rules; they are signals worth paying attention to.</p>\n\n<h2>Make Invalid States Impossible</h2>\n<p>The third lesson is about making invalid states unrepresentable. C# has been getting better at this for years. <strong>Nullable reference types</strong>, discriminated union patterns via records and sealed hierarchies, required properties: all of these make it harder to construct an object in a state your system cannot handle.</p>\n<p>The bugs that cause the worst incidents are almost always states the developer did not think were possible. Making them impossible at the type level is worth a lot.</p>\n\n<h2>The Evidence Accumulates</h2>\n<p>None of this is news to anyone who thinks seriously about code quality. The difference is that I now have thirty years of evidence that it actually matters, and that the people who will thank you most for writing clear, well-structured code are often yourself coming back to it three years later.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4,
                column: "Body",
                value: "<p>JWT is everywhere in modern web APIs. It is also one of the things I see misunderstood most often when I review code. Not because people do not understand what a JWT is in general terms, but because there are a handful of details that look minor until they become the reason your authentication is compromised.</p>\n\n<h2>Algorithm Confusion</h2>\n<p>The first issue is algorithm confusion. A JWT has a header that declares which algorithm was used to sign it. The algorithm <code>none</code> means no signature at all. Early libraries would happily accept this if you did not explicitly configure which algorithms you would accept.</p>\n<p><strong>ASP.NET Core</strong> does not have this problem by default if you use the standard JwtBearer middleware and configure <code>ValidAlgorithms</code>. But if you are rolling your own token validation or using a third-party library, check that it rejects unsigned tokens explicitly. This is not a theoretical concern; it has been exploited in real applications.</p>\n\n<h2>Key Length Matters More Than You Think</h2>\n<p>The second thing is key length. A symmetric HMAC key for HS256 should be at least 256 bits, which is 32 bytes. I have reviewed systems where the key was a short memorable string like a product name or a domain.</p>\n<p>Brute-forcing a weak key against a known JWT structure is not difficult. More importantly, if anyone can get hold of the signing key by any means, they can issue themselves any token they want. That key needs to be treated like a password and stored accordingly.</p>\n\n<h2>Expiry and Revocation</h2>\n<p>The third thing is expiry and revocation. JWTs are stateless, which is part of their appeal, but that statelessness means you cannot revoke one once issued unless you maintain a denylist.</p>\n<p>Short expiry times with a <strong>refresh token pattern</strong> are the standard answer. The refresh token lives server-side and can be revoked. Short access token expiry limits the damage window if one gets intercepted. A one-hour access token expiry is reasonable for most applications. Twenty-four hours is pushing it. No expiry is how you end up with tokens that are valid forever after an employee leaves.</p>\n\n<h2>A Practical Configuration</h2>\n<p>In ASP.NET Core I configure token validation like this: require issuer, require audience, validate lifetime, validate the signing key, reject tokens with no expiry, and specify the exact algorithm. That covers the common pitfalls.</p>\n<p>Then I make sure the key comes from a secrets store rather than a config file that ends up in source control, because I have seen that too and it is a bad day when you notice it.</p>\n<p>JWT done right is a solid foundation for stateless API authentication. JWT done casually is a set of vulnerabilities waiting to be found by someone who is looking.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5,
                column: "Body",
                value: "<p>The support ticket said the dashboard was slow. That was all the information I had when I started. By the time I had finished I had a 99% reduction in load time and a better understanding of why production and development environments lie to you about performance in different ways.</p>\n\n<h2>Start With Measurement, Not Assumptions</h2>\n<p>The first instinct when something is slow is to open the code and look for the obvious problem. I have learned not to do this. The obvious problem is rarely the actual problem, and the time you spend fixing the obvious thing is time you are not spending measuring.</p>\n<p>I started by adding <strong>Application Insights</strong> telemetry to the API endpoint and attaching a SQL trace to the production database. Within ten minutes I had the data I needed. The dashboard query was issuing 847 SQL statements per request.</p>\n\n<h2>The N+1 Query Hidden in Plain Sight</h2>\n<p>Eight hundred and forty-seven queries. On a developer laptop with a database containing twelve rows, this is imperceptible. In production with three thousand rows and a database server on a separate host, the round-trip latency alone adds up fast.</p>\n<p>The root cause was a classic <strong>N+1 query problem</strong> buried inside a service method that had grown organically over eighteen months. The outer query fetched a list of records. A foreach loop then called a second method for each record. That second method hit the database independently, once per item. Nobody had noticed because the test data was tiny and the database was local.</p>\n<p>In <strong>Entity Framework Core</strong> terms, the fix was straightforward: replace the loop with a single query using <code>Include</code> and <code>ThenInclude</code> for the related data, and add a projection to fetch only the columns the dashboard actually needed rather than materialising full entities. The resulting SQL was one statement joining four tables.</p>\n\n<h2>The Second Problem the First Problem Was Hiding</h2>\n<p>With the N+1 query fixed, load time dropped from eight seconds to about 900 milliseconds. Better, but still not good enough. The SQL trace showed a single query now, but it was doing a full table scan on a column that appeared in every WHERE clause.</p>\n<p>The column had no index. It had never had an index because in development the table had a handful of rows and a sequential scan was trivially fast. In production the table had grown to three hundred thousand rows and a sequential scan was taking 700 milliseconds on its own.</p>\n<p>Adding a composite index on the two most-used filter columns brought the query time to under 20 milliseconds. Combined with the N+1 fix, the total endpoint time measured at 80 milliseconds from the load test client.</p>\n\n<h2>What This Experience Reinforced</h2>\n<p>Performance problems are rarely where you think they are. Measure first with real production data and real production topology before touching a line of code. The investment in proper observability — structured logging, APM tooling, query tracing — pays back in hours, not weeks.</p>\n<p>The other thing this reinforced is that code review catches logical problems but usually not performance problems. Performance testing against production-scale data is a separate discipline, and it is worth building into your delivery process rather than treating as an emergency response when something goes wrong in production.</p>\n<p>The fix itself took about two hours. Finding it took about thirty minutes with the right tools. Getting the right tools in place took most of the afternoon of the day I joined the team. That was the best afternoon I spent on that project.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6,
                column: "Body",
                value: "<p>BookIt started as a client request for a simple online booking system. By the time it shipped it had become one of the most satisfying builds I have done in recent years, partly because of the technology choices and partly because of what those choices enabled at the UI layer.</p>\n\n<h2>A Flexible Domain Model from the Start</h2>\n<p>The brief was straightforward: businesses needed to manage appointments, track resources, and let customers book online. The tricky part was that the businesses themselves were diverse: a hair salon, a physio clinic, a training room hire company, and each had slightly different ideas about what a booking even was.</p>\n<p>That diversity pushed me toward a flexible domain model early on rather than hardcoding assumptions about slot length, resource type, or cancellation policy.</p>\n\n<h2>Why Blazor Server Was the Right Choice</h2>\n<p>I chose <strong>Blazor Server</strong> for the front end from the start. The real-time availability requirements meant that a traditional request and response cycle would create friction in the UI. With Blazor Server and <strong>SignalR</strong> underneath, I could push availability updates to connected clients the moment a slot was taken without the client polling. In practice this makes the booking experience feel instant in a way that a standard MVC form cannot match.</p>\n\n<h2>MudBlazor Did the Heavy Lifting</h2>\n<p><strong>MudBlazor</strong> was the natural choice for the component library. The project needed a professional-grade UI with both light and dark mode, and MudBlazor's theming system handles that with almost no ceremony. The MudDataGrid component handled the admin booking list, MudCalendar handled the visual schedule view, and the chip system made tagging bookings with status and category feel polished without custom CSS.</p>\n\n<h2>Clean Architecture Paid Off</h2>\n<p>The data layer uses <strong>Entity Framework Core 9</strong> with SQL Server. I applied a clean architecture pattern with domain entities, repository interfaces in the application layer, and EF Core implementations in the infrastructure layer. This made unit testing the booking logic straightforward and kept the domain model free of EF Core attributes.</p>\n\n<h2>SMS Notifications: A Late Addition That Became Essential</h2>\n<p>SMS notifications were a late addition that turned out to be heavily used. Customers get a confirmation text when they book, a reminder 24 hours before, and a follow-up after. I built a provider-agnostic SMS abstraction so the underlying provider could be swapped without touching the booking logic. In production we are using ClickSend, but Twilio support is also built in.</p>\n\n<h2>The One Thing I Would Do Differently</h2>\n<p>The one thing I would do differently is implement optimistic concurrency on slot reservations from the very first sprint. We had a brief window early in development where two simultaneous bookings for the same slot were both confirmed. The fix was a database-level check combined with a retry policy on the Blazor component, but it would have been simpler to design for that from the start.</p>\n<p>BookIt is now live and has processed thousands of bookings across multiple businesses. The Blazor real-time approach has aged well and I am still satisfied with the architectural decisions made at the outset.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7,
                column: "Body",
                value: "<p>Building software for healthcare is different from building it for almost any other domain. The margin for error is smaller, the compliance requirements are real, and the people using the system are often under significant time and emotional pressure. Curo was built with all of that in mind.</p>\n\n<h2>Replacing Paper with a Live Dashboard</h2>\n<p>The platform was commissioned to replace a paper-based care plan system used by a team of community carers. Their workflow involved visiting patients at home, recording observations, completing tasks from a care plan, and escalating anything concerning to a care manager. On paper this meant a lot of phone calls, lost handover notes, and care managers who had no live visibility into what was happening in the field.</p>\n<p>Curo brings that workflow into a <strong>Blazor</strong> application accessible on any device. Carers use it on tablets during visits to check tasks, record vitals, and mark care plan steps complete. Care managers get a live dashboard showing which carers are active, which patients have been visited, and which tasks are overdue. The real-time updates come via <strong>SignalR</strong>, the same mechanism that makes Blazor Server so well suited to operational dashboards.</p>\n\n<h2>The Hardest Design Problem: Care Plans</h2>\n<p>The domain model for care plans was the hardest design problem. A care plan for one patient might have fifty tasks across medication, nutrition, mobility, and social engagement, each with different frequencies, assigned carers, and escalation thresholds. Getting that model right took several iterations and some very direct conversations with the clinical team about what they actually needed versus what they initially asked for.</p>\n\n<h2>Security and Compliance Were Non-Negotiable</h2>\n<p>The application holds personal health information, which meant encryption at rest and in transit, role-based access control at every API endpoint, full audit logging of every data access, and a carefully scoped permission model so that a carer could only see the patients assigned to them.</p>\n<p>I used <strong>ASP.NET Core Identity</strong> with custom claims for the RBAC layer and a purpose-built audit middleware that writes to a separate audit log table rather than mixing audit records with application data.</p>\n\n<h2>Azure Hosting on a Reasonable Budget</h2>\n<p>Azure hosting gave us the infrastructure story at reasonable cost. The application runs on <strong>Azure App Service</strong> with Azure SQL Database. Azure Active Directory handles the identity provider for the care management organisation. Deployment is automated through a GitHub Actions pipeline that builds, tests, runs OWASP dependency checks, and deploys to staging before production.</p>\n\n<h2>The Moment It Mattered</h2>\n<p>The feedback from carers after go-live was genuinely moving. The care manager described being able to see her team's visits happening in real time as transformative. That is the kind of outcome that reminds me why building useful software matters.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8,
                column: "Body",
                value: "<p>Recruitment is one of those domains that seems simple until you are three weeks into building the software for it. The surface area is manageable: jobs, candidates, applications, interviews and offers, but the workflow complexity underneath is substantial and every organisation does it slightly differently.</p>\n\n<h2>From Spreadsheets to a Single Platform</h2>\n<p>TalentConnect was built for a recruitment team who needed to move away from spreadsheets and shared inboxes. They were tracking candidates across multiple roles simultaneously, scheduling interviews via email chains, and producing monthly reports by hand from a dozen different sources. The brief was to consolidate all of that into a single <strong>Blazor</strong> application.</p>\n\n<h2>The Candidate Pipeline as the Heart of the Design</h2>\n<p>The candidate pipeline was the heart of the design. Every candidate progresses through configurable stages: applied, screening, shortlisted, first interview, second interview, offer, hired or rejected. Each transition is timestamped and the history is preserved so the team can see how long candidates have been at each stage and where the bottlenecks in their process are. That audit trail also proved valuable for compliance purposes.</p>\n\n<h2>Real-Time Updates via Blazor Server</h2>\n<p><strong>Blazor Server</strong> was the right choice here for the same reasons it works well in other operational tools: the recruiter-facing pipeline board benefits from live updates when a colleague moves a candidate or adds a note, and the team is always working from current data without needing to refresh.</p>\n\n<h2>MudBlazor Made the Kanban Board Possible</h2>\n<p><strong>MudBlazor</strong>'s drag-and-drop support made the kanban-style pipeline board possible without writing a single line of custom JavaScript. The component library's data grid handled the tabular candidate list, filter chips made filtering by role, stage, and recruiter feel natural, and the stepper component guided new job posting creation through a structured flow that reduced data entry errors.</p>\n\n<h2>Notifications Driven by Domain Events</h2>\n<p>The notification system was a significant piece of work in its own right. Candidates receive automated emails at key pipeline stages: application received, shortlisted, interview invitation and outcome. Recruiters get Blazor toast notifications when a candidate completes an online assessment or when an interview response comes in. The whole thing is driven by an event-based model where stage transitions publish domain events and notification handlers decide what to send to whom.</p>\n\n<h2>Analytics That Required Careful Query Design</h2>\n<p>One of the more interesting technical challenges was the analytics reporting. The team wanted to see time-to-hire by role, source channel conversion rates, and interviewer pass rates. These calculations involve aggregating across pipeline history and doing date arithmetic across potentially thousands of candidate journeys.</p>\n<p>Getting those queries right in <strong>EF Core</strong> without full table scans required some careful index design and a few projection-only queries that bypass change tracking entirely.</p>\n<p>TalentConnect is now the primary recruitment tool for the team that commissioned it. Spreadsheet use has dropped to zero and the monthly report now takes minutes rather than a working day.</p>");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TwilioFromNumber",
                table: "SmsSettings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TwilioAuthToken",
                table: "SmsSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TwilioAccountSid",
                table: "SmsSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "SmsSettings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabled",
                table: "SmsSettings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ClickSendUsername",
                table: "SmsSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClickSendFromName",
                table: "SmsSettings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClickSendApiKey",
                table: "SmsSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminReceiverNumber",
                table: "SmsSettings",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SmsSettings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Projects",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "TechStack",
                table: "Projects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Projects",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Projects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "LiveUrl",
                table: "Projects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "Projects",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Projects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GitHubUrl",
                table: "Projects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Projects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "MenuItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "MenuItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "OpenInNewTab",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "MenuItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MenuItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MenuItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "HeroStats",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "HeroStats",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "HeroStats",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "HeroStats",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HeroStats",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "CmsPages",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CmsPages",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "CmsPages",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "CmsPages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "OgImage",
                table: "CmsPages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaTitle",
                table: "CmsPages",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "CmsPages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "CmsPages",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "FeaturedImage",
                table: "CmsPages",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CanonicalUrl",
                table: "CmsPages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "CmsPages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CmsPages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "BlogPosts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BlogPosts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "BlogPosts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "BlogPosts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "BlogPosts",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<int>(
                name: "ReadMinutes",
                table: "BlogPosts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "BlogPosts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "OgImage",
                table: "BlogPosts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaTitle",
                table: "BlogPosts",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "BlogPosts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "BlogPosts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "FeaturedImage",
                table: "BlogPosts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "BlogPosts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CanonicalUrl",
                table: "BlogPosts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BlogPosts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "ApiBaseUrl",
                table: "AppSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AppSettings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Body",
                value: "<p>I spent a good chunk of last year integrating large language model features into a healthcare application. Not a side project, not a conference demo. A real system where a nurse practitioner would read the output and make decisions based on it. That context changes everything about how you approach AI.</p>\r\n\r\n<h2>The Happy Path Is Not the Full Story</h2>\r\n<p>The tutorials all show you the happy path. You call the API, you get a smart-looking response, and you feel great about the future. What they do not show you is what happens when the model confidently returns something that sounds completely reasonable and is completely wrong. In a healthcare context that is not an embarrassing chatbot moment. It is a clinical risk.</p>\r\n\r\n<h2>Build an Evaluation Layer First</h2>\r\n<p>The first thing I learned is that you need an evaluation layer before you expose any AI output to users. For us that meant a confidence-scoring approach where low-confidence responses were flagged for human review rather than displayed directly. <strong>Semantic Kernel</strong> makes this fairly manageable because the plugin model lets you compose your own validation steps alongside the model calls.</p>\r\n\r\n<h2>Prompt Injection Is Real</h2>\r\n<p>The second thing I learned is about prompt injection. I had read about it in theory but seeing it attempted in the wild is something else. Users discover quickly that phrasing their input in certain ways can influence what the model does. When your AI assistant has access to a patient record database, that is a problem you care about very much.</p>\r\n<p>The fix is treating all user input as untrusted data before it ever reaches the prompt. That sounds obvious when you say it, but it is easy to be sloppy about in practice.</p>\r\n\r\n<h2>Semantic Kernel as a Solid Foundation</h2>\r\n<p><strong>Semantic Kernel</strong> has been a reliable foundation for this work. The ability to define typed functions that the kernel can call, log, and audit makes building responsible AI much more tractable than rolling everything by hand. The memory and vector search support is good enough for most RAG scenarios without needing to reach for separate infrastructure.</p>\r\n\r\n<h2>Stream Everything</h2>\r\n<p>The third lesson, and probably the most important one, is about latency expectations. Users who have never interacted with an LLM before will stare at a spinner for about three seconds before concluding the application is broken. <strong>Streaming responses</strong> change the experience completely.</p>\r\n<p>In C# the async enumerable pattern works beautifully for this. Tokens arrive at the client almost immediately and the perception of performance is transformed, even when the total time is the same.</p>\r\n\r\n<h2>Four Things That Will Save You Pain</h2>\r\n<p>If I were starting a new AI integration today I would go straight to Semantic Kernel, set up proper structured logging from day one, build the evaluation layer before wiring anything to the UI, and stream everything. Those four things will save you enormous amounts of pain and produce a result that real users can trust.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Body",
                value: "<p>There is a version of security awareness that goes like this: you download the OWASP Top Ten PDF, read through it a few days before launch, tick off the obvious ones, and ship. I did that version for longer than I would like to admit.</p>\r\n\r\n<h2>What Changed My Thinking</h2>\r\n<p>The shift happened when I was working on a system that stored clinical data and we brought in an external security consultant for a review. They found four significant vulnerabilities in about two hours. None of them were exotic. All of them were on the list I had been scanning.</p>\r\n<p>The problem was not that I did not know about injection or broken authentication. The problem was that I had been reading the list as a set of abstract categories rather than as a description of how real attacks actually happen.</p>\r\n\r\n<h2>Injection Is About Trust Boundaries</h2>\r\n<p>Take injection. Reading it as a checkbox, you think about SQL injection, add parameterised queries everywhere, and consider it done. But injection covers a much broader story. It is about trusting data that comes from outside your control boundary.</p>\r\n<p>In a modern .NET application that boundary is much larger than it used to be. You have user input, obviously. But you also have data coming back from third-party APIs, from uploaded files, from webhook payloads, from AI model outputs. When I started thinking about injection as a story about trust boundaries rather than a technique, I found three places in our application where we were trusting data we should not have been.</p>\r\n\r\n<h2>Broken Access Control Is About What Happens After Login</h2>\r\n<p>Broken access control is the same thing. The list tells you to check authorisation. Fine. But the story underneath that is about what happens when someone knows the shape of your URLs. In one of our early applications we had resource IDs that were sequential integers. You did not need to be a penetration tester to realise that incrementing the number in the URL gave you access to another user's records.</p>\r\n<p>We had authentication working perfectly. We had no authorisation on the actual resource. Two completely different things.</p>\r\n<p>In .NET I now use a combination of resource-based authorisation via <strong>IAuthorizationService</strong> and opaque identifiers rather than sequential database IDs for anything that appears in a URL. Neither is complicated. Both would have caught the issues above.</p>\r\n\r\n<h2>Use the List as a Story, Not a Checklist</h2>\r\n<p>The way I approach security reviews now is to sit with the OWASP list and for each item ask not whether we have done the obvious thing, but where in our specific system the story that category describes could unfold. It is slower. But the things it surfaces are real.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Body",
                value: "<p>I wrote code in the late nineties that I had to maintain in the early two thousands. I wrote code in the early two thousands that I had to go back to in 2015. Each of those encounters with past me was educational in ways that no conference talk or book ever matched.</p>\r\n\r\n<h2>Naming Is Not a Minor Detail</h2>\r\n<p>The first thing past me always got wrong was naming. Not catastrophically wrong, just slightly off in ways that compound. A class called <strong>DataHelper</strong>. A method called <strong>Process</strong>. A variable called <code>temp</code> that turned out to be a tax calculation result that several other things depended on.</p>\r\n<p>I have spent real hours in old codebases trying to understand what something does before I dare change it. The cost of that time is invisible in the moment when you are rushing to ship, but very visible when you are the one paying it later.</p>\r\n<p>Modern C# is genuinely excellent for expressing intent clearly. Record types, pattern matching, and the improvements to switch expressions since C# 8 have made it possible to write code that reads almost like a description of the business problem rather than a set of mechanical instructions. I use those features aggressively now because I know future me will be grateful.</p>\r\n\r\n<h2>Coupling Is the Quiet Killer</h2>\r\n<p>The second thing past me got wrong was coupling. Not always the obvious kind where everything calls everything else. More often the subtle kind where a bunch of things share a database table, or where a service class has seventeen methods that are really three different concepts that grew together because nobody stopped to separate them.</p>\r\n<p>The single responsibility principle sounds like a rule imposed by someone who did not have deadlines. Having maintained code that violated it for years, it sounds like common sense.</p>\r\n<p>In C# terms this means being quite aggressive about keeping classes small and focused. When a class crosses about 200 lines I start asking whether it is actually two things. When a method needs more than three parameters I ask whether those parameters are describing a concept that deserves its own type. These are not hard rules; they are signals worth paying attention to.</p>\r\n\r\n<h2>Make Invalid States Impossible</h2>\r\n<p>The third lesson is about making invalid states unrepresentable. C# has been getting better at this for years. <strong>Nullable reference types</strong>, discriminated union patterns via records and sealed hierarchies, required properties: all of these make it harder to construct an object in a state your system cannot handle.</p>\r\n<p>The bugs that cause the worst incidents are almost always states the developer did not think were possible. Making them impossible at the type level is worth a lot.</p>\r\n\r\n<h2>The Evidence Accumulates</h2>\r\n<p>None of this is news to anyone who thinks seriously about code quality. The difference is that I now have thirty years of evidence that it actually matters, and that the people who will thank you most for writing clear, well-structured code are often yourself coming back to it three years later.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4,
                column: "Body",
                value: "<p>JWT is everywhere in modern web APIs. It is also one of the things I see misunderstood most often when I review code. Not because people do not understand what a JWT is in general terms, but because there are a handful of details that look minor until they become the reason your authentication is compromised.</p>\r\n\r\n<h2>Algorithm Confusion</h2>\r\n<p>The first issue is algorithm confusion. A JWT has a header that declares which algorithm was used to sign it. The algorithm <code>none</code> means no signature at all. Early libraries would happily accept this if you did not explicitly configure which algorithms you would accept.</p>\r\n<p><strong>ASP.NET Core</strong> does not have this problem by default if you use the standard JwtBearer middleware and configure <code>ValidAlgorithms</code>. But if you are rolling your own token validation or using a third-party library, check that it rejects unsigned tokens explicitly. This is not a theoretical concern; it has been exploited in real applications.</p>\r\n\r\n<h2>Key Length Matters More Than You Think</h2>\r\n<p>The second thing is key length. A symmetric HMAC key for HS256 should be at least 256 bits, which is 32 bytes. I have reviewed systems where the key was a short memorable string like a product name or a domain.</p>\r\n<p>Brute-forcing a weak key against a known JWT structure is not difficult. More importantly, if anyone can get hold of the signing key by any means, they can issue themselves any token they want. That key needs to be treated like a password and stored accordingly.</p>\r\n\r\n<h2>Expiry and Revocation</h2>\r\n<p>The third thing is expiry and revocation. JWTs are stateless, which is part of their appeal, but that statelessness means you cannot revoke one once issued unless you maintain a denylist.</p>\r\n<p>Short expiry times with a <strong>refresh token pattern</strong> are the standard answer. The refresh token lives server-side and can be revoked. Short access token expiry limits the damage window if one gets intercepted. A one-hour access token expiry is reasonable for most applications. Twenty-four hours is pushing it. No expiry is how you end up with tokens that are valid forever after an employee leaves.</p>\r\n\r\n<h2>A Practical Configuration</h2>\r\n<p>In ASP.NET Core I configure token validation like this: require issuer, require audience, validate lifetime, validate the signing key, reject tokens with no expiry, and specify the exact algorithm. That covers the common pitfalls.</p>\r\n<p>Then I make sure the key comes from a secrets store rather than a config file that ends up in source control, because I have seen that too and it is a bad day when you notice it.</p>\r\n<p>JWT done right is a solid foundation for stateless API authentication. JWT done casually is a set of vulnerabilities waiting to be found by someone who is looking.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5,
                column: "Body",
                value: "<p>The support ticket said the dashboard was slow. That was all the information I had when I started. By the time I had finished I had a 99% reduction in load time and a better understanding of why production and development environments lie to you about performance in different ways.</p>\r\n\r\n<h2>Start With Measurement, Not Assumptions</h2>\r\n<p>The first instinct when something is slow is to open the code and look for the obvious problem. I have learned not to do this. The obvious problem is rarely the actual problem, and the time you spend fixing the obvious thing is time you are not spending measuring.</p>\r\n<p>I started by adding <strong>Application Insights</strong> telemetry to the API endpoint and attaching a SQL trace to the production database. Within ten minutes I had the data I needed. The dashboard query was issuing 847 SQL statements per request.</p>\r\n\r\n<h2>The N+1 Query Hidden in Plain Sight</h2>\r\n<p>Eight hundred and forty-seven queries. On a developer laptop with a database containing twelve rows, this is imperceptible. In production with three thousand rows and a database server on a separate host, the round-trip latency alone adds up fast.</p>\r\n<p>The root cause was a classic <strong>N+1 query problem</strong> buried inside a service method that had grown organically over eighteen months. The outer query fetched a list of records. A foreach loop then called a second method for each record. That second method hit the database independently, once per item. Nobody had noticed because the test data was tiny and the database was local.</p>\r\n<p>In <strong>Entity Framework Core</strong> terms, the fix was straightforward: replace the loop with a single query using <code>Include</code> and <code>ThenInclude</code> for the related data, and add a projection to fetch only the columns the dashboard actually needed rather than materialising full entities. The resulting SQL was one statement joining four tables.</p>\r\n\r\n<h2>The Second Problem the First Problem Was Hiding</h2>\r\n<p>With the N+1 query fixed, load time dropped from eight seconds to about 900 milliseconds. Better, but still not good enough. The SQL trace showed a single query now, but it was doing a full table scan on a column that appeared in every WHERE clause.</p>\r\n<p>The column had no index. It had never had an index because in development the table had a handful of rows and a sequential scan was trivially fast. In production the table had grown to three hundred thousand rows and a sequential scan was taking 700 milliseconds on its own.</p>\r\n<p>Adding a composite index on the two most-used filter columns brought the query time to under 20 milliseconds. Combined with the N+1 fix, the total endpoint time measured at 80 milliseconds from the load test client.</p>\r\n\r\n<h2>What This Experience Reinforced</h2>\r\n<p>Performance problems are rarely where you think they are. Measure first with real production data and real production topology before touching a line of code. The investment in proper observability — structured logging, APM tooling, query tracing — pays back in hours, not weeks.</p>\r\n<p>The other thing this reinforced is that code review catches logical problems but usually not performance problems. Performance testing against production-scale data is a separate discipline, and it is worth building into your delivery process rather than treating as an emergency response when something goes wrong in production.</p>\r\n<p>The fix itself took about two hours. Finding it took about thirty minutes with the right tools. Getting the right tools in place took most of the afternoon of the day I joined the team. That was the best afternoon I spent on that project.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6,
                column: "Body",
                value: "<p>BookIt started as a client request for a simple online booking system. By the time it shipped it had become one of the most satisfying builds I have done in recent years, partly because of the technology choices and partly because of what those choices enabled at the UI layer.</p>\r\n\r\n<h2>A Flexible Domain Model from the Start</h2>\r\n<p>The brief was straightforward: businesses needed to manage appointments, track resources, and let customers book online. The tricky part was that the businesses themselves were diverse: a hair salon, a physio clinic, a training room hire company, and each had slightly different ideas about what a booking even was.</p>\r\n<p>That diversity pushed me toward a flexible domain model early on rather than hardcoding assumptions about slot length, resource type, or cancellation policy.</p>\r\n\r\n<h2>Why Blazor Server Was the Right Choice</h2>\r\n<p>I chose <strong>Blazor Server</strong> for the front end from the start. The real-time availability requirements meant that a traditional request and response cycle would create friction in the UI. With Blazor Server and <strong>SignalR</strong> underneath, I could push availability updates to connected clients the moment a slot was taken without the client polling. In practice this makes the booking experience feel instant in a way that a standard MVC form cannot match.</p>\r\n\r\n<h2>MudBlazor Did the Heavy Lifting</h2>\r\n<p><strong>MudBlazor</strong> was the natural choice for the component library. The project needed a professional-grade UI with both light and dark mode, and MudBlazor's theming system handles that with almost no ceremony. The MudDataGrid component handled the admin booking list, MudCalendar handled the visual schedule view, and the chip system made tagging bookings with status and category feel polished without custom CSS.</p>\r\n\r\n<h2>Clean Architecture Paid Off</h2>\r\n<p>The data layer uses <strong>Entity Framework Core 9</strong> with SQL Server. I applied a clean architecture pattern with domain entities, repository interfaces in the application layer, and EF Core implementations in the infrastructure layer. This made unit testing the booking logic straightforward and kept the domain model free of EF Core attributes.</p>\r\n\r\n<h2>SMS Notifications: A Late Addition That Became Essential</h2>\r\n<p>SMS notifications were a late addition that turned out to be heavily used. Customers get a confirmation text when they book, a reminder 24 hours before, and a follow-up after. I built a provider-agnostic SMS abstraction so the underlying provider could be swapped without touching the booking logic. In production we are using ClickSend, but Twilio support is also built in.</p>\r\n\r\n<h2>The One Thing I Would Do Differently</h2>\r\n<p>The one thing I would do differently is implement optimistic concurrency on slot reservations from the very first sprint. We had a brief window early in development where two simultaneous bookings for the same slot were both confirmed. The fix was a database-level check combined with a retry policy on the Blazor component, but it would have been simpler to design for that from the start.</p>\r\n<p>BookIt is now live and has processed thousands of bookings across multiple businesses. The Blazor real-time approach has aged well and I am still satisfied with the architectural decisions made at the outset.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7,
                column: "Body",
                value: "<p>Building software for healthcare is different from building it for almost any other domain. The margin for error is smaller, the compliance requirements are real, and the people using the system are often under significant time and emotional pressure. Curo was built with all of that in mind.</p>\r\n\r\n<h2>Replacing Paper with a Live Dashboard</h2>\r\n<p>The platform was commissioned to replace a paper-based care plan system used by a team of community carers. Their workflow involved visiting patients at home, recording observations, completing tasks from a care plan, and escalating anything concerning to a care manager. On paper this meant a lot of phone calls, lost handover notes, and care managers who had no live visibility into what was happening in the field.</p>\r\n<p>Curo brings that workflow into a <strong>Blazor</strong> application accessible on any device. Carers use it on tablets during visits to check tasks, record vitals, and mark care plan steps complete. Care managers get a live dashboard showing which carers are active, which patients have been visited, and which tasks are overdue. The real-time updates come via <strong>SignalR</strong>, the same mechanism that makes Blazor Server so well suited to operational dashboards.</p>\r\n\r\n<h2>The Hardest Design Problem: Care Plans</h2>\r\n<p>The domain model for care plans was the hardest design problem. A care plan for one patient might have fifty tasks across medication, nutrition, mobility, and social engagement, each with different frequencies, assigned carers, and escalation thresholds. Getting that model right took several iterations and some very direct conversations with the clinical team about what they actually needed versus what they initially asked for.</p>\r\n\r\n<h2>Security and Compliance Were Non-Negotiable</h2>\r\n<p>The application holds personal health information, which meant encryption at rest and in transit, role-based access control at every API endpoint, full audit logging of every data access, and a carefully scoped permission model so that a carer could only see the patients assigned to them.</p>\r\n<p>I used <strong>ASP.NET Core Identity</strong> with custom claims for the RBAC layer and a purpose-built audit middleware that writes to a separate audit log table rather than mixing audit records with application data.</p>\r\n\r\n<h2>Azure Hosting on a Reasonable Budget</h2>\r\n<p>Azure hosting gave us the infrastructure story at reasonable cost. The application runs on <strong>Azure App Service</strong> with Azure SQL Database. Azure Active Directory handles the identity provider for the care management organisation. Deployment is automated through a GitHub Actions pipeline that builds, tests, runs OWASP dependency checks, and deploys to staging before production.</p>\r\n\r\n<h2>The Moment It Mattered</h2>\r\n<p>The feedback from carers after go-live was genuinely moving. The care manager described being able to see her team's visits happening in real time as transformative. That is the kind of outcome that reminds me why building useful software matters.</p>");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8,
                column: "Body",
                value: "<p>Recruitment is one of those domains that seems simple until you are three weeks into building the software for it. The surface area is manageable: jobs, candidates, applications, interviews and offers, but the workflow complexity underneath is substantial and every organisation does it slightly differently.</p>\r\n\r\n<h2>From Spreadsheets to a Single Platform</h2>\r\n<p>TalentConnect was built for a recruitment team who needed to move away from spreadsheets and shared inboxes. They were tracking candidates across multiple roles simultaneously, scheduling interviews via email chains, and producing monthly reports by hand from a dozen different sources. The brief was to consolidate all of that into a single <strong>Blazor</strong> application.</p>\r\n\r\n<h2>The Candidate Pipeline as the Heart of the Design</h2>\r\n<p>The candidate pipeline was the heart of the design. Every candidate progresses through configurable stages: applied, screening, shortlisted, first interview, second interview, offer, hired or rejected. Each transition is timestamped and the history is preserved so the team can see how long candidates have been at each stage and where the bottlenecks in their process are. That audit trail also proved valuable for compliance purposes.</p>\r\n\r\n<h2>Real-Time Updates via Blazor Server</h2>\r\n<p><strong>Blazor Server</strong> was the right choice here for the same reasons it works well in other operational tools: the recruiter-facing pipeline board benefits from live updates when a colleague moves a candidate or adds a note, and the team is always working from current data without needing to refresh.</p>\r\n\r\n<h2>MudBlazor Made the Kanban Board Possible</h2>\r\n<p><strong>MudBlazor</strong>'s drag-and-drop support made the kanban-style pipeline board possible without writing a single line of custom JavaScript. The component library's data grid handled the tabular candidate list, filter chips made filtering by role, stage, and recruiter feel natural, and the stepper component guided new job posting creation through a structured flow that reduced data entry errors.</p>\r\n\r\n<h2>Notifications Driven by Domain Events</h2>\r\n<p>The notification system was a significant piece of work in its own right. Candidates receive automated emails at key pipeline stages: application received, shortlisted, interview invitation and outcome. Recruiters get Blazor toast notifications when a candidate completes an online assessment or when an interview response comes in. The whole thing is driven by an event-based model where stage transitions publish domain events and notification handlers decide what to send to whom.</p>\r\n\r\n<h2>Analytics That Required Careful Query Design</h2>\r\n<p>One of the more interesting technical challenges was the analytics reporting. The team wanted to see time-to-hire by role, source channel conversion rates, and interviewer pass rates. These calculations involve aggregating across pipeline history and doing date arithmetic across potentially thousands of candidate journeys.</p>\r\n<p>Getting those queries right in <strong>EF Core</strong> without full table scans required some careful index design and a few projection-only queries that bypass change tracking entirely.</p>\r\n<p>TalentConnect is now the primary recruitment tool for the team that commissioned it. Spreadsheet use has dropped to zero and the monthly report now takes minutes rather than a working day.</p>");
        }
    }
}
