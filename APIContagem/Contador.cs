using System.Runtime.InteropServices;

namespace APIContagem;

public class Contador
{
    private static readonly string _LOCAL;
    private static readonly string _KERNEL;
    private static readonly string _FRAMEWORK;

    static Contador()
    {
        _LOCAL = Environment.MachineName;
        _KERNEL = Environment.OSVersion.VersionString;
        _FRAMEWORK = RuntimeInformation.FrameworkDescription;
    }

    private int _valorAtual = 0;

    public int ValorAtual { get => _valorAtual; }
    public string Local { get => _LOCAL; }
    public string Kernel { get => _KERNEL; }
    public string Framework { get => _FRAMEWORK; }

    public void Incrementar()
    {
        _valorAtual++;
    }
}