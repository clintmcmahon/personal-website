namespace Website.Services;

/// <summary>
/// A portfolio case study. The pages are scaffolded ahead of the writing, so each
/// one carries a Published flag. While it is false the route 404s and the portfolio
/// card links straight out to the live site instead. Flip it to true once the copy
/// is written and the page goes live, the card starts pointing at the case study,
/// and the URL enters the sitemap.
/// </summary>
public record CaseStudy(string Slug, string Title, string LiveUrl, bool Published)
{
    public string Url => $"/portfolio/{Slug}";

    /// <summary>Where the portfolio card should point right now.</summary>
    public string CardHref => Published ? Url : LiveUrl;

    // Razor omits an attribute entirely when its value expression is null, so these
    // keep target/rel off the anchor once the link is internal.
    public string? CardTarget => Published ? null : "_blank";
    public string? CardRel => Published ? null : "noopener noreferrer";
}

public static class CaseStudies
{
    public static readonly CaseStudy MinnesotaSecretaryOfState = new(
        Slug: "minnesota-secretary-of-state",
        Title: "Minnesota Secretary of State",
        LiveUrl: "https://www.sos.state.mn.us",
        Published: true);

    public static readonly CaseStudy SrtrInteractiveReports = new(
        Slug: "srtr-interactive-reports",
        Title: "SRTR Interactive Reports",
        LiveUrl: "https://srtr.hrsa.gov/transplant-professionals/program-specific-report/program-specific-reports-psr/",
        Published: true);

    public static readonly IReadOnlyList<CaseStudy> All =
    [
        MinnesotaSecretaryOfState,
        SrtrInteractiveReports
    ];
}
