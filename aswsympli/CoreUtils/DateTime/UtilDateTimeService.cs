namespace CoreUtils.DateTime
{
    public class UtilDateTimeService : IDateTimeService
    {
        public virtual System.DateTime GetNow()
        {
            return System.DateTime.Now;
        }

        public virtual System.DateTime GetUtcNow()
        {
            return System.DateTime.UtcNow;
        }
    }
}
