using AutoFixture;

namespace SpotifyPlaylistJanitorAPI.Tests
{
    public abstract class TestBase
    {
        protected IFixture Fixture { get; }

        protected TestBase()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}
