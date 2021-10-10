namespace Librame.Extensions.Resources
{
    class TestResourceDictionary : AbstractResourceDictionary<TestResourceDictionary>
    {
        public TestResourceDictionary()
            : base("Test")
        {
        }


        public string TestName
        {
            get => GetString(nameof(TestName));
            set => Add(nameof(TestName), value);
        }

    }
}
