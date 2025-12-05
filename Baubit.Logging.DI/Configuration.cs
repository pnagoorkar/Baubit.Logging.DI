using Baubit.DI;

namespace Baubit.Logging.DI
{
    /// <summary>
    /// Configuration class for the Logging DI module.
    /// Specifies which logging providers to register in the service collection.
    /// </summary>
    public class Configuration : AConfiguration
    {
        /// <summary>
        /// Gets or sets whether to add the console logging provider.
        /// When <c>true</c>, console logging is enabled.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool AddConsole { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to add the debug logging provider.
        /// When <c>true</c>, debug logging is enabled.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool AddDebug { get; set; } = false;
    }
}
