namespace Machete.Test.Integration.Fluent
{
    public static class FluentRecordBaseFactory
    {
        private static FluentRecordBase _frb;

        static FluentRecordBaseFactory()
        {
            _frb = new FluentRecordBase();
        }

        public static FluentRecordBase Get()
        {
            if (_frb == null)
              _frb = new FluentRecordBase();

            return _frb;
        }
    }
}