using Octokit;

namespace Website.Services;

public class GitHubService
{
    private readonly GitHubClient _client;
    private readonly string _owner;
    private readonly string _repo;
    private readonly string _branch;

    public GitHubService(string personalAccessToken, string owner, string repo, string branch = "main")
    {
        _owner = owner;
        _repo = repo;
        _branch = branch;
        _client = new GitHubClient(new ProductHeaderValue("personal-website-admin"))
        {
            Credentials = new Credentials(personalAccessToken)
        };
    }

    // Commits multiple files in a single commit using the Git Data API (tree + commit).
    // files: list of (repoPath, fileBytes) where repoPath is relative to repo root.
    public async Task CommitFilesAsync(IEnumerable<(string RepoPath, byte[] Content)> files, string commitMessage)
    {
        var repoRef = await _client.Git.Reference.Get(_owner, _repo, $"heads/{_branch}");
        var latestCommitSha = repoRef.Object.Sha;
        var latestCommit = await _client.Git.Commit.Get(_owner, _repo, latestCommitSha);

        var newTree = new NewTree { BaseTree = latestCommit.Tree.Sha };

        foreach (var (repoPath, content) in files)
        {
            var blob = await _client.Git.Blob.Create(_owner, _repo, new NewBlob
            {
                Content = Convert.ToBase64String(content),
                Encoding = EncodingType.Base64
            });

            newTree.Tree.Add(new NewTreeItem
            {
                Path = repoPath,
                Mode = "100644",
                Type = TreeType.Blob,
                Sha = blob.Sha
            });
        }

        var createdTree = await _client.Git.Tree.Create(_owner, _repo, newTree);

        var newCommit = await _client.Git.Commit.Create(_owner, _repo, new NewCommit(commitMessage, createdTree.Sha, new[] { latestCommitSha })
        {
            Author = new Committer("Clint McMahon", "clint@clintmcmahon.com", DateTimeOffset.UtcNow)
        });

        await _client.Git.Reference.Update(_owner, _repo, $"heads/{_branch}", new ReferenceUpdate(newCommit.Sha));
    }
}
