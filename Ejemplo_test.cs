// ANTES — código duplicado en cada test
[Test]
public void Test1()
{ var s = new SistemaArchivos(); /* ... */ }
[Test]
public void Test2()
{ var s = new SistemaArchivos(); /* ... */ }
[Test]
public void Test3()
{ var s = new SistemaArchivos(); /* ... */ }
// DESPUÉS — usando [SetUp]
[TestFixture]
public class SistemaArchivosTests
{
    private SistemaArchivos _sut;
    /* Tu código refactorizado aquí */
    [setUp]
    public void CrearSistemaArchivo()
    {
        _sut = new SistemaArchivos();
    }
    [Test]
    Test1()
    {  /* ... */ }
    [Test]
    Test2()
    {  /* ... */ }
    [Test]
    Test3()
    {  /* ... */ }
}
