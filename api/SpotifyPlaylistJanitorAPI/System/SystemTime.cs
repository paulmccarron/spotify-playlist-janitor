namespace SpotifyPlaylistJanitorAPI.System
{
    /// <summary>
    /// Static class to use in place of DateTime.Now to make time sensitive code unit testable
    /// </summary>
    public static class SystemTime
    {
        /// <summary>
        /// Return currently set value for Now, defaults to DateTime.Now
        /// </summary>
        public static Func<DateTime> Now = () => DateTime.Now;

        /// <summary>
        /// Set new value for Now
        /// </summary>
        /// <param name="dateTimeNow"></param>
        public static void SetDateTime(DateTime dateTimeNow)
        {
            Now = () => dateTimeNow;
        }

        /// <summary>
        /// Reset value to DateTime.Now
        /// </summary>
        public static void ResetDateTime()
        {
            Now = () => DateTime.Now;
        }
    }
}
