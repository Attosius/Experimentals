namespace NetTests
{
    public class A
    {
        public virtual void Method1(int x)
        {

        }
        public virtual void Method2<T>(params T[] x)
        {

        }
        public virtual void F()
        {

        }
    }
    public class B : A
    {
        public override void Method1(int x)
        {

        }
        public new void Method2<T>(params T[] x)
        {

        }
        public new void F()
        {

        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var b = new B();
            b.F();
            A a = b;
            a.F();
            a.Method2(1);
            b.F();
            Console.WriteLine("Hello, World!");
        }
    }
}
