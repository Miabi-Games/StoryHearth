namespace StoryHearth.Engine;

public enum ApplicationRunResult
{
    Success,
    Failure,
}

public static class ApplicationRunResultExtensions
{
    extension(ApplicationRunResult result)
    {
        public bool IsSuccess => result == ApplicationRunResult.Success;
    }
}
