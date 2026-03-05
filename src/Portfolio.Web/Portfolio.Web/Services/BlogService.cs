namespace Portfolio.Web.Services;

public record BlogPost(
    string Slug,
    string Title,
    string Summary,
    string Category,
    string PublishedDate,
    int ReadMinutes,
    string[] Tags,
    string Body
);

public static class BlogService
{
    public static IReadOnlyList<BlogPost> GetAllPosts() => _posts;

    public static BlogPost? GetBySlug(string slug) =>
        _posts.FirstOrDefault(p => p.Slug == slug);

    private static readonly BlogPost[] _posts =
    [
        new BlogPost(
            Slug: "building-ai-into-dotnet-without-losing-your-mind",
            Title: "Building AI into .NET Without Losing Your Mind",
            Summary: "Real lessons from shipping Semantic Kernel and Azure OpenAI in production. What the tutorials skip over and what actually matters when real users are involved.",
            Category: "AI",
            PublishedDate: "4 March 2025",
            ReadMinutes: 8,
            Tags: ["AI", "Semantic Kernel", "Azure OpenAI", ".NET", "C#"],
            Body: """
                I spent a good chunk of last year integrating large language model features into a healthcare
                application. Not a side project, not a demo for a conference talk. A real system where a nurse
                practitioner would be reading the output and making decisions based on it. That context changes
                everything about how you approach AI.

                The tutorials all show you the happy path. You call the API, you get a smart looking response,
                you feel great about the future. What they do not show you is what happens when the model
                confidently returns something that sounds completely reasonable and is completely wrong. In a
                healthcare context that is not an embarrassing chatbot moment. It is a clinical risk.

                So the first thing I learned is that you need an evaluation layer before you expose any AI
                output to users. For us that meant a confidence scoring approach where low confidence responses
                got flagged for human review rather than displayed directly. Semantic Kernel makes this fairly
                manageable because the plugin model lets you compose your own validation steps alongside the
                model calls.

                The second thing I learned is about prompt injection. I had read about it in theory but seeing
                it attempted in the wild is something else. Users discover quickly that if they phrase their
                input in certain ways they can influence what the model does. When your AI assistant has access
                to a patient record database that is a problem you care about very much. The fix is treating
                all user input as untrusted data before it ever reaches the prompt, which sounds obvious when
                you say it but it is easy to be sloppy about in practice.

                Semantic Kernel has been a solid foundation for this work. The ability to define typed functions
                that the kernel can call, log, and audit makes building responsible AI much more tractable than
                rolling everything by hand. The memory and vector search support is good enough for most RAG
                scenarios without needing to reach for separate infrastructure.

                The third lesson and probably the most important one is about latency expectations. Users who
                have never interacted with an LLM before will stare at a spinner for about three seconds before
                they conclude that the application is broken. Streaming responses changes the experience
                completely. In C# the async enumerable pattern works beautifully for this. You get tokens
                arriving at the client almost immediately and the perception of performance is transformed even
                when the total time is the same.

                If I were starting a new AI integration today I would go straight to Semantic Kernel, set up
                proper structured logging from day one, build the evaluation layer before you wire anything to
                the UI, and stream everything. Those four things will save you enormous amounts of pain.
                """
        ),

        new BlogPost(
            Slug: "the-owasp-top-ten-is-not-a-checklist-it-is-a-story",
            Title: "The OWASP Top Ten Is Not a Checklist. It Is a Story.",
            Summary: "After working on systems that handle patient data and financial records, the OWASP list stopped being something I scan before a launch. It became a way of thinking about how software fails.",
            Category: "Security",
            PublishedDate: "14 February 2025",
            ReadMinutes: 7,
            Tags: ["Security", "OWASP", "ASP.NET Core", "C#"],
            Body: """
                There is a version of security awareness that goes like this. You download the OWASP Top Ten
                PDF. You read through it a few days before launch. You tick off the obvious ones. You ship.
                I did that version for longer than I would like to admit.

                The shift happened when I was working on a system that stored clinical data and we brought in
                an external security consultant for a review. They found four significant vulnerabilities in
                about two hours. None of them were exotic. All of them were on the list I had been scanning.
                The problem was not that I did not know about injection or broken authentication. The problem
                was that I had been reading the list as a set of abstract categories rather than as a
                description of how real attacks actually happen.

                Take injection. Reading it as a checkbox you think about SQL injection, you add parameterised
                queries everywhere, done. But injection covers a much broader story. It is about trusting data
                that comes from outside your control boundary. In a modern .NET application that boundary is
                much larger than it used to be. You have user input, obviously. But you also have data coming
                back from third party APIs, from uploaded files, from webhook payloads, from AI model outputs.
                When I started thinking about injection as a story about trust boundaries rather than a
                technique, I found three places in our application where we were trusting data we should not
                have been trusting.

                Broken access control is the same thing. The list tells you to check authorisation. Fine. But
                the story underneath that is about what happens when someone knows the shape of your URLs. In
                one of our early applications we had resource IDs that were sequential integers. You did not
                need to be a penetration tester to realise that incrementing the number in the URL gave you
                access to another user's records. We had authentication working perfectly. We had no authorisation
                on the actual resource. Two completely different things.

                In .NET I now use a combination of resource based authorisation via IAuthorizationService and
                opaque identifiers rather than sequential database IDs for anything that appears in a URL.
                Neither of those is complicated. Both of them would have caught the issues above.

                The way I approach security reviews now is to sit with the OWASP list and for each item ask
                not whether we have done the obvious thing but where in our specific system the story that
                category is describing could unfold. It is slower but the things it surfaces are real.
                """
        ),

        new BlogPost(
            Slug: "what-thirty-years-of-csharp-taught-me-about-code-that-lasts",
            Title: "What Three Decades of Software Development Taught Me About Writing Code That Lasts",
            Summary: "I have maintained codebases I wrote fifteen years ago. That experience is humbling and instructive in ways that no architecture course ever was.",
            Category: ".NET",
            PublishedDate: "20 January 2025",
            ReadMinutes: 6,
            Tags: [".NET", "C#", "Architecture", "Clean Code"],
            Body: """
                I wrote code in the late nineties that I had to maintain in the early two thousands. I wrote
                code in the early two thousands that I had to go back to in 2015. Each of those encounters
                with past me was educational in ways that no conference talk or book ever matched.

                The first thing past me always got wrong was naming. Not catastrophically wrong, just slightly
                off in ways that compound. A class called DataHelper. A method called Process. A variable called
                temp that turned out to be a tax calculation result that several other things depended on.
                I have spent real hours in old codebases trying to understand what something does before I
                dare change it. The cost of that time is invisible in the moment when you are rushing to ship
                but it is very visible when you are the one paying it later.

                Modern C# is genuinely excellent for naming intent clearly. Record types, pattern matching,
                and the improvements to switch expressions since C# 8 have made it possible to write code
                that reads almost like a description of the business problem rather than a set of mechanical
                instructions. I use those features aggressively now because I know that future me will be
                grateful.

                The second thing past me got wrong was coupling. Not always the obvious kind where everything
                calls everything else. More often the subtle kind where a bunch of things share a database
                table, or where a service class has seventeen methods that are really three different concepts
                that grew together because nobody stopped to separate them. The single responsibility
                principle sounds like a rule imposed by someone who did not have deadlines. Having maintained
                code that violated it for years, it sounds like common sense.

                In C# terms this means being quite aggressive about keeping classes small and focused. When
                a class crosses about 200 lines I start asking whether it is actually two things. When a method
                needs more than three parameters I ask whether those parameters are describing a concept that
                deserves its own type. These are not hard rules. They are signals worth paying attention to.

                The third lesson is about making invalid states unrepresentable. C# has been getting better
                at this for years. Nullable reference types, discriminated union patterns via records and
                sealed hierarchies, required properties. All of these make it harder to construct an object
                that is in a state your system cannot handle. The bugs that cause the worst incidents are
                almost always states that the developer did not think were possible. Making them impossible
                at the type level is worth a lot.

                None of this is news to anyone who thinks seriously about code quality. The difference is
                that I now have thirty years of evidence that it actually matters, and that the people who
                will thank you most for writing clear, well structured code are often yourself coming back
                to it three years later.
                """
        ),

        new BlogPost(
            Slug: "jwt-tokens-are-not-magic",
            Title: "JWT Tokens Are Not Magic and That Matters",
            Summary: "I have seen JWT used correctly and I have seen it used in ways that made me genuinely anxious. Here is what you actually need to know when building authentication in ASP.NET Core.",
            Category: "Security",
            PublishedDate: "8 January 2025",
            ReadMinutes: 7,
            Tags: ["Security", "JWT", "ASP.NET Core", "Authentication", "OAuth2"],
            Body: """
                JWT is everywhere in modern web APIs. It is also one of the things I see misunderstood most
                often when I review code. Not because people do not understand what a JWT is in general terms,
                but because there are a handful of details that look minor until they become the reason your
                authentication is compromised.

                The first one is algorithm confusion. A JWT has a header that declares which algorithm was
                used to sign it. The algorithm none means no signature at all. Early libraries would happily
                accept this if you did not explicitly configure which algorithms you would accept. ASP.NET
                Core does not have this problem by default if you use the standard JwtBearer middleware and
                configure ValidAlgorithms. But if you are rolling your own token validation or using a third
                party library, check that it rejects unsigned tokens explicitly. This is not a theoretical
                concern. It has been exploited in real applications.

                The second thing is key length. A symmetric HMAC key for HS256 should be at least 256 bits,
                which is 32 bytes. I have reviewed systems where the key was a short memorable string like a
                product name or a domain. Brute forcing a weak key against a known JWT structure is not
                difficult. More importantly, if anyone can get hold of the signing key by any means, they can
                issue themselves any token they want. That key needs to be treated like a password and stored
                accordingly.

                The third thing is expiry and revocation. JWTs are stateless which is part of their appeal,
                but that statelessness means you cannot revoke one once issued unless you maintain a denylist.
                Short expiry times with a refresh token pattern are the standard answer. The refresh token
                lives server side and can be revoked. Short access token expiry limits the damage window if
                one gets intercepted. A one hour access token expiry is reasonable for most applications.
                Twenty four hours is pushing it. No expiry is how you end up with tokens that are valid
                forever after an employee leaves.

                In ASP.NET Core I configure token validation like this in practice: require issuer, require
                audience, validate lifetime, validate the signing key, reject tokens with no expiry, and
                specify the exact algorithm. That covers the common pitfalls. Then I make sure the key comes
                from a secrets store rather than a config file that ends up in source control, because I have
                seen that too, and it is a bad day when you notice it.

                JWT done right is a solid foundation for stateless API authentication. JWT done casually is
                a set of vulnerabilities waiting to be found by someone who is looking.
                """
        ),

        new BlogPost(
            Slug: "when-ai-caught-a-bug-my-tests-missed",
            Title: "When AI Caught a Bug My Tests Missed",
            Summary: "I was sceptical about using AI for code review. Then it found a logic error in a patient record query that had been sitting unnoticed for months. That changed my thinking.",
            Category: "AI",
            PublishedDate: "18 December 2024",
            ReadMinutes: 5,
            Tags: ["AI", ".NET", "C#", "Testing", "Healthcare"],
            Body: """
                I will be honest. When AI assisted coding tools started appearing in my workflow I was fairly
                dismissive. I had good tests. I had code reviews. I had colleagues who would spot obvious
                problems. What was an autocomplete on steroids going to add?

                The answer turned out to be something I did not expect: it caught a class of error that humans
                are actually bad at catching. Specifically, logical errors that are consistent with the
                surrounding code and consistent with the tests but wrong in a way that matters.

                Here is what happened. We had a query in a patient record system that pulled upcoming
                appointments for a given patient. The query was filtering by patient ID and by a date range.
                The tests passed because the test data was structured in a way that made the wrong result
                indistinguishable from the correct result. Both patients in the test set had appointments in
                the target range. The bug was that under certain conditions the query would include appointments
                belonging to a different patient if they shared a practitioner. The practitioner ID was being
                used where the patient ID should have been in one branch of a conditional.

                An AI review of that function flagged it by noting that the conditional branches were
                using different ID fields and asking whether that was intentional. It was one sentence in
                a list of minor comments. I almost scrolled past it. Then I read it twice and went cold.

                The root cause was a copy paste from a practitioner schedule query earlier in the file. The
                shape of the code was right. The variable names were reasonable. The tests tested the right
                thing but with data that did not expose the edge case. Three humans had reviewed this code
                at various points including me. None of us caught it.

                I do not think AI code review replaces human review. But I do think it catches different
                things. Humans are good at catching things that feel wrong, things that violate conventions,
                things that are structurally unusual. We are less good at the systematic comparison of whether
                every occurrence of a pattern in a file is using the right variable. That systematic
                comparison is something a language model does without getting tired or distracted.

                I now use AI review as a second pass on anything that touches data access in sensitive domains.
                It is not perfect. It flags things that are fine. But the one time in ten where it finds
                something real is worth the noise.
                """
        ),
    ];
}
