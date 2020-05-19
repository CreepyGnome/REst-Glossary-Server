using System.Linq;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Limits the number of circular dependencies that will be auto-generated.
    /// </summary>
    public class OmitOnRecursionBehaviorCustomization : ICustomization
    {
        /// <summary>
        /// The recursion depth to which circular dependencies will be auto-generated. 
        /// <para>A value of 1 and will prevent any circular dependencies from being generated.</para>
        /// </summary>
        public int RecursionDepth { get; set; }
        
        /// <inheritdoc/>
        public void Customize(IFixture fixture)
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(RecursionDepth));
        }
    }
}
